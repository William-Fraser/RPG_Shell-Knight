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

        string recentMessage;
        string middleMessage = "";
        string decayingMessage = "";
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
            Console.SetCursorPosition(1, Console.WindowHeight - 3);
            Console.Write(message);
            if (message != blank)
            {
                Console.Write(", press any key to continue");
                saveMessage(message);
                DisplayText(blank);
                Console.ReadKey(true);
            }
        }
        public void saveMessage(string message)
        {
            decayingMessage = middleMessage;
            middleMessage = recentMessage;
            recentMessage = message;
            Console.SetCursorPosition(1, Console.WindowHeight - 4);
            Console.Write(recentMessage);
            Console.SetCursorPosition(1, Console.WindowHeight - 5);
            Console.Write(middleMessage);
            Console.SetCursorPosition(1, Console.WindowHeight - 6);
            Console.Write(decayingMessage);
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
