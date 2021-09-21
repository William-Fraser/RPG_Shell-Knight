using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    public enum RANGE // used with RANGE data like damage / range data is often used with the random method in toolkit
    {
        LOW,
        HIGH
    }
    class Battle
    {
        string[,] _display = new string[Camera.borderHeight, Camera.borderWidth];
        Camera camera;
        HUD hud;
        Enemy opponent;

        bool turn; // true if player's turn
        //constructor
        public Battle()
        {
            camera = new Camera();
        }

        // ----- private methods
        private void Fight(Player player, Enemy enemy, Item item, Inventory inventory) // turn based combat, one starting fight attacks first
        {
            int damage;
            if (turn)
            {
                // replaced by input class methods
                ConsoleKeyInfo battleDecision = Console.ReadKey(true);
                if (battleDecision.Key == ConsoleKey.Spacebar)
                {
                    damage = player.DealDamage(enemy.Health(), hud, true) * player.damageMultiplier;
                    player.DisplayDamageToHUD(enemy.Name(), damage, enemy.Health(), hud, true);
                }
                
                if (battleDecision.Key == ConsoleKey.X)
                {
                    inventory.UseHealthPot(player, item, hud);
                }

                if (battleDecision.Key == ConsoleKey.Z)
                {
                    inventory.UseShieldPot(player, item, hud);
                }
                turn = false;
            }
            else
            {
                damage = enemy.DealDamage(player.Health(), hud, false, player.Shield());
                enemy.DisplayDamageToHUD(player.Name(), damage, player.Health(), hud, false);

                turn = true;
            }
        }
        // display visuals method?
        private GAMESTATE End() // stops the battle sequence / player victory
        {
            // if enemy dies
            return GAMESTATE.MAP;
        }
        private GAMESTATE GameOver() // on player death
        {
            return GAMESTATE.GAMEOVER;
        }

        // ----- public methods
        public void Draw() 
        {
            hud.Draw();
            camera.DrawBorder();
        }
        public GAMESTATE Begin(Player player, Enemy enemy, Inventory inventory, bool isPlayer = false)
        {
            turn = isPlayer;
            opponent = enemy;
            hud = new HUD(player.Name());
            Console.Clear();
            hud.Update(player, inventory);
            hud.DrawTextBox();
            return GAMESTATE.BATTLE;
        }
        public GAMESTATE Update(Player player, GAMESTATE gameState, Item item, Inventory inventory)
        {
            hud.Update(player, inventory);
            Fight(player, opponent, item, inventory);

            // ends fight
            if (player.Health()[(int)STATUS.CURRENT] == 0)
            { return GameOver(); }
            if (opponent.Health()[(int)STATUS.CURRENT] == 0)
            {   opponent.CheckForDying(camera, hud);
                return End(); }
            return gameState;
        }
    }
}
