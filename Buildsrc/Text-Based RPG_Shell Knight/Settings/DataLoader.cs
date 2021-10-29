using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class DataLoader // reads data from config files for use in game
    {
        //Values
        private EnemyValues enemyValues = new EnemyValues();

        //private methods
        private void EnemyInfo() // change to load in from file
        {
            enemyValues.enemyNames.Add("Spider");
            enemyValues.enemyAvatars.Add('#');
            enemyValues.enemyHealths.Add(20);
            enemyValues.enemyDamages.Add(new int[] { 7, 17 });
            enemyValues.enemyAIs.Add(AI.FLEEANDCHASE);

            enemyValues.enemyNames.Add("Goblin");
            enemyValues.enemyAvatars.Add('&');
            enemyValues.enemyHealths.Add(70);
            enemyValues.enemyDamages.Add(new int[] { 9, 25 });
            enemyValues.enemyAIs.Add(AI.CHASE);

            enemyValues.enemyNames.Add("Knight");
            enemyValues.enemyAvatars.Add('%');
            enemyValues.enemyHealths.Add(150);
            enemyValues.enemyDamages.Add(new int[] { 17, 40 });
            enemyValues.enemyAIs.Add(AI.IDLEANDCHASE);

            enemyValues.enemyNames.Add("King");
            enemyValues.enemyAvatars.Add('$');
            enemyValues.enemyHealths.Add(275);
            enemyValues.enemyDamages.Add(new int[] { 20, 50 });
            enemyValues.enemyAIs.Add(AI.IDLEANDCHASE);
        }

        //public methods
        #region obtaining loaded info
        // enemy
        public string GetEnemyName(ENEMY e) { if (enemyValues.enemyNames[(int)e] != null) return enemyValues.enemyNames[(int)e]; else return null; }
        public char GetEnemyAvatar(ENEMY e) { return enemyValues.enemyAvatars[(int)e]; }
        public int GetEnemyHealth(ENEMY e) { return enemyValues.enemyHealths[(int)e]; }
        public int[] GetEnemyDamageRange(ENEMY e) { if (enemyValues.enemyNames[(int)e] != null) { return enemyValues.enemyDamages[(int)e]; } else return null; }
        public AI GetEnemyEnemyAI(ENEMY e) { return enemyValues.enemyAIs[(int)e]; }
        #endregion

        //utility methods
        public int[] TryParseXYFromString(string xCommaY)
        {
            int[] results = new int[2];
            int result;
            string[] strings = xCommaY.Split(','); // because xy should have a comma, splits them

            for (int i = 0; i < 2; i++)
            { 
                if (Int32.TryParse(strings[i], out result))
                {
                    results[i] = result;
                }
            }

            return results;
        }
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
