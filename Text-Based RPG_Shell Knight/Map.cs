using System;
using System.IO;
using System.Collections.Generic;

namespace Text_Based_RPG_Shell_Knight
{
    class Map
    {
        static int MapDisplay_X = Console.WindowWidth+8;
        static int MapDisplay_Y = Console.WindowHeight-2;
        
        public List<string> wallHold = new List<string>();

        public List<string> enemyHold = new List<string>();

        // Map
        private char[,] displayMap = new char[MapDisplay_X, MapDisplay_Y];

        // border
        private string borderString = "";
        private string[,] borderMap;

        // ----- constructor
        public Map()
        {

        }

        // ----- gets & manager tools
        public char[] getTile(Toolkit toolkit, string _name, int x = 2, int y = 2) 
        {
            //toolkit.DisplayText($"checking {_name}");
            char[] directions = new char[5];
            directions[0] = displayMap[x, y];
            if (y > 0)
            {
                directions[1] = displayMap[x, y - 1];
            }
            else {
                directions[1] = displayMap[x, y];
            }
            directions[2] = displayMap[x+1, y];
            directions[3] = displayMap[x, y+1];
            if (x > 0)
            {
                directions[4] = displayMap[x-1, y];
            }
            else
            {
                directions[1] = displayMap[x, y];
            }
            return directions;
        }
        public string getWallHold()
        { 
            string allwalls = string.Join("", wallHold);
            return allwalls;
        }
        public string[] getEnemyHold()
        {
            string[] allenemies = new string[enemyHold.Count];
            for (int i = 0; i < enemyHold.Count; i++)
            { allenemies[i] = enemyHold[i]; }
            return allenemies;
        }
        public void openDoor()
        {
            string[] allwalls = wallHold.ToArray();
            for (int i = 0; i < wallHold.Count; i++)
            {
                wallHold.Remove(allwalls[i]);
            }
            for (int i = 0; i < allwalls.Length; i++)
                if (allwalls[i] != "D")
                {
                    wallHold.Add(allwalls[i]);
                }
            

        }

        // ----- public methods
        public void checkPosition(int x, int y, string[] enemyInfo) { // debug
            string allWalls = string.Join("", wallHold);
            Console.SetCursorPosition(1, 0);
            Console.Write($"selected map tile: [{displayMap[x, y]}] walls available: [{allWalls}] enemy check: [{enemyInfo[2]}]"); // --- debug
            Console.ReadKey(true);
        } //debug
        /// legacy code
        //public bool isWall(int y, int x ) {
        //    string allWalls = string.Join("", wallHold);
        //    string[] wallgroup = allWalls.Split();
        //    for (int i = 0; i < wallgroup.Length; i++) {
        //        if (displayMap[x, y] == wallgroup[i]) {
        //            return true;
        //        }
        //        else { } // do nothing
        //    }
        //    return false;

        

        // ----- Manager Builders
        public void CreateWindowBorder() // walls ═ ║ 
        {
            borderMap = new string[Console.WindowWidth, Console.WindowHeight];

            Console.SetCursorPosition(0, 0);
            borderString += "╔,"; //Console.Write("╔"); 
            for (int i = 0; i < Console.WindowWidth -2; i++)
            {
                borderString += "═,"; //Console.Write("═"); 
            }
            borderString += "╗;"; //Console.Write("╗"); 
            for (int i = 0; i < Console.WindowHeight - 4; i++)
            {
                borderString += "║,"; //Console.Write("║"); 
                for (int j = 0; j < Console.WindowWidth - 2; j++)
                {
                    borderString += " ,"; //Console.Write(" "); 
                }
                borderString += "║;"; //Console.Write("║"); 
            }
            borderString += "╚,"; //Console.Write("╚"); 
            for (int i = 0; i < Console.WindowWidth -2; i++)
            {
                borderString += "═,"; //Console.Write("═"); 
            }
            borderString += "╝"; //Console.Write("╝");

            string[] borderY = borderString.Split(';');
            
            for (int i = 0; i < borderY.Length; i++) 
            {
                string[] borderX = borderY[i].Split(','); // used for .Length and to Map the Xcoordinates on 
                for (int j = 0; j < borderX.Length; j++)
                {
                    borderMap[j, i] = borderX[j];
                }
            }

        }
        public void DrawWindowBorder() 
        {
            string[] borderY = borderString.Split(';');
            string[] borderXLength = borderY[0].Split(','); //used for loop length

            //Console.WriteLine($"x: {MapXLength.Length} y: {MapY.Length}");   //debug
            //Console.WriteLine($"Map 0,0: [{borderMap[0, 0]}]"); // --- /debug/


            Console.SetCursorPosition(0, 1);
            for (int i = 0; i < borderY.Length; i++) // nested loop to write window border
            {
                for (int j = 0; j < borderXLength.Length; j++)
                {
                    Console.Write(borderMap[j, i]);                
                }
            }
            //for (int i = 0; i <= borderY.Length; i++)
            //{
            //    Console.Write(borderY[i]);
            //}
            Console.SetCursorPosition(1, 1);
        }
        public void SetCurrent()//(currently gets Map_test for prototype))
        {
            wallHold.Remove(getWallHold());
            string[] removingHold = enemyHold.ToArray();
            for (int i = 0; i < enemyHold.Count; i++)
            { 
                enemyHold.Remove(removingHold[i]);
            }
            string[] MapY = File.ReadAllLines("Map_test.txt");// this changes to string input for Maps is _test for now
            if (!File.Exists("Map_test.txt")) {// input same string
                throw new Exception("File path does not Exist");
            }

            ///read map info
            string info = MapY[0];// line 0 passes info
            string[] readInfo = info.Split(';');
            string walls = readInfo[0]; //value 0 of info is walls
            string[] enemyInfo = readInfo[1].ToString().Split('.');
            string[] enemies = new string[enemyInfo.Length];
            for (int i = 0; i < (enemyInfo.Length); i++)
            {
                enemies[i] = enemyInfo[i];
            }

            ///read map
            for (int y = 1; y < MapDisplay_Y; y++){ // starts at 1 because line 0 is to pass information
                char[] MapX = new char[MapDisplay_X];
                string xHolder =  MapY[y];
                for (int i = 0; i < xHolder.Length; i++)
                {
                    MapX[i] = xHolder[i];
                }
                for (int x = 0; x < MapDisplay_X; x++) {
                    //create array
                    displayMap[x, y] = MapX[x];
                }
            }
            wallHold.Add(walls);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemyHold.Add(enemies[i]);
            }
        }
        public void DrawCurrent() 
        {
            //Console.SetCursorPosition(0,0);
            //Console.Write(displayMap[0,0]);
            int y;
            int x;
            Console.SetCursorPosition(0, 1);
            for (y = 1; y < MapDisplay_Y; y++) { // line 0 is to pass info,  
                for (x = 0; x < MapDisplay_X; x++) {
                    Console.Write(displayMap[x, y]);
                }
            }
        } 

    }
}
