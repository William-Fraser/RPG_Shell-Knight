using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class GameManager
    {
        private int _gameState = GAMESTATE_CHANGEMAP;
        private static int GAMESTATE_GAMEOVER = 0;
        private static int GAMESTATE_MAP = 1;
        private static int GAMESTATE_CHANGEMAP = 2;
        private static int GAMESTATE_BATTLE = 3;

        static int CONSOLE_HEIGHT;
        static int CONSOLE_WIDTH;

        Toolkit toolkit = new Toolkit();
        
        Map map = new Map();
        UI ui = new UI();
        Player player = new Player("John Smith", '@');
        Item item = new Item("Key", 'k');
        Enemy enemy;

        // constructor
        public GameManager()
        {
            toolkit.SetConsoleSize();
            Console.CursorVisible = false;
            map.CreateWindowBorder();

            CONSOLE_HEIGHT = Console.WindowHeight;
            CONSOLE_WIDTH = Console.WindowWidth;
        }

        // ----- gets/sets
        public int getGameState()
        {
            return _gameState;
        }
        public void setGameState(int GAMESTATE_)
        {
            _gameState = GAMESTATE_;
        }
        public void setGameOver()
        {
            if (player.getAlive() == false) { setGameState(GAMESTATE_GAMEOVER); }
            else { }
        }

        // ----- Manager Methods
        public void UpdateDisplay()
        {
            map.SetCurrent();
            enemy = new Enemy(map.getEnemyHold());
            map.DrawWindowBorder();
            ui.getHUDvalues(player.getHealth());
            Draw();
        }
        public void Draw()
        {
            ui.HUD();
            map.DrawCurrent();
            item.Draw();
            enemy.Draw();
            player.Draw();
        }
        public void Update()
        {
            //map.checkPosition( 0, 0, map.getEnemyHold());
            player.Update(enemy.X(), enemy.Y(), map.getTile(toolkit, player.getName(), player.X(), player.Y()), map.getWallHold(), enemy.getAlive(), enemy.getDamage(player.X(),player.Y()), ui);
            player.GetKey(item.pickupItem(player.X(), player.Y()));
            if (player.GethasKey())
            {
                map.openDoor();
            }
            enemy.Update(player.X(), player.Y(), map.getTile(toolkit, player.getName(), enemy.X(), enemy.Y()), map.getWallHold(), player.getAlive(), player.getDamage(), ui);
            ui.getHUDvalues(player.getHealth());
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
                
            }
        }
    }
}
