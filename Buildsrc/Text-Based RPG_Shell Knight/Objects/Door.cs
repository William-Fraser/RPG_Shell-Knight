using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Door : Object
    {
        private bool _opened;

        public Door(string doorInfo)
        {
            ReadItemInfo(doorInfo);
        }
        // read info
        public void ReadItemInfo(string doorInfo)
        {
            //recognize item info
            string[] avatarAndPos = doorInfo.Split(':');

            //set item fields
            _avatar = doorInfo[0];
            _name = "Door";
            _opened = false;

            //set position
            string[] posHold = avatarAndPos[1].Split(',');
            int[] posXY = new int[2];
            for (int i = 0; i < posHold.Length; i++)
            { posXY[i] = Int32.Parse(posHold[i]); }
            x = posXY[0];
            y = posXY[1];

        }

        //gets sets
        public bool PickedUp() { return _opened; }

        // ----- private methods
        public void OpenDoor(Player player, HUD hud, Toolkit toolkit)
        {
            int[] stock = hud.getInventoryStock();

            if (this._avatar == 'd')
            {
                if (stock[HUD.ITEM_KEYSMALL] > 0)
                {

                    player.RemoveItemFromInventory(HUD.ITEM_KEYSMALL, hud);
                    hud.Draw(toolkit);
                    aliveInWorld = false;
                    hud.DisplayText($"< {player.Name()} opened a small door with a small key >");
                    _opened = true;
                }
                else
                {
                    hud.DisplayText("< " + player.Name() + " tried to open the small door, but it was locked >", false);        
                }
            }
            if (this._avatar == 'D')
            {
                if (stock[HUD.ITEM_KEYBIG] > 0)
                {
                    player.RemoveItemFromInventory(HUD.ITEM_KEYBIG, hud);
                    hud.Draw(toolkit);
                    aliveInWorld = false;
                    hud.DisplayText($"< {player.Name()} opened the big door with the big key >");
                    _opened = true;
                }
                else
                {
                    hud.DisplayText($"< {player.Name()} tried to open the big door, but it's sealed shut >");
                }
            }
        }

        // ----- public methods

        public void CheckForDoor(Player player, HUD hud, Toolkit toolkit)
        {
            if (aliveInWorld)
            {
                if (player.XYHolder()[0] == this.x)
                {
                    if (player.XYHolder()[1] == this.y)
                    {
                        OpenDoor(player, hud, toolkit);
                    }
                }
            }
        }
    }
}
