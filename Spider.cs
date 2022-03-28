using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Silent_Void
{
    class Spider : Entity
    {
        public static Texture2D texture;
        public int speed = 10;
        float acc = 0.2f;
        double deviance = 0;
        public Spider(Vector2 pos, float rad) : this(texture, pos, rad)
        {
        }
        public Spider(Texture2D tex, Vector2 pos, float rad)
        {
            base.colour = Color.White;
            base.tex = tex;
            base.size = new Vector2(32, 32);
            hitBoxSize = new Vector2(32, 32);
            base.pos = pos;
            base.rad = rad;
            base.hp = 1;
            friendly = false;
            isBullet = false;
        }
        public override void Update()
        {
            Vector2 target = player.pos - pos;
            rad = (float)(Math.Atan2(target.Y, target.X) + Math.PI);

            double ang = Math.Atan2(target.Y, target.X);
            deviance += (Enemy.rnd.NextDouble() - 0.5) * Math.PI / 6;
            if (deviance > Math.PI / 4)
            {
                deviance = Math.PI / 4;
            }
            if (deviance < -Math.PI / 4)
            {
                deviance = -Math.PI / 4;
            }
            ang += deviance;

            vel += new Vector2((float)Math.Cos(ang) * acc, (float)Math.Sin(ang) * acc);

            if (vel.LengthSquared() > speed * speed)
            {
                vel.Normalize();
                vel *= speed;
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
            base.Update();
        }
    }
}