using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class ListManager
    {

        /// <HandleList>
        /// Enemy
        /// Item
        /// Door
        /// 
        /// <HandleList/itemizes the gameobjects that require ListManager>
        public List<Item> items;
        public List<Enemy> enemies;
        public List<Door> doors;

        // ----- private methods
        // needs to be up to date with HandleList
        private bool IdetifyType<T>(string checkFor) // true if checkFor == Type / cleaner than checking type of every time
        {
            bool isTypeof = false;
            if (typeof(T) == typeof(Item) && checkFor == "Item")
            {
                isTypeof = true;
            }
            else if (typeof(T) == typeof(Enemy) && checkFor == "Enemy")
            {
                isTypeof = true;
            }
            else if (typeof(T) == typeof(Door) && checkFor == "Door")
            {
                isTypeof = true;
            }
            return isTypeof;

        }
        private void SuperInit<T>(string[] objectInfo, Map map, string identifyer) // initializes the Lists according to their Type
        {
            //create Typeof List
            if (IdetifyType<Item>(identifyer))
            {
                items = new List<Item>();
            }
            else if (IdetifyType<Enemy>(identifyer))
            {
                enemies = new List<Enemy>();
            }
            else if (IdetifyType<Door>(identifyer))
            {
                doors = new List<Door>();
            }

            //Create list of Type
            for (int i = 0; i < objectInfo.Length; i++)
            {
                if (IdetifyType<Item>(identifyer))
                {
                    Item listObject = new Item(objectInfo[i]);
                    items.Add(listObject);
                }
                else if (IdetifyType<Enemy>(identifyer))
                {
                    Enemy listObject = new Enemy(objectInfo[i]);
                    enemies.Add(listObject);
                }
                else if (IdetifyType<Door>(identifyer))
                {
                    Door listObject = new Door(objectInfo[i]);
                    doors.Add(listObject);
                }
                else { }
            }

        }


        // ----- public methods

        //item

        //Door
        public List<Door> Init<T>(string itemInfo, Map map, Door identifyer)
        {
            
            SuperInit<Door>(itemInfo.Split('|'), map, "Door");

            return doors;
        }
        public void Draw(List<Door> doors, Camera camera, Door identifyer)
        {
            for (int i = 0; i < doors.Count; i++) 
            { 
                doors[i].Draw(camera); 
            }
        }
        public void Update(List<Door> doors, Player player, HUD hud, Toolkit toolkit, Door identifyer)
        {
            for (int i = 0; i < doors.Count; i++) 
            { 
                doors[i].CheckForDoor(player, hud, toolkit); 
            }
        }


        //enemy
        public List<Enemy> Init<T>(string enemyInfo, Map map, Enemy identifyer)
        {
            SuperInit<Enemy>(enemyInfo.Split('|'), map, "Enemy");
            return enemies;
        }
        public void Draw(List<Enemy> enemies, Camera camera, Enemy identifyer)
        {
            for (int i = 0; i < enemies.Count; i++) 
            { 
                enemies[i].Draw(camera); 
            }
        }
        public void Update(List<Enemy> enemies, Player player, Map map, Camera camera, Toolkit toolkit, HUD hud, Enemy identifyer, int state)
        {
            for (int i = 0; i < enemies.Count; i++) 
            { 
                enemies[i].Update(player, map, camera, hud, toolkit); 
            }
        }


        //item
        public List<Item> Init<T>(string itemInfo, Map map, Item identifyer)
        {

            SuperInit<Item>(itemInfo.Split('|'), map, "Item");
            return items;
        }
        public void Draw(List<Item> items , Camera camera, Item item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw(camera);
            }
        }
        public void Update(List<Item> items, Player player, Toolkit toolkit, HUD hud)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update(player, items, toolkit, hud);
            }
        }
    }
}
