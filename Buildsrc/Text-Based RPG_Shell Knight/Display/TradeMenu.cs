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

        //ints to keep track of specific buff on specific items
        //weapons
        public int daggerDamageMultiplier = 0;
        public int shortswordDamageMultiplier = 0;
        public int broadswordDamageMultplier = 0;
        public int longswordDamageMultplier = 0;
        public int claymoreDamageMultiplier = 0;
        public int kaliburnDamageMultiplier = 0;
        //items
        public int healthpEffectMultiplier = 0;
        public int shellpEffectMultiplier = 0;

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
            //UI and decisions based on type of vendor, here player can choose an item to either buy or sell
            if (vendorBeingTradedWith.type == Vendor.Type.BLACKSMITH)
            {
                Console.SetCursorPosition(10, 5); Console.WriteLine("Choose a weapon to either buy or sell.....");
                {
                    int i = 9;
                    for (int j = 0; j < Global.globalAccess.weaponIDs.Count; j++)
                    {
                        string jstring = j.ToString();
                        Console.SetCursorPosition(10, i); Console.WriteLine(j.ToString()+" - " + Global.WEAPON_NAME(jstring) + " " + Global.WEAPON_AVATAR(jstring));
                        i = +2;
                    }
                    Console.SetCursorPosition(10, i); Console.WriteLine("Esc X 2 - Leave");
                }
                ConsoleKeyInfo weaponDecision = Console.ReadKey(true);
                //random generated buy and sell prices prices for all items
                for (int i = 1; i <= Global.globalAccess.weaponIDs.Count; i++)
                {
                    ConsoleKey activationKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), $"D{i}");
                    if (weaponDecision.Key == activationKey) { tradePrice = rnd.Next(100, 500); BuyOrSellWeapon(i, inventory, player); }
                }
                /*if (weaponDecision.Key == ConsoleKey.D1) { tradePrice = rnd.Next(100, 500); BuyOrSellWeapon(WEAPON.DAGGER, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D2) { tradePrice = rnd.Next(200, 600); BuyOrSellWeapon(WEAPON.SHORTSWORD, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D3) { tradePrice = rnd.Next(300, 800); BuyOrSellWeapon(WEAPON.BROADSWORD, inventory, player);  }
                if (weaponDecision.Key == ConsoleKey.D4) { tradePrice = rnd.Next(350, 850); BuyOrSellWeapon(WEAPON.LONGSWORD, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D5) { tradePrice = rnd.Next(400, 900); BuyOrSellWeapon(WEAPON.CLAYMORE, inventory, player); }
                if (weaponDecision.Key == ConsoleKey.D6) { tradePrice = rnd.Next(500, 1000); BuyOrSellWeapon(WEAPON.KALIBURN, inventory, player); }*/
            }
            else if (vendorBeingTradedWith.type == Vendor.Type.POTIONEER)
            {
                Console.SetCursorPosition(10, 5); Console.WriteLine("Choose a potion to either buy or sell.....");
                for (int i = 0; i < Global.globalAccess.itemIDs.Count; i++)
                {
                    string ID = i.ToString();
                    Console.SetCursorPosition(10, 7); Console.WriteLine((i+1)+" - " + Global.ITEM_NAME(ID) + " " + Global.ITEM_AVATAR(ID));
                }
                Console.SetCursorPosition(10, 20); Console.WriteLine("Esc X 2 - Leave");

                ConsoleKeyInfo itemDecision = Console.ReadKey(true);
                for (int i = 0; i < Global.globalAccess.itemIDs.Count; i++)
                {
                    string ID = i.ToString();
                    ConsoleKey activationKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), $"D{i}");
                    if (itemDecision.Key == activationKey) { tradePrice = rnd.Next(200, 500); BuyOrSellItem(ID, inventory, player); }
                }
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
        private void BuyOrSellItem(string ID, Inventory inventory, Player player)
        {
            // this is for when you choose an item to buy or sell, items chosen will check things like your amount of money for buying and, check your items for selling.......
            //if you have no items to sell, the deal is cancle as well as if you have not enough money to buy
            tradeBuff = 0;
            Draw();
            Console.SetCursorPosition(10, 5); Console.WriteLine("Are you buying or selling.....");
            Console.SetCursorPosition(10, 7); Console.WriteLine("1 - Buy -$" + tradePrice);
            Console.SetCursorPosition(10, 9); Console.WriteLine("2 - Sell +$" + tradePrice);
            Console.SetCursorPosition(10, 11);Console.WriteLine("3 - Cancel");
            ConsoleKeyInfo tradeDecision = Console.ReadKey(true);
            Draw();
            Console.SetCursorPosition(10, 5); Console.WriteLine("Press any button to continue.....");

            for (int i = 0; i < Global.globalAccess.itemIDs.Count; i++)
            {
                ConsoleKey activationKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), $"D{i}");
                if (tradeDecision.Key == activationKey)
                {
                    if (ID == i.ToString()) { CompleteItemTransaction(ID, player, inventory); }
                    else if (ID == i.ToString()) { CompleteItemTransaction(ID, player, inventory); }
                }
            }

            /*if (tradeDecision.Key == ConsoleKey.D1)
            {
                if (item == ITEM.POTSHELL) { CompleteItemTransaction(ITEM.POTSHELL, player, inventory); }
                else if (item == ITEM.POTHEAL) { CompleteItemTransaction(ITEM.POTHEAL, player, inventory); }
            }
            if (tradeDecision.Key == ConsoleKey.D2)
            {
                if (item == ITEM.POTSHELL) { if (inventory.ItemIsAvailable(ITEM.POTSHELL) == true) { inventory.DecreaseStock(ITEM.POTSHELL); player.currentMoney = player.currentMoney + tradePrice; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.ITEM_NAME(ITEM.POTSHELL) + " >", false); } else { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.ITEM_NAME(ITEM.POTSHELL) + " to sell! >", false); } }
                else if (item == ITEM.POTHEAL) { if (inventory.ItemIsAvailable(ITEM.POTHEAL) == true) { inventory.DecreaseStock(ITEM.POTHEAL); player.currentMoney = player.currentMoney + tradePrice; hud.Draw(); hud.DisplayText($"< " + player.Name() + " sold a " + Global.ITEM_NAME(ITEM.POTHEAL) + " >", false); } else { hud.Draw(); hud.DisplayText($"< " + player.Name() + " does not own a " + Global.ITEM_NAME(ITEM.POTHEAL) + " to sell! >", false); } }
            }*/
        }
        private void BuyOrSellWeapon(int weapon, Inventory inventory, Player player)
        {
            // this is for when you choose an weapon to buy or sell, items chosen will check things like your amount of money for buying and, check your items for selling.......
            //if you have no items to sell, the deal is cancle as well as if you have not enough money to buy
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
                //always equips fist when wepon is boughten or sold to prevent weapon being in still in hand when sold bug
                player.EquipWeapon("0"); // 0 should be the default weapon
                for (int i = 0; i < Global.globalAccess.weaponIDs.Count; i++)
                {
                    string ID = i.ToString();
                    if (weapon == i) { if (inventory.StockWeapons[i] == true) {hud.DisplayText($"< " + player.Name() + " already owns a " + Global.WEAPON_NAME(ID) + " >", false); } else { CompleteWeaponTransaction(Global.globalAccess.weaponIDs[ID], player, inventory); } }
                }
                /*else if (weapon == WEAPON.SHORTSWORD) { if (inventory.shortswordOwned == true) {hud.DisplayText($"< " + player.Name() + " already owns a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " >", false); } else { CompleteWeaponTransaction(WEAPON.SHORTSWORD, player, inventory); } }
                else if (weapon == WEAPON.BROADSWORD) { if (inventory.broadswordOwned == true) {hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " >", false); } else { CompleteWeaponTransaction(WEAPON.BROADSWORD, player, inventory); } }
                else if (weapon == WEAPON.LONGSWORD) { if (inventory.longswordOwned == true) {hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " >", false); } else { CompleteWeaponTransaction(WEAPON.LONGSWORD, player, inventory); } }
                else if (weapon == WEAPON.CLAYMORE) { if (inventory.claymoreOwned == true) {hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " >", false); } else { CompleteWeaponTransaction(WEAPON.CLAYMORE, player, inventory); } }
                else if (weapon == WEAPON.KALIBURN) { if (inventory.kaliburnOwned == true) {hud.DisplayText($"< You already own a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " >", false); } else { CompleteWeaponTransaction(WEAPON.KALIBURN, player, inventory); } }*/
            }
            if (tradeDecision.Key == ConsoleKey.D2)
            {
                player.EquipWeapon("0");
                for (int i = 0; i < Global.globalAccess.weaponIDs.Count; i++)
                {
                    string ID = i.ToString();
                    if (weapon == i) { if (inventory.StockWeapons[i] == false) { hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(ID) + " to sell! >", false); } else { daggerDamageMultiplier = 0; inventory.StockWeapons[i] = false; hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(ID) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                }
                /*if (weapon == WEAPON.DAGGER) { if (inventory.daggerOwned == false) {hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.DAGGER) + " to sell! >", false); } else { daggerDamageMultiplier = 0; inventory.daggerOwned = false; hud.DisplayText($"< " + player.Name() +" sold a " + Global.WEAPON_NAME(WEAPON.DAGGER) +" >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.SHORTSWORD) { if (inventory.shortswordOwned == false) { hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " to sell! >", false); } else { shortswordDamageMultiplier = 0; inventory.shortswordOwned = false; hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.BROADSWORD) { if (inventory.broadswordOwned == false) { hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " to sell! >", false); } else { broadswordDamageMultplier = 0; inventory.broadswordOwned = false; hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.LONGSWORD) { if (inventory.longswordOwned == false) { hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " to sell! >", false); } else { longswordDamageMultplier = 0; inventory.longswordOwned = false; hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.CLAYMORE) { if (inventory.claymoreOwned == false) { hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " to sell! >", false); } else { claymoreDamageMultiplier = 0; inventory.claymoreOwned = false; hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }
                else if (weapon == WEAPON.KALIBURN) { if (inventory.kaliburnOwned == false) { hud.DisplayText($"< " + player.Name() + " does not own a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " to sell! >", false); } else { kaliburnDamageMultiplier = 0; inventory.kaliburnOwned = false; hud.DisplayText($"< " + player.Name() + " sold a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " >", false); player.currentMoney = player.currentMoney + tradePrice; } }*/
            }
        }
        //these methods are for buying weapons and items (execute bigger things to handle) to prevent code clogging......
        private void CompleteWeaponTransaction(int weapon, Player player, Inventory inventory)
        {
            //buff chance is randomized if the set number is chosen player will recieve a buff on next boughten item
            buffChance = rnd.Next(1, buffSet + 1);
            if (player.currentMoney < tradePrice) {hud.DisplayText($"< " + player.Name() + " doesn't have enough money! >", false); }
            else 
            {
                if (buffChance == buffSet) { tradeBuff = 2; }
                for (int i = 0; i < Global.globalAccess.weaponIDs.Count; i++)
                {
                    string ID = i.ToString();
                    if (weapon == i) { inventory.StockWeapons[i] = true; hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(ID) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; daggerDamageMultiplier = tradeBuff; }
                }
                
                /*else if (weapon == WEAPON.SHORTSWORD) { inventory.shortswordOwned = true; hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.SHORTSWORD) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; shortswordDamageMultiplier = tradeBuff; }
                else if (weapon == WEAPON.BROADSWORD) { inventory.broadswordOwned = true; hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.BROADSWORD) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; broadswordDamageMultplier = tradeBuff; }
                else if (weapon == WEAPON.LONGSWORD) { inventory.longswordOwned = true; hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.LONGSWORD) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; longswordDamageMultplier = tradeBuff; }
                else if (weapon == WEAPON.CLAYMORE) { inventory.claymoreOwned = true; hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.CLAYMORE) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; claymoreDamageMultiplier = tradeBuff; }
                else if (weapon == WEAPON.KALIBURN) { inventory.kaliburnOwned = true; hud.DisplayText($"< " + player.Name() + " bought a " + Global.WEAPON_NAME(WEAPON.KALIBURN) + " with a X" + tradeBuff + " buff! >", false); player.currentMoney = player.currentMoney - tradePrice; kaliburnDamageMultiplier = tradeBuff; }*/
            }
        }
        private void CompleteItemTransaction(string ID, Player player, Inventory inventory)
        {
            buffChance = rnd.Next(1, buffSet + 1);
            if (player.currentMoney < tradePrice) { hud.DisplayText($"< " + player.Name() + " doesn't have enough money! >", false); }
            else
            {
                if (buffChance == buffSet) { tradeBuff = 2; }
                for (int i = 0; i < Global.globalAccess.itemIDs[ID]; i++)
                {
                    string checkedID = i.ToString();
                    if (ID == checkedID) { player.currentMoney = player.currentMoney - tradePrice; inventory.IncreaseStock(ID); hud.DisplayText($"< " + player.Name() + " bought a " + Global.ITEM_NAME(ID) + " >", false); }

                }
                
                
            }
        }
    }
}
