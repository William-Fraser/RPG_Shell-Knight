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
        Enemy[] enemies;

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
            if (player.Alive == false) { GameState = GAMESTATE_GAMEOVER; }
            else { }
        }

        // ----- Manager Methods
        public void UpdateDisplay()
        {
            map.SetCurrent();
            enemies = new Enemy[map.readEnemyHold().Length]; // because enemies.length makes more sense

            string[] enemyInfo = map.getEnemyHold().Split('|');
            for (int i = 0; i < enemyInfo.Length; i++) // nested loop to write window border
            {
                Enemy enemy = new Enemy(enemyInfo[i]);
                enemies[i] = enemy;
            }

            map.DrawWindowBorder();
            ui.getHUDvalues(player.getHealth());
            Draw();
        }
        public void Draw()
        {
            ui.HUD();
            map.DrawCurrent();
            item.Draw();
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Draw();
            }
            player.Draw();
        }
        public void Update()
        {
            //map.checkPosition( 0, 0, map.getEnemyHold());
            for (int i = 0; i < enemies.Length; i++)
            {
                player.Update(enemies[i], map, ui);
                enemies[i].Update(player, map, ui);
            }
            ui.getHUDvalues(player.getHealth());
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
