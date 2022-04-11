using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Silent_Void
{
    class MamaSpider : Spider // subclass of spider
    {
        // texture and constructor that takes in information from parent class
        public static new Texture2D texture;
        public MamaSpider(Vector2 pos, float rad) : base(texture, pos, rad)
        {
            base.size = new Vector2(72, 72);
            hitBoxSize = size - new Vector2(10, 10);
            // manage speed and hp
            base.hp = 3;
            base.speed = 7; 
        }

        public override void OnDeath() // spawn a batch of small spiders after death of big mama spider
        {
            for (int i = 0; i < 3; i++)
            {
                game.planes.Add(new Spider(this.pos, 0));
            }
            base.OnDeath();
        }
        public override void OnHit()// spawn a spider after death of big mama spider
        {
            game.planes.Add(new Spider(this.pos, 0));
            base.OnHit();
        }
    }
}
