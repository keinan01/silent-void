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
    class Item
    {

        public static Texture2D spriteSheet;
        public Rectangle pos, spriteRect;
        ItemType itemType;
        public Item(ItemType item)
        {
            itemType = item;
        }
        void drawItem()
        {
            if (itemType == ItemType.HPBuff)
            {
                spriteRect = new Rectangle(0, 0, 64, 64);
            }
            if (itemType == ItemType.AtkBuff)
            {
                spriteRect = new Rectangle(64, 0, 64, 64);
            }
        }
        public void activateItem()
        {
            if (itemType == ItemType.HPBuff)
            {
                Entity.player.buffHp();
            }
        }

        public void setPos(Rectangle newPos)
        {
            pos = newPos;
        }
        public enum ItemType
        {
            HPBuff,
            AtkBuff,
            Null
        }
    }
}
