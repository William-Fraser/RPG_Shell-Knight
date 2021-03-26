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
        public static int DIRECTION_NULL = 0;
        public static int DIRECTION_UP = 1;
        public static int DIRECTION_RIGHT = 2;
        public static int DIRECTION_DOWN = 3;
        public static int DIRECTION_LEFT = 4;

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

        // ----- gets / sets 
        public int[] Health()
        {
            return _health;
        } // get
        public int[] Damage()
        {
            return _damage;
        } // get
        public int[] XYHolder() 
        {
            return _XYHolder;
        } // get

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
        protected private void CheckDirection(int DIRECTION_) //moves the player in the specifyed DICRECTION_ 
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
        protected private void Move() // also stops character at boundaries        // change bounds to border for screen scrolling?
        {
            if (aliveInWorld)
            {
                if      (_directionMoving == DIRECTION_DOWN && _XYHolder[1] < Map.height +1) { y++; }
                else if (_directionMoving == DIRECTION_UP && _XYHolder[1] > 1)                      { y--; }
                else if (_directionMoving == DIRECTION_LEFT && _XYHolder[0] > 0)                    { x--; }
                else if (_directionMoving == DIRECTION_RIGHT && _XYHolder[0] < Map.width +1) { x++; }
            }
        }
        private int Attack(int[] health, HUD ui, Toolkit toolkit, bool isEnemy, int[] shield = null)
        {
            //calculate damage
            int finalDamage = toolkit.RandomNumBetween(_damage[0], _damage[1]);
            int firstDamage = finalDamage;
            if (aliveInWorld)
            {
                int passDamage = 0;
                if (shield != null) /// this if statement could alternatively be in enemy.dealdamage() but it was moved here and now makes more sense
                {
                    // calculate damage
                    int calcDamageSpill = firstDamage - shield[0];

                    // deal damage to player shield 
                    shield[0] -= firstDamage;
                    if (shield[0] < 0) { passDamage = calcDamageSpill; }
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
                }

                if (!isEnemy)
                {
                    //update HUD
                    ui.setHudHealthAndShield(health, shield);
                    ui.Draw(toolkit);
                }

                //sets damage to total amount of damage done if it spills into health
                if (passDamage != 0) { finalDamage = firstDamage; }
            }
            else
            {
                //err character should not attack if dead
                finalDamage = -1;
            }
            return finalDamage;
        }
        protected private void DealDamage(string name, bool alive, int[] health, bool isEnemy, HUD hud, Toolkit toolkit, int[] shield = null)
        {
            if (alive)
            {
                int damage = Attack(health, hud, toolkit, isEnemy, shield);
                DisplayDamageToHUD(name, damage, health, hud, isEnemy);
            }
        }
        protected private void DisplayDamageToHUD(string name, int attackDamage, int[] health, HUD hud, bool isEnemy)
        {
            health[0] = setStatToLimits(health[0], health[1]);
            string damageMessage = $"< {name} taking {attackDamage} points of Damage ";
            if (isEnemy)
            { damageMessage += $"[{health[0]}/{health[1]}] >"; }
            else 
            { damageMessage += $">"; }
            hud.DisplayText(damageMessage);
        }
        protected private bool CheckForWall(char map, string walls)
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
        protected private bool CheckForCharacterCollision(int collidingX, int collidingY, bool alive)
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
        protected private void KillIfDead(Camera camera, Map map, HUD hud)
        {
            if (aliveInWorld)
            {
                KillCharacter(_name, camera, map, hud);
            }
        }
        protected private void KillCharacter(string name, Camera camera, Map map, HUD hud, int state = 0)
        {
            if (_health[0] <= 0) {
                _avatar = 'X';
                Draw(camera);
                string deathMessage = $"< {name} has been slain >";
                aliveInWorld = false;
                hud.DisplayText(deathMessage, false);
                if (this._avatar == '$')
                {
                    hud.DisplayText("< you ursurped the King and claimed the Throne> ", false);
                    state = GameManager.GAMESTATE_GAMEOVER;
                }
            }
        }
        new public void Draw(Camera camera)
        {
            camera.GameWorldTile(_avatar, x, y);
        }


    }
}
