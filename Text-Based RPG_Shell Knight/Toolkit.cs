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
            Console.WindowWidth = 128;
            Console.WindowHeight = 36;
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
                string continueMessage = ", Press any key to Continue.";
                char[] printContinue = new char[continueMessage.Length];

                for (int j = 0; j < continueMessage.Length; j++) {
                    printContinue[j] = continueMessage[j];
                }
                Thread.Sleep(100);
                for (int i = 0; i < printContinue.Length; i++) {
                    Thread.Sleep(7);
                    Console.Write(printContinue[i]);
                }
                Console.ReadKey(true);
                DisplayText(blank);
            }
            Console.CursorVisible = false;
        }
    }
}
