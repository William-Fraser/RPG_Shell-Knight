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
        private static int GAMESTATE_GAMEOVER = 0;
        private static int GAMESTATE_MAP = 1;
        private static int GAMESTATE_BATTLE = 2;

        static int CONSOLE_HEIGHT;
        static int CONSOLE_WIDTH;

        Toolkit toolkit = new Toolkit();
        
        Map map = new Map();
        Player player = new Player("John Smith", '@');
        Enemy enemy1 = new Enemy("enemy1", '#', 0);
        Enemy enemy2 = new Enemy("enemy2", '#', 1);

        // constructor
        public GameManager()
        {
            toolkit.SetConsoleSize();
            Console.CursorVisible = false;
            //map.CreateWindowBorder();     //<<<<<<<<<<<<  FIX THIS

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
        // Manager Methods
        public void Draw()
        {
            map.DrawCurrentMap();
            enemy1.Draw();
            enemy2.Draw();
            player.Draw();
        }
        public void Update()
        {
            player.Update();
            //MapMAN.checkPosition(player.getAxisX(), player.getAxisY()); // debug
            player.CheckForWall(map.getMap(player.X(), player.Y()), map.getwallHold());

            enemy1.ChecktoTakeDamage(player.Attack(enemy1.X(), enemy1.Y()), player.getDamage());
            enemy2.ChecktoTakeDamage(player.Attack(enemy1.X(), enemy1.Y()), player.getDamage());
            enemy1.MoveChasePlayer(player.X(), player.Y());
            enemy2.MoveChasePlayer(player.X(), player.Y());
            player.ChecktoTakeDamage(enemy1.Attack(player.X(), player.Y()), enemy1.getDamage());
        }

        // game loop
        public void Game()
        {
            _gameState = GAMESTATE_MAP;
            if (_gameState == GAMESTATE_MAP)
            {
                map.SetCurrentMap();
                //map.DrawWindowBorder();

                Draw();

                if (player.getAlive() == false) { setGameState(GAMESTATE_GAMEOVER); }
                else { }
            }
        }
    }
}
