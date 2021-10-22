using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class DataManager // reads data from config files for use in game
    {
        //Enemies
        private EnemyValues eValues = new EnemyValues();

        private void LoadEnemyInfo() // change to load in from file
        {
            eValues.enemyNames.Add("Spider");
            eValues.enemyAvatars.Add('#');
            eValues.enemyHealths.Add(20);
            eValues.enemyDamages.Add(new int[] { 7, 17 });
            eValues.enemyAIs.Add(AI.FLEEANDCHASE);

            eValues.enemyNames.Add("Goblin");
            eValues.enemyAvatars.Add('&');
            eValues.enemyHealths.Add(70);
            eValues.enemyDamages.Add(new int[] { 9, 25 });
            eValues.enemyAIs.Add(AI.CHASE);

            eValues.enemyNames.Add("Knight");
            eValues.enemyAvatars.Add('%');
            eValues.enemyHealths.Add(150);
            eValues.enemyDamages.Add(new int[] { 17, 40 });
            eValues.enemyAIs.Add(AI.IDLEANDCHASE);

            eValues.enemyNames.Add("King");
            eValues.enemyAvatars.Add('$');
            eValues.enemyHealths.Add(275);
            eValues.enemyDamages.Add(new int[] { 20, 50 });
            eValues.enemyAIs.Add(AI.IDLEANDCHASE);
        }

        //methods
        public string GetEnemyName(ENEMY e) { if (eValues.enemyNames[(int)e] != null) return eValues.enemyNames[(int)e]; else return null; }
        public char GetEnemyAvatar(ENEMY e) { return eValues.enemyAvatars[(int)e]; }
        public int GetEnemyHealth(ENEMY e) { return eValues.enemyHealths[(int)e]; }
        public int[] GetEnemyDamageRange(ENEMY e) { if (eValues.enemyNames[(int)e] != null) { return eValues.enemyDamages[(int)e]; } else return null; }
        public AI GetEnemyEnemyAI(ENEMY e) { return eValues.enemyAIs[(int)e]; }

    }
    class EnemyValues
    {
        public List<string> enemyNames = new List<string>();
        public List<char> enemyAvatars = new List<char>();
        public List<int> enemyHealths = new List<int>();
        public List<int[]> enemyDamages = new List<int[]>();
        public List<AI> enemyAIs = new List<AI>();
    }
}
