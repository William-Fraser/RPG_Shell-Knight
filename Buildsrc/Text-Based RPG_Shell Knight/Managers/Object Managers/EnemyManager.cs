using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class EnemyManager:ObjectManager
    {
        private Random rnd = new Random();
        public List<Enemy> Init(string[] enemyInfo, DataLoader dataLoader) // init enemy group in a for loop
        {
            List<Enemy> enemies = new List<Enemy>();

            for (int i = 0; i < enemyInfo.Length; i++)
            {
                Enemy listObject = new Enemy(enemyInfo[i], dataLoader);
                enemies.Add(listObject);
            }

            return enemies;
        }
        public void Draw(List<Enemy> enemies, Camera camera) // draw all enemies in list in a for loop
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(camera);
                if (enemies[i].Name() == "Spider") { enemies[i].currentMoney = rnd.Next(5, 20); }
                if (enemies[i].Name() == "Goblin") { enemies[i].currentMoney = rnd.Next(25, 55); }
                if (enemies[i].Name() == "Knight") { enemies[i].currentMoney = rnd.Next(60, 150); }
                if (enemies[i].Name() == "King") { enemies[i].currentMoney = rnd.Next(300, 2000); }
            }
        }
        public GAMESTATE Update(List<Enemy> enemies, Player player, Map map, Camera camera, HUD hud, Battle battle, Inventory inventory, GAMESTATE gameState) // update list of enemies in a for loop
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                gameState = enemies[i].Update(player, map, camera, hud, battle, inventory, gameState);
            }
            return gameState;
        }
    }
}
