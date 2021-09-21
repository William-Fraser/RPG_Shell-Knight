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

        public int daggerDamageMultiplier = 0;
        public int shortswordDamageMultiplier = 0;
        public int broadswordDamageMultplier = 0;
        public int longswordDamageMultplier = 0;
        public int claymoreDamageMultiplier = 0;
        public int kaliburnDamageMultiplier = 0;

        private Random rnd = new Random();
        private int tradePrice;

        private int buffChance;
        private int tradeBuff;
        private int buffSet = 5;
        

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
                if (weaponDecision.Key == ConsoleKey.D1) { tradePrice = rnd.Next(100, 500); BuyOrSellWeapon(WEAPON.DAGGER, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D2) { tradePrice = rnd.Next(200, 600); BuyOrSellWeapon(WEAPON.SHORTSWORD, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D3) { tradePrice = rnd.Next(300, 800); BuyOrSellWeapon(WEAPON.BROADSWORD, inventory, player);  }
                if (weaponDecision.Key == ConsoleKey.D4) { tradePrice = rnd.Next(350, 850); BuyOrSellWeapon(WEAPON.LONGSWORD, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D5) { tradePrice = rnd.Next(400, 900); BuyOrSellWeapon(WEAPON.CLAYMORE, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D6) { tradePrice = rnd.Next(500, 1000); BuyOrSellWeapon(WEAPON.KALIBURN, inventory, player); }
                else{ hud.Draw(); hud.DisplayText($"< " + player.Name() + " didn't select the proper value >", false);}
            }
            else if (vendorBeingTradedWith.type == Vendor.Type.POTIONEER)
            {
                Console.SetCursorPosition(10, 5); Console.WriteLine("Choose a potion to either buy or sell.....");
                Console.SetCursorPosition(10, 7); Console.WriteLine("1 - " + Global.ITEM_NAME(ITEM.POTSHELL) + " " + Global.ITEM_AVATAR(ITEM.POTSHELL));
                Console.SetCursorPosition(10, 9); Console.WriteLine("2 - " + Global.ITEM_NAME(ITEM.POTHEAL) + " " + Global.ITEM_AVATAR(ITEM.POTHEAL));
                Console.SetCursorPosition(10, 20); Console.WriteLine("Esc X 2 - Leave");

                ConsoleKeyInfo itemDecision = Console.ReadKey(true);
                if (itemDecision.Key == ConsoleKey.D1) { tradePrice = rnd.Next(200, 500); BuyOrSellItem(ITEM.POTSHELL, inventory, player); }
                if (itemDecision.Key == ConsoleKey.D2) { tradePrice = rnd.Next(600, 1000); BuyOrSellItem(ITEM.POTHEAL, inventory, player); }
                else{hud.Draw(); hud.DisplayText($"< " + player.Name() + " didn't select the proper value >", false);}
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
        private void BuyOrSellItem(ITEM item, Inventory inventory, Player player)
        {
            Draw();
            Console.SetCursorPosition(10, 5); Console.WriteLine("Are you buying or selling.....");
            Console.SetCursorPosition(10, 7); Console.WriteLine("1 - Buy -$" + tradePrice);
            Console.SetCursorPosition(10, 9); Console.WriteLine("2 - Sell +$" + tradePrice);
            Console.SetCursorPosition(10, 11);Console.WriteLine("3 - Cancel");
            ConsoleKeyInfo tradeDecision = Console.ReadKey(true);
            Draw();
            Console.SetCursorPosition(10, 5); Console.WriteLine("Press any button to continue.....");
            if (tradeDecision.Key == ConsoleKey.D1)
            {
                if (item == ITEM.POTSHELL) { CompleteItemTransaction(ITEM.POTSHELL, player, inventory); }
                else if (item == ITEM.POTHEAL) { CompleteItemTransaction(ITEM.POTHEAL, player, inventory); }
            }
            if (tradeDecision.Key == ConsoleKey.D2)
            {
                if (item == ITEM.POTSHELL) { if (inventory.ItemIsAvailable(ITEM.POTSHELL) == true) { inventory.DecreaseStock(ITEM.POTSHELL); player.currentMoney = player.currentMoney + tradePrice; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.ITEM_NAME(ITEM.POTSHELL) + " >", false); } else { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.ITEM_NAME(ITEM.POTSHELL) + " to sell! >", false); } }
                else if (item == ITEM.POTHEAL) { if (inventory.ItemIsAvailable(ITEM.POTHEAL) == true) { inventory.DecreaseStock(ITEM.POTHEAL); player.currentMoney = player.currentMoney + tradePrice; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.ITEM_NAME(ITEM.POTHEAL) + " >", false); } else { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.ITEM_NAME(ITEM.POTHEAL) + " to sell! >", false); } }
            }
        }
        private void BuyOrSellWeapon(WEAPON weapon, Inventory inventory, Player player)
        {
            tradeBuff = 0;
            Draw();
            Console.SetCursorPosition(10, 5); Console.WriteLine("Are you buying or selling.....");
            Console.SetCursorPosition(10, 7); Console.WriteLine("1 - Buy -$" + tradePrice);
            Console.SetCursorPosition(10, 9); Console.WriteLine("2 - Sell +$" + tradePrice);
            Console.SetCursorPosition(10, 11); Console.WriteLine("3 - Cancel");
            ConsoleKeyInfo tradeDecision = Console.ReadKey(true);
            Draw();
            Console.SetCursorPosition(10, 5); Console.WriteLine("Press any button to continue.....");
            if (tradeDecision.Key == ConsoleKey.D1) 
            {
                player.EquipWeapon(WEAPON.FISTS);
                if (weapon == WEAPON.DAGGER) { if (inventory.daggerOwned == true) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " already owns a " + Global.WEAPON_NAME(WEAPON.DAGGER) + " >", false); } else { CompleteWeaponTransaction(WEAPON.DAGGER, player, inventory); } }
                else if (weapon == WEAPON.SHORTSWORD) { if (inventory.shortswordOwned == true) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " already owns a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " >", false); } else { CompleteWeaponTransaction(WEAPON.SHORTSWORD, player, inventory); } }
                else if (weapon == WEAPON.BROADSWORD) { if (inventory.broadswordOwned == true) { hud.Draw(); hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " >", false); } else { CompleteWeaponTransaction(WEAPON.BROADSWORD, player, inventory); } }
                else if (weapon == WEAPON.LONGSWORD) { if (inventory.longswordOwned == true) { hud.Draw(); hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " >", false); } else { CompleteWeaponTransaction(WEAPON.LONGSWORD, player, inventory); } }
                else if (weapon == WEAPON.CLAYMORE) { if (inventory.claymoreOwned == true) { hud.Draw(); hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " >", false); } else { CompleteWeaponTransaction(WEAPON.CLAYMORE, player, inventory); } }
                else if (weapon == WEAPON.KALIBURN) { if (inventory.kaliburnOwned == true) { hud.Draw(); hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " >", false); } else { CompleteWeaponTransaction(WEAPON.KALIBURN, player, inventory); } }
            }
            if (tradeDecision.Key == ConsoleKey.D2)
            {
                player.EquipWeapon(WEAPON.FISTS);
                if (weapon == WEAPON.DAGGER) { if (inventory.daggerOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.DAGGER) + " to sell! >", false); } else { daggerDamageMultiplier = 0; inventory.daggerOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() +" sold a " + Global.WEAPON_NAME(WEAPON.DAGGER) +" >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.SHORTSWORD) { if (inventory.shortswordOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " to sell! >", false); } else { shortswordDamageMultiplier = 0; inventory.shortswordOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.BROADSWORD) { if (inventory.broadswordOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " to sell! >", false); } else { broadswordDamageMultplier = 0; inventory.broadswordOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.LONGSWORD) { if (inventory.longswordOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " to sell! >", false); } else { longswordDamageMultplier = 0; inventory.longswordOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.CLAYMORE) { if (inventory.claymoreOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " to sell! >", false); } else { claymoreDamageMultiplier = 0; inventory.claymoreOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.KALIBURN) { if (inventory.kaliburnOwned == false) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " to sell! >", false); } else { kaliburnDamageMultiplier = 0; inventory.kaliburnOwned = false; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
            }
            else
            {
                hud.Draw(); hud.DisplayText($"< " + player.Name() + " didn't select the proper value >", false);
            }
        }
        private void CompleteWeaponTransaction(WEAPON weapon, Player player, Inventory inventory)
        {
            buffChance = rnd.Next(buffSet);
            if (player.currentMoney < tradePrice) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " doesn't have enough money! >", false); }
            else 
            {
                if (buffChance == buffSet) { tradeBuff = rnd.Next(2, 3); }
                if (weapon == WEAPON.DAGGER) { inventory.daggerOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.DAGGER) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; daggerDamageMultiplier = tradeBuff; }
                else if (weapon == WEAPON.SHORTSWORD) { inventory.shortswordOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; shortswordDamageMultiplier = tradeBuff; }
                else if (weapon == WEAPON.BROADSWORD) { inventory.broadswordOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; broadswordDamageMultplier = tradeBuff; }
                else if (weapon == WEAPON.LONGSWORD) { inventory.longswordOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; longswordDamageMultplier = tradeBuff; }
                else if (weapon == WEAPON.CLAYMORE) { inventory.claymoreOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; claymoreDamageMultiplier = tradeBuff; }
                else if (weapon == WEAPON.KALIBURN) { inventory.kaliburnOwned = true; hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; kaliburnDamageMultiplier = tradeBuff; }
            }
        }
        private void CompleteItemTransaction(ITEM item, Player player, Inventory inventory)
        {
            if (player.currentMoney < tradePrice) { hud.Draw(); hud.DisplayText($"< " + player.Name() + " doesn't have enough money! >", false); }
            else
            {
                if (item == ITEM.POTSHELL) { player.currentMoney = player.currentMoney - tradePrice; inventory.IncreaseStock(ITEM.POTSHELL); hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.ITEM_NAME(ITEM.POTSHELL) + " >", false); }
                if (item == ITEM.POTHEAL) { player.currentMoney = player.currentMoney - tradePrice; inventory.IncreaseStock(ITEM.POTHEAL); hud.Draw(); hud.DisplayText($"< " + player.Name() + " bought a " + Global.ITEM_NAME(ITEM.POTHEAL) + " >", false); }
            }
        }
    }
}
