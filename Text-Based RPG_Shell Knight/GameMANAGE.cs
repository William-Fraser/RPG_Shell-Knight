using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class GameMANAGE
    {
        public GameMANAGE()
        {
            Console.CursorVisible = false;
        }
        
        MapMANAGE MapMAN = new MapMANAGE();
        Player player = new Player("John Smith", '@');
        Enemy enemy1 = new Enemy("enemy1", '#', 0);
        Enemy enemy2 = new Enemy("enemy2", '#', 1);

        //MapMAN.CreateWindowBorder();


        public void GameplayMap()
        {


            MapMAN.SetCurrentMap(); 
            //MapMAN.DrawWindowBorder();

            MapMAN.DrawCurrentMap();
            player.Draw();
            enemy1.Draw();
            
            player.Update();
            //MapMAN.checkPosition(player.getAxisX(), player.getAxisY()); // debug
            player.CheckForWall(MapMAN.getdisplayMapTile(player.getAxisX(), player.getAxisY()), MapMAN.getwallHold());
            enemy1.AttackedifAlive(player.attack(enemy1.getAxisX(), enemy1.getAxisY()));
            enemy2.AttackedifAlive(player.attack(enemy1.getAxisX(), enemy1.getAxisY()));
            //Console.ReadKey(true); // debug
            enemy1.idleMove();
            enemy2.idleMove();
            player.AttackedifAlive(enemy1.attack(player.getAxisX(), player.getAxisY()));

        }
        
    }
}
