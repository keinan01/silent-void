using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Silent_Void
{
    class Enemy : Entity
    {
        public static Random rnd = new Random();
        public static Texture2D texture;
        public int reload = 200;
        public int cooldown = rnd.Next(200);
        int speed = 5;
        float acc = 0.1f;
        double deviance = 0;
        public Enemy(Vector2 pos, float rad) : this(texture, pos, rad)
        {
        }
        public Enemy(Texture2D tex, Vector2 pos, float rad)
        {
            base.colour = Color.White;
            base.tex = tex;
            base.size = new Vector2(72, 72);
            base.pos = pos;
            base.rad = rad;
            friendly = false;
            isBullet = false;
        }

        public override void Update()
        {
            Vector2 target = player.pos - pos;
            rad = (float)(Math.Atan2(target.Y, target.X) + Math.PI);
            if ((player.pos - pos).Length() < 350)
            {
                target = pos - player.pos;
            }
            if ((player.pos - pos).Length() < 350 || (player.pos - pos).Length() > 450)
            {
                double ang = Math.Atan2(target.Y, target.X);
                deviance += (rnd.NextDouble() - 0.5) * Math.PI / 6;
                if (deviance > Math.PI / 2)
                {
                    deviance = Math.PI / 2;
                }
                if (deviance < -Math.PI / 2)
                {
                    deviance = -Math.PI / 2;
                }
                ang += deviance;

                vel += new Vector2((float)Math.Cos(ang) * acc, (float)Math.Sin(ang) * acc);
            }

            if (vel.LengthSquared() > speed * speed)
            {
                vel.Normalize();
                vel *= speed;
            }

            if (cooldown >= reload)
            {
                Shoot(pos, rad, false);
                cooldown = 0;
            }

            cooldown++;

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

            base.Update();
        }
        public virtual void Shoot(Vector2 pos, float rad, bool friendly)
        {
            game.Add(new Projectile(game.enemyBullet, pos, rad, friendly, 15));
        }
    }
}