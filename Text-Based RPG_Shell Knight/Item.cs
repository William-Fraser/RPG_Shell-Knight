using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Item : Object
    {
        public Item(string name, char avatar)
        {
            _name = name;
            _avatar = avatar;

            x = 26;
            y = 8;
            aliveInWorld = true;

        }

        public bool pickupItem(int playerX, int playerY)
        {
            if (x == playerX)
            {
                if (y == playerY)
                {
                    aliveInWorld = false;
                    return true;
                }
            }
            return false;
        }
       
    }
}
