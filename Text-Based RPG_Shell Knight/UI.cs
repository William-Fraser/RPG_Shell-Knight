using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    /// <Contains>
    /// HUD
    /// player menu access by menu button
    ///     equipment & items
    ///     player revealed world map?
    /// Save & Load Screen
    /// </Contains>

    class UI
    {
        private int[] displayHealth = { 0, 0 };
        private int[] displayShield = { 0, 0 };

        public void getHUDvalues(int[] health, int[] shield)
        {
            if (shield == null) { shield = displayShield; } // sets shield if it's null
            displayHealth[0] = health[0]; 
            displayHealth[1] = health[1];
            displayShield[0] = shield[0];
            displayShield[1] = shield[1];
        }
        public void HUD(string name, Toolkit toolkit)
        {
            //Shell
            //Health
            //Essence - magic unlocked? strech goal...
            Console.SetCursorPosition(1, Console.WindowHeight-8);
            Console.WriteLine(toolkit.blank);
            Console.SetCursorPosition(1, Console.WindowHeight - 8);
            Console.Write($" {name} |");
            Console.Write($" Shell : {displayShield[0]}/{displayShield[1]} |");
            Console.Write($" Health : {displayHealth[0]}/{displayHealth[1]} ");
            Console.WriteLine();
            Console.Write("╠"); 
            for (int i = 0; i < Console.WindowWidth - 2; i++)
            {
                 Console.Write("═"); 
            }
            Console.Write("╣");
        }
    }
}
