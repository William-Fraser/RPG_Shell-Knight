using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Battle
    {
        Camera camera;
        string[,] _display = new string[Camera.borderHeight, Camera.borderWidth];
        Enemy opponent;

        bool turn; // true if player's turn
        public Battle()
        {
            camera = new Camera();
        }
        private void BattleDisplay() // graphical design for battles
        { 
        
        }

        public GAMESTATE Begin(Enemy enemy, HUD hud)
        {
            opponent = enemy;
            Console.Clear();
            hud.UpdateTextBox();
            return GAMESTATE.BATTLE;
        }
        public GAMESTATE BattleController(Player player, HUD hud, Toolkit toolkit, GAMESTATE gameState, Item item, Inventory inventory)
        {
            Fight(player, opponent, hud, toolkit, item, inventory);
            
            // ends fight
            if (player.Health()[(int)STATUS.CURRENT] == 0)
            { return GameOver(); }
            if (opponent.Health()[(int)STATUS.CURRENT] == 0)
            { return End(); }
            return gameState;
        }
        public void drawBorder() 
        {
            camera.DrawBorder();
        }
        public void Fight(Player player, Enemy enemy, HUD hud,Toolkit toolkit, Item item, Inventory inventory) // turn based combat, one starting fight attacks first
        {
            int damage;
            if (turn)
            {
                // replaced by input class methods
                ConsoleKeyInfo battleDecision = Console.ReadKey(true);
                if (battleDecision.Key == ConsoleKey.Spacebar)
                {
                    damage = player.DealDamage(enemy.Health(), hud, toolkit, true);
                    player.DisplayDamageToHUD(enemy.Name(), damage, enemy.Health(), hud, true);
                }
                else { }

                while (Console.KeyAvailable)
                {
                    if (battleDecision.Key == ConsoleKey.X && inventory.ItemStock()[(int)ITEM.POTHEAL] > 0)
                    {
                        player.HealHealth(item.Power(inventory.ItemAvatars()[(int)ITEM.POTHEAL]));
                        inventory.UseItem((int)ITEM.POTHEAL);
                        hud.DisplayText($"< {player.Name()} {Global.MESSAGE_POTHEALTHDRINK} >", false);

                    }
                    else
                    {
                        hud.DisplayText($"< {player.Name()} {Global.MESSAGE_POTHEALTHMISSING} >", false);
                    }

                    if (battleDecision.Key == ConsoleKey.Z && inventory.ItemStock()[(int)ITEM.POTHEAL] > 0)
                    {
                        player.HealShell(item.Power(inventory.ItemAvatars()[(int)ITEM.POTSHELL]));
                        inventory.UseItem(ITEM.POTSHELL);
                        hud.DisplayText($"< {player.Name()} {Global.MESSAGE_POTSHIELDDRINK} >", false);
                    }
                    else
                    {
                        hud.DisplayText($"< {player.Name()} {Global.MESSAGE_POTSHIELDMISSING} >", false);
                    }
                }
            turn = false;
            }
            else
            {
                damage = enemy.DealDamage(player.Health(), hud, toolkit, false, player.Shield());
                enemy.DisplayDamageToHUD(player.Name(), damage, player.Health(), hud, false);

                turn = true;
            }
        }
        public GAMESTATE End() // stops the battle sequence / player victory
        {
            // if enemy dies
            return GAMESTATE.MAP;
        }
        public GAMESTATE GameOver() // on player death
        {

            return GAMESTATE.GAMEOVER;
        }
        
    }
}
