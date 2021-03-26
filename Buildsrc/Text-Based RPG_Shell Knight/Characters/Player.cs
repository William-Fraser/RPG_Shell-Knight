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
        public Player(string name, char avatar, HUD ui) : base(name, avatar, Global.PLAYER_HEALTH)
        {

            _shield = new int[] { Global.PLAYER_SHIELD, Global.PLAYER_SHIELD };
            _damage = new int[] { Global.PLAYER_DAMGERANGE[0], Global.PLAYER_DAMGERANGE[1] };

            y = 56;
            x = 9;

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
        private bool CheckForCharacterCollision(Door door)
        {
            bool collision;
            collision = base.CheckForCharacterCollision(door.X(), door.Y(), door.AliveInWorld());
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
        
        private void UseItemInventoryOutput(Item item, HUD hud, Toolkit toolkit)
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
        public void RemoveItemFromInventory(int index, HUD ui)
        {
            int[] stock = ui.getInventoryStock();
            ui.setInventoryStockItem(index, stock[index] - 1);
        }
        public void Update(List<Enemy> enemies, List<Door> doors, Map map, Camera camera, List<Item> item, HUD hud, Toolkit toolkit) {

            //Stops character update and ends game
            KillIfDead(camera, map, hud);
            
            // gets Input
            GetInput();

            // "Looks at" selected direction
            DirectionalOutput();

            // used in place of Directional, only Healing items currently
            UseItemInventoryOutput(item[0], hud, toolkit);// item is passed in blank because it's recognized inside the method this is for typing and can probably be done better


            //check everything for collision
            bool collision = false;
            
            //Enemy Collision / Attacking
            for (int i = 0; i < enemies.Count; i++)
            {
                if (CheckForCharacterCollision(enemies[i])) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                {
                    //collide
                    collision = true;
                    
                    //collide to deal damage
                    DealDamage(enemies[i], hud, toolkit);
                }
            }

            //Door Collision / Unlocking
            for (int i = 0; i < doors.Count; i++)
            {
                if (CheckForCharacterCollision(doors[i]))
                {
                    //collide
                    collision = true;

                    // Use Key or not
                    doors[i].OpenDoor(this, hud, toolkit);
                }
            }

                //toolkit.DisplayText($"checking at: {map.getTile(_XYHolder[0], _XYHolder[1])}"); //    --------- debugg
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


