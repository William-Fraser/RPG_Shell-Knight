using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Item : GameObject
    {
        public Item(string name, char avatar)
        {
            _name = name;
            _avatar = avatar;

            _posX = 26;
            _posY = 8;
            aliveInWorld = true;

        }

        public bool pickupItem(int playerX, int playerY)
        {
            if (_posX == playerX)
            {
                if (_posY == playerY)
                {
                    aliveInWorld = false;
                    return true;
                }
            }
            return false;
        }
       
    }
}
