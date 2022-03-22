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
    public class Entity
    {
        public Vector2 pos, vel, size;
        public Texture2D tex;
        public Color colour;
        public float rad;
        public static Game1 game;
        public bool removed = false;
        public static Entity player;
        public bool friendly;
        public bool isBullet;
        //public bool isDead;
        public Entity()
        {

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 shift)
        {
            Vector2 origin = new Vector2(tex.Bounds.Width / 2, tex.Bounds.Height / 2);
            spriteBatch.Draw(tex, new Rectangle((int)(pos.X + shift.X), (int)(pos.Y + shift.Y), (int)size.X, (int)size.Y), null, colour, rad - (float)Math.PI / 2, origin, SpriteEffects.None, 0f);
        }

        public virtual void Update()
        {
            pos += vel;
        }
        public bool collides(Entity other)
        {
            return new Rectangle((int)pos.X + 4, (int)pos.Y + 4, (int)size.X - 4, (int)size.Y - 4).Intersects(new Rectangle((int)other.pos.X + 4, (int)other.pos.Y + 4, (int)other.size.X - 4, (int)other.size.Y - 4));
        }
    }
}