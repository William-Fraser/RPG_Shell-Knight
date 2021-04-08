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
        public void OpenDoor(Player player, HUD hud)
        {
            int[] stock = hud.InventoryStock();

            if (this._avatar == 'd')
            {
                if (stock[(int)HUD.ITEM.KEYSMALL] > 0)
                {

                    player.RemoveItemFromInventory((int)HUD.ITEM.KEYSMALL, hud);
                    hud.Draw(); // updates visible inventory
                    aliveInWorld = false;
                    hud.DisplayText($"< {player.Name()} {Global.MESSAGE_DOORSMALLOPEN} >");
                    _opened = true;
                }
                else
                {
                    hud.DisplayText($"<  { player.Name()} {Global.MESSAGE_DOORSMALLLOCKED} >", false);        
                }
            }
            if (this._avatar == 'D')
            {
                if (stock[(int)HUD.ITEM.KEYBIG] > 0)
                {
                    player.RemoveItemFromInventory((int)HUD.ITEM.KEYBIG, hud);
                    hud.Draw(); // updates visible inventory
                    aliveInWorld = false;
                    hud.DisplayText($"< {player.Name()} {Global.MESSAGE_DOORBIGOPEN} >");
                    _opened = true;
                }
                else
                {
                    hud.DisplayText($"< {player.Name()} {Global.MESSAGE_DOORBIGLOCKED} >");
                }
            }
        }
    }
}
