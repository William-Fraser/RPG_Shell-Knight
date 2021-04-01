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
        
        ListManager list; // manages the lists below

        List<Item> items; // Items in the current game world
        Item identifyerItem;

        List<Enemy> enemies; // Enemies in the current game world
        Enemy identifyerEnemy;

        List<Door> doors;
        Door identifyerDoor;

        // constructor
        public GameManager()
        {
            Console.CursorVisible = false;
            _gameState = GAMESTATE_CHANGEMAP;

            map = new Map();
            camera = new Camera(map);
            player = new Player(Global.PLAYER_DEFAULTNAME, Global.PLAYER_AVATAR, hud);
            hud = new HUD(player.Name());

            list = new ListManager();
            items = new List<Item>();
            enemies = new List<Enemy>();
            doors = new List<Door>();
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
            doors.Clear();

            //read new map info
            map.loadMap();
            hud.AdjustTextBox();
            camera.GameWorldGetMap();

            // adding gameplay elements to Display 
            items = list.Init<Item>(map.getItemHold(), map, identifyerItem);
            enemies = list.Init<Enemy>(map.getEnemyHold(), map, identifyerEnemy);
            doors = list.Init<Door>(map.getDoorHold(), map, identifyerDoor);
            
            
            hud.Update(player, items);
            //sDraw();
        }
        public void Draw()
        {
            //draw gameplay elements to gameworld, Heirarchy: lowest is on top

            list.Draw(items, camera, identifyerItem);
            list.Draw(enemies, camera, identifyerEnemy);
            list.Draw(doors, camera, identifyerDoor);
            
            player.Draw(map, camera, hud, toolkit);

            // draw GameWorld
            camera.Draw(player);
        }
        public void Update()
        {
            //update by prioity

            hud.Update(player, items);
            player.Update(enemies, doors, map, camera, items, hud, toolkit);

            list.Update(items, player, toolkit, hud);
            list.Update(enemies, player, map, camera, toolkit, hud, identifyerEnemy, _gameState);
            //list.Update(doors, player, hud, toolkit, identifyerDoor);

            camera.Update(player); // updated last to catch all character and object updates on gameworld
            
            setGameOver(); // check for gameover
        }
        public void GameOver()
        {
            hud.DisplayText(Global.MESSAGE_GAMEOVER);
        }
        public void FixDisplay()
        {
            camera.ResetConsole(hud); // checks for console size change and starts handling it
            Console.CursorVisible = false;
        }



        // ----- Game loop
        public void Game()
        {
            // fixes display if Console size changes

            // turns off cursor in gameplay
            if (Console.CursorVisible == true)
            {  }

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
                    FixDisplay();

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
