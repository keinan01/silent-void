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

namespace Silent_Void

{
    class Player : Entity
    { 
        int cooldown = 0;
        int reload = 10;
        public int points;
        float acc = 1f;
        int speed = 8;
        public Player(Texture2D tex, Vector2 pos, float rad)
        {
            base.colour = Color.White;
            base.tex = tex;
            size = new Vector2(72, 72);
            base.pos = pos;
            base.rad = rad;
            friendly = true;
            isBullet = false;
            points = 0;
            vel = new Vector2(0, 0);

        }
        public override void Update()
        {
            MouseState mouseState = Mouse.GetState();

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            Vector2 dPos = this.pos - mousePosition;

            rad = (float)Math.Atan2(dPos.Y, dPos.X);

            //vel = -new Vector2((float)Math.Cos(rad), (float)Math.Sin(rad)) * 5;

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

            if (Keyboard.GetState().IsKeyDown(Keys.D))

            {
                vel.X += acc;
            }

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

            if (mouseState.LeftButton == ButtonState.Pressed && cooldown >= reload)
            {
                Shoot(pos, rad, true);
                cooldown = 0;
            }
            cooldown++;

            base.Update();
        }

        public void Shoot(Vector2 pos, float rad, bool friendly)
        {
            game.Add(new Projectile(game.bullet, pos, rad, friendly, 15));
        }
    }
}