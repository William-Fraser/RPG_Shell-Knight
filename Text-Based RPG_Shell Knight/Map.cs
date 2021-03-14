using System;
using System.IO;
using System.Collections.Generic;

namespace Text_Based_RPG_Shell_Knight
{
    class Map
    {
        // Map File Lines/Info Readers
        string[] allRowsAndInfo = File.ReadAllLines("Map_test.txt");// this changes to string constant is _test for now
        string[] allRows;
        char[] row;

        // height and width of the Map
        static int height;
        static int width;
        
        // lists containing Map Information for the game system to read
        public List<string> wallHold = new List<string>();
        public List<string> enemyHold = new List<string>();

        // Map
        private char[,] map;

        // border -- part of the HUD, HUD perhaps should be renamed to Camera?
        private string borderString = "";
        private string[,] borderArray;

        // ----- constructor
        public Map()
        {
            //init length of rows and columns
            allRows = new string[allRowsAndInfo.Length - 1]; // -1 removing Info
            for (int i = 0; i < allRowsAndInfo.Length - 1; i++) {
                allRows[i] = allRowsAndInfo[i + 1];
            }
            
            row = new char[allRows[0].Length];
            string holder = allRows[0];// used in loop to convert string to char
            for (int i = 0; i < holder.Length - 1; i++){
                row[i] = holder[i];
            }

            //init bounds
            height = allRows.Length;
            width = row.Length;

            map = new char[width, height];
        }
        // ----- gets & manager tools
        public char[] getTile(Toolkit toolkit, string _name, int x = 2, int y = 2) 
        {
            //toolkit.DisplayText($"checking {_name}");
            char[] directions = new char[5];
            directions[0] = map[x, y];
            if (y > 0)
            {
                directions[1] = map[x, y - 1];
            }
            else {
                directions[1] = map[x, y];
            }
            directions[2] = map[x+1, y];
            directions[3] = map[x, y+1];
            if (x > 0)
            {
                directions[4] = map[x-1, y];
            }
            else
            {
                directions[1] = map[x, y];
            }
            return directions;
        }
        public string getWallHold()
        { 
            string allwalls = string.Join("", wallHold);
            return allwalls;
        }
        public string getEnemyHold() 
        {
            string allEnemies = string.Join("", enemyHold);
            return allEnemies;
        }
        public string[] readEnemyHold()
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
            Console.Write($"selected map tile: [{map[x, y]}] walls available: [{allWalls}] enemy check: [{enemyInfo[2]}]"); // --- debug
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
            borderArray = new string[Console.WindowWidth, Console.WindowHeight];

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
                    borderArray[j, i] = borderX[j];
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
                    Console.Write(borderArray[j, i]);                
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
            enemyHold.Remove(getEnemyHold());
            string[] removingHold = enemyHold.ToArray();
            for (int i = 0; i < enemyHold.Count; i++)
            { 
                enemyHold.Remove(removingHold[i]);
            }
            
            if (!File.Exists("Map_test.txt")) {// input same string
                throw new Exception("File path does not Exist");
            }

            ///read map info
            string info = allRowsAndInfo[0];// line 0 passes info
            string[] readInfo = info.Split(';');

            string walls = readInfo[0]; //value 0 of info is walls
            
            // value 1 is for enemys
            string[] enemyInfo = readInfo[1].ToString().Split('.');
            string[] enemies = new string[enemyInfo.Length];
            for (int i = 0; i < (enemyInfo.Length); i++)
            {
                enemies[i] = enemyInfo[i];
            }

            ///read map
            for (int y = 0; y < height; y++){ 
                string xHolder = allRows[y];
                for (int i = 0; i < xHolder.Length; i++)
                {
                    row[i] = xHolder[i];
                }
                for (int x = 0; x < width; x++) {
                    //create array
                    map[x, y] = row[x];
                }
            }
            //two ways to handle it
            wallHold.Add(walls); // walls are in one listing: 0
            for (int i = 0; i < enemies.Length; i++) // enemies are in incremental listings: 0, 1, 2
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
            for (y = 1; y < height; y++) { // line 0 is to pass info,  
                for (x = 0; x < width; x++) {
                    Console.Write(map[x, y]);
                }
            }
        } 

    }
}
