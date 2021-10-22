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
        INVENTORY,
        TRADING
    }
    class GameManager
    {
        public GAMESTATE _gameState;

        // display init
        readonly Camera camera; // frames the map for gameplay
        readonly Map map; // gameworld for all game objects to be drawn on and saved to (player isn't saved on map)
        readonly HUD hud; // appears under camera to give player information
        readonly Inventory inventory; // changes game state to interactable inventory
        readonly Battle battle;
        readonly TradeMenu tradeMenu;

        // Data init
        readonly DataManager dataManager;

        // Character init
        readonly Player player; // controls the player
        List<Enemy> enemies; // List of Enemies in the current game world
        readonly EnemyManager manageEnemies;

        List<Vendor> vendors;
        readonly VendorManager manageVendors;

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
            dataManager = new DataManager();

            map = new Map();
            camera = new Camera();
            battle = new Battle();
            tradeMenu = new TradeMenu();
            player = new Player(Global.PLAYER_DEFAULTNAME, Global.PLAYER_AVATAR);
            inventory = new Inventory(player);
            hud = new HUD(player.Name());
            enemies = new List<Enemy>();
            manageEnemies = new EnemyManager();

            vendors = new List<Vendor>();
            manageVendors = new VendorManager();

            //init object manager
            manageObjects = new ObjectManager[2];
            items = new List<Item>();
            manageObjects[(int)OBJECTS.ITEM] = new ItemManager();
            doors = new List<Door>();
            manageObjects[(int)OBJECTS.DOOR] = new DoorManager();
        }

        // ----- Manager Methods
        private static GAMESTATE GameOverPlayerDeath(Player player, GAMESTATE gameState) // checks for player death and sets game over
        {
            if (!player.AliveInWorld()) { gameState = GAMESTATE.GAMEOVER; }
            else { }
            return gameState;
        }
        private static GAMESTATE GameOverWinCondition(Player player, HUD hud, GAMESTATE gameState) // checks if win condition is met and sets game over
        { /// current win condition, reach the goal point
            if (player.X() == Global.PLAYER_WINPOINT[0])
            {
                if (player.Y() == Global.PLAYER_WINPOINT[1])
                {
                    string victoryMessage;
                    victoryMessage = $"< {Global.MESSAGE_PLAYERVICTORY} > ";
                    hud.DisplayText(victoryMessage);
                    gameState = GAMESTATE.GAMEOVER;
                }
            }
            return gameState;
        }
        private void GameOverMessage()
        {
            hud.DisplayText(Global.MESSAGE_GAMEOVER);
        }

        // ----- Game loop
        public void Game()
        {
            Toolkit.FixDisplay(camera, hud);   

            // check for gameover
            GameOverPlayerDeath(player, _gameState);
            GameOverWinCondition(player, hud, _gameState);

            // game loop
            switch (_gameState)
            {
                //update what the screen displays
                case GAMESTATE.CHANGEMAP:
                    #region Change Map
                    //clear console & objects
                    Console.Clear();
                    items.Clear();
                    enemies.Clear();
                    doors.Clear();
                    vendors.Clear();
                    //read new map info/ load it into the camera
                    map.loadMap();
                    camera.GameWorldGetMap(map);
                    //create camera border /reload TextBox(change to create new hud for every map)
                    camera.DrawBorder();
                    hud.DrawTextBox();
                    // creating object managers
                    enemies = manageEnemies.Init(map.getEnemyHold().Split('|'));
                    items = (manageObjects[(int)OBJECTS.ITEM] as ItemManager).Init(map.getItemHold().Split('|'));
                    doors = (manageObjects[(int)OBJECTS.DOOR] as DoorManager).Init(map.getDoorHold().Split('|'));
                    vendors = manageVendors.Init(map.getVendorHold().Split('|'));

                    //change state to display the freshly loaded map & objects
                    _gameState = GAMESTATE.MAP;
                    #endregion
                    break;
                
                case GAMESTATE.MAP:
                    #region Play Map
                    //DRAW
                    //gameplay elements to gameworld, Heirarchy: bottomlayer is on top
                    (manageObjects[(int)OBJECTS.ITEM] as ItemManager).Draw(items, camera);
                    (manageObjects[(int)OBJECTS.DOOR] as DoorManager).Draw(doors, camera);
                    manageEnemies.Draw(enemies, camera);
                    manageVendors.Draw(vendors, camera);
                    player.Draw(camera);
                    //GameWorld / including objects added above
                    camera.Draw(player);

                    //UPDATE
                    //by prioity, Heirarchy: highest is on top
                    hud.Update(player, inventory);
                    _gameState = player.Update(enemies, doors, items, map, camera, hud, battle, inventory, _gameState, vendors, tradeMenu);
                    _gameState = manageEnemies.Update(enemies, player, map, camera, hud, battle, inventory, _gameState);
                    _gameState = manageVendors.Update(vendors, player, map, camera, hud, battle, inventory, _gameState);
                    (manageObjects[(int)OBJECTS.ITEM] as ItemManager).Update(items, player, inventory, hud);
                    camera.Update(map); // updated last to catch all character and object updates on gameworld
                    #endregion
                    break;
                
                case GAMESTATE.BATTLE:
                    //DRAW
                    battle.Draw();
                    //UPDATE
                    _gameState = battle.Update(player, _gameState, items[0], inventory);
                    break;
                
                case GAMESTATE.INVENTORY:
                    //DRAW
                    inventory.Draw();
                    //UPDATE
                    _gameState = inventory.Update(_gameState, player, items[0], inventory, tradeMenu);
                    break;
                
                case GAMESTATE.GAMEOVER:
                    GameOverMessage();
                    break;
                case GAMESTATE.TRADING:
                    //tradeMenu.Draw();
                    _gameState = tradeMenu.Update(player, _gameState, items[0], inventory);
                    break;

            }
        }
    }
}
