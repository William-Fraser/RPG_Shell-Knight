using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class GameManager
    {
        private int _gameState;
        private static readonly int GAMESTATE_GAMEOVER = 0;
        private static readonly int GAMESTATE_MAP = 1;
        private static readonly int GAMESTATE_CHANGEMAP = 2;
        private static readonly int GAMESTATE_BATTLE = 3;

        readonly Toolkit toolkit = new Toolkit();

        readonly Map map;
        readonly UI ui;
        readonly Player player;
        readonly Item item;
        readonly List<Enemy> enemies = new List<Enemy>();

        // constructor
        public GameManager()
        {
            toolkit.SetConsoleSize();
            Console.CursorVisible = false;
            _gameState = GAMESTATE_CHANGEMAP;

            map = new Map();
            ui = new UI();
            player = new Player("John Smith", '@');
            item = new Item("Key", 'k');

            

            map.CreateWindowBorder();
            
        }

        // ----- gets/sets
        public int GameState() { return _gameState; }
        public void setGameOver()
        {
            if (player.AliveInWorld() == false) { _gameState = GAMESTATE_GAMEOVER; }
            else { }
        } //reads players state and set's game to gameover

        // ----- Manager Methods
        public void UpdateDisplay()
        {
            enemies.Clear();
            map.SetCurrent();
            string[] enemyInfo = map.getEnemyHold().Split('|');

            for (int i = 0; i < enemyInfo.Length; i++) 
            {
                Enemy enemy = new Enemy(enemyInfo[i]);
                enemies.Add(enemy);
            }

            map.DrawWindowBorder();
            ui.getHUDvalues(player.Health(), player.Shield());
            Draw();
        }
        public void Draw()
        {
            //draw current map and objects
            map.DrawCurrent();
            item.Draw();

            //draw characters, lowest is on top
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw();
            }
            player.Draw(ui, toolkit);
        }
        public void Update()
        {
            //update by prioity

            //map.checkPosition( 0, 0, map.getEnemyHold()); -- debug
            player.Update(enemies, map, ui, toolkit);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(player, map, ui, toolkit);
            }    
            ui.getHUDvalues(player.Health(), player.Shield());
        }

        // ----- game loop
        public void Game()
        {
            if (_gameState == GAMESTATE_CHANGEMAP)
            {
                UpdateDisplay();
                _gameState = GAMESTATE_MAP;
            }
            else if (_gameState == GAMESTATE_MAP)
            {
                Update();
                Draw();
                
                setGameOver();
            }
            else if (_gameState == GAMESTATE_GAMEOVER)
            {
                toolkit.DisplayText(" > GAME OVER < ", false);
                Console.ReadKey(true);
            }
        }
    }
}
