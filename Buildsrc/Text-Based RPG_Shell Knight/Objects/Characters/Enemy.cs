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
    // Enemy type enum
    /*public enum ENEMY // depricated / replaced by loaded dictionary
    {
        SPIDER,
        GOBLIN,
        KNIGHT,
        KING,
        TOTALENEMIES
    }*/
    // State enunm for AI
    public enum AI
    {
        CHASE,
        FLEE,
        FLEEANDCHASE,
        IDLEANDCHASE
    }
    class Enemy : Character
    {
        private AI _ai;
        //constructor
        public Enemy(string enemyInfo, DataLoader dataLoader, string name = "errBlank", char avatar = '!') : base(name, avatar, 0) 
        {
            LoadEnemyInfo(enemyInfo, dataLoader);
        }

        // ----- private methods
        private void LoadEnemyInfo(string enemyInfo, DataLoader dataLoader) // Constructor child: distinguishes all enemies available to print on map
        {
            string[] identifyerAndPos = enemyInfo.Split(':'); // parsing passed map info // 0 identifies and 1 is position
            string identity = identifyerAndPos[0]; 

            // set position
            int[] setPos = dataLoader.TryParseXYFromString(identifyerAndPos[1]);
            x = setPos[(int)AXIS.X];
            y = setPos[(int)AXIS.Y];


            // creating enemy from identity

            //inst of fields
            _avatar = Global.GetEnemyAvatar(identity);
            _name = Global.GetEnemyName(identity);
            _damage = Global.GetEnemyDamageRange(identity);
            for (int i = 0; i < 2; i++)
            {
                _health[i] = Global.GetEnemyHealth(identity);
            }

            // setting AI
            _ai = Global.GetEnemyEnemyAI(identity);

            
            //int[] _XYHolder = new int[] { x, y };

            aliveInWorld = true;

            _directionMoving = DIRECTION.NULL;
        }
        private void IdentifyAndCreateEnemy() 
        {
        
        }
        private string RecognizeInfo(char identity) // read enemy info child: Holds the init info for all enemy types
        {// Obsolete, change to be read from file, load enum too
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

        ///AI
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
        public GAMESTATE Update(Player player, Map map, Camera camera, HUD hud, Battle battle, Inventory inventory, GAMESTATE gameState)
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
                    gameState = StartAttacking(aliveInWorld, battle, player, this, gameState, inventory);
                }

                if (!CheckForWallCollision(map.getTile(_XYHolder[0], _XYHolder[1]-1), map.getWallHold())) // -1 to fix bug from result of other fix
                {
                    if (!collision)
                    {
                        Move();
                    }
                }
                CheckForDying(camera, hud);
            }
            return gameState;
        }
    }
}
