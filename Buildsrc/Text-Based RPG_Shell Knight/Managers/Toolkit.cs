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
        public static int RandomNumBetween(int lowNumber, int highNumber) // used to find random numbers
        {
            Random rdm = new Random();
            int finalDamage = rdm.Next(lowNumber, highNumber);
            return finalDamage;
        }
        public static void FixDisplay(Camera camera, HUD hud)
        {
            camera.FixModifyedConsole(hud); // checks for console size change and starts handling it
            Console.CursorVisible = false;
        }
        public static int TwoOptionMenu(string MenuDisplay, int _currentState, string option1, int result1, string option2, int result2)
        {
            ConsoleKeyInfo decision = new ConsoleKeyInfo();
            bool choiceChosen = false;
            bool retryMenu = false;

            //loop until a choice is made
            while (!choiceChosen)
            {
                Console.Clear();
                
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
                        choiceChosen = true;
                        break;

                    case ConsoleKey.D2:
                        _currentState = result2;
                        choiceChosen = true;
                        break;
                    default: break;
                }
                //sets to ask for proper value if trying again
                retryMenu = true;
            }
            return _currentState;
        }

    }
}
