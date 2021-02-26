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
        private int[] displayHealth = {0,0};

        public void getHUDvalues(int[] health)
        {
            displayHealth[0] = health[0]; 
            displayHealth[1] = health[1];
        }
        public void HUD()
        {
            Console.SetCursorPosition(1, Console.WindowHeight-8);
            Console.WriteLine($"Health: {displayHealth[0]}/{displayHealth[1]}");
            Console.Write("╠"); 
            for (int i = 0; i < Console.WindowWidth - 2; i++)
            {
                 Console.Write("═"); 
            }
            Console.Write("╣");
        }
    }
}
