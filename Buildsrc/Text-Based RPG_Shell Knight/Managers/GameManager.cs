using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class GameManager
    {
        //State machine
        public enum GAMESTATE
        {
            GAMEOVER,
            MAP,
            CHANGEMAP
        }
        public GAMESTATE _gameState;

        readonly Toolkit toolkit = new Toolkit();
        
        // display init
        readonly Camera camera; // frames the map for gameplay
        readonly Map map; // gameworld for all game objects to be drawn on and saved to (player isn't saved on map)
        readonly HUD hud; // appears under camera to give player information

        // Character init
        readonly Player player; // controls the player
        List<Enemy> enemies; // List of Enemies in the current game world
        readonly EnemyManager manageEnemies;

        // Object init
        public enum OBJECTS //Object statemachine 
        {
        ITEM,
        DOOR
        };
        readonly ObjectManager[] manageObjects; // holds all object managers for easy reading
        List<Item> items; // Items in the current game world
        List<Door> doors; // Doors in the current game world

        // constructor
        public GameManager()
        {
            Console.CursorVisible = false;
            _gameState = GAMESTATE.CHANGEMAP;

            //init fields
            map = new Map();
            camera = new Camera(map);
            player = new Player(Global.PLAYER_DEFAULTNAME, Global.PLAYER_AVATAR);
            hud = new HUD(player.Name());
            enemies = new List<Enemy>();
            manageEnemies = new EnemyManager();

            //init object manager
            manageObjects = new ObjectManager[2];
            items = new List<Item>();
            manageObjects[(int)OBJECTS.ITEM] = new ItemManager();
            doors = new List<Door>();
            manageObjects[(int)OBJECTS.DOOR] = new DoorManager();
        }

        // ----- gets/sets
        public void GameOverPlayerDeath() // checks for player death and sets game over
        {
            if (!player.AliveInWorld()) { _gameState = GAMESTATE.GAMEOVER; }
            else { }
        } 
        public void GameOverWinCondition(int CharacterX, int CharacterY, HUD hud) // checks if win condition is met and sets game over
        { /// current win condition, reach the goal point
            if (CharacterX == Global.PLAYER_WINPOINT[0])
            {
                if (CharacterY == Global.PLAYER_WINPOINT[1])
                {
                    string victoryMessage;
                    victoryMessage = $"< {Global.MESSAGE_PLAYERVICTORY} > ";
                    hud.DisplayText(victoryMessage);
                    _gameState = GameManager.GAMESTATE.GAMEOVER;
                }
            }
        }

        // ----- Manager Methods
        public void ChangeDisplay()// updates display screen to represent the selected Map // set up for multiple map files
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
            enemies = manageEnemies.Init(map.getEnemyHold().Split('|'));
            items = (manageObjects[(int)OBJECTS.ITEM] as ItemManager).Init(map.getItemHold().Split('|'));
            doors = (manageObjects[(int)OBJECTS.DOOR] as DoorManager).Init(map.getDoorHold().Split('|'));


            Draw();
            hud.Update(player, items);
        }
        public void Draw()
        {
            //draw gameplay elements to gameworld, Heirarchy: lowest is on top
            (manageObjects[(int)OBJECTS.DOOR] as DoorManager).Draw(doors, camera);
            (manageObjects[(int)OBJECTS.ITEM] as ItemManager).Draw(items, camera);
            manageEnemies.Draw(enemies, camera);
            
            player.Draw(camera);

            // draw GameWorld
            camera.Draw(player);
        }
        public void Update()
        {
            
            //update by prioity
            hud.Update(player, items);
            player.Update(enemies, doors, map, camera, items, hud, toolkit);
            (manageObjects[(int)OBJECTS.ITEM] as ItemManager).Update(items, player, hud);
            manageEnemies.Update(enemies, player, map, camera, toolkit, hud);

            hud.Update(player, items);
            camera.Update(); // updated last to catch all character and object updates on gameworld
        }
        public void GameOver()
        {
            hud.DisplayText(Global.MESSAGE_GAMEOVER);
        }
        public void FixDisplay()
        {
            camera.FixModifyedConsole(hud, toolkit); // checks for console size change and starts handling it
            Console.CursorVisible = false;
        }

        // ----- Game loop
        public void Game()
        {
            // check for gameover
            GameOverWinCondition(player.X(), player.Y(), hud);
            GameOverPlayerDeath();

            // game loop
            switch (_gameState)
            {
                case GAMESTATE.CHANGEMAP:

                    //update what the screen displays
                    ChangeDisplay();

                    //run the map changed to
                    _gameState = GAMESTATE.MAP;
                    break;
                case GAMESTATE.MAP:

                    // fixes display if Console size changes
                    FixDisplay();

                    //gameplay
                    while (Console.KeyAvailable)
                    {   // progress loop
                        Update();
                        Draw();
                    }
                    break;
                case GAMESTATE.GAMEOVER:

                    // display gameover
                    GameOver();
                    break;
            }
        }
    }
}
