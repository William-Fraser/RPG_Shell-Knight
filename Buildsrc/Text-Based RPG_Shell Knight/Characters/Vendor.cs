using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Vendor : Character

    {
        //different types of vendors
        public enum Type
        {
            BLACKSMITH,
            POTIONEER
        }
        public Type type;

        public Vendor(string vendorInfo, string name = "vendor", char avatar = '!') :base(name, avatar, 0)
        {
            readVendorInfo(vendorInfo);
        }
       
        private void readVendorInfo(string vendorInfo)
        {
            //recognize item info
            string[] avatarAndPos = vendorInfo.Split(':');
            string[] recognizedItem = RecognizeInfo(vendorInfo[0]).Split(';'); ;

            //set item fields
            _avatar = vendorInfo[0];
            _name = "Vendor";

            //set position
            string[] posHold = avatarAndPos[1].Split(',');
            int[] posXY = new int[2];
            for (int i = 0; i < posHold.Length; i++)
            { posXY[i] = Int32.Parse(posHold[i]); }
            x = posXY[0];
            y = posXY[1];
        }

        private string RecognizeInfo(char identity)
        {
            string identifyed = "";
            //identifies different shop types based on appearance
            if (identity == 'B') 
            {
                identifyed += "Black Smith;";
                type = Type.BLACKSMITH;
                aliveInWorld = true;

            }
            else if (identity == 'P') 
            {
                identifyed += "Potioneer;";
                type = Type.POTIONEER;
                aliveInWorld = true;

            }

            return identifyed;
        }

        public GAMESTATE Update(Player player, Map map, Camera camera, HUD hud, Battle battle, Inventory inventory, GAMESTATE gameState)
        {
            //Shop keeper have no AI, they are always stationary
            return gameState;
        }




    }
}
