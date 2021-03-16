using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class GameManager
    {
        private int _gameState = GAMESTATE_CHANGEMAP;
        private static int GAMESTATE_GAMEOVER = 0;
        private static int GAMESTATE_MAP = 1;
        private static int GAMESTATE_CHANGEMAP = 2;
        private static int GAMESTATE_BATTLE = 3;

        static int CONSOLE_HEIGHT;
        static int CONSOLE_WIDTH;

        Toolkit toolkit = new Toolkit();

        Map map;
        UI ui;
        Player player;
        Item item;
        List<Enemy> enemies = new List<Enemy>();

        // constructor
        public GameManager()
        {
            toolkit.SetConsoleSize();
            Console.CursorVisible = false;

            CONSOLE_HEIGHT = Console.WindowHeight;
            CONSOLE_WIDTH = Console.WindowWidth;

            map = new Map();
            ui = new UI();
            player = new Player("John Smith", '@');
            item = new Item("Key", 'k');

            

            map.CreateWindowBorder();
            
        }

        // ----- gets/sets
        public int GameState { get; set; }
        public void setGameOver()
        {
            if (player.AliveInWorld() == false) { GameState = GAMESTATE_GAMEOVER; }
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
            ui.getHUDvalues(player.Health());
            Draw();
        }
        public void Draw()
        {
            ui.HUD();
            map.DrawCurrent();
            item.Draw();

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw();
            }
            player.Draw();
        }
        public void Update()
        {
            //map.checkPosition( 0, 0, map.getEnemyHold());
            player.Update(enemies, map, ui, toolkit);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(player, map, ui, toolkit);
            }    
            ui.getHUDvalues(player.Health());
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
                
            }
        }
    }
}
