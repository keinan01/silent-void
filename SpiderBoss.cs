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
    class SpiderBoss : Enemy
    {
        public static new Texture2D texture;
        public bool spread = false;
        private int spreadcount = 0;
        public float phase2angle = (float)(rnd.NextDouble() * Math.PI * 2);
        public float phase2anglechange = 0;
        private float ticks = 0;

        public SpiderBoss(Vector2 pos, float rad) : base(texture, pos, rad)
        {
            base.cooldown = rnd.Next(100);
            base.reload = 100;

            base.size = new Vector2(288, 288);
            hitBoxSize = size - new Vector2(10, 10);
            base.hp = 200;
            base.speed = 1;
            base.acc = 0.02f;
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

            if(hp < 100)
            {
                reload = 10;
                phase2angle += phase2anglechange;
                phase2anglechange = (float)(Math.Sin(ticks/200) + (Math.PI-1/2));
                ticks++;
                spread = true;
            }
            else
            {
                spread = false;
            }

            if (cooldown >= reload)
            {
                Shoot(pos, rad, false);
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


            base.UpdatePos();
        }
        public override void Shoot(Vector2 pos, float rad, bool friendly)
        {

            if (hp < 100)
            {
                if (rnd.NextDouble() < 0.05)
                {
                    Spider spider = new Spider(this.pos, 0);
                    spider.vel = new Vector2((float)Math.Cos(phase2angle), (float)Math.Sin(phase2angle)) * spider.speed;
                    game.Add(spider);

                }
                else
                {
                    OrbitalSpider spider = new OrbitalSpider(this.pos, 0, this);
                    spider.vel = new Vector2((float)Math.Cos(phase2angle), (float)Math.Sin(phase2angle)) * spider.speed;
                    game.Add(spider);
                }
            }
            else
            {
                if ((rnd.NextDouble() < 0.3 && spreadcount > 0) || spreadcount > 4)
                {
                    spread = true;
                    spreadcount = 0;
                }
                else if (rnd.NextDouble() < 0.5)
                {
                    spreadcount++;
                    for (int i = 0; i < 10; i++)
                    {
                        OrbitalSpider spider = new OrbitalSpider(this.pos, (float)(Enemy.rnd.NextDouble() * Math.PI * 2), this);
                        spider.vel = new Vector2((float)Math.Cos(spider.rad), (float)Math.Sin(spider.rad)) * spider.speed;
                        game.Add(spider);
                    }
                }
                else
                {
                    game.Add(new SpiderBall(game.enemyBullet, pos, rad, 10));
                }
            }
            cooldown = 0;
        }
    }
}