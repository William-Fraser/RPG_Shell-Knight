using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Player : Character
    {
        public ConsoleKeyInfo _playerInput;
        public int damageMultiplier;
        public int effectMultiplier;

        private int[] _shield; // displayed as Shell, set states: 0 = current, 1 = max, health follows the same number convention
        private Weapon equipedWeapon;

        //constructor
        public Player(string name, char avatar) : base(name, avatar, Global.PLAYER_HEALTH)
        {
            //init fields
            equipedWeapon = new Weapon(WEAPON.FISTS);
            int[] damageRange = equipedWeapon.DamageRange();
            _shield = new int[] { Global.PLAYER_SHIELD, Global.PLAYER_SHIELD };
            _damage = new int[] { damageRange[(int)RANGE.LOW], damageRange[(int)RANGE.HIGH] };
            currentMoney = 200;
            

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
        public void EquipWeapon(WEAPON w)
        {
            equipedWeapon.IdentifyAndEquip(w);
            _damage = equipedWeapon.DamageRange();
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
        private GAMESTATE OpenInventoryOutput(Inventory inventory, GAMESTATE gameState, Battle battle, Enemy enemy)
        {
            if (_playerInput.Key == ConsoleKey.E)
            {
                inventory.InitInventory(this, gameState);
                gameState = GAMESTATE.INVENTORY;
                return gameState;
            }

            //used to start and debug battles
            if (_playerInput.Key == ConsoleKey.P)
            {
                return battle.Begin(this, enemy, inventory, true);
            }

            return gameState;
        }
        private GAMESTATE UseItemOutput(Item item, HUD hud, Inventory inventory, GAMESTATE gameState)// uses item if player inputs
        {
            // Use Health Pot
            if (_playerInput.Key == ConsoleKey.X)
            {
                inventory.UseHealthPot(this, item, hud);
            }

            // Use Shield Pot
            if (_playerInput.Key == ConsoleKey.Z)
            {
                inventory.UseShieldPot(this, item, hud);
            }
            return gameState;
        }

        // ----- Public Methods
        public void HealHealth(int value, Inventory inventory, HUD hud)// restores health and values to limits if broken
        {
            int[] health = Health();
            if (inventory.buffedHealthPotions <= 0) { health[(int)STATUS.CURRENT] += value; hud.DisplayText($"< {Name()} {Global.MESSAGE_POTHEALTHDRINK} [+" + value + "] >", false); }
            else { health[(int)STATUS.CURRENT] += value * effectMultiplier; inventory.buffedHealthPotions--; hud.DisplayText($"< {Name()} {Global.MESSAGE_POTHEALTHDRINK} [+" + value + " X" + effectMultiplier + "] >", false); }
            health[(int)STATUS.CURRENT] = SetStatToLimits(health[(int)STATUS.CURRENT], health[(int)STATUS.MAX]);
        }
        public void HealShell(int value, Inventory inventory, HUD hud)// restores shield and values to limits if broken
        {
            int[] shield = Shield();
            if (inventory.buffedShellHeal <= 0) { shield[(int)STATUS.CURRENT] += (value - 20); hud.DisplayText($"< {Name()} {Global.MESSAGE_POTSHIELDDRINK} [+" + (value-20) + "] >", false);}
            else { shield[(int)STATUS.CURRENT] += (value - 20) * effectMultiplier; inventory.buffedShellHeal--; hud.DisplayText($"< {Name()} {Global.MESSAGE_POTSHIELDDRINK} [+" + (value-20) + " X" + effectMultiplier + "] >", false); }
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
        public GAMESTATE Update(List<Enemy> enemies, List<Door> doors, List<Item> item, Map map, Camera camera, HUD hud, Battle battle, Inventory inventory, GAMESTATE gameState, List<Vendor> vendors, TradeMenu tradeMenu)
        {

            if (aliveInWorld)
            {
                CheckForDying(camera, hud);

                // gets Input
                GetInput();

                // "Looks at" selected direction
                DirectionalOutput();

                // used in place of Directional, Opens inventory (and debug battle if not disabled)
                gameState = OpenInventoryOutput(inventory, gameState, battle, enemies[0]); // battle and enemies used for debug

                // used in place of Directional, only Healing items currently
                gameState = UseItemOutput(item[0], hud, inventory, gameState);// item is passed in blank because it's recognized inside the method this is for typing and can probably be done better
                
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
                        gameState = StartAttacking(enemies[i].AliveInWorld(), battle, this, enemies[i], gameState, inventory);
                    }
                }
                for (int i = 0; i < vendors.Count; i++)
                {
                    if (CheckForCharacterCollision(vendors[i].X(), vendors[i].Y(), vendors[i].AliveInWorld())) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                    {
                        //access weapons damage

                        //collide
                        //collision = true;

                        hud.Draw();
                        hud.DisplayText($"< This is a Vendor, Press 'T' to Trade >", false);

                        if (_playerInput.Key == ConsoleKey.T)
                        {
                            gameState = StartTrading(vendors[i].AliveInWorld(), tradeMenu, this, vendors[i], gameState, inventory);
                            
                        }
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


