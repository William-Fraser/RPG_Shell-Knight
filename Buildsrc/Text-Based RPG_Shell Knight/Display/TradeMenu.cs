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
        Item itemSelectedForTrading;
        

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
        public void ChooseItem(Inventory inventory, Player player)
        {
            
            if (vendorBeingTradedWith.type == Vendor.Type.BLACKSMITH)
            {
                Console.SetCursorPosition(10, 5); Console.WriteLine("Choose a weapon to either buy or sell.....");
                Console.SetCursorPosition(10, 7); Console.WriteLine("1 - " + Global.WEAPON_NAME(WEAPON.DAGGER) + " " + Global.WEAPON_AVATAR(WEAPON.DAGGER));
                Console.SetCursorPosition(10, 9); Console.WriteLine("2 - " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " " + Global.WEAPON_AVATAR(WEAPON.SHORTSWORD));
                Console.SetCursorPosition(10, 11); Console.WriteLine("3 - " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " " + Global.WEAPON_AVATAR(WEAPON.BROADSWORD));
                Console.SetCursorPosition(10, 13); Console.WriteLine("4 - " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " " + Global.WEAPON_AVATAR(WEAPON.LONGSWORD));
                Console.SetCursorPosition(10, 15); Console.WriteLine("5 - " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " " + Global.WEAPON_AVATAR(WEAPON.CLAYMORE));
                Console.SetCursorPosition(10, 17); Console.WriteLine("6 - " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " " + Global.WEAPON_AVATAR(WEAPON.KALIBURN));
                Console.SetCursorPosition(10, 20); Console.WriteLine("Esc X 2 - Leave");

                ConsoleKeyInfo weaponDecision = Console.ReadKey(true);
                if (weaponDecision.Key == ConsoleKey.D1) { BuyOrSellWeapon(WEAPON.DAGGER, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D2) { BuyOrSellWeapon(WEAPON.SHORTSWORD, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D3) { BuyOrSellWeapon(WEAPON.BROADSWORD, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D4) { BuyOrSellWeapon(WEAPON.LONGSWORD, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D5) { BuyOrSellWeapon(WEAPON.CLAYMORE, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D6) { BuyOrSellWeapon(WEAPON.KALIBURN, inventory, player); }
            }
            else if (vendorBeingTradedWith.type == Vendor.Type.POTIONEER)
            {
                Console.SetCursorPosition(10, 5); Console.WriteLine("Choose a potion to either buy or sell.....");
                Console.SetCursorPosition(10, 7); Console.WriteLine("1 - " + Global.ITEM_NAME(ITEM.POTHEAL) + " " + Global.ITEM_AVATAR(ITEM.POTHEAL));
                Console.SetCursorPosition(10, 9); Console.WriteLine("2 - " + Global.ITEM_NAME(ITEM.POTSHELL) + " " + Global.ITEM_AVATAR(ITEM.POTSHELL));
                Console.SetCursorPosition(10, 20); Console.WriteLine("Esc X 2 - Leave");
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
            ChooseItem(inventory, player);

            ConsoleKeyInfo exitDecision = Console.ReadKey(true);
            if (exitDecision.Key == ConsoleKey.Escape) { return ExitTrade(); }
            return gameState;
        }
        private void BuyOrSellWeapon(WEAPON weapon, Inventory inventory, Player player)
        {
            Draw();
            Console.SetCursorPosition(10, 5); Console.WriteLine("Are you buying or selling.....");
            Console.SetCursorPosition(10, 7); Console.WriteLine("1 - Buy");
            Console.SetCursorPosition(10, 9); Console.WriteLine("2 - Sell");
            ConsoleKeyInfo tradeDecision = Console.ReadKey(true);
            Draw();
            Console.SetCursorPosition(10, 5); Console.WriteLine("Press any button to continue.....");
            if (tradeDecision.Key == ConsoleKey.D1) 
            {
                if (weapon == WEAPON.DAGGER) { if (inventory.daggerOwned == true) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " already owns a " + Global.WEAPON_NAME(WEAPON.DAGGER) + " >", false); } else { inventory.daggerOwned = true; hud.Draw(); hud.DisplayText($"< " +  player.Name() +" bought a " + Global.WEAPON_NAME(WEAPON.DAGGER) + " >", false); } }
                if (weapon == WEAPON.SHORTSWORD) { if (inventory.shortswordOwned == true) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " already owns a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " >", false); } else { inventory.shortswordOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " >", false); } }
                if (weapon == WEAPON.BROADSWORD) { if (inventory.broadswordOwned == true) { hud.Draw(); hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " >", false); } else { inventory.broadswordOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " >", false); } }
                if (weapon == WEAPON.LONGSWORD) { if (inventory.longswordOwned == true) { hud.Draw(); hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " >", false); } else { inventory.longswordOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " >", false); } }
                if (weapon == WEAPON.CLAYMORE) { if (inventory.claymoreOwned == true) { hud.Draw(); hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " >", false); } else { inventory.claymoreOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " >", false); } }
                if (weapon == WEAPON.KALIBURN) { if (inventory.kaliburnOwned == true) { hud.Draw(); hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " >", false); } else { inventory.kaliburnOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " >", false); } }
            }
            if (tradeDecision.Key == ConsoleKey.D2)
            {
                if (weapon == WEAPON.DAGGER) { if (inventory.daggerOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.DAGGER) + " to sell! >", false); } else { inventory.daggerOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() +" sold a " + Global.WEAPON_NAME(WEAPON.DAGGER) +" >", false);  } }
                if (weapon == WEAPON.SHORTSWORD) { if (inventory.shortswordOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " to sell! >", false); } else { inventory.shortswordOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " >", false); } }
                if (weapon == WEAPON.BROADSWORD) { if (inventory.broadswordOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " to sell! >", false); } else { inventory.broadswordOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " >", false); } }
                if (weapon == WEAPON.LONGSWORD) { if (inventory.longswordOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " to sell! >", false); } else { inventory.longswordOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " >", false); } }
                if (weapon == WEAPON.CLAYMORE) { if (inventory.claymoreOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " to sell! >", false); } else { inventory.claymoreOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " >", false); } }
                if (weapon == WEAPON.KALIBURN) { if (inventory.kaliburnOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " to sell! >", false); } else { inventory.kaliburnOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " >", false); } }
            }
        }
    }
}
