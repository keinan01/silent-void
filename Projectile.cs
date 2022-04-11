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
    class Projectile : Entity // subclass of entity
    {
        // constructor that initializes some base and class variables
        public Projectile(Texture2D tex, Vector2 pos, float rad, bool friendly, float speed)
        {
            base.colour = Color.White;
            base.tex = tex;
            size = new Vector2(25, 25);
            hitBoxSize = size - new Vector2(10, 10);
            base.pos = pos;
            base.rad = rad;
            base.friendly = friendly;
            base.hp = 1;
            isBullet = true;

            this.vel = vel -new Vector2((float)Math.Cos(rad), (float)Math.Sin(rad)) * speed; // initialize & manage velocity
        }

        public override void Update()
        {
            // remove the bullet if out of bounds
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
            // call parent class update method for normal movement
                base.Update();
        }
    }
}
