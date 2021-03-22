using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <ItemAvatars>
/// UPDATE IN LEGEND!!!
/// 
///  k - Small key, unlocks a small door <d>
///  
///  K - Big key, unlocks a big door <D>
///  
///  ö - healingpot, heals 50 HP
///  
///  ï - shellbanding, heals 50 SP
/// </icons used to display Items, and discriptions of what they should do>


namespace Text_Based_RPG_Shell_Knight
{
    class Item : Object
    {
        private bool _pickedUpByPlayer;
        private int _power; // value the item contains

        // constructor
        public Item(string itemInfo)
        {
            ReadItemInfo(itemInfo);

            //constant
            _pickedUpByPlayer = false;
            aliveInWorld = true;
        }
        
        // readInfo 
        public void ReadItemInfo(string itemInfo)
        {
            //recognize item info
            string[] avatarAndPos = itemInfo.Split(':');
            string[] recognizedItem = RecognizeInfo(itemInfo[0]).Split(';'); ;

            //set item fields
            _avatar = itemInfo[0];
            _name = recognizedItem[0];
            if (recognizedItem.Length > 1) { _power = Int32.Parse(recognizedItem[1]); }

            //set position
            string[] posHold = avatarAndPos[1].Split(',');
            int[] posXY = new int[2];
            for (int i = 0; i < posHold.Length; i++)
            { posXY[i] = Int32.Parse(posHold[i]); }
            x = posXY[0];
            y = posXY[1];

            
        }
        private string RecognizeInfo(char identity) ///holds extra Info for Items found in ^ItemAvatars^
        {
            //1 name
            //2 power
            //

            string identifyed = "";
            if (identity == 'ö') ///ö   - Health Potion / health +50 /
            {
                identifyed += "Health Potion;";
                identifyed += "50";
            } 
            else if (identity == 'ï') /// ï   - Shell Banding / shield +30 /
            {
                identifyed += "Shell Banding;";
                identifyed += "30";
            }
            else if (identity == 'k') /// k   - Small Key / opens small doors /
            {
                identifyed += "Small Key";
            }
            else if (identity == 'K') /// K   - Big Key / opens big doors /
            {
                identifyed += "Big Key";
            }
            return identifyed; 
        }
        
        // gets
        public int Power(char identity) 
        {
            string[] itemInfo = RecognizeInfo(identity).Split(';');
            _power = Int32.Parse(itemInfo[1]); // reads one because 0 is name and 1 is power
            return _power; 
        }
        public bool PickedUp() { return _pickedUpByPlayer; }
        public void PickedUp(bool value) { _pickedUpByPlayer = value; }

        // private methods
        private bool PickUpItem(Player player, List<Item> items, Toolkit toolkit, HUD ui)
        {
            if (aliveInWorld)
            {
                if (x == player.X())
                {
                    if (y == player.Y())
                    {
                        aliveInWorld = false; _pickedUpByPlayer = true; //removes sprite from stage and tells game to add it in HUD
                        ui.AdjustInvetory(items);
                        ui.Draw(toolkit);
                        ui.DisplayText($"< {player.Name()} picked up {_name} [{_avatar}] >", false);
                        return true;
                    }
                }
            }
            return false;
        }

        // public methods
        public void Update(Player player, List<Item> items, Toolkit toolkit, HUD ui)
        {
            PickUpItem(player, items, toolkit, ui);
        }
    }
}
