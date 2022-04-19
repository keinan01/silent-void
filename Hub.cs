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
    class Hub
    {
        KeyboardState oldkey = Keyboard.GetState();
        Texture2D hubBg, highlight;
        int rectPosX = 100, rectPosY = 400;
        Vector2 highlightPos;
        int activeItemX, activeItemY;
        //List<Item> items = new List<Item>();
        Item[,] items = new Item[6,2];
        Rectangle[,] grid = new Rectangle[6, 2];
        //int[,] activeItem = new int[6, 2];
        public Hub(Texture2D hubBg, Texture2D highlight)
        {
            this.hubBg = hubBg;
            highlightPos = new Vector2(200, 400);

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                rectPosX += 100;
                for (int e = 0; e < grid.GetLength(1); e++)
                {
                    
                    grid[i, e] = new Rectangle((int) rectPosX, (int) rectPosY, 64, 64);

                    Debug.WriteLine(rectPosX + ", " + rectPosY);
                    rectPosY += 70;
                }
                rectPosY = 400;
                
            }
            this.highlight = highlight;

            for (int i = 0; i < items.GetLength(0); i++)
            {
                
                for (int e = 0; e < items.GetLength(1); e++)
                {

                    items[i, e] = new Item(Item.ItemType.HPBuff);
                    items[i, e].setPos(new Rectangle(rectPosX, rectPosY, 64, 64));

                }
                

            }
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int e = 0; e < items.GetLength(1); e++)
                {
                    
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(hubBg, new Rectangle(0, 0, 1920, 1080), Color.White);
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int e = 0; e < items.GetLength(1); e++)
                {
                    spriteBatch.Draw(Item.spriteSheet,  items[i, e].pos, items[i, e].spriteRect, Color.White);
                }
            }
            
            spriteBatch.Draw(highlight, new Rectangle((int) highlightPos.X , (int)highlightPos.Y , 64, 64), Color.White);
        }
        public void Update()
        {
            KeyboardState key = Keyboard.GetState();
            if (!oldkey.IsKeyDown(Keys.Left) && key.IsKeyDown(Keys.Left))
            {
                
                activeItemX--;
                if (activeItemX < 0 && activeItemY == 0)
                {
                    activeItemX = items.GetLength(0) - 1;
                    activeItemY = 1;
                } else if (activeItemX < 0 && activeItemY == 1)
                {
                    activeItemX = items.GetLength(0) - 1;
                    activeItemY = 0;
                }
                Debug.WriteLine(activeItemX);
                
            }
            if (!oldkey.IsKeyDown(Keys.Down) && key.IsKeyDown(Keys.Down) || !oldkey.IsKeyDown(Keys.Up) && key.IsKeyDown(Keys.Up))
            {

                
                if (activeItemY == 0)
                {
                    
                    activeItemY = 1;
                }
                else if (activeItemY == 1)
                {

                    activeItemY = 0;
                }
                

            }
            
            if (!oldkey.IsKeyDown(Keys.Right) && (key.IsKeyDown(Keys.Right)))
            {
                activeItemX++;
                if (activeItemX >= items.GetLength(0) && activeItemY == 0)
                {
                    activeItemX = 0;
                    activeItemY = 1;
                    
                } else if (activeItemX >= items.GetLength(0) && activeItemY == 1)
                {
                    activeItemX = 0;
                    activeItemY = 0;

                }
                Debug.WriteLine(activeItemX);
                
            }
            highlightPos.X = grid[activeItemX, activeItemY].X;
            highlightPos.Y = grid[activeItemX, activeItemY].Y;

            Debug.WriteLine(activeItemX + ", " + activeItemY);

            if (!oldkey.IsKeyDown(Keys.Enter) && key.IsKeyDown(Keys.Enter))
            {
                items[activeItemX, activeItemY].activateItem();
            }

            oldkey = key;
           

        }
    }
}
