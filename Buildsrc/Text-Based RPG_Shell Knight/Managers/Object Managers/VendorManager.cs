using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class VendorManager:ObjectManager
    {
        public List<Vendor> Init(string[] vendorInfo) // init enemy group in a for loop
        {
            List<Vendor> vendors = new List<Vendor>();

            for (int i = 0; i < vendorInfo.Length; i++)
            {
                Vendor listObject = new Vendor(vendorInfo[i]);
                vendors.Add(listObject);
            }

            return vendors;
        }
        public void Draw(List<Vendor> vendors, Camera camera) // draw all enemies in list in a for loop
        {
            for (int i = 0; i < vendors.Count; i++)
            {
                vendors[i].Draw(camera);
            }
        }
        public GAMESTATE Update(List<Vendor> vendors, Player player, Map map, Camera camera, HUD hud, Battle battle, Inventory inventory, GAMESTATE gameState) // update list of enemies in a for loop
        {
            for (int i = 0; i < vendors.Count; i++)
            {
                gameState = vendors[i].Update(player, map, camera, hud, battle, inventory, gameState);
            }
            return gameState;
        }
    }
}
