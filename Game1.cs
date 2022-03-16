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
        Shop,
        YouDied
    }
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SoundEffect sfxShot;
        KeyboardState oldkey = Keyboard.GetState();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        List<Entity> planes;
        SpriteFont font;
        Texture2D bg, systemBg, arrow, deathBg;
        Texture2D playerTex;
        public Texture2D bullet;
        Vector2 screen, cameraPos;
        float rotationRadians = 0f;
        MouseState mouseState;
        Vector2 arrowPos;
        int arrowCycle;
        Enemy villain;
        GameState gameState = GameState.LevelWorld;
        int enemySpawnCooldown;
        
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
            enemySpawnCooldown = 0;
            arrowCycle = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTex = this.Content.Load<Texture2D>("player");
            bg = this.Content.Load<Texture2D>("bg");
            bullet = this.Content.Load<Texture2D>("bullet");
            Entity.player = player = new Player(playerTex, screen / 2, rotationRadians);
            planes = new Entity[] { player }.ToList();
            arrow = this.Content.Load<Texture2D>("arrow");
            font = this.Content.Load<SpriteFont>("SpriteFont1");
            systemBg = this.Content.Load<Texture2D>("sair conglomerate");
            deathBg = this.Content.Load<Texture2D>("deadth screen");
            coords.AddRange(new List<int>() { 1690, 330, 1175, 525, 135, 875, 405, 195, 135, 975 });
            arrowPos = new Vector2(coords[0], coords[1]);
            villain = new OpEnemy(playerTex, new Vector2(200, 200), 1f);
            planes.Add(villain);
            sfxShot = this.Content.Load<SoundEffect>("gunshot");
            for (int i = 0; i < 5; i++)
            {
                planes.Add(new OpEnemy(playerTex, new Vector2(200, 200), 1f));
            }
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
            if(gameState == GameState.Overworld)
            {
                
                if (!oldkey.IsKeyDown(Keys.Enter) && key.IsKeyDown(Keys.Enter))
                {
                    gameState = GameState.LevelWorld;
                }
                if (!oldkey.IsKeyDown(Keys.Left) && key.IsKeyDown(Keys.Left))
                {
                    arrowCycle += 2;
                    
                    if (arrowCycle > coords.Count - 4)
                    {
                        arrowCycle = 0;
                    }
                    Debug.WriteLine(coords[arrowCycle] + ", "+ coords[arrowCycle + 1]);
                    arrowPos = new Vector2(coords[arrowCycle], coords[arrowCycle + 1]);
                }
                if (!oldkey.IsKeyDown(Keys.Right) && (key.IsKeyDown(Keys.Right)))
                {
                    arrowCycle -= 2;
                    
                    if (arrowCycle < 0)
                    {
                        arrowCycle = coords.Count - 4;
                    }
                    Debug.WriteLine(coords[arrowCycle] + ", " + coords[arrowCycle + 1]);
                    arrowPos = new Vector2(coords[arrowCycle], coords[arrowCycle + 1]);
                }
            }
            if (gameState == GameState.LevelWorld)
            {
                if (player.removed)
                {
                    sfxShot.Play();
                    gameState = GameState.YouDied;
                    

                }
                for (int i = 0; i < planes.Count; i++)
                {
                    planes[i].Update();
                    for (int j = 0; j < planes.Count; j++)
                    {
                        if (planes[i].collides(planes[j]) && i != j && !(planes[i].isBullet && planes[j].isBullet) && planes[i].friendly != planes[j].friendly)
                        {
                            //sfxShot.Play();
                            planes[i].removed = true;
                            planes[j].removed = true;
                            player.points += 100;
                        }
                    }
                }




                for (int i = planes.Count - 1; i >= 0; i--)
                {
                    if (planes[i].removed)
                    {
                        sfxShot.Play();
                        planes.RemoveAt(i);

                    }
                    
                }
                if (key.IsKeyDown(Keys.Back))
                {
                    gameState = GameState.Overworld;
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
            spriteBatch.Begin();
            if (gameState == GameState.Overworld)
            {
                
                spriteBatch.Draw(systemBg, new Rectangle(0, 0, 1920, 1080), Color.White);
                spriteBatch.Draw(arrow, new Rectangle((int) arrowPos.X, (int) arrowPos.Y, 50, 50), Color.White);
            }
            if (gameState == GameState.LevelWorld)
            {
                spriteBatch.Draw(bg, new Rectangle(0, 0, (int)screen.X, (int)screen.Y), Color.White);
                for (int i = 0; i < planes.Count; i++)
                {
                    planes[i].Draw(spriteBatch, cameraPos);
                }
                spriteBatch.DrawString(font, player.points.ToString(), new Vector2(0, 0), Color.White);
            }
            if (gameState == GameState.YouDied)
            {
                spriteBatch.Draw(deathBg, new Rectangle(0, 0, 1920, 1080), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        
    }
}