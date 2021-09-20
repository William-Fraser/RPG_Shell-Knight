using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class TradeMenu
    {

        Camera camera;
        HUD hud;
        Vendor vendorBeingTradedWith;

        public TradeMenu()
        {
            camera = new Camera();
        }

        public void Draw()
        {
            hud.Draw();
            camera.DrawBorder();
        }
        private GAMESTATE ExitTrade()
        {
            return GAMESTATE.MAP;
        }
        public void ChooseItem()
        {
            
            if (vendorBeingTradedWith.type == Vendor.Type.BLACKSMITH)
            {

                Console.SetCursorPosition(10, 5); Console.WriteLine("Choose a weapon to either buy or sell.....");
                Console.SetCursorPosition(10, 7); Console.WriteLine("1 - Dagger ┼──");
                Console.SetCursorPosition(10, 9); Console.WriteLine("2 - Short Sword ─┼═══─");
                Console.SetCursorPosition(10, 11); Console.WriteLine("3 - Broad Sword ──╬═════─");
                Console.SetCursorPosition(10, 13); Console.WriteLine("4 - Long Sword o────╬■■■▄▄▄▄▄▄■■■■▀▀");
                Console.SetCursorPosition(10, 15); Console.WriteLine("5 - Claymore ├═┼══╣█████████■");
                Console.SetCursorPosition(10, 17); Console.WriteLine("6 - Kaliburn ╔──┼──╬■■█■■■■■■▄▄▄_");
                Console.SetCursorPosition(10, 20); Console.WriteLine("Esc X 2 - Leave");
                ConsoleKeyInfo weaponDecision = Console.ReadKey(true);
            }
           
        }
        public GAMESTATE Begin(Player player, Vendor vendor, Inventory inventory, bool isPlayer = false)
        {
            vendorBeingTradedWith = vendor;
            hud = new HUD(player.Name());
            Console.Clear();
            hud.Update(player, inventory);
            hud.DrawTextBox();
            return GAMESTATE.TRADING;
        }

        public GAMESTATE Update(Player player, GAMESTATE gameState, Item item, Inventory inventory)
        {
            
            hud.Update(player, inventory);
            Draw();
            ChooseItem();

            ConsoleKeyInfo tradeDecision = Console.ReadKey(true);
            if (tradeDecision.Key == ConsoleKey.Escape) { return ExitTrade(); }
            return gameState;
        }
    }
}
