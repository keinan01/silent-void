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
    class Player : Entity // player is subclass of entity
    {
        // needed instance fields for managing the player state like reload and speed
        int cooldown = 0;
        int reload = 10;
        public int points;
        float acc = 1f;
        int speed = 8;
        public float hurtTransparency = 0f;
        public float iframe = 0;
        Vector2 defaultPos;
        // player constructor - initialized a lot of base variables and class variables
        public Player(Texture2D tex, Vector2 pos, float rad)
        {
            base.colour = Color.White;
            base.tex = tex;
            size = new Vector2(72, 72);
            hitBoxSize = size - new Vector2(10, 10);
            defaultPos = pos;
            base.rad = rad;
            
            base.maxHp = 5;
            friendly = true;
            isBullet = false;
            points = 0;
            vel = new Vector2(0, 0);

        }
        public override void Update() // update position of the player - overrides entity update
        {
            // change directon based on mouse direction
            MouseState mouseState = Mouse.GetState();

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            Vector2 dPos = this.pos - mousePosition;

            rad = (float)Math.Atan2(dPos.Y, dPos.X); 

            //vel = -new Vector2((float)Math.Cos(rad), (float)Math.Sin(rad)) * 5;

            // move left, right, up, down based WASD
            if (Keyboard.GetState().IsKeyDown(Keys.W))

            {
                vel.Y -= acc;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))

            {
                vel.X -= acc;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))

            {
                vel.Y += acc;
            }

            if (vel.LengthSquared() > speed * speed)
            {
                vel.Normalize();
                vel *= speed;
            }
            // increase velocity going right
            if (Keyboard.GetState().IsKeyDown(Keys.D))

            {
                vel.X += acc;
            }
            // rebounding the player 
            if (pos.X < 0)
            {
                pos.X = 0;
                vel.X = -vel.X;
            }
            if (pos.X > 1920)
            {
                pos.X = 1920;
                vel.X = -vel.X;
            }
            if (pos.Y < 0)
            {
                pos.Y = 0;
                vel.Y = -vel.Y;
            }
            if (pos.Y > 1080)
            {
                pos.Y = 1080;
                vel.Y = -vel.Y;
            }
            // shoot 
            if (mouseState.LeftButton == ButtonState.Pressed && cooldown >= reload)
            {
                Shoot(pos, rad, true);
                cooldown = 0;
            }
            cooldown++;
            // change how much its been hurt
            hurtTransparency -= 0.05f;
            if (hurtTransparency < 0)
            {
                hurtTransparency = 0;
            }

            // managing invincible state
            iframe--;
            if (iframe < 0)
            {
                iframe = 0;
                invincible = false;
            }
            if (iframe % 4 >= 2)
            {
                base.colour = Color.Black;
            }
            else
            {
                base.colour = Color.White;
            }

            base.Update();
        }

        public override void OnHit()
        {
            // managing stuff once hit - calling base OnHit method
            if (iframe <= 0)
            {
                hurtTransparency = 0.5f;
                invincible = true;
                iframe = 60;
                base.OnHit();
            }
        }
        // reset the player's position, HP, and velocity presumably after each level
        public void reset()
        {
            base.hp = maxHp;
            pos = defaultPos;
            vel = new Vector2(0, 0);
        }
        // shoot projectile with game bullet texture
        public void Shoot(Vector2 pos, float rad, bool friendly)
        {
            game.Add(new Projectile(game.bullet, pos, rad, friendly, 15));
            sfx.Play(); // play sound effect of bullet
        }
    }
}
