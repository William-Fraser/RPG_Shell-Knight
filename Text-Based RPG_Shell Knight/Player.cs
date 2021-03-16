using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Player : GameCharacter
    {
        private ConsoleKeyInfo _playerInput;
        private bool hasKey = false;

        //constructor
        public Player(string name, char avatar)
        {
            _name = name;
            _avatar = avatar;
            _health = new int[] { 999, 999 };
            _damage = new int[] { 50, 75 };

            x = Console.WindowWidth / 2;
            y = Console.WindowHeight / 2;

            this.aliveInWorld = true;
        }

        // ----- gets sets
        public bool HasKey { get; set; }
        
        // ----- Private Methods
        private void DirectionalOutput() 
        {
            if (_playerInput.Key == ConsoleKey.S || _playerInput.Key == ConsoleKey.DownArrow)
            {
                CheckDirection(DIRECTION_DOWN);
            }
            else if (_playerInput.Key == ConsoleKey.W || _playerInput.Key == ConsoleKey.UpArrow)
            {
                CheckDirection(DIRECTION_UP);
            }
            else if (_playerInput.Key == ConsoleKey.A || _playerInput.Key == ConsoleKey.LeftArrow)
            {
                CheckDirection(DIRECTION_LEFT);
            }
            else if (_playerInput.Key == ConsoleKey.D || _playerInput.Key == ConsoleKey.RightArrow)
            {
                CheckDirection(DIRECTION_RIGHT);
            }
            else
            { 
                _directionMoving = DIRECTION_NULL;
                _XYHolder[0] = x;
                _XYHolder[1] = y;
            }
        }

        // ----- Public Methods
        new public void Draw() 
        {
            base.Draw();
            //toolkit.DisplayText("drawing");
        }
        public void GetInput()
        {
            _playerInput = Console.ReadKey(false);
            while (Console.KeyAvailable)
            {
                _playerInput = Console.ReadKey(true);
            }
        }
        public bool CheckForCharacterCollision(Enemy enemy)
        {
            bool collision;
            collision = base.CheckForCharacterCollision(enemy.X(), enemy.Y(), enemy.AliveInWorld());
            return collision;
        }
        public void DealDamage(Enemy enemy, UI ui, Toolkit toolkit)
        {
            base.DealDamage(enemy.Name(), enemy.AliveInWorld(), enemy.Health(), false, ui, toolkit);
        }

        public void Update(List<Enemy> enemy, Map map, UI ui, Toolkit toolkit) {
            //toolkit.DisplayText(toolkit.blank);// clears the text after it's been displayed once - LEGACYCODE
            KillIfDead(toolkit);
            
            GetInput();

            toolkit.SetConsoleSize();
            DirectionalOutput();


            //check each enemy for collision
            bool collision = false;
            for (int i = 0; i < enemy.Count; i++)
            {
                if (CheckForCharacterCollision(enemy[i])) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                {
                    collision = true;
                    DealDamage(enemy[i], ui, toolkit);
                }
            }

            //toolkit.DisplayText($"checking at: {map.getTile(_XYHolder[0], _XYHolder[1])}");
            if (!CheckForWall(map.getTile(_XYHolder[0], _XYHolder[1]-1), map.getWallHold()))
            {
                if (!collision)
                {
                    Move();
                }
            }

        }
    }
}
