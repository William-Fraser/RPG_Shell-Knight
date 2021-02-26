using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <EnemyAvatars>
/// #   - Spider / Health: 20 Attack: 7-17 / runs towards player, runs to walls and along them until player is close then runs at player
/// 
/// %   - Knight / Health: 70 Attack: 9-25 / runs towards player, smart runs around walls similar to spider? 
/// 
/// $   - King / Health: 200 Attack: 1-15 /current Boss, walks back and fourth not strong
/// </icons used to display enemies>

namespace Text_Based_RPG_Shell_Knight
{
    class Enemy : GameCharacter
    {


        int _enemyNumber;
        static int _totalEnemyNumber;
        new string[] _name;
        new char[] _avatar;
        new int[,] _health;
        new int[,] _damage;
        new int[] _posX;
        new int[] _posY;
        new private bool[] _aliveInWorld;
        new private int[] _directionMoving;

        //constructor
        public Enemy(string[] enemyInfo) 
        {
            //Console.SetCursorPosition(1, 0);
            //Console.Write($"displaying :{enemyInfo.Length}:"); // --- debug
            //Console.ReadKey(true);
            // reinitializes new enemy group for every map change, because enemies change with map
            _totalEnemyNumber = enemyInfo.Length;
            _name = new string[enemyInfo.Length]; 
            _avatar = new char[enemyInfo.Length]; // used to identify the enemy
            _health = new int[enemyInfo.Length, 2];
            _damage = new int[enemyInfo.Length, 2];
            _posX = new int[enemyInfo.Length];
            _posY = new int[enemyInfo.Length];
            _aliveInWorld = new bool[enemyInfo.Length];
            _directionMoving = new int[enemyInfo.Length];
            readEnemyInfo(enemyInfo);
        }

        // ----- gets / sets
        new public string getName()
        {
            return _name[_enemyNumber];
        }
        ///public void setEnemyPos(int pos) //legacy Enemy positioning
        //{
        //    if (pos == 0) {
        //        _posX = Console.WindowWidth / 4;
        //        _posY = Console.WindowHeight / 4;
        //    }
        //    else if (pos == 1)
        //    {
        //        _posX = (Console.WindowWidth / 4) * 3;
        //        _posY = Console.WindowHeight / 4;
        //    }
        //    else if (pos == 2)
        //    {
        //        _posX = Console.WindowWidth / 4;
        //        _posY = (Console.WindowHeight / 4) * 3;
        //    }
        //    else if (pos == 3)
        //    {
        //        _posX = (Console.WindowWidth / 4) * 3;
        //        _posY = (Console.WindowHeight / 4) * 3;
        //    }
        //}
        public int[] getDamage(int playerX, int playerY)
        {
            int[] _damageNumber = new int[base._damage.Length]; // used to hold damage info from string to int

            if (locateEnemy(playerX, playerY) > 0)
            {
                for (int i = 0; i < _damageNumber.Length; i++)
                {

                    _damageNumber[i] = _damage[locateEnemy(playerX, playerY), i];
                }
            }
            else { _damageNumber[0] = 0; _damageNumber[1] = 1; }
            return _damageNumber;
        }
        public int[] getHealth(int playerX, int playerY)
        {
            int[] _healthNumber = new int[base._health.Length]; // used to hold damage info from string to int
            if (locateEnemy(playerX, playerY) > 0)
            {
                for (int i = 0; i < _healthNumber.Length; i++)
                {
                    _healthNumber[i] = _health[locateEnemy(playerX, playerY), i];
                }
            }
            else { _healthNumber[0] = 0; _healthNumber[1] = 1; }
            return _healthNumber;
        }
        public bool getAlive(int playerX, int playerY)
        {
            return _aliveInWorld[locateEnemy(playerX, playerY)];
        }


        // ----- private methods

