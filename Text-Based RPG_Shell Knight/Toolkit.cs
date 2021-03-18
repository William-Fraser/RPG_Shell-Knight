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

        string currentMessage;
        string recentMessage = "";
        string middleMessage = "";
        string decayingMessage = "";
        public void SetConsoleSize()
        {
            Console.SetWindowSize(128, 36);
            Console.WindowWidth = 128;
            Console.WindowHeight = 36;
        }
        public int RandomNumBetween(int lowNumber, int highNumber)
        {
            Random rdm = new Random();
            int finalDamage = rdm.Next(lowNumber, highNumber);
            return finalDamage;
        }
        public void DisplayText(string message, bool alive = true) // bool for game over text
        {
            //if (message != blank)
            //{
            //    string PaktC = ", Press any key to Continue.";
            //    message += PaktC;
            //}

            if (message != blank)
            {
                SaveMessage(currentMessage);
                DisplayText(blank);
            }
            Console.SetCursorPosition(1, Console.WindowHeight - 3);
            Console.Write(message);
            if (message != blank)
            {
                if (alive)
                {
                    Console.CursorVisible = true;
                 
                    
                    
                    Console.Write(", press any key to continue");
                    Console.ReadKey(true);
                    Console.CursorVisible = false;
                    DisplayText(blank);
                }
                currentMessage = message;
                Console.SetCursorPosition(1, Console.WindowHeight - 3);
                Console.Write(currentMessage);
            }
        }
        public void SaveMessage(string message)
        {
            if (message != "")
            {
                decayingMessage = middleMessage;
                middleMessage = recentMessage;
                recentMessage = currentMessage;
                
                Console.SetCursorPosition(1, Console.WindowHeight - 4);
                Console.Write(blank);
                Console.SetCursorPosition(1, Console.WindowHeight - 4);
                Console.Write(recentMessage);
                Console.SetCursorPosition(1, Console.WindowHeight - 5);
                Console.Write(blank);
                Console.SetCursorPosition(1, Console.WindowHeight - 5);
                Console.Write(middleMessage);
                Console.SetCursorPosition(1, Console.WindowHeight - 6);
                Console.Write(blank);
                Console.SetCursorPosition(1, Console.WindowHeight - 6);
                Console.Write(decayingMessage);
            }
        }
        //scrolling text

        //cursor = true
        //    string continueMessage = ", Press any key to Continue.";
        //    char[] printContinue = new char[continueMessage.Length];

        //            for (int j = 0; j<continueMessage.Length; j++) {
        //                printContinue[j] = continueMessage[j];
        //            }
        //Thread.Sleep(100);
        //            for (int i = 0; i<printContinue.Length; i++) {
        //                Thread.Sleep(7);
        //                Console.Write(printContinue[i]);
        //            }
        //cursor = false
    }
}
