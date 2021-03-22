using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class GameManager
    {
        private int _gameState;
        private static readonly int GAMESTATE_GAMEOVER = 0;
        private static readonly int GAMESTATE_MAP = 1;
        private static readonly int GAMESTATE_CHANGEMAP = 2;
        private static readonly int GAMESTATE_BATTLE = 3;


        Toolkit toolkit = new Toolkit();

        Camera camera; // frames the map for gameplay

        Map map; // gameworld for all game objects to be drawn on and saved to (player isn't saved on map)
        
        HUD hud; // appears under camera to give player information
        
        Player player; // controls the player
        
        List<Item> items = new List<Item>(); // Items in the current game world
        
        List<Enemy> enemies = new List<Enemy>(); // Enemies in the current game world

        // constructor
        public GameManager()
        {
            Console.CursorVisible = false;
            _gameState = GAMESTATE_CHANGEMAP;

            map = new Map();
            camera = new Camera(map);
            player = new Player("John Smith", '@', hud);
            hud = new HUD(player.Name());
            camera.AdjustDisplayedArea(player);
            camera.UpdateWindowBorder();
        }

        // ----- gets/sets
        public int GameState() { return _gameState; }
        public void setGameOver()
        {
            if (player.AliveInWorld() == false) { _gameState = GAMESTATE_GAMEOVER; }
            else { }
        } //reads players state and set's game to gameover

        // ----- Manager Methods
        public void UpdateDisplay()// updates display screen to represent the selected Map
        {
            //clear objects
            items.Clear();
            enemies.Clear();

            //set Current Display Map
            map.loadMap();
            
            // adding items to Display
            string[] itemInfo = map.getItemHold().Split('|');
            for (int i = 0; i < itemInfo.Length; i++)
            {
                Item item= new Item(itemInfo[i]);
                items.Add(item);
            }

            // adding enemies to Display
            string[] enemyInfo = map.getEnemyHold().Split('|');
            for (int i = 0; i < enemyInfo.Length; i++) 
            {
                Enemy enemy = new Enemy(enemyInfo[i]);
                enemies.Add(enemy);
            }
            
            hud.Update(player, items);
            Draw();
        }
        public void Draw()
        {
            camera.ResetDisplay(hud);
            camera.AdjustDisplayedArea(player);
            camera.UpdateWindowBorder();
            camera.DrawWindowBorder(); // border

            // create replacement gameworld
            map.loadMap();
            camera.GameWorldGetMap();

            //draw characters, lowest is on top
            for (int i = 0; i < enemies.Count; i++) { enemies[i].Draw(camera); }
            for (int i = 0; i < items.Count; i++) { items[i].Draw(camera); }
            player.Draw(map, camera, hud, toolkit);

            // draw GameWorld
            camera.Draw(player);
        }
        public void Update()
        {
            //update by prioity

            //map.checkPosition( 0, 0, map.getEnemyHold()); -- debug
            player.Update(enemies, map, camera, items, hud, toolkit);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(player, map, camera, hud, toolkit);
            }
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update(player, items, toolkit, hud);
            }
            hud.Update(player, items);
        }

        // ----- game loop
        public void Game()
        {
            
            if (_gameState == GAMESTATE_CHANGEMAP)
            {
                UpdateDisplay();
                _gameState = GAMESTATE_MAP;
            }
            else if (_gameState == GAMESTATE_MAP)
            {
                Update();
                Draw();
                
                setGameOver();
            }
            else if (_gameState == GAMESTATE_GAMEOVER)
            {
                hud.DisplayText(" > GAME OVER < ", false);
                Console.ReadKey(true);
            }
        }
    }
}
