using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <EnemyAvatars>
///UPDATE LEGEND
/// 
/// #   - Spider / Health: 20  / Attack: 7-17  / AI: Flee and Chase in prox
///
/// &   - Goblin / Health: 70  / Attack: 9-25  / AI: Chase
///
/// %   - Knight / Health: 150 / Attack: 17-40 / AI: Idle to Chase in prox
///
/// $   - King   / Health: 275 / Attack: 20-50 / AI: Idle to Chase in prox
///
/// </icons used to display enemies, and discriptions of what they should do>

namespace Text_Based_RPG_Shell_Knight
{
    class Enemy : Character
    {
        // State machine for AI
        private enum AI
        {
            CHASE,
            FLEE,
            FLEEANDCHASE,
            IDLEANDCHASE
        }
        private AI _ai;

        //constructor
        public Enemy(string enemyInfo, string name = "errBlank", char avatar = '!') : base(name, avatar, 0) 
        {
            ReadEnemyInfo(enemyInfo);
        }

        // ----- private methods
        private void ReadEnemyInfo(string enemyInfo) // Constructor child: distinguishes all enemies available to print on map
        {
            // parsing passed info
            string[] avatarAndPos = enemyInfo.Split(':');
            string avatarHold = avatarAndPos[0];
            string posHold = avatarAndPos[1];

            // reading identity for creating
            char identity = avatarHold[0];
            string[] identifyed = RecognizeInfo(identity).Split(';');

            // creating enemy form identity
            
            //init of fields
            _avatar = avatarHold[0];
            _name = identifyed[0];
            string[] setHealth = identifyed[1].Split(',');
            string[] setDamage = identifyed[2].Split(',');
            for (int i = 0; i < 2; i++)
            {
                _health[i] = Int32.Parse(setHealth[i]);
                _damage[i] = Int32.Parse(setDamage[i]);
            }

            // setting AI
            _ai = (AI)Int32.Parse(identifyed[3]);

            // set position
            string[] setPos = posHold.Split(',');
            x = Int32.Parse(setPos[0]);
            y = Int32.Parse(setPos[1]);
            //int[] _XYHolder = new int[] { x, y };

            aliveInWorld = true;

            _directionMoving = DIRECTION.NULL;
        }
        private string RecognizeInfo(char identity) // read enemy info child: Holds the init info for all enemy types
        { // THIS IS WHAT DEFINES ^^^Enemy Avatars^^^ NOT THE OTHER WAY AROUND
            string identifyed = "";
            /// identifyed string order
            /// NAME
            /// HEALTH current,max
            /// DAMAGE high,low
            /// AI
            ///
            if (identity == '#') /// #   - Spider / Health: 20 / Attack: 7-17 / AI: Flee and Chase in prox
            {
                identifyed += "Spider;";
                identifyed += "20,20;";
                identifyed += "7,17;";
                identifyed += ((int)AI.FLEEANDCHASE).ToString();
            }
            else if (identity == '&') /// &    - Goblin / Health: 70 / Attack: 9-25 / AI: Chase
            {
                identifyed += "Goblin;";
                identifyed += "70,70;";
                identifyed += "9,25;";
                identifyed += ((int)AI.CHASE).ToString();
            }
            else if (identity == '%') /// %   - Knight / Health: 150 / Attack: 17-40 / AI: Idle to Chase in prox
            {
                identifyed += "Knight;";
                identifyed += "150,150;";
                identifyed += "17,40;";
                identifyed += ((int) AI.IDLEANDCHASE).ToString();
            }
            else if (identity == '$') /// $   - King / Health: 275 / Attack: 20-50 / AI: Idle to Chase in prox
            {
                identifyed += "King;";
                identifyed += "275,275;";
                identifyed += "20,50;";
                identifyed += ((int)AI.IDLEANDCHASE).ToString();
            } 
            return identifyed;
        }