        //read info
        private void readEnemyInfo(string[] enemyInfo) // Constructor child: reads all enemies available to print on map
        {

            for (int i = 0; i < enemyInfo.Length; i++)
            {
                string[] avatarAndPos = enemyInfo[i].Split(':');

                    string avatarHold = avatarAndPos[0];
                    string posHold = avatarAndPos[1];
                    char identity = avatarHold[0];
                    string[] identifyed = recognizeInfo(identity).Split(';');
                    _name[i] = identifyed[0];
                    _avatar[i] = avatarHold[0];
                    string[] setHealth = identifyed[1].Split(',');
                    string[] setDamage = identifyed[2].Split(',');
                    for (int t = 0; t < 2; t++)
                    {
                        _health[i, t] = Int32.Parse(setHealth[t]);
                        _damage[i, t] = Int32.Parse(setDamage[t]);
                    }
                    string[] setPos = posHold.Split(',');
                    _posX[i] = Int32.Parse(setPos[0]);
                    _posY[i] = Int32.Parse(setPos[1]);
                    _aliveInWorld[i] = true;
                    _directionMoving[i] = DIRECTION_NULL;
            }
            _totalEnemyNumber = enemyInfo.Length;

        }// Constructor child: reads all enemies available to print on map
        private string recognizeInfo(char identity) ///holds stats for enemys found in ^EnemyAvatars^
        {
            string identifyed = "";
            if (identity == '#') ///#   - Spider / Health: 20 Attack: 7-17 /
            {
                identifyed += "Spider;";
                identifyed += "20,20;";
                identifyed += "7,17";

            } ///#   - Spider / Health: 20 Attack: 7-17 /
            else if (identity == '%') /// %   - Knight / Health: 70 Attack: 9-25 /
            {
                identifyed += "Knight;";
                identifyed += "70,70;";
                identifyed += "9,25";
            } /// %   - Knight / Health: 70 Attack: 9-25 /
            else if (identity == '$') /// $   - King / Health: 200 Attack: 1-15 /
            {
                identifyed += "King;";
                identifyed += "200,200;";
                identifyed += "1,15";
            } /// $   - King / Health: 200 Attack: 1-15 /
            return identifyed;
        }

        // Enemy control
        new protected private void moveBack() // moves player back if they're supposed to collide with something
        {
            if (_directionMoving[_enemyNumber] == DIRECTION_UP) { Move(DIRECTION_DOWN); }
            else if (_directionMoving[_enemyNumber] == DIRECTION_DOWN) { Move(DIRECTION_UP); }
            else if (_directionMoving[_enemyNumber] == DIRECTION_LEFT) { Move(DIRECTION_RIGHT); }
            else if (_directionMoving[_enemyNumber] == DIRECTION_RIGHT) { Move(DIRECTION_LEFT); }
            else { }
        }
        private int locateEnemy(int playerX, int playerY)
        {
            int enemyNumber = -7;
            for (int i = 0; i < _totalEnemyNumber; i++)
            {
                if (playerX == _posX[i])
                {
                    if (playerY == _posY[i])
                    {
                        enemyNumber = i;
                    }
                }
            }
            return enemyNumber;
        }
        private void killCharacter()
        {
            if (_health[_enemyNumber,0] <= 0)
            {
                string deathMessage = $"< {_name[_enemyNumber]} has been slain >";
                _aliveInWorld[_enemyNumber] = false;
                toolkit.DisplayText(deathMessage);
                Console.ReadKey(true);
                _avatar[_enemyNumber] = 'X';
            }
        }
        private int takeDamage(int[] _damage)
        {
            int finalDamage = toolkit.RandomNumBetween(_damage[0], _damage[1]);
            _health[_enemyNumber, 0]  -= finalDamage;
            setStatToLimits(_health[_enemyNumber, 0], _health[_enemyNumber, 1]);
            return finalDamage;
        }
        new protected private void displayDamage(int finalDamage, UI ui)
        {
            setStatToLimits(_health[_enemyNumber, 0], _health[_enemyNumber, 1]);
            string damageMessage = $"< {_name[_enemyNumber]} taking {finalDamage} points of Damage, {_health[_enemyNumber, 0]}/{_health[_enemyNumber, 1]} Health Remaining>";
            toolkit.DisplayText(damageMessage);
        }

        // ----- public methods
        new public void Move(int DIRECTION_) //moves the player in the specifyed DICRECTION_ 
        {
            if (_aliveInWorld[_enemyNumber])
            {
                if (DIRECTION_ == DIRECTION_DOWN) { if (_posY[_enemyNumber] < Console.WindowHeight - 1) { _posY[_enemyNumber] += 1; _directionMoving[_enemyNumber] = DIRECTION_DOWN; } }
                else if (DIRECTION_ == DIRECTION_UP) { if (Y() > 0) { _posY[_enemyNumber] -= 1; _directionMoving[_enemyNumber] = DIRECTION_UP; } }
                else if (DIRECTION_ == DIRECTION_LEFT) { if (X() > 0) { _posX[_enemyNumber] -= 1; _directionMoving[_enemyNumber] = DIRECTION_LEFT; } }
                else if (DIRECTION_ == DIRECTION_RIGHT) { if (X() < Console.WindowWidth - 1) { _posX[_enemyNumber] += 1; _directionMoving[_enemyNumber] = DIRECTION_RIGHT; } }
            }
        }
        new public void ChecktoTakeDamage(int playerX, int playerY, bool alive, int[] damage, UI ui)
        {
            if (alive)
            {
                if (playerX == _posX[_enemyNumber])
                {
                    if (playerY == _posY[_enemyNumber])
                    {
                        moveBack();
                        displayDamage(takeDamage(damage), ui);
                        killCharacter();
                    }
                }
            }
        }

