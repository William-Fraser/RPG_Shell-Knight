using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    public enum DIRECTION //direction used for movement
    {
        NULL,
        UP,
        RIGHT,
        DOWN,
        LEFT
    };
    public enum STATUS // status information accessors
    {
        CURRENT,
        MAX,
    };
    abstract class Character : Object
    {
        /// Initialization
        public int currentMoney;
        // direction the character "looks at"
        protected private int[] _XYHolder = new int[2]; // 0 = X, 1 = Y

        // Moving that direction
        protected private DIRECTION _directionMoving;  

        //character fields.
        protected private int[] _health = new int[2]; // set states: 0 = current health / 1 = max health
        protected private int[] _damage = new int[2]; // set range: 0 = Lowest / 1 = Highest // Range is only calculated once and therefore doesnt need state machine

        //constructor
        public Character(string name, char avatar, int health)
        {
            //init of properties
            _name = name;
            _avatar = avatar;
            _health = new int[] { health, health };
            _XYHolder[0] = 1;
            _XYHolder[1] = 1;

            //set position
            x = Console.WindowWidth / 2;
            y = Console.WindowHeight / 2;

            //set alive
            this.aliveInWorld = true;
        }

        // ----- gets / sets 
        public int[] Health()
        {
            return _health;
        } // get
        public int[] XYHolder() 
        {
            return _XYHolder;
        } // get

        // ----- internal sets
        protected private int SetStatToLimits(int currentStatus, int maxStatus) // checks if stats are overlimit and resets them to their bounds
        {
            //case no fixes needed
            int fixedStatus = currentStatus;

            //fixes under limit
            if (currentStatus < 0){
                fixedStatus = 0;
            } 

            //fixes over limit
            else if (currentStatus >= maxStatus){
                fixedStatus = maxStatus;
            } 
            
            return fixedStatus;
        }

        // ----- private methods
        protected private void CheckDirection(DIRECTION DIRECTION_) // "Looks at" specified direction if alive, else do nothing
        {
            //initalizes & re-inits holders for modifying
            int XHolder = x;
            int YHolder = y;

            //start Reading Input
            if      (DIRECTION_ == DIRECTION.DOWN)  { _directionMoving = DIRECTION.DOWN; _XYHolder[0] = XHolder; _XYHolder[1] = YHolder += 1; }
            else if (DIRECTION_ == DIRECTION.UP)    { _directionMoving = DIRECTION.UP; _XYHolder[0] = XHolder; _XYHolder[1] = YHolder -= 1; }
            else if (DIRECTION_ == DIRECTION.LEFT)  { _directionMoving = DIRECTION.LEFT; _XYHolder[0] = XHolder -= 1; _XYHolder[1] = YHolder; }
            else if (DIRECTION_ == DIRECTION.RIGHT) { _directionMoving = DIRECTION.RIGHT; _XYHolder[0] = XHolder += 1; _XYHolder[1] = YHolder; }
        }
        protected private void Move() // moves the character and stops them at boundaries if alive, else do nothing
        {
            //finalize Reading Input
            if      (_directionMoving == DIRECTION.DOWN && _XYHolder[1] < Map.height + 1) { y++; }
            else if (_directionMoving == DIRECTION.UP && _XYHolder[1] > 1)                { y--; }
            else if (_directionMoving == DIRECTION.LEFT && _XYHolder[0] > 0)              { x--; }
            else if (_directionMoving == DIRECTION.RIGHT && _XYHolder[0] < Map.width + 1) { x++; }
        }
        protected private void KillCharacter(string name, Camera camera, HUD hud) // kills character and displays that to HUD holds gameComepletionMessage
        {
            aliveInWorld = false;

            //show dead Charcter
            _avatar = Global.CHARACTER_DEADAVATAR;
            Draw(camera); //immediately update sprite

            //draw death to HUD
            string deathMessage = $"< {name} {Global.MESSAGE_SLAIN} >";
            hud.DisplayText(deathMessage);
        }
        new public void Draw(Camera camera) // Draws character to game world
        {
            camera.GameWorldTile(_avatar, x, y);
        }
        protected private GAMESTATE StartAttacking(bool victimIsAlive, Battle battle, Player player, Enemy enemy, GAMESTATE gameState, Inventory inventory) // attacks if colliding character is alive, else do nothing
        {
            if (victimIsAlive)
            {
                gameState = battle.Begin(player, enemy, inventory);
            }
            return gameState;
        }
        protected private GAMESTATE StartTrading(bool vendorIsAlive, TradeMenu tradeMenu, Player player, Vendor vendor, GAMESTATE gameState, Inventory inventory) // attacks if colliding character is alive, else do nothing
        {
            if (vendorIsAlive)
            {
                gameState = tradeMenu.Begin(player, vendor, inventory);
            }
            return gameState;
        }

        // character checks
        protected private bool CheckForWallCollision(char map, string walls) // checks for collision and returns true is collides, else false
        {
            bool iswall = false;
            //only check if moving due to if  alive = false  then  Moving = null
            if (_directionMoving != DIRECTION.NULL)
            {
                //reads wall info
                char[] wallGroup = new char[walls.Length];
                for (int i = 0; i < walls.Length; i++)
                {
                    wallGroup[i] = walls[i];
                }

                //checks each wall for collision
                for (int i = 0; i < wallGroup.Length; i++)
                {
                    //toolkit.DisplayText(wallGroup[i].ToString()); // debug
                    if (map == wallGroup[i])
                    {
                        
                        iswall = true; // collision
                    }
            
                }
            }
            return iswall;
        }
        protected private bool CheckForCharacterCollision(int collidingX, int collidingY, bool alive) // checks for collision and returns true if collides, else false *only checks if alive, else do nothing
        {
            if (alive)
            {
                ///LEGACY  aka: wrong way to do it :)
                ///if (_directionMoving == DIRECTION_UP) { playerY--; }
                ///else if (_directionMoving == DIRECTION_DOWN) { playerY++; }
                ///else if (_directionMoving == DIRECTION_LEFT) { playerX--; }
                ///else if (_directionMoving == DIRECTION_RIGHT) { playerX++; }
                ///else { }
                if (_XYHolder[0] == collidingX)
                {
                    if (_XYHolder[1] == collidingY)
                    {
                        return true; // collison
                    }
                }
                
            }
            return false;
        }

        // ----- public methods

        public void DisplayDamageToHUD(string name, int attackDamage, int[] health, HUD hud, bool isEnemy) // translates damage done into parsable message and displays it in the HUD
        { // holds its own message due to complexity in display not much vaiation visible = no desire to modulate
            health[(int)STATUS.CURRENT] = SetStatToLimits(health[(int)STATUS.CURRENT], health[(int)STATUS.MAX]);
            string damageMessage = $"< {name} taking {attackDamage} points of Damage ";
            if (isEnemy)
            { damageMessage += $"[{health[(int)STATUS.CURRENT]}/{health[(int)STATUS.MAX]}] >"; }
            else
            { damageMessage += $">"; }
            hud.DisplayText(damageMessage, false);
        }
        public int DealDamage(int[] health, HUD ui, bool isEnemy, int[] shield = null) // calculates damage and deals it out
        {
            int damageToHealth = -1;

            //calculate damage
            damageToHealth = Toolkit.RandomNumBetween(_damage[0], _damage[1]); //random runber between a high and low value
            int damageToShield = damageToHealth; //names for convenience and to display to HUD textbox
            int passDamage = 0; // null if not passing

            if (shield != null) /// this if statement could alternatively be in enemy.dealdamage() but it was moved here and now makes more sense
            {

                // calculate spill damage
                int calcDamageSpill = damageToShield - shield[(int)STATUS.CURRENT];

                // deal damage to player shield 
                shield[(int)STATUS.CURRENT] -= damageToShield;

                // passes spill damage and sets stat to limit
                if (shield[(int)STATUS.CURRENT] < 0) { passDamage = calcDamageSpill; }
                shield[(int)STATUS.CURRENT] = SetStatToLimits(shield[(int)STATUS.CURRENT], shield[(int)STATUS.MAX]);
            }

            if (shield == null || shield[(int)STATUS.CURRENT] == 0)
            {
                // calculate damage
                if (passDamage != 0) { damageToHealth = passDamage; } // sets the damage to the leftover shield break damage

                //deal damage to character health and checks the limits
                health[(int)STATUS.CURRENT] -= damageToHealth;
                health[(int)STATUS.CURRENT] = SetStatToLimits(health[(int)STATUS.CURRENT], health[(int)STATUS.MAX]); // does nothing if limit doesnt break
            }

            if (!isEnemy)
            {
                //update player HUD bar
                ui.setHudHealthAndShield(health, shield);
                ui.Draw();
            }

            //sets damage to total amount of damage done if it spills into health
            if (passDamage != 0) { damageToHealth = damageToShield; }
            return damageToHealth;
        }
        public void CheckForDying(Camera camera, HUD hud) // checks if character is alive and at 0 health, then kills them
        { // if 0HP then death time
            if (aliveInWorld)
            {
                if (_health[(int)STATUS.CURRENT] <= 0)
                {
                    KillCharacter(_name, camera, hud);
                }
            }
        }
    }
}
