using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Silent_Void
{
    class OrbitalSpider : Spider
    {
        float acc = 0.2f;
        double deviance = 0;
        public SpiderBoss parent;
        bool spread = false;
        public OrbitalSpider(Vector2 pos, float rad, SpiderBoss parent) : base(pos, rad)
        {
            this.parent = parent;
        }
        public override void Update()
        {
            if(parent.spread || parent.removed)
            {
                spread = true;
            }
            Vector2 target = parent.pos - pos;
            rad = (float)(Math.Atan2(target.Y, target.X) + Math.PI / 2);

            target.Normalize();
            target = target * speed - vel;

            double angle = Math.Atan2(target.Y, target.X);
            Vector2 target2 = new Vector2((float)Math.Cos(angle - Math.PI / 2), (float)Math.Sin(angle - Math.PI / 2));
            target2.Normalize();
            target2 = target2 * speed - vel;

            target = target * (parent.pos - pos).Length() / 800 + target2 * (1 - (parent.pos - pos).Length() / 1000);

            if(spread)
            {
                target = pos - parent.pos + (player.pos - pos) / 3;
                rad = (float)(Math.Atan2(target.Y, target.X) + Math.PI);
            }

            double ang = Math.Atan2(target.Y, target.X);
            deviance += (Enemy.rnd.NextDouble() - 0.5) * Math.PI / 50;
            if (deviance > Math.PI / 10)
            {
                deviance = Math.PI / 10;
            }
            if (deviance < -Math.PI / 10)
            {
                deviance = -Math.PI / 10;
            }
            ang += deviance;

            vel += new Vector2((float)Math.Cos(ang) * acc, (float)Math.Sin(ang) * acc);

            if (vel.LengthSquared() > speed * speed)
            {
                vel.Normalize();
                vel *= speed;
            }
            if (spread)
            {
                if (pos.X < 0)
                {
                    removed = true;
                }
                if (pos.X > 1920)
                {
                    removed = true;
                }
                if (pos.Y < 0)
                {
                    removed = true;
                }
                if (pos.Y > 1080)
                {
                    removed = true;
                }
            }
            base.UpdatePos();
        }
    }
}