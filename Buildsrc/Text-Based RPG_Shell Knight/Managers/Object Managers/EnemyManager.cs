using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class EnemyManager:ObjectManager
    {
        public List<Enemy> Init(string[] enemyInfo) // init enemy group in a for loop
        {
            List<Enemy> enemies = new List<Enemy>();

            for (int i = 0; i < enemyInfo.Length; i++)
            {
                Enemy listObject = new Enemy(enemyInfo[i]);
                enemies.Add(listObject);
            }

            return enemies;
        }
        public void Draw(List<Enemy> enemies, Camera camera) // draw all enemies in list in a for loop
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(camera);
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
