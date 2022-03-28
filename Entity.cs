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
        public Vector2 pos, vel, size, hitBoxSize;
        public Texture2D tex;
        public Color colour;
        public float rad;
        public static Game1 game;
        public bool removed = false;
        public static Entity player;
        public bool friendly;
        public bool isBullet;
        public int hp;
        public bool invincible = false;
        public Entity()
        {
            hitBoxSize = new Vector2(0, 0);
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

        public virtual void OnHit()
        {
            hp--;
        }


        public virtual void OnDeath()
        {
        }

        public bool collides(Entity other)
        {
            return new Rectangle((int)(pos.X - hitBoxSize.X/2), (int)(pos.Y - hitBoxSize.Y / 2), (int)hitBoxSize.X, (int)hitBoxSize.Y - 20).Intersects(new Rectangle((int)(other.pos.X - other.hitBoxSize.X / 2), (int)(other.pos.Y - other.hitBoxSize.Y / 2), (int)other.hitBoxSize.X, (int)other.hitBoxSize.Y));
        }
    }
}