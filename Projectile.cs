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
    class Projectile : Entity
    {

        public Projectile(Texture2D tex, Vector2 pos, float rad, bool friendly, Vector2 vel)
        {
            base.colour = Color.White;
            base.tex = tex;
            size = new Vector2(25, 25);
            base.pos = pos;
            base.rad = rad;
            base.friendly = friendly;
            isBullet = true;

            this.vel = vel -new Vector2((float)Math.Cos(rad), (float)Math.Sin(rad)) * 15;
        }
        public override void Update()
        {

            if ((player.pos - pos).Length() > 2000)
            {
                removed = true;
            }

            base.Update();
        }
    }
}