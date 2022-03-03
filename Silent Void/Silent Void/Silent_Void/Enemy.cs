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
        float baseAng;
        int reload = 1000;
        int cooldown = rnd.Next(1000);

        public Enemy(Texture2D tex, Vector2 pos, float rad)
        {
            base.tex = tex;
            base.size = new Vector2(72, 72);
            base.pos = pos;
            base.rad = rad;
            baseAng = rad;
            base.colour = new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            friendly = false;
            isBullet = false;
        }
        public override void Update()
        {
            double a = (rad % (2 * Math.PI) + 2 * Math.PI) % (2 * Math.PI);

            double b = baseAng;
            if ((player.pos - pos).Length() < 500)
            {
                b = Math.Atan2((pos - player.pos).Y, (pos - player.pos).X);
            }

            if ((b - a + 3 * Math.PI) % (2 * Math.PI) - Math.PI > 0.1)
            {
                rad += (float)Math.PI / 100;
            }
            else if ((b - a + 3 * Math.PI) % (2 * Math.PI) - Math.PI < -0.1)
            {
                rad -= (float)Math.PI / 100;
            }
            vel = -new Vector2((float)Math.Cos(rad), (float)Math.Sin(rad)) * 2;

            if ((player.pos - pos).Length() > 2000)
            {
                removed = true;
            }

            if (cooldown >= reload)
            {
                Shoot(pos, rad, false, vel);
                cooldown = 0;
            }

          cooldown++;

            base.Update();
        }
        public void Shoot(Vector2 pos, float rad, bool friendly, Vector2 vel)
        {
            game.Add(new Projectile(game.bullet, pos, rad, friendly, vel));
        }

    }
}