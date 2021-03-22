using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Player : Character
    {
        private ConsoleKeyInfo _playerInput;

        private int[] _shield; // displayed as armour, 0 = current, 1 = maxa

        //constructor
        public Player(string name, char avatar, HUD ui) : base(name, avatar, 100)
        {
            _shield = new int[] { 50, 50 };

            _damage = new int[] { 50, 75 };

            y = Camera.displayHeight / 2;
            x = Camera.displayWidth / 2;

            this.aliveInWorld = true;
        }

        // ----- gets sets
        public int[] Shield()
        {
            return _shield;
        }
        
        // ----- Private Methods
        private void GetInput()
        {
            if (aliveInWorld)
            {
                _playerInput = Console.ReadKey(true);
                while (Console.KeyAvailable)
                _playerInput = Console.ReadKey(true);
            }
        }
        private void DirectionalOutput() 
        {
            if (_playerInput.Key == ConsoleKey.S || _playerInput.Key == ConsoleKey.DownArrow)
            {
                CheckDirection(DIRECTION_DOWN);
            }
            else if (_playerInput.Key == ConsoleKey.W || _playerInput.Key == ConsoleKey.UpArrow)
            {
                CheckDirection(DIRECTION_UP);
            }
            else if (_playerInput.Key == ConsoleKey.A || _playerInput.Key == ConsoleKey.LeftArrow)
            {
                CheckDirection(DIRECTION_LEFT);
            }
            else if (_playerInput.Key == ConsoleKey.D || _playerInput.Key == ConsoleKey.RightArrow)
            {
                CheckDirection(DIRECTION_RIGHT);
            }
            else
            { 
                _directionMoving = DIRECTION_NULL;
                _XYHolder[0] = x;
                _XYHolder[1] = y;
            }
        }
        private bool CheckForCharacterCollision(Enemy enemy)
        {
            bool collision;
            collision = base.CheckForCharacterCollision(enemy.X(), enemy.Y(), enemy.AliveInWorld());
            return collision;
        }
        private void DealDamage(Enemy enemy, HUD ui, Toolkit toolkit)
        {
            base.DealDamage(enemy.Name(), enemy.AliveInWorld(), enemy.Health(), true, ui, toolkit);
        }
        private void HealHealth(int value)
        {
            int[] health = Health();
            health[0] += value;
            health[0] = setStatToLimits(health[0], health[1]);
        }
        private void HealShell(int value)
        {
            int[] shield = Shield();
            shield[0] += value;
            shield[0] = setStatToLimits(shield[0], shield[1]);
        }
        private void RemoveItemFromInventory(int index, HUD ui)
        {
            int[] stock = ui.getInventoryStock();
            ui.setInventoryStockItem(index, stock[index]-1);
        }
        private void UseItemInventory(Item item, HUD hud, Toolkit toolkit)
        {
            char[] avatar = hud.getInventoryAvatars();
            int[] stock = hud.getInventoryStock();
            if (_playerInput.Key == ConsoleKey.X)
            {
                if (stock[HUD.ITEM_POTHEAL] > 0)
                {
                    HealHealth(item.Power(avatar[HUD.ITEM_POTHEAL]));
                    RemoveItemFromInventory(HUD.ITEM_POTHEAL, hud);
                    hud.setHudHealthAndShield(_health, _shield);
                    hud.Draw(toolkit);
                    hud.DisplayText($"< {_name} drinks a Health Potion [+50 HP] >");
                }
                else
                {
                    hud.DisplayText($"< {_name} looked for a HealthPotion but found none >", false);
                }
            }
            if (_playerInput.Key == ConsoleKey.Z)
            {
                if (stock[HUD.ITEM_POTSHELL] > 0)
                {
                    RemoveItemFromInventory(HUD.ITEM_POTSHELL, hud);
                    HealShell(item.Power(avatar[HUD.ITEM_POTSHELL]));
                    hud.setHudHealthAndShield(_health, _shield);
                    hud.Draw(toolkit);
                    hud.DisplayText($"< {_name} used some Shell Banding [+30 SP] >");
                }
                else
                {
                    hud.DisplayText($"< {_name} is fresh out of Shell Banding >", false);
                }
            }
        }
        
        // ----- Public Methods
        public void Draw(Map map, Camera camera, HUD ui, Toolkit toolkit) 
        {
            ui.Draw(toolkit);
            //if (map.getStateMap() == Map.MAP_MIDDLE)
            //{
            //    y = Map.displayHeight / 2;
            //    x = Map.displayWidth / 2;
            //}
            base.Draw(camera);
            //toolkit.DisplayText("drawing");
        }
        public void Update(List<Enemy> enemy, Map map, Camera camera, List<Item> item, HUD hud, Toolkit toolkit) {
            //toolkit.DisplayText(toolkit.blank);// clears the text after it's been displayed once - LEGACYCODE
            KillIfDead(camera, map, hud);
            
            GetInput();

            DirectionalOutput();
            UseItemInventory(item[0], hud, toolkit);// item is pass in blank because it's recognized inside the method


            //check each enemy for collision
            bool collision = false;
            for (int i = 0; i < enemy.Count; i++)
            {
                if (CheckForCharacterCollision(enemy[i])) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                {
                    collision = true;
                    DealDamage(enemy[i], hud, toolkit);
                }
            }

            //toolkit.DisplayText($"checking at: {map.getTile(_XYHolder[0], _XYHolder[1])}");
            if (!CheckForWall(map.getTile(_XYHolder[0], _XYHolder[1]-1), map.getWallHold()))
            {
                if (!collision)
                {
                    Move();
                }
            }

        }
    }
}
