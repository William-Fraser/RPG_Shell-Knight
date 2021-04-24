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
        public int RandomNumBetween(int lowNumber, int highNumber) // used to find random numbers
        {
            Random rdm = new Random();
            int finalDamage = rdm.Next(lowNumber, highNumber);
            return finalDamage;
        }
        public static int TwoOptionMenu(string MenuDisplay, int _currentState, string option1, int result1, string option2, int result2)
        {
            bool retryMenu = false;
            ConsoleKeyInfo decision = new ConsoleKeyInfo();

            // ERR input proper value
            if (retryMenu)
            {
                Console.WriteLine("\n please enter the proper value");
            }

            //menu and input
            Console.WriteLine($"\n     {MenuDisplay}\n        1 - {option1}\n        2 - {option2}");
            decision = Console.ReadKey(true);

            //choices
            switch (decision.Key)
            {
                case ConsoleKey.D1:
                    _currentState = result1;
                    break;

                case ConsoleKey.D2:
                    _currentState = result2;
                    break;
                default: break;
            }
            //sets to ask for proper value if trying again
            retryMenu = true;

            Console.Clear();
            return _currentState;
        }

    }
}
