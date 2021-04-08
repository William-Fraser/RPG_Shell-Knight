using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class ItemManager:ObjectManager
    {

        public List<Item> Init(string[] itemInfo) // init of items in a for loop
        {
            List<Item> items = new List<Item>();

            for (int i = 0; i < itemInfo.Length; i++)
            {
                Item listObject = new Item(itemInfo[i]);
                items.Add(listObject);
            }

            return items;
        }
        public void Draw(List<Item> items, Camera camera)// draw a list of items with a for loop
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw(camera);
            }
        }
        public void Update(List<Item> items, Player player, HUD hud) // update a list of items with a for loop
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update(player, items, hud);
            }
        }
    }
}
