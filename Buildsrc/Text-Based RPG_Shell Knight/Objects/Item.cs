using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <ItemAvatars>
/// UPDATE LEGEND
///
///  k - Small key,	   / unlocks a small door <d>
///  
///  K - Big key,	   / unlocks a big door <D>
///  
///  ö - healingpot,   / heals 50 HP
///  
///  ï - shellbanding, / heals 35 SP
///
///  d - small door,   / unlocked by small key <k>
///
///  B - Big door,     / unlocked by Big key <K>
///
/// </icons used to display Items, and discriptions of what they should do>


namespace Text_Based_RPG_Shell_Knight
{
    enum ITEM// to be moved to global with other Item constants when Item class is updated //its important to note that if iterating through the actual number is off by 1
    {
        POTHEAL,
        POTSHELL,
        KEYBIG,
        KEYSMALL,
        TOTALITEMS // used to quantify enum
    };
    class Item : Object
    {
        private bool _pickedUpByPlayer;
        private int _power; // value the item contains

        // constructor
        public Item(string itemInfo, DataLoader dataLoader)
        {
            LoadItemInfo(itemInfo, dataLoader);

            _pickedUpByPlayer = false;
            aliveInWorld = true;
        }
        
        // readInfo 
        public void LoadItemInfo(string itemInfo, DataLoader dataLoader)
        {
            //recognize item info
            string[] identifyerAndPos = itemInfo.Split(':');
            string identity = identifyerAndPos[0];

            //set item fields
            int[] setPos = dataLoader.TryParseXYFromString(identifyerAndPos[1]);
            x = setPos[(int)AXIS.X];
            y = setPos[(int)AXIS.Y];

            _avatar = Global.ITEM_AVATAR(identity);
            _name = Global.ITEM_NAME(identity);
            //continue with all values if working submit?
        }
        /*private string RecognizeInfo(char identity) ///holds extra Info for Items found in ^ItemAvatars^
        {
            //1 name
            //2 power
            //

            string identifyed = "";
            if      (identity == Global.ITEM_AVATAR(ITEM.POTHEAL)) ///ö   - Health Potion / health +50 /
            {
                identifyed += Global.ITEM_NAME(ITEM.POTHEAL);
                identifyed += ";";
                identifyed += Global.ITEM_POWER(ITEM.POTHEAL);
            } 
            else if (identity == Global.ITEM_AVATAR(ITEM.POTSHELL)) /// ï   - Shell Banding / shield +30 /
            {
                identifyed += Global.ITEM_NAME(ITEM.POTSHELL);
                identifyed += ";";
                identifyed += Global.ITEM_POWER(ITEM.POTSHELL);
            }
            else if (identity == Global.ITEM_AVATAR(ITEM.KEYSMALL)) /// k   - Small Key / opens small doors /
            {
                identifyed += Global.ITEM_NAME(ITEM.KEYSMALL);
            }
            else if (identity == Global.ITEM_AVATAR(ITEM.KEYBIG)) /// K   - Big Key / opens big doors /
            {
                identifyed += Global.ITEM_NAME(ITEM.KEYBIG);
            }
            return identifyed; 
        }*/

        // gets
        public bool PickedUpByPlayer()
        {
            return _pickedUpByPlayer;
        }
        public void PickedUpByPlayer(bool value)
        {
            _pickedUpByPlayer = value;
        }
        public int Power(char identity) 
        {
            return _power; 
        }

        // private methods
        private void PickUpOnColl(Player player, List<Item> items, Inventory inventory, HUD hud )
        {
            if (x == player.X())
            {
                if (y == player.Y())
                {
                    this._pickedUpByPlayer = true;
                    aliveInWorld = false;
                    inventory.PickupFoundItems(player, items, hud);
                }
            }

        }

        // public methods
        public void Update(Player player, List<Item> items, Inventory inventory, HUD ui)
        {
            if (aliveInWorld)
            {
                PickUpOnColl(player, items, inventory, ui);
            }
        }
    }
}
