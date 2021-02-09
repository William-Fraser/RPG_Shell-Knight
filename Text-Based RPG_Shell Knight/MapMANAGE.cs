using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class MapMANAGE
    {
        private string[,] borderMap;
        private char[] _walls = new char[] { '═', '║' };
        public void DrawWindowBorder() // ═ ║ 
        {
            string borderString = "";
            Console.SetCursorPosition(0, 0);
            //Console.Write("╔"); 
            borderString += "╔,";
            for (int i = 1; i < Console.WindowWidth - 1; i++ )
            {
                //Console.Write("═"); 
                borderString += "═,";
            }
            //Console.Write("╗"); 
            borderString += "╗;";
            for (int i = 1; i < Console.WindowHeight - 2; i++)
            {
                //Console.Write("║"); 
                borderString += "║,";
                for (int j = 1; j < Console.WindowWidth - 1; j++)
                {
                    //Console.Write(" "); 
                    borderString += " ,";
                }
                //Console.Write("║"); 
                borderString += "║;";
            }
            //Console.Write("╚"); 
            borderString += "╚,";
            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                //Console.Write("═"); 
                borderString += "═,";
            }
            //Console.Write("╝");
            borderString += "╝;";
            string[] borderArray = borderString.Split(';');
            for (int i = 0; i < borderArray.Length; i++) 
            {
                string[] MapX = borderArray[i].Split(','); // used for .Length
                for (int j = 0; j > MapX.Length; j++)
                {
                    borderMap[i, j] = MapX[j];
                }
            }
            for (int i = 0; i < borderArray.Length; i++)
            {
                string[] MapX = borderArray[i].Split(',');
                for (int j = 0; j > MapX.Length; j++)
                {
                    Console.WriteLine(borderMap[i, j]);
                }
            }
        }

    }
}
