using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Text_Based_RPG_Shell_Knight
{
    class Toolkit
    {
        public string blank = "                                                                                                                              ";
       
        public void SetConsoleSize()
        {
            Console.WindowWidth = 32 * 4;
            Console.WindowHeight = 9 * 4;
        }
        public int RandomNumBetween(int lowNumber, int highNumber)
        {
            Random rdm = new Random();
            int finalDamage = rdm.Next(lowNumber, highNumber);
            return finalDamage;
        }
        public void DisplayText(string message)
        {
            //if (message != blank)
            //{
            //    string PaktC = ", Press any key to Continue.";
            //    message += PaktC;
            //}
            Console.CursorVisible = true;
            Console.SetCursorPosition(1, Console.WindowHeight - 2);
            Console.Write(message);
            if (message != blank)
            {
                Thread.Sleep(1000);
                Console.Write(", Press any key to Continue.");
                Console.Write(" any");
                Console.ReadKey(true);
            }
            Console.CursorVisible = false;
        }
    }
}
