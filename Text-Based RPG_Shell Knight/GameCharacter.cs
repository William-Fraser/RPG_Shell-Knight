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
        protected private int _directionMoving;
        protected static int DIRECTION_NULL = 0;
        protected static int DIRECTION_UP = 1;
        protected static int DIRECTION_RIGHT = 2;
        protected static int DIRECTION_DOWN = 3;
        protected static int DIRECTION_LEFT = 4;

        //character fields.
        protected private int[] _health = new int[2]; // set: current health / max health
        protected private int[] _damage = new int[2]; // set range: Lowest / Highest

        //death animation
        private int displayDeath = 0; //used in draw to show death once

        // ----- gets
        public int X { get; }
        public int Y { get; }
        public int Direction { get; }
        public int[] getHealth() 
        {
            return _health; 
        }
        public int[] getDamage()
        {
            return _damage;
        }
        public char Avatar { get; }

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

        // ----- private methods
        protected private void MoveBack() // moves player back if they're supposed to collide with something
        {
            if (_directionMoving == DIRECTION_UP) { Move(DIRECTION_DOWN); }
            else if (_directionMoving == DIRECTION_DOWN) { Move(DIRECTION_UP); }
            else if (_directionMoving == DIRECTION_LEFT) { Move(DIRECTION_RIGHT); }
            else if (_directionMoving == DIRECTION_RIGHT) { Move(DIRECTION_LEFT); }
            else { }
        }
        private void KillCharacter()
        {
            if (_health[0] <= 0) {
                string deathMessage = $"< {_name} has been slain >";
                aliveInWorld = false;
                toolkit.DisplayText(deathMessage);
                Console.ReadKey(true);
                _avatar = 'X';
            }
        }
        private int TakeDamage(int[] _damage)
        {
            int finalDamage = toolkit.RandomNumBetween(_damage[0], _damage[1]);
            _health[0] -= finalDamage;
            setStatToLimits(_health[0], _health[1]);
            return finalDamage;
        }
        protected private void DisplayDamageToHUD(int finalDamage, UI ui)
        {
            string damageMessage = $"< {_name} taking {finalDamage} points of Damage >";
            toolkit.DisplayText(damageMessage);
        }

        // ----- public methods
        new public void Draw()
        {
            if (displayDeath == 0)
            {
                if (!aliveInWorld) displayDeath++; // displays death once
                base.Draw();
            }
        }
        public void Move(int DIRECTION_) //moves the player in the specifyed DICRECTION_ 
        {
            if (aliveInWorld)
            {
                if (DIRECTION_ == DIRECTION_DOWN) { if (y < Console.WindowHeight - 1) { y += 1; _directionMoving = DIRECTION_DOWN; } }
                else if (DIRECTION_ == DIRECTION_UP) { if (Y > 0) { y -= 1; _directionMoving = DIRECTION_UP; } }
                else if (DIRECTION_ == DIRECTION_LEFT) { if (X > 0) { x -= 1; _directionMoving = DIRECTION_LEFT; } }
                else if (DIRECTION_ == DIRECTION_RIGHT) { if (X < Console.WindowWidth - 1) { x += 1; _directionMoving = DIRECTION_RIGHT; } }
            }
        }
        public void CheckForWall(char[] map, string walls)
        {
            if (_directionMoving != DIRECTION_NULL)
            {
                char[] wallGroup = new char[walls.Length];
                for (int i = 0; i < walls.Length; i++)
                {
                    wallGroup[i] = walls[i];
                }
                for (int i = 0; i < wallGroup.Length; i++)
                {

                    if (map[_directionMoving] == wallGroup[i])
                    { 
                        MoveBack();
                    }
                }
            }
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
                if (x == collidingX)
                {
                    if (y == collidingY)
                    {
                        return true;
                    }
                }
            }
            return false;
        }// collides with objects
        public void ChangetoDealDamage(int enemyX, int enemyY, bool alive, int[] health, UI ui)
        {
            if (alive)
            {
                if (enemyX == x)
                {
                    if (enemyY == y)
                    {
                        DisplayDamageToHUD(TakeDamage(health), ui);
                        KillCharacter();
                    }
                }
            }
        }
    }

}
