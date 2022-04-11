

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

    enum GameState // enum for managing which screens the game is on (gamestate)
    {
        LevelWorld,
        Overworld,
        Shop,
        YouDied,
        TitleScreen,
        EndLevel
    }
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // alot of instance fields like sound, keyboard, textures, vectors, etc
        public SoundEffect sfxShot;
        KeyboardState oldkey = Keyboard.GetState();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        public List<Entity> planes;
        SpriteFont font;
        Texture2D systemBg, arrow, deathBg, titleBg, overlay;
        Dictionary<string,Texture2D> bgs;
        Texture2D playerTex;
        public Texture2D bullet, enemyBullet;
        Vector2 screen;
        float rotationRadians = 0f;
        MouseState mouseState;
        Vector2 arrowPos;
        int arrowCycle, levelCycle, LevelCount;
        GameState gameState = GameState.TitleScreen;
        Vector2 hpPos = new Vector2(0, 20);

        private Level level; // make a level variable

        List<int> coords = new List<int>(); // coord ranges

        public Game1()
        {
            this.Window.AllowUserResizing = true;


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            

            // changing the generic screen size to preferred size
            graphics.PreferredBackBufferWidth = 1920;
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
            // initialize game cycle and the UI
            this.IsMouseVisible = true;
            Entity.game = this; 
            screen = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            arrowCycle = 0;
            levelCycle = 1;
            base.Initialize();


        }

        private void CreateLevel(String path) // make a level based on the text file
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
            // load in player, enemy, and background textures
            playerTex = this.Content.Load<Texture2D>("player");
            Enemy.texture = this.Content.Load<Texture2D>("spitter");
            OpEnemy.texture = this.Content.Load<Texture2D>("sprayer");
            bgs = new Dictionary<string, Texture2D>();
            bgs.Add("bg", this.Content.Load<Texture2D>("bg"));
            bgs.Add("bg2", this.Content.Load<Texture2D>("bg2"));
            bgs.Add("bg3", this.Content.Load<Texture2D>("bg3"));
            bgs.Add("bg4", this.Content.Load<Texture2D>("bg4"));



            titleBg = this.Content.Load<Texture2D>("title screen");

            // loading in bullet and initializing sizes of previous textures + more backgrounds
            bullet = this.Content.Load<Texture2D>("bullet");
            enemyBullet = this.Content.Load<Texture2D>("glob");
            Entity.player = player = new Player(playerTex, screen / 2, rotationRadians);
            planes = new Entity[] { player }.ToList();
            arrow = this.Content.Load<Texture2D>("arrow");
            font = this.Content.Load<SpriteFont>("SpriteFont1");
            systemBg = this.Content.Load<Texture2D>("sair conglomerate");
            deathBg = this.Content.Load<Texture2D>("deadth screen");

            overlay = new Texture2D(GraphicsDevice, 1, 1);
            overlay.SetData(new Color[] { Color.White });

            // add ranges to coords
            coords.AddRange(new List<int>() { 135, 875, 1690, 330, 1175, 525, 405, 195});
            LevelCount = 4; // num of levels so far

            arrowPos = new Vector2(coords[0], coords[1]); // for selecting arrow
            sfxShot = this.Content.Load<SoundEffect>("gunshot"); // gunshot sfx
        

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
            // managing the gamestate based on key presses
            if (gameState == GameState.TitleScreen)
            {
                if (!oldkey.IsKeyDown(Keys.Enter) && key.IsKeyDown(Keys.Enter))
                {
                    
                    gameState = GameState.Overworld;
                }
            } else if (gameState == GameState.Overworld)
            {
                player.reset();

                if (!oldkey.IsKeyDown(Keys.Enter) && key.IsKeyDown(Keys.Enter))
                {
                    CreateLevel(@"Content\levels\level0" + levelCycle + ".txt"); // go to specified level
                    gameState = GameState.LevelWorld; 
                }
                if (!oldkey.IsKeyDown(Keys.Left) && key.IsKeyDown(Keys.Left)) // moving arrow left
                {
                    arrowCycle += 2; // managing where arrow can move 
                    levelCycle += 1;
                    if (arrowCycle > coords.Count - 2) // rebounding it to the first one after 4th one
                    {
                        arrowCycle = 0;
                        levelCycle = 1;
                    }
                    Debug.WriteLine(coords[arrowCycle] + ", " + coords[arrowCycle + 1]);
                    arrowPos = new Vector2(coords[arrowCycle], coords[arrowCycle + 1]); // change arrow pos
                }
                if (!oldkey.IsKeyDown(Keys.Right) && (key.IsKeyDown(Keys.Right))) // moving arrow right, same logic as moving (slightly different)
                {
                    arrowCycle -= 2;
                    levelCycle -= 1;
                    if (arrowCycle < 0)
                    {
                        arrowCycle = coords.Count - 2;
                        levelCycle = LevelCount;
                    }
                    Debug.WriteLine(coords[arrowCycle] + ", " + coords[arrowCycle + 1]);
                    arrowPos = new Vector2(coords[arrowCycle], coords[arrowCycle + 1]);
                }
            }
            if (gameState == GameState.LevelWorld)
            {
                
                if (player.removed)
                {
                    sfxShot.Play(); // on death, play sound & move to you died screen
                    gameState = GameState.YouDied;


                }
                for (int i = 0; i < planes.Count; i++) // manage the shooting logic 
                {
                    planes[i].Update();
                    for (int j = 0; j < planes.Count; j++)
                    {
                        if (planes[i].collides(planes[j]) && i != j && !(planes[i].isBullet && planes[j].isBullet) && planes[i].friendly != planes[j].friendly && !planes[i].invincible && !planes[j].invincible)
                        {
                            //sfxShot.Play();
                            planes[i].OnHit();
                            planes[j].OnHit(); // killing enemy after shot
                            player.points += 100; // adding points
                        }
                    }
                }




                for (int i = planes.Count - 1; i >= 0; i--)
                {
                    if (planes[i].removed)
                    {
                        sfxShot.Play();
                        planes.RemoveAt(i); // after enemy died, play sound & remove

                    }

                }
                // start and end levels
                if (planes.All(planes => planes.friendly) && level.waveNum >= level.wavePlural.Count)
                {
                    gameState = GameState.EndLevel; 

                }
                else if (planes.All(planes => planes.friendly) && level.waveNum < level.wavePlural.Count)
                {
                    level.startWave();
                }
     
                

            }
            // control to go back to overworld screen
            if (gameState == GameState.EndLevel)
            {
                if (key.IsKeyDown(Keys.Back))
                {
                    gameState = GameState.Overworld;
                }
            }
            // TODO: Add your update logic here
            oldkey = key;

            base.Update(gameTime);
        }

        public void Add(Entity e) // add planes
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
            // draw based on gamestate
            if (gameState == GameState.TitleScreen)
            {
                spriteBatch.Draw(titleBg, new Rectangle(0, 0, 1920, 1080), Color.White); // draw title background with game name
            }
            if (gameState == GameState.Overworld) // draw solar system and arrow
            {

                spriteBatch.Draw(systemBg, new Rectangle(0, 0, 1920, 1080), Color.White);
                spriteBatch.Draw(arrow, new Rectangle((int)arrowPos.X, (int)arrowPos.Y, 50, 50), Color.White);
            }
            if (gameState == GameState.LevelWorld) // draw level based on the specifications of the text file
            {
                spriteBatch.Draw(bgs[level.bg], new Rectangle(0, 0, (int)screen.X, (int)screen.Y), Color.White);
                for (int i = 0; i < planes.Count; i++)
                {
                    planes[i].Draw(spriteBatch, new Vector2(0, 0));
                }
                spriteBatch.Draw(overlay, new Rectangle(0, 0, (int)screen.X, (int)screen.Y), Color.Red * player.hurtTransparency); // hit feedback for player
                spriteBatch.Draw(overlay, new Rectangle((int)hpPos.X, (int)hpPos.Y, player.hp * 20, 20), Color.Green);
                spriteBatch.DrawString(font, player.points.ToString(), new Vector2(0, 0), Color.White); // show player points
            }
            if (gameState == GameState.YouDied)
            {
                spriteBatch.Draw(deathBg, new Rectangle(0, 0, 1920, 1080), Color.White); // death screen
            }
            if (gameState == GameState.EndLevel)  // level summary screen
            {
                //spriteBatch.Draw(deathBg, new Rectangle(0, 0, 1920, 1080), Color.White);
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.DrawString(font, "Level End!!! \n press Backspace to enter overworld", new Vector2(1920 / 2, 1080 / 2), Color.White);
                spriteBatch.DrawString(font, player.points.ToString(), new Vector2(1920 / 2, (1080 / 2) + 50), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}
