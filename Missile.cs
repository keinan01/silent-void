using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silent_Void
{
    class Missile : Spider
    {
        public static new Texture2D texture;
        public static Texture2D fireball;
        public int lifetime = 120;
        public Missile(Vector2 pos, float rad) : base(texture, pos, rad)
        {

        }
        public override void Update()
        {
            lifetime--;
            if(lifetime < 0)
            {
                this.removed=true;
            }
            base.Update();
        }

        public override void OnDeath()
        {
            for (int i = 0; i < 6; i++)
            {
                Projectile projectile = new Projectile(fireball, this.pos, this.rad + (float)Math.PI / 3 * i, false, 10);
                game.planes.Add(projectile);
            }
            base.OnDeath();
        }
    }
}
