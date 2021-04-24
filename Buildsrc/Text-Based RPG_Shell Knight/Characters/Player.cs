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

        private int[] _shield; // displayed as Shell, set states: 0 = current, 1 = max, health follows the same number convention
        private Weapon equipedWeapon;

        //constructor
        public Player(string name, char avatar) : base(name, avatar, Global.PLAYER_HEALTH)
        {
            //init fields
            equipedWeapon = new Weapon(WEAPON.FISTS);
            int[] damageRange = equipedWeapon.DamageRange();
            _shield = new int[] { 6, Global.PLAYER_SHIELD };
            _damage = new int[] { damageRange[(int)RANGE.LOW], damageRange[(int)RANGE.HIGH] };

            //init spawn
            x = Global.PLAYER_SPAWNPOINT[0];
            y = Global.PLAYER_SPAWNPOINT[1]; // magic numbers explained in 

            this.aliveInWorld = true;
        }

        // ----- gets sets
        public Weapon EquipedWeapon()
        {
            return equipedWeapon;
        }
        public void EquipedWeaponDamageRange(int[] value)
        {
            _damage = value;
        }
        public int[] Shield()
        {
            return _shield;
        }
        
        // ----- Private Methods
        private void GetInput() // recieves player input from a single keypress
        {
            // inital read key to stop
            _playerInput = Console.ReadKey(true);
            // every key press after is read from this ReadKey and once it's stopped so is the console display
            while (Console.KeyAvailable)
            _playerInput = Console.ReadKey(true);
        }
        private void DirectionalOutput()// checks or 'looks at' the direction output of the playes input
        {
            if (_playerInput.Key == ConsoleKey.S || _playerInput.Key == ConsoleKey.DownArrow)
            {
                CheckDirection(DIRECTION.DOWN);
            }
            else if (_playerInput.Key == ConsoleKey.W || _playerInput.Key == ConsoleKey.UpArrow)
            {
                CheckDirection(DIRECTION.UP);
            }
            else if (_playerInput.Key == ConsoleKey.A || _playerInput.Key == ConsoleKey.LeftArrow)
            {
                CheckDirection(DIRECTION.LEFT);
            }
            else if (_playerInput.Key == ConsoleKey.D || _playerInput.Key == ConsoleKey.RightArrow)
            {
                CheckDirection(DIRECTION.RIGHT);
            }
            else // its possible not to move
            { 
                _directionMoving = DIRECTION.NULL;
                _XYHolder[0] = x;
                _XYHolder[1] = y;
            }
        }
        private GAMESTATE UseItemHotbarOutput(Item item, HUD hud, Inventory inventory, GAMESTATE gameState, Battle battle, Enemy enemy)// uses item if player inputs
        {

            // init values for easy reading
            char[] avatar = inventory.ItemAvatars();
            int[] stock = inventory.ItemStock();

            if (_playerInput.Key == ConsoleKey.I)
            {
                gameState = GAMESTATE.INVENTORY;
                return gameState;
            }

            //used to start and debug battles
            if (_playerInput.Key == ConsoleKey.P)
            {
                return battle.Begin(enemy, hud);
            }

            // Use Health Pot
            if (_playerInput.Key == ConsoleKey.X)
            {
                if (stock[(int)ITEM.POTHEAL] > 0)
                {
                    HealHealth(item.Power(avatar[(int)ITEM.POTHEAL]));
                    inventory.UseItem((int)ITEM.POTHEAL);

                    //update HUD bar and display to HUD text box
                    hud.setHudHealthAndShield(_health, _shield);
                    hud.Draw();// updates visible inventory
                    hud.UpdateHotBar(this, inventory);
                    hud.DisplayText($"< {_name} {Global.MESSAGE_POTHEALTHDRINK} >", false);
                    
                }
                else
                {
                    hud.DisplayText($"< {_name} {Global.MESSAGE_POTHEALTHMISSING} >", false);
                }
            }

            // Use Shield Pot
            if (_playerInput.Key == ConsoleKey.Z)
            {
                if (stock[(int)ITEM.POTSHELL] > 0)
                {
                    inventory.UseItem(ITEM.POTSHELL);
                    HealShell(item.Power(avatar[(int)ITEM.POTSHELL]));

                    //update HUD bar and display to HUD text box
                    hud.setHudHealthAndShield(_health, _shield);
                    hud.Draw();// updates visible inventory
                    hud.UpdateHotBar(this, inventory);
                    hud.DisplayText($"< {_name} {Global.MESSAGE_POTSHIELDDRINK} >", false);
                }
                else
                {
                    hud.DisplayText($"< {_name} {Global.MESSAGE_POTSHIELDMISSING} >", false);
                }
            }
            return gameState;
        }

        // ----- Public Methods
        public void HealHealth(int value)// restores health and values to limits if broken
        {
            int[] health = Health();
            health[(int)STATUS.CURRENT] += value;
            health[(int)STATUS.CURRENT] = SetStatToLimits(health[(int)STATUS.CURRENT], health[(int)STATUS.MAX]);
        }
        public void HealShell(int value)// restores shield and values to limits if broken
        {
            int[] shield = Shield();
            shield[(int)STATUS.CURRENT] += value;
            shield[(int)STATUS.CURRENT] = SetStatToLimits(shield[(int)STATUS.CURRENT], shield[(int)STATUS.MAX]);
        }
        new public void Draw(Camera camera) // Draws to HUD bar then calls base
        {   ///   Legacy
            ///if (map.getStateMap() == Map.MAP_MIDDLE)
            ///{
            ///    y = Map.displayHeight / 2;
            ///    x = Map.displayWidth / 2;
            ///}
            base.Draw(camera);
            ///toolkit.DisplayText("drawing");
        }
        public GAMESTATE Update(List<Enemy> enemies, List<Door> doors, Map map, Camera camera, List<Item> item, HUD hud, Toolkit toolkit, Inventory inventory, GAMESTATE gameState, Battle battle)
        {

            if (aliveInWorld)
            {

                // gets Input
                GetInput();

                // "Looks at" selected direction
                DirectionalOutput();

                // used in place of Directional, only Healing items currently

                gameState = UseItemHotbarOutput(item[0], hud, inventory, gameState, battle, enemies[0]);// item is passed in blank because it's recognized inside the method this is for typing and can probably be done better
                
                //check everything for collision
                bool collision = false;

                //Enemy Collision / Attacking
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (CheckForCharacterCollision(enemies[i].X(), enemies[i].Y(), enemies[i].AliveInWorld())) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                    {
                        //access weapons damage

                        //collide
                        collision = true;

                        //collide to deal damage
                        gameState = StartAttacking(enemies[i].AliveInWorld(), battle, enemies[i], gameState, hud);

                        //Stops character update and ends game
                        enemies[i].CheckForDying(camera, hud);
                    }
                }

                //Door Collision / Unlocking
                for (int i = 0; i < doors.Count; i++)
                {
                    if (CheckForCharacterCollision(doors[i].X(), doors[i].Y(), doors[i].AliveInWorld()))
                    {
                        // Use Key or not
                        doors[i].OpenDoor(this, hud, inventory);

                        //collide
                        collision = true;
                    }
                }

                //toolkit.DisplayText($"checking at: {map.getTile(_XYHolder[0], _XYHolder[1])}"); //    --------- debugg
                if (!CheckForWallCollision(map.getTile(_XYHolder[0], _XYHolder[1] - 1), map.getWallHold()))
                {
                    if (!collision)
                    {
                        Move();
                    }
                }
                
            }

            return gameState;
        }
    }
}


