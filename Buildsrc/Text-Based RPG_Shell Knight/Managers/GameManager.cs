using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    enum GAMESTATE
    {
        GAMEOVER,
        CHANGEMAP,
        MAP,
        BATTLE,
        INVENTORY
    }
    class GameManager
    {
        public GAMESTATE _gameState;

        //toolkit should change to static global tool
        readonly Toolkit toolkit = new Toolkit();
        
        // display init
        readonly Camera camera; // frames the map for gameplay
        readonly Map map; // gameworld for all game objects to be drawn on and saved to (player isn't saved on map)
        readonly HUD hud; // appears under camera to give player information
        readonly Inventory inventory; // changes game state to interactable inventory
        readonly Battle battle;

        // Character init
        readonly Player player; // controls the player
        List<Enemy> enemies; // List of Enemies in the current game world
        readonly EnemyManager manageEnemies;

        // Object init
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
            inventory = new Inventory();
            battle = new Battle();
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
                    _gameState = GAMESTATE.GAMEOVER;
                }
            }
        }

        // ----- Manager Methods
        public void ChangeDisplay()// updates display screen to represent the selected Map // set up for multiple map files
        {
            Console.Clear();
            //clear objects
            items.Clear();
            enemies.Clear();
            doors.Clear();

            //read new map info
            map.loadMap();
            hud.UpdateTextBox();
            camera.GameWorldGetMap();

            // adding gameplay elements to Display 
            enemies = manageEnemies.Init(map.getEnemyHold().Split('|'));
            items = (manageObjects[(int)OBJECTS.ITEM] as ItemManager).Init(map.getItemHold().Split('|'));
            doors = (manageObjects[(int)OBJECTS.DOOR] as DoorManager).Init(map.getDoorHold().Split('|'));


            Draw();
            hud.Update(player, inventory);
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
            camera.DrawBorder();
        }
        public void Update()
        {
            
            //update by prioity
            hud.Update(player, inventory);
            _gameState = player.Update(enemies, doors, map, camera, items, hud, toolkit, inventory, _gameState, battle);
            (manageObjects[(int)OBJECTS.ITEM] as ItemManager).Update(items, player, inventory, hud);
            _gameState = manageEnemies.Update(enemies, player, map, camera, toolkit, hud, battle, _gameState);
            
            hud.Update(player, inventory);
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
            FixDisplay();   

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

                    Draw();
                    //Main Gameplay Loop
                        Update();
                        Draw();
                    break;
                case GAMESTATE.BATTLE:

                    // Battle sequence
                    hud.UpdateHotBar(player, inventory);
                    hud.Draw();
                    battle.drawBorder();
                    _gameState = battle.BattleController(player,  hud, toolkit, _gameState, items[0], inventory); // item is passed to use power method

                    break;
                case GAMESTATE.INVENTORY:

                    //Invetory Menu
                    hud.UpdateHotBar(player, inventory);
                    hud.Draw();
                    inventory.Draw();
                    inventory.Update(player, hud);
                    _gameState = inventory.Navigate(_gameState, player, items[0]);

                    break;
                case GAMESTATE.GAMEOVER:

                    // display gameover
                    GameOver();
                    break;
            }
           
        }
    }
}
