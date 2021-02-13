using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Program
    {
        static bool turnOffCursor = false;
        static void Main(string[] args)
        {

            Console.CursorVisible = turnOffCursor; 
            MapMANAGE MapMAN = new MapMANAGE();
            Player player = new Player("John Smith", '@');
            Enemy enemy1 = new Enemy("enemy1", '#', 0);
            Enemy enemy2 = new Enemy("enemy2", '#', 1);
            bool gameover = false;
            //MapMAN.CreateWindowBorder();

                MapMAN.SetCurrentMap();
            while (gameover == false)
            {
                ///map
                //MapMAN.DrawWindowBorder();
                MapMAN.DrawCurrentMap();
                ///player turn
                player.Draw();
                enemy1.Draw();
                player.Update();
                //MapMAN.checkPosition(player.getAxisX(), player.getAxisY()); // debug
                player.CheckForWall(MapMAN.getdisplayMap(player.getAxisX(), player.getAxisY()), MapMAN.getwallHold());
                ///enemy turn
                enemy1.AttackedifAlive(player.attack(enemy1.getAxisX(), enemy1.getAxisY()));
                Console.ReadKey(true);
                enemy1.idleMove();
                player.AttackedifAlive(enemy1.attack(player.getAxisX(), player.getAxisY()));
                //checks if player is alive
                if (player.getAliveStatus() == false) {
                    gameover = true;
                }
            }
        }
    }
}
