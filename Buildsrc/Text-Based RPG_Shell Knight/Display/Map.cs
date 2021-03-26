using System;
using System.IO;
using System.Collections.Generic;

namespace Text_Based_RPG_Shell_Knight
{
    class Map
    {
        // height and width of the Whole Map
        public static int height;
        public static int width;

        // lists containing Map Information for the game system to read
        public List<string> wallHold = new List<string>();
        public List<string> enemyHold = new List<string>();
        public List<string> itemHold = new List<string>();
        public List<string> doorHold = new List<string>();

        // Map File Lines/Info Readers
        private string[] allRowsAndInfo = File.ReadAllLines("Map_test.txt");// this changes to string constant is _test for now
        private string[] allRows;
        private char[] row;

        // Map
        private char[,] map;

        // ----- constructor
        public Map()
        {
            //init length of rows and columns
            allRows = new string[allRowsAndInfo.Length - 1]; // -1 removing Info
            for (int i = 0; i < allRowsAndInfo.Length - 1; i++) {
                allRows[i] = allRowsAndInfo[i + 1];
            }

            row = new char[allRows[0].Length +1];
            string holder = allRows[0];// used in loop to convert string to char
            for (int i = 0; i < holder.Length - 1; i++) {
                row[i] = holder[i];
            }

            //init bounds
            height = allRows.Length;
            width = row.Length;

            map = new char[height, width];
        }

        // ----- gets & manager tools
        public char getTile(int x = 0, int y = 0)
        {
            return map[y, x];
        }
        public char[,] getMap()
        {
            return map;
        }
        public string getWallHold()
        {
            string allwalls = string.Join("", wallHold);
            return allwalls;
        }
        public string getEnemyHold()
        {
            string allEnemies = "";
            for (int i = 0; i < enemyHold.Count; i++)
            {
                allEnemies += enemyHold[i];
                if (i != enemyHold.Count - 1)
                {
                    allEnemies += "|";
                }
            }
            return allEnemies;
        }
        public string getItemHold()
        {
            string allItems = "";
            for (int i = 0; i < itemHold.Count; i++)
            {
                allItems += itemHold[i];
                if (i != itemHold.Count - 1)
                {
                    allItems += "|";
                }
            }
            return allItems;
        }
        public string getDoorHold()
        {
            string allItems = "";
            for (int i = 0; i < doorHold.Count; i++)
            {
                allItems += doorHold[i];
                if (i != doorHold.Count - 1)
                {
                    allItems += "|";
                }
            }
            return allItems;
        }
        public string[] ReadEnemyHold()
        {
            string[] allenemies = new string[enemyHold.Count];
            for (int i = 0; i < enemyHold.Count; i++)
            { allenemies[i] = enemyHold[i]; }
            return allenemies;
        }
        public void OpenDoor()
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


        } // move to DoorGate

        // ----- Public Methods
        public void loadMap()// loads the map from a text file
        {
            // set width and height of displayed map
            Camera.displayWidth = Camera.borderWidth - 1;
            Camera.displayHeight = Camera.borderHeight - 1;

            // removing objects from current map
            for (int i = 0; i < wallHold.Count; i++)
            {
                wallHold.Remove(wallHold[i]);
            }
            for (int i = 0; i < enemyHold.Count; i++)
            {
                enemyHold.Remove(enemyHold[i]);
            }
            for (int i = 0; i < itemHold.Count; i++)
            {
                itemHold.Remove(itemHold[i]);
            }
            for (int i = 0; i < doorHold.Count; i++)
            {
                doorHold.Remove(doorHold[i]);
            }

            // check for file
            if (!File.Exists("Map_test.txt")) {// input same string
                throw new Exception("File path does not Exist");
            }

            ///read map and info
            string info = allRowsAndInfo[0];// line 0 passes info
            string[] readInfo = info.Split('!');

            ///read info
            //value 0 of info is walls
            string[] wallInfo = readInfo[0].ToString().Split();
            for (int i = 0; i < (wallInfo.Length); i++)
            {
                wallHold.Add(wallInfo[i]);
            }
            // value 1 is for enemys
            string[] enemyInfo = readInfo[1].ToString().Split('|');
            for (int i = 0; i < (enemyInfo.Length); i++)
            {
                enemyHold.Add(enemyInfo[i]);
            }
            //value 2 of info is items
            string[] itemInfo = readInfo[2].ToString().Split('|');
            for (int i = 0; i < (itemInfo.Length); i++)
            {
                itemHold.Add(itemInfo[i]);
            }
            //value 3 of info is doors
            string[] doorInfo = readInfo[3].ToString().Split('|');
            for (int i = 0; i < (doorInfo.Length); i++)
            {
                doorHold.Add(doorInfo[i]);
            }


            ReadMap();
        }

        public void ReadMap()
        {
            ///read map
            for (int y = 0; y < height; y++)
            {
                string xHolder = allRows[y];
                for (int i = 0; i < xHolder.Length; i++)
                {
                    row[i] = xHolder[i];
                }
                for (int x = 0; x < width; x++)
                {
                    //create array
                    map[y, x] = row[x];
                }
            }
        }

        public void CheckPosition(int x, int y, string[] enemyInfo)
        { // debug
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
    }
}
