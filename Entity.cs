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
        // instance fields like position, velocity, colour, etc for managing the state of the entity
        public Vector2 pos, vel, size, hitBoxSize;
        public Texture2D tex;
        public Color colour;
        public float rad;
        public SoundEffect sfx;
        public static Game1 game;
        public bool removed = false;
        public static Entity player;
        public bool friendly;
        public bool isBullet;
        public int hp, maxHp;
        public bool invincible = false;
        public Entity() // entity constructor
        {
            hitBoxSize = new Vector2(0, 0);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 shift) // draw the enemy with variables that update on movement
        {
            Vector2 origin = new Vector2(tex.Bounds.Width / 2, tex.Bounds.Height / 2);
            spriteBatch.Draw(tex, new Rectangle((int)(pos.X + shift.X), (int)(pos.Y + shift.Y), (int)size.X, (int)size.Y), null, colour, rad - (float)Math.PI / 2, origin, SpriteEffects.None, 0f);
        }

        public virtual void Update() // move it generically
        {
            pos += vel;
        }

        public virtual void OnHit() // remove hp - mainly for player
        {
            hp--;
            if (hp <= 0)
            {
                removed = true;
            }
        }


        public virtual void OnDeath() 
        {
        }

        public bool collides(Entity other) // returns boolean value upon collision
        {
            return new Rectangle((int)(pos.X - hitBoxSize.X / 2), (int)(pos.Y - hitBoxSize.Y / 2), (int)hitBoxSize.X, (int)hitBoxSize.Y - 20).Intersects(new Rectangle((int)(other.pos.X - other.hitBoxSize.X / 2), (int)(other.pos.Y - other.hitBoxSize.Y / 2), (int)other.hitBoxSize.X, (int)other.hitBoxSize.Y));
        }
        public void loadSfx(SoundEffect sfx) // generic sound effect loading method
        {
            this.sfx = sfx;
        }
    }
}
