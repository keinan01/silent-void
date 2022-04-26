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
    class Silo : Enemy
    {
        public static new Texture2D texture;
        public Silo(Vector2 pos, float rad) : base(texture, pos, rad)
        {
            reload = 500;
            cooldown = rnd.Next(500);
            speed = 2;
            hp = 10;
        }
        public override void Shoot(Vector2 pos, float rad, bool friendly)
        {
            game.Add(new Missile(pos , rad));
        }


        public override void OnDeath()
        {
            for (int i = 0; i < 12; i++)
            {
                Projectile projectile = new Projectile(Missile.fireball, this.pos, this.rad + (float)Math.PI / 6 * i, false, 10);
                game.planes.Add(projectile);
            }
            base.OnDeath();
        }
    }
}