using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silent_Void
{
    class SpiderBall : Projectile
    {
        public SpiderBall(Texture2D tex, Vector2 pos, float rad, float speed) : base(tex, pos, rad, false, speed)
        {

            this.vel = vel - new Vector2((float)Math.Cos(rad), (float)Math.Sin(rad)) * speed;
            size = new Vector2(50, 50);
            hitBoxSize = size - new Vector2(10, 10);
        }

        public override void OnDeath()
        {
            for (int i = 0; i < 3; i++)
            {
                Spider spider = new Spider(this.pos, (float)(Enemy.rnd.NextDouble() * Math.PI * 2));
                spider.vel = new Vector2((float)Math.Cos(spider.rad), (float)Math.Sin(spider.rad)) * spider.speed;
                game.planes.Add(spider);
            }
            base.OnDeath();
        }
    }
}
