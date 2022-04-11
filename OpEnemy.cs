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
    class OpEnemy : Enemy // sublcass of enemy class
    {
        // texture and constructor based off of info given from the base constructor
        public static new Texture2D texture;
        public OpEnemy(Vector2 pos, float rad) : base(texture, pos, rad)
        {
            // initialize parent class variables
            base.cooldown = rnd.Next(20);
            base.reload = 20;
        }
        // shooting method - shoots enemy bullet texture
        public override void Shoot(Vector2 pos, float rad, bool friendly)
        {
            game.Add(new Projectile(game.enemyBullet, pos , rad + (float)(rnd.NextDouble() - 0.5), friendly, 10));
        }
    }
}
