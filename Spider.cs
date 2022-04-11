using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Silent_Void
{
    class Spider : Entity // subclass of entity
    {
        // needed instance fields for the class to function
        public static Texture2D texture;
        public int speed = 10;
        float acc = 0.2f;
        double deviance = 0;
        // empty constructor that derives info from the other constructor
        public Spider(Vector2 pos, float rad) : this(texture, pos, rad)
        {
        }
        // constructor that initializes various base and class variables
        public Spider(Texture2D tex, Vector2 pos, float rad)
        {
            // managing position, hp, etc
            base.colour = Color.White;
            base.tex = tex;
            base.size = new Vector2(36, 36);
            hitBoxSize = new Vector2(36, 36);
            base.pos = pos;
            base.rad = rad;
            base.hp = 1;
            friendly = false;
            isBullet = false;
        }
        // update the spider position, speed, movement, etc
        public override void Update()
        {
            // going towards the player - not too close though
            Vector2 target = player.pos - pos;
            rad = (float)(Math.Atan2(target.Y, target.X) + Math.PI);

            double ang = Math.Atan2(target.Y, target.X);
            // making sure enemies dont bunch up together - deviance
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

            if (vel.LengthSquared() > speed * speed) //make sure its not going too fast
            {
                vel.Normalize();
                vel *= speed;
            }
            // making sure its within bounds, rebounding if needed
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
