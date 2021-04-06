using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class GameManager
    {
        public enum GAMESTATE
        {
            GAMEOVER,
            MAP,
            CHANGEMAP,
            BATTLE
        }
        public GAMESTATE _gameState;

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
            _gameState = GAMESTATE.CHANGEMAP;

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
        public int GameState() { return (int)_gameState; }
        public void GameOverPlayerDeath() // checks for player death and sets game over
        {
            if (player.AliveInWorld() == false) { _gameState = GAMESTATE.GAMEOVER; }
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
            list.Update(enemies, player, map, camera, toolkit, hud, identifyerEnemy, 0); // 

            camera.Update(player); // updated last to catch all character and object updates on gameworld

            GameOverWinCondition(player.X(), player.Y(), hud);
            GameOverPlayerDeath(); // check for gameover
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
            while (_gameState != GAMESTATE.GAMEOVER)
            {
                if (_gameState == GAMESTATE.CHANGEMAP) // updates the map if the correct transition square is walked on
                {
                    //update what the screen displays
                    UpdateDisplay();

                    //run the map changed to
                    _gameState = GAMESTATE.MAP;
                }
                else if (_gameState == GAMESTATE.MAP) // playing the Map screen;
                {
                    FixDisplay();

                    //gameplay
                    Draw();
                    Update();
                }
                else { }
            }
            if (_gameState == GAMESTATE.GAMEOVER) // That's all folks
            {
                // display gameover
                GameOver();
            }
        }
    }
}
