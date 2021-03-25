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
        public static readonly int GAMESTATE_GAMEOVER = 0;
        public static readonly int GAMESTATE_MAP = 1;
        public static readonly int GAMESTATE_CHANGEMAP = 2;
        public static readonly int GAMESTATE_BATTLE = 3;


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

            //read new map info
            map.loadMap();
            hud.AdjustTextBox();
            camera.GameWorldGetMap();
            
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
            //draw characters/objects to gameworld, Heirarchy: lowest is on top

            for (int i = 0; i < items.Count; i++) { items[i].Draw(camera); }
            for (int i = 0; i < enemies.Count; i++) { enemies[i].Draw(camera); }
            player.Draw(map, camera, hud, toolkit);

            // draw GameWorld
            camera.Draw(player);
        }
        public void Update()
        {
            //update by prioity

            hud.Update(player, items);
            player.Update(enemies, map, camera, items, hud, toolkit);

            for (int i = 0; i < enemies.Count; i++) { enemies[i].Update(player, map, camera, hud, toolkit); }
            for (int i = 0; i < items.Count; i++) { items[i].Update(player, items, toolkit, hud); }

            camera.Update(player); // updated last to catch all character and object updates on gameworld
            
            setGameOver(); // check for gameover
        }
        public void GameOver()
        {
            hud.DisplayText(" > GAME OVER < ");
        }

        // ----- Game loop
        public void Game()
        {
            // fixes display if Console size changes
            camera.ResetConsole(hud); // checks for console size change and starts handling it

            // turns off cursor in gameplay
            if (Console.CursorVisible == true)
            { Console.CursorVisible = false; }

            // game loop
            while (_gameState != GAMESTATE_GAMEOVER)
            {
                if (_gameState == GAMESTATE_CHANGEMAP) // updates the map if the correct transition square is walked on
                {
                    //update what the screen displays
                    UpdateDisplay();

                    //run the map changed to
                    _gameState = GAMESTATE_MAP;
                }
                else if (_gameState == GAMESTATE_MAP) // playing the Map screen;
                {
                    //gameplay
                    Draw();
                    Update();
                }
                else { }
            }
            if (_gameState == GAMESTATE_GAMEOVER) // That's all folks
            {
                // display gameover
                GameOver();
            }
        }
    }
}
