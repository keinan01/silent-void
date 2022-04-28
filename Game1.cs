using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Silent_Void
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    enum GameState
    {
        LevelWorld,
        Overworld,
        GUIHub,
        YouDied,
        TitleScreen,
        EndLevel
    }
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public SoundEffect sfxShot, sfxShotPlayer, sfxPlayerDeath;
        KeyboardState oldkey = Keyboard.GetState();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Hub hub;
        public List<Entity> planes;
        SpriteFont font;
        Texture2D systemBg, arrow, deathBg, titleBg, overlay, hubBg, highlightTex;
        Dictionary<string, Texture2D> bgs;
        Texture2D playerTex;
        public Texture2D bullet, enemyBullet;
        Vector2 screen;
        float rotationRadians = 0f;
        MouseState mouseState;
        Vector2 arrowPos;
        int arrowCycle, levelCycle, LevelCount, winCount = 0;
        GameState gameState = GameState.TitleScreen;
        Vector2 hpPos = new Vector2(0, 20);

        private Level level;

        List<int> coords = new List<int>();
        public Game1()
        {
            this.Window.AllowUserResizing = true;


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1920;
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            Entity.game = this;
            screen = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            arrowCycle = 0;
            levelCycle = 1;
            base.Initialize();


        }

        private void CreateLevel(String path)
        {
            level = new Level(Services, path, this);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTex = this.Content.Load<Texture2D>("img/player");
            Enemy.texture = this.Content.Load<Texture2D>("img/spitter");
            OpEnemy.texture = this.Content.Load<Texture2D>("img/sprayer");
            bgs = new Dictionary<string, Texture2D>();
            bgs.Add("bg", this.Content.Load<Texture2D>("img/bg"));
            bgs.Add("bg2", this.Content.Load<Texture2D>("img/bg2"));
            bgs.Add("bg3", this.Content.Load<Texture2D>("img/bg3"));
            bgs.Add("bg4", this.Content.Load<Texture2D>("img/bg4"));

            Spider.texture = this.Content.Load<Texture2D>("img/baby");
            SpiderBoss.texture = MamaSpider.texture = this.Content.Load<Texture2D>("img/spider");

            

            hubBg = this.Content.Load<Texture2D>("img/GUI hub");
            highlightTex = this.Content.Load<Texture2D>("img/highlight");
            hub = new Hub(hubBg, highlightTex);
            titleBg = this.Content.Load<Texture2D>("img/title screen");

            Item.spriteSheet = this.Content.Load<Texture2D>("img/spritesheet");

            bullet = this.Content.Load<Texture2D>("img/bullet");
            enemyBullet = this.Content.Load<Texture2D>("img/glob");
            Entity.player = player = new Player(playerTex, screen / 2, rotationRadians);
            planes = new Entity[] { player }.ToList();
            arrow = this.Content.Load<Texture2D>("img/arrow");
            font = this.Content.Load<SpriteFont>("SpriteFont1");
            systemBg = this.Content.Load<Texture2D>("img/sair conglomerate");
            deathBg = this.Content.Load<Texture2D>("img/deadth screen");

            overlay = new Texture2D(GraphicsDevice, 1, 1);
            overlay.SetData(new Color[] { Color.White });

            coords.AddRange(new List<int>() { 196, 888, 1752, 448, 1080, 460, 392, 140  });
            LevelCount = 4;

            arrowPos = new Vector2(coords[0], coords[1]);
            sfxShot = this.Content.Load<SoundEffect>("music sfx/alien shot");
            sfxShotPlayer = this.Content.Load<SoundEffect>("music sfx/alien death sfx");
            sfxPlayerDeath = this.Content.Load<SoundEffect>("music sfx/death sfx");
            


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)

        {
            KeyboardState key = Keyboard.GetState();

            mouseState = Mouse.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (gameState == GameState.TitleScreen)
            {
                if (!oldkey.IsKeyDown(Keys.Enter) && key.IsKeyDown(Keys.Enter))
                {

                    gameState = GameState.Overworld;
                }
            }
            else if (gameState == GameState.Overworld)
            {
                player.reset();
                if(winCount == 5)
                {
                    systemBg = this.Content.Load<Texture2D>("img/sair conglomerate alt");
                    coords.Add(1460);
                    coords.Add(870);
                    
                }
                if (!oldkey.IsKeyDown(Keys.Enter) && key.IsKeyDown(Keys.Enter))
                {
                    CreateLevel(@"Content\levels\level0" + levelCycle + ".txt");
                    gameState = GameState.LevelWorld;
                }
                if (!oldkey.IsKeyDown(Keys.Left) && key.IsKeyDown(Keys.Left))
                {
                    arrowCycle -= 2;
                    levelCycle -= 1;
                    if (arrowCycle <0)
                    {
                        arrowCycle = coords.Count - 2;
                        levelCycle = LevelCount;
                    }
                    Debug.WriteLine(coords[arrowCycle] + ", " + coords[arrowCycle + 1]);
                    arrowPos = new Vector2(coords[arrowCycle], coords[arrowCycle + 1]);
                }
                if (!oldkey.IsKeyDown(Keys.Right) && (key.IsKeyDown(Keys.Right)))
                {
                    arrowCycle += 2;
                    levelCycle += 1;
                    if (arrowCycle > coords.Count - 2)
                    {
                        arrowCycle = 0;
                        levelCycle = 1;
                    }
                    Debug.WriteLine(coords[arrowCycle] + ", " + coords[arrowCycle + 1]);
                    arrowPos = new Vector2(coords[arrowCycle], coords[arrowCycle + 1]);
                }
                if (!oldkey.IsKeyDown(Keys.M) && key.IsKeyDown(Keys.M))
                {

                    gameState = GameState.GUIHub;
                }

            }
            if (gameState == GameState.GUIHub)
            {
                if (!oldkey.IsKeyDown(Keys.Enter) && key.IsKeyDown(Keys.Enter))
                {

                    gameState = GameState.Overworld;
                }
                hub.Update();
            }
            if (gameState == GameState.LevelWorld)
            {

                if (player.removed)
                {
                    
                    gameState = GameState.YouDied;


                }
                for (int i = 0; i < planes.Count; i++)
                {
                    planes[i].Update();
                    for (int j = 0; j < planes.Count; j++)
                    {
                        if (planes[i].collides(planes[j]) && i != j && !(planes[i].isBullet && planes[j].isBullet) && planes[i].friendly != planes[j].friendly && !planes[i].invincible && !planes[j].invincible)
                        {
                            
                            planes[i].OnHit();
                            planes[j].OnHit();
                            player.points += 100;
                        }
                    }
                }




                for (int i = planes.Count - 1; i >= 0; i--)
                {
                    if (planes[i].removed)
                    {

                        planes[i].OnDeath();
                        sfxShot.Play();
                        planes.RemoveAt(i);

                    }

                }
                if (planes.All(planes => planes.friendly) && level.waveNum >= level.wavePlural.Count)
                {
                    gameState = GameState.EndLevel;

                }
                else if (planes.All(planes => planes.friendly) && level.waveNum < level.wavePlural.Count)
                {
                    level.startWave();
                }
                


            }
            if (gameState == GameState.EndLevel)
            {
                if (key.IsKeyDown(Keys.Back))
                {
                    winCount++;
                    gameState = GameState.Overworld;
                }
            }

            if (gameState == GameState.YouDied)
            {
                if (key.IsKeyDown(Keys.Back))
                {
                    gameState = GameState.Overworld;
                    planes = new List<Entity>();
                    planes.Add(Entity.player = player = new Player(playerTex, screen / 2, rotationRadians));
                    
                }
            }
            // TODO: Add your update logic here
            oldkey = key;

            base.Update(gameTime);
        }

        public void Add(Entity e)
        {
            planes.Add(e);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred,
                  BlendState.AlphaBlend,
                  SamplerState.PointClamp,
                  null, null, null);
            if (gameState == GameState.TitleScreen)
            {
                spriteBatch.Draw(titleBg, new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            if (gameState == GameState.Overworld)
            {

                spriteBatch.Draw(systemBg, new Rectangle(0, 0, 1920, 1080), Color.White);
                spriteBatch.Draw(arrow, new Rectangle((int)arrowPos.X, (int)arrowPos.Y, 50, 50), Color.White);
            }
            if (gameState == GameState.LevelWorld)
            {
                spriteBatch.Draw(bgs[level.bg], new Rectangle(0, 0, (int)screen.X, (int)screen.Y), Color.White);
                for (int i = 0; i < planes.Count; i++)
                {
                    planes[i].Draw(spriteBatch, new Vector2(0, 0));
                }
                spriteBatch.Draw(overlay, new Rectangle(0, 0, (int)screen.X, (int)screen.Y), Color.Red * player.hurtTransparency);
                spriteBatch.Draw(overlay, new Rectangle((int)hpPos.X, (int)hpPos.Y, player.hp * 20, 20), Color.Green);
                spriteBatch.DrawString(font, player.points.ToString(), new Vector2(0, 0), Color.White);
            }
            if (gameState == GameState.YouDied)
            {
                spriteBatch.Draw(deathBg, new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            if (gameState == GameState.GUIHub)
            {
                hub.Draw(spriteBatch);
            }
            if (gameState == GameState.EndLevel)
            {
                
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.DrawString(font, "Level End!!! \n press Backspace to enter overworld", new Vector2(1920 / 2, 1080 / 2), Color.White);
                spriteBatch.DrawString(font, player.points.ToString(), new Vector2(1920 / 2, (1080 / 2) + 50), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}
