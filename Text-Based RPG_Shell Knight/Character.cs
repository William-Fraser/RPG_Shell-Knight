using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    abstract class Character : Object
    {
        ///INIT
        //directional States
        protected private int[] _XYHolder = new int[2]; // 0 = X, 1 = Y
        protected private int _directionMoving;
        protected static int DIRECTION_NULL = 0;
        protected static int DIRECTION_UP = 1;
        protected static int DIRECTION_RIGHT = 2;
        protected static int DIRECTION_DOWN = 3;
        protected static int DIRECTION_LEFT = 4;

        //character fields.
        protected private int[] _health = new int[2]; // set: 0 = current health / 1 = max health
        protected private int[] _damage = new int[2]; // set range: 0 = Lowest / 1 = Highest

        //constructor
        public Character(string name, char avatar, int health)
        {
            _name = name;
            _avatar = avatar;
            _health = new int[] { health, health };

            x = Console.WindowWidth / 2;
            y = Console.WindowHeight / 2;

            this.aliveInWorld = true;
        }

        // ----- gets
        public int[] Health()
        {
            return _health;
        }
        public void Health(int value)
        {
            _health[0] = value;
            _health[0] = setStatToLimits(_health[0], _health[1]);
        }
        public int[] getDamage()
        {
            return _damage;
        }

        // ----- internal sets
        protected private int setStatToLimits(int currentStatus, int maxStatus)
        {
            int fixedStatus = currentStatus;
            if (currentStatus < 0){
                fixedStatus = 0;
            } else if (currentStatus >= maxStatus){
                fixedStatus = maxStatus;
            } return fixedStatus;
        }

        /// ----- private methods
        //protected private void MoveBack() // moves player back if they're supposed to collide with something
        //{
        //    if (_directionMoving == DIRECTION_UP) { Move(DIRECTION_DOWN); }
        //    else if (_directionMoving == DIRECTION_DOWN) { Move(DIRECTION_UP); }
        //    else if (_directionMoving == DIRECTION_LEFT) { Move(DIRECTION_RIGHT); }
        //    else if (_directionMoving == DIRECTION_RIGHT) { Move(DIRECTION_LEFT); }
        //    else { }
        //}
        private void KillCharacter(string name, Toolkit toolkit)
        {
            if (_health[0] <= 0) {
                _avatar = 'X';
                Draw();
                string deathMessage = $"< {name} has been slain >";
                aliveInWorld = false;
                toolkit.DisplayText(deathMessage, false);
            }
        }
        private int Attack(int[] health, UI ui, Toolkit toolkit, bool isEnemy, int[] shield = null)
        {
            int finalDamage = toolkit.RandomNumBetween(_damage[0], _damage[1]);
            if (aliveInWorld)
            {
                int passDamage = 0;
                if (shield != null) /// this if statement could alternatively be in enemy.dealdamage()
                {
                    // calculate damage
                    int firstDamage = toolkit.RandomNumBetween(_damage[0], _damage[1]);

                    // deal damage to player shield 
                    shield[0] -= firstDamage;
                    if (shield[0] < 0) { passDamage = (shield[0] * -1); }
                    shield[0] = setStatToLimits(shield[0], shield[1]);
                }
                else if (!isEnemy) { shield[0] = 0; }
                if (shield == null || shield[0] == 0)
                {
                    // calculate damage
                    if (passDamage != 0) { finalDamage = passDamage; } // sets the damage to the leftover shield break damage

                    //deal damage to character health
                    health[0] -= finalDamage;
                    health[0] = setStatToLimits(health[0], health[1]);

                    //update HUD
                    ui.getHUDvalues(health, shield);
                }
            }
            else
            {
                //err character should not attack if dead
                finalDamage = -1;
            }
            return finalDamage;
        }
        protected private void DisplayDamageToHUD(string name, int attackDamage, int[] health, Toolkit toolkit, bool isEnemy)
        {
            health[0] = setStatToLimits(health[0], health[1]);
            string damageMessage = $"< {name} taking {attackDamage} points of Damage ";
            if (isEnemy)
            { damageMessage += $"[{health[0]}/{health[1]}] >"; }
            else 
            { damageMessage += $">"; }
            toolkit.DisplayText(damageMessage);
        }

        // ----- public methods
        new public void Draw()
        {
            Console.SetCursorPosition(x, y);
            Console.Write(_avatar);
        }
        public void CheckDirection(int DIRECTION_) //moves the player in the specifyed DICRECTION_ 
        {
            if (aliveInWorld)
            {
                int XHolder = x;
                int YHolder = y;
                if      (DIRECTION_ == DIRECTION_DOWN) { _directionMoving = DIRECTION_DOWN; _XYHolder[0] = XHolder; _XYHolder[1] = YHolder+=1;}
                else if (DIRECTION_ == DIRECTION_UP)    { _directionMoving = DIRECTION_UP; _XYHolder[0] = XHolder; _XYHolder[1] = YHolder-=1; }
                else if (DIRECTION_ == DIRECTION_LEFT) { _directionMoving = DIRECTION_LEFT; _XYHolder[0] = XHolder-=1; _XYHolder[1] = YHolder; } 
                else if (DIRECTION_ == DIRECTION_RIGHT) { _directionMoving = DIRECTION_RIGHT; _XYHolder[0] = XHolder+=1; _XYHolder[1] = YHolder; } 
            }
            
        }
        public void Move() // also stops character at boundaries        // change bounds to border for screen scrolling?
        {
            if (aliveInWorld)
            {
                if      (_directionMoving == DIRECTION_DOWN && _XYHolder[1] < Map.height)        { y++; }
                else if (_directionMoving == DIRECTION_UP && _XYHolder[1] > 2)                   { y--; }
                else if (_directionMoving == DIRECTION_LEFT && _XYHolder[0] > 0)                 { x--; }
                else if (_directionMoving == DIRECTION_RIGHT && _XYHolder[0] < Map.width - 1)    { x++; }
            }
        }
        public bool CheckForWall(char map, string walls)
        {
            bool iswall = false;
            if (_directionMoving != DIRECTION_NULL)
            {
                char[] wallGroup = new char[walls.Length];
                for (int i = 0; i < walls.Length; i++)
                {
                    wallGroup[i] = walls[i];
                }
                for (int i = 0; i < wallGroup.Length; i++)
                {
                    //toolkit.DisplayText(wallGroup[i].ToString());
                    if (map == wallGroup[i])
                    {
                        //MoveBack();
                        iswall = true;
                    }
                }
            }
            return iswall;
        }
        public bool CheckForCharacterCollision(int collidingX, int collidingY, bool alive)
        {// collides with objects, seperate from map wall collision
            if (alive)
            {
                //if (_directionMoving == DIRECTION_UP) { playerY--; }
                //else if (_directionMoving == DIRECTION_DOWN) { playerY++; }
                //else if (_directionMoving == DIRECTION_LEFT) { playerX--; }
                //else if (_directionMoving == DIRECTION_RIGHT) { playerX++; }
                //else { }
                if (_XYHolder[0] == collidingX)
                {
                    if (_XYHolder[1] == collidingY)
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }// collides with objects
        public void DealDamage(string name, bool alive, int[] health, bool isEnemy, UI ui, Toolkit toolkit, int[] shield = null)
        {
            if (alive)
            {   // Attack() method is the Damage Dealer
                DisplayDamageToHUD(name, Attack(health, ui, toolkit, isEnemy, shield), health, toolkit, isEnemy);
            }
        }
        public void KillIfDead(Toolkit toolkit)
        {
            if (aliveInWorld)
            {
                KillCharacter(_name, toolkit);
            }
        }
    }

}
