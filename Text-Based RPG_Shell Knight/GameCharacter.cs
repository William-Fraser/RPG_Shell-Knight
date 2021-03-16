using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    abstract class GameCharacter : GameObject
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

        //death animation
        private int displayDeath = 0; //used in draw to show death once

        // ----- gets
        public int[] Health()
        {
            return _health;
        }
        public void Health(int value)
        {
            _health[0] = value;
            setStatToLimits(_health[0], _health[1]);
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
                string deathMessage = $"< {name} has been slain >";
                aliveInWorld = false;
                toolkit.DisplayText(deathMessage);
                Console.ReadKey(true);
                _avatar = 'X';
            }
        }
        private int Attack(int[] health, UI ui, Toolkit toolkit)
        {
            if (aliveInWorld)
            {
                int finalDamage = toolkit.RandomNumBetween(_damage[0], _damage[1]);
                health[0] -= finalDamage;
                setStatToLimits(health[0], health[1]);
                ui.getHUDvalues(health);
                return finalDamage;
            }
            else
            {
                return 0;
            }
        }
        protected private void DisplayDamageToHUD(string name, int attackDamage, int[] health, Toolkit toolkit, bool isEnemy)
        {
            setStatToLimits(health[0], health[1]);
            string damageMessage = $"< {name} taking {attackDamage} points of Damage ";
            if (!isEnemy)
            { damageMessage += $"{health[0]}/{health[1]} >"; }
            else 
            { damageMessage += $">"; }
            toolkit.DisplayText(damageMessage);
        }

        // ----- public methods
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
        public void DealDamage(string name, bool alive, int[] health, bool isEnemy, UI ui, Toolkit toolkit)
        {
            if (alive)
            {
                DisplayDamageToHUD(name, Attack(health, ui, toolkit), health, toolkit, isEnemy);
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
