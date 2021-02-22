using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class GameManager
    {
        Toolkit toolkit = new Toolkit();
        static int CONSOLE_HEIGHT;
        static int CONSOLE_WIDTH;
        Map map = new Map();
        Player player = new Player("John Smith", '@');
        Enemy enemy1 = new Enemy("enemy1", '#', 0);
        Enemy enemy2 = new Enemy("enemy2", '#', 1);

        // constructor
        public GameManager()
        {
            toolkit.SetConsoleSize();
            Console.CursorVisible = false;
            //map.CreateWindowBorder();     <<<<<<<<<<<<  FIX THIS

            CONSOLE_HEIGHT = Console.WindowHeight;
            CONSOLE_WIDTH = Console.WindowWidth;
        }

        // Map game loop
        public bool Map()
        {
            map.SetCurrentMap(); 
            map.DrawWindowBorder();

            Draw();         
            
            player.Update();
            //MapMAN.checkPosition(player.getAxisX(), player.getAxisY()); // debug
            player.CheckForWall(map.getMap(player.getX(), player.getY()), map.getwallHold());
            enemy1.ChecktoTakeDamage(player.Attack(enemy1.getX(), enemy1.getY()), player.getDamage());
            enemy2.ChecktoTakeDamage(player.Attack(enemy1.getX(), enemy1.getY()), player.getDamage());
            enemy1.idleMove();
            enemy2.idleMove();
            player.ChecktoTakeDamage(enemy1.Attack(player.getX(), player.getY()), enemy1.getDamage()); // TAKE DAMAGE BETTER***********
            if (player.getAlive() == false) return false;
            else return true;
        }
        public void Draw()
        {
            map.DrawCurrentMap();
            player.Draw();
            enemy1.Draw();
            enemy2.Draw();
        }


    }
}