        //AI
        public void MoveChasePlayer(int playerX, int playerY)
        {
            bool moveAlongAxis = false; // if /true = y/ else /false = x/
            int mapQuadrent = 0; // enemy location is 0, 0 /x,-y = 0/x, y = 1/-x, -y = 2/-x, y = 3/ <NW

            if (_posX[_enemyNumber] == playerX && _posY[_enemyNumber] == playerY)
            {
                _directionMoving[_enemyNumber] = DIRECTION_NULL;
            }
            else 
            {
                    if (_posY[_enemyNumber] == playerY)
                    {
                        moveAlongAxis = false;
                    }
                    else if (_posX[_enemyNumber] == playerX)
                    {
                        moveAlongAxis = true;
                    }
                    else
                    {
                        // finding the quadrent with the player
                        if (playerX > _posX[_enemyNumber])
                        {
                            mapQuadrent = 1; // <NE
                            if (playerY > _posY[_enemyNumber])
                            {
                                mapQuadrent = 3; // <SE
                            }

                        }
                        else if (playerY > _posY[_enemyNumber])
                        {
                            mapQuadrent = 2; // <SW
                        }

                        // check quadrent for which axis to move
                        if (mapQuadrent == 0)
                        {
                            if ((_posX[_enemyNumber] - playerX) < (_posY[_enemyNumber] - playerY))
                            {
                                moveAlongAxis = true;
                            }
                            else { moveAlongAxis = false; }
                        }
                        else if (mapQuadrent == 1)
                        {
                            if ((playerX - _posX[_enemyNumber]) < (playerY - _posY[_enemyNumber]))
                            {
                                moveAlongAxis = true;
                            }
                            else { moveAlongAxis = false; }
                        }
                        else if (mapQuadrent == 2)
                        {
                            if ((_posX[_enemyNumber] - playerX) < (playerY - _posY[_enemyNumber]))
                            {
                                moveAlongAxis = true;
                            }
                            else { moveAlongAxis = false; }
                        }
                        else if (mapQuadrent == 3)
                        {
                            if ((playerX - _posX[_enemyNumber]) < (playerY - _posY[_enemyNumber]))
                            {
                                moveAlongAxis = true;
                            }
                            else { moveAlongAxis = false; }
                        }
                    }

                    // check direction to move axis in
                    if (moveAlongAxis == false)
                    {
                        if (playerX > _posX[_enemyNumber])
                        {
                            Move(DIRECTION_RIGHT);
                        }
                        else
                        {
                            Move(DIRECTION_LEFT);
                        }
                    }
                    else
                    {
                        if (playerY > _posY[_enemyNumber])
                        {
                            Move(DIRECTION_DOWN);
                        }
                        else
                        {
                            Move(DIRECTION_UP);
                        }
                    }

            }
        }
        
        //Draw & update
        new public void Draw()
        {
                //Console.SetCursorPosition(1, 0);
                //Console.Write($"displaying :{_totalEnemyNumber}:"); // --- debug
                //Console.ReadKey(true);
            for (_enemyNumber = 0; _enemyNumber < _totalEnemyNumber; _enemyNumber++)
            {
                //Console.SetCursorPosition(1, 1);              //
                //Console.Write($"X: { _posX } Y: { _posY }");  // --- debug / display cursor XY
                Console.SetCursorPosition(_posX[_enemyNumber], _posY[_enemyNumber]);
                Console.Write(_avatar[_enemyNumber]);
            }
        }
        public void Update(int playerX, int playerY, char[] mapMovement, string walls, bool alive, int[] health, UI ui)
        {
            for (_enemyNumber = 0; _enemyNumber < _totalEnemyNumber; _enemyNumber++)
            {
                ChecktoTakeDamage(playerX, playerY, alive, health, ui);
                MoveChasePlayer(playerX, playerY);
                CheckForWall(mapMovement, walls);
            }
        }
    }
}