        //AI
        private void AIMoveChasePlayer(Player player) // chases player
        {/// needs revisioning to move to the top of player if they're close enough

            int playerX = player.X();
            int playerY = player.Y();

            bool moveYAlongAxis; // if /true = y/ else /false = x/
            

            //dont move
            if (x == playerX && y == playerY)
            {
                _directionMoving = DIRECTION.NULL;
                _XYHolder[0] = x;
                _XYHolder[1] = y;
            }
            else
            {
                moveYAlongAxis = AICheckQuadrent(playerX, playerY);

                // check direction to move axis in
                // AI TECH CHASE
                if (moveYAlongAxis == false)
                {
                    if (playerX > x)
                    {
                        CheckDirection(DIRECTION.RIGHT);
                    }
                    else
                    {
                        CheckDirection(DIRECTION.LEFT);
                    }
                }
                else
                {
                    if (playerY > y)
                    {
                        CheckDirection(DIRECTION.DOWN);
                    }
                    else
                    {
                        CheckDirection(DIRECTION.UP);
                    }
                }

            }
        }
        private void AIMoveFleePlayer(Player player) // runs away from player
        {
            int playerX = player.X();
            int playerY = player.Y();

            bool moveYAlongAxis; // if /true = y/ else /false = x/
            
            //dont move
            if (x == playerX && y == playerY)
            {
                _directionMoving = DIRECTION.NULL;
                _XYHolder[0] = x;
                _XYHolder[1] = y;
            }
            else
            {
                moveYAlongAxis = AICheckQuadrent(playerX, playerY);

                // check direction to move axis in
                // AI TECH FLEE
                if (moveYAlongAxis == false)
                {
                    if (playerX > x)
                    {
                        CheckDirection(DIRECTION.LEFT);
                    }
                    else
                    {
                        CheckDirection(DIRECTION.RIGHT);
                    }
                }
                else
                {
                    if (playerY > y)
                    {
                        CheckDirection(DIRECTION.UP);
                    }
                    else
                    {
                        CheckDirection(DIRECTION.DOWN);
                    }
                }

            }
        }
        private void AIMoveFleeThenChaseInProx(Player player) // runs away but then chases when the player is close enough
        {
            int playerX = player.X();
            int playerY = player.Y();

            bool inprox = AICheckProximity(playerX, playerY, 20);
            if (inprox)
            {
                AIMoveChasePlayer(player);
            }
            else
            { 
                AIMoveFleePlayer(player);
            }
        }
        private void AIIdleThenChaseInProx(Player player) // idles but chases when the player is close enough
        {
            int playerX = player.X();
            int playerY = player.Y();

            bool inProx = AICheckProximity(playerX, playerY, 20);
            if (inProx)
            { AIMoveChasePlayer(player); }
            else { }
        }
        //checks
        private bool AICheckQuadrent(int playerX, int playerY)
        {
            //init
            bool moveOnY = false;
            
            // enters value if x or y line up otherwise finds a quadrent
            if (y == playerY)
            {
                moveOnY = false;
            }
            else if (x == playerX)
            {
                moveOnY = true;
            }
            else
            {
                // creates a grid with enemy location at 0,0 
                // x,-y = 0 // x, y = 1 // -x, -y = 2 // -x, y = 3 //
                int quadrent = 0; //0 <NW compass orientation to help explain

                // finding the quadrent with the player
                if (playerX > x)
                {
                    quadrent = 1; // <NE
                    if (playerY > y)
                    {
                        quadrent = 3; // <SE
                    }

                }
                else if (playerY > y)
                {
                    quadrent = 2; // <SW
                }

                // check quadrent for which axis to move // note in this calc enemy is not 0,0 yet
                if (quadrent == 0)
                {
                    if ((x - playerX) <= (y - playerY))
                    {
                        moveOnY = true;
                    }
                    else { moveOnY = false; }
                }
                else if (quadrent == 1)
                {
                    if ((playerX - x) <= (y - playerY))
                    {
                        moveOnY = true;
                    }
                    else { moveOnY = false; }
                }
                else if (quadrent == 2)
                {
                    if ((x - playerX) <= (playerY - y))
                    {
                        moveOnY = true;
                    }
                    else { moveOnY = false; }
                }
                else if (quadrent == 3)
                {
                    if ((playerX - x) <= (playerY - y))
                    {
                        moveOnY = true;
                    }
                    else { moveOnY = false; }
                }
            }
            return moveOnY;
        }
        private bool AICheckProximity(int playerX, int playerY, int distance) 
        {
            bool inProx = false;
            if (
            (x - playerX) + (y - playerY) >= distance||
            (playerX - x) + (y - playerY) >= distance||
            (x - playerX) + (playerY - y) >= distance||
            (playerX - x) + (playerY - y) >= distance)
            {}
            else
            { inProx = true; }
            return inProx;
        }


        // ----- public methods
        public void Update(Player player, Map map, Camera camera, HUD hud, Toolkit toolkit)
        {

            if (aliveInWorld)
            {    
                switch(_ai)
                {
                    case AI.CHASE:
                        AIMoveChasePlayer(player);
                        break;
                    case AI.FLEE:
                        AIMoveFleePlayer(player);
                        break;
                    case AI.FLEEANDCHASE:
                        AIMoveFleeThenChaseInProx(player);
                        break;
                    case AI.IDLEANDCHASE:
                        AIIdleThenChaseInProx(player);
                        break;
                }

                bool collision = false;
                if (CheckForCharacterCollision(player.X(), player.Y(), player.AliveInWorld())) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                {
                    collision = true;
                    StartAttacking(player.Name(), player.AliveInWorld(), player.Health(), false, hud, toolkit, player.Shield());

                    //Stops character update and ends game
                    player.CheckForDying(camera, hud);
                }

                if (!CheckForWallCollision(map.getTile(_XYHolder[0], _XYHolder[1]-1), map.getWallHold())) // -1 to fix bug from result of other fix
                {
                    if (!collision)
                    {
                        Move();
                    }
                }
            }
        }
    }
}
