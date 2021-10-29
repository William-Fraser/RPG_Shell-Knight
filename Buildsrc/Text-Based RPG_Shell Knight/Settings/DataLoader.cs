using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Text_Based_RPG_Shell_Knight
{
    class DataLoader // reads data from config files for use in game
    {
        // variables


        public DataLoader()
        {
            LoadAllFromFiles();
        }

        //private methods
        #region loader methods(loaded from files)
        
        private void L_EnemyInfo() // change to load in from file
        {
            EnemyValues enemyValues = new EnemyValues();

            string[] loadedData = ParseDataFile("EnemyData.txt").Split('~');
            string[] loadedIDs = ConfigLabelRemover(loadedData[0]).Split(',');

            for (int i = 0; i < loadedIDs.Length; i++)
            {
                Global.globalAccess.enemyIDs.Add(loadedIDs[i], i);
            }

            for (int i = 1; i <= loadedIDs.Length; i++)
            {
                string[] loadedValues = loadedData[i].Split(';');
                for (int j = 0; j < loadedValues.Length-1; j++) // -1 due to each value containing an extra empty space from parsing
                {
                    string loadedAvatar = ConfigLabelRemover(loadedValues[1]);
                    int loadedHealth = Int32.Parse(ConfigLabelRemover(loadedValues[2]));
                    string[] Damages = ConfigLabelRemover(loadedValues[3]).Split(',');
                    int[] loadedDamageRange = new int[2];
                    
                    for (int l = 0; l < Damages.Length; l++)
                    { 
                        loadedDamageRange[l] = Int32.Parse(Damages[l]);
                    }

                    AI loadedAI = (AI)Enum.Parse(typeof(AI), ConfigLabelRemover(loadedValues[4]));

                    enemyValues.names.Add(ConfigLabelRemover(loadedValues[0])); // gets the first 
                    enemyValues.avatars.Add(loadedAvatar[0]); // gets the first char in string
                    enemyValues.healths.Add(loadedHealth);
                    enemyValues.damages.Add(loadedDamageRange);
                    enemyValues.aIs.Add(loadedAI);
                }
            }

            Global.globalAccess.EnemyValues = enemyValues;
        }
        private void L_WeaponInfo()
        {
            WeaponValues weaponValues = new WeaponValues();
            string[] loadedData = ParseDataFile("WeaponData.txt").Split('~');

            for (int i = 1; i <= loadedData.Length-1; i++)
            {
                Global.globalAccess.weaponIDs.Add((i-1).ToString(), i-1); // -1 because loop starts at 1 but we want the IDs to start at 0
                string[] loadedValues = loadedData[i].Split(';');
                string loadedAvatar = loadedValues[1];
                string[] Damages = ConfigLabelRemover(loadedValues[2]).Split(',');
                int[] loadedDamageRange = new int[2];

                for (int j = 0; j < Damages.Length; j++)
                {
                    loadedDamageRange[j] = Int32.Parse(Damages[j]);
                }

                weaponValues.names.Add(ConfigLabelRemover(loadedValues[0]));
                weaponValues.avatars.Add(ConfigLabelRemover(loadedAvatar));    
                weaponValues.damages.Add(loadedDamageRange);
            }

            Global.globalAccess.WeaponValues = weaponValues;
        }
        private void L_ItemInfo()
        {
            ItemValues itemValues = new ItemValues();
            string[] loadedData = ParseDataFile("ItemData.txt").Split('~');
            string[] loadedIDs = ConfigLabelRemover(loadedData[0]).Split(',');

            for (int i = 0; i < loadedIDs.Length; i++)
            {
                Global.globalAccess.itemIDs.Add(i.ToString(), i);
            }

            for (int i = 1; i <= loadedIDs.Length; i++)
            {
                string[] loadedValues = loadedData[i].Split(';');
                for (int j = 0; j < loadedValues.Length - 1; j++) // -1 due to each value containing an extra empty space from parsing
                {
                    char loadedAvatar = ConfigLabelRemover(loadedValues[1])[0];
                    int loadedPower = Int32.Parse(ConfigLabelRemover(loadedValues[3]));

                    itemValues.names.Add(ConfigLabelRemover(loadedValues[0]));
                    itemValues.avatars.Add(loadedAvatar);
                    itemValues.effects.Add(ConfigLabelRemover(loadedValues[2]));
                    itemValues.powers.Add(loadedPower);
                    itemValues.desc.Add(ConfigLabelRemover(loadedValues[4]));
                    itemValues.textSuccess.Add(ConfigLabelRemover(loadedValues[5]));
                    itemValues.textFailure.Add(ConfigLabelRemover(loadedValues[6]));
                }
            }

            Global.globalAccess.ItemValues = itemValues;
            //Console.ReadKey(true);
        }
        private bool loadingString = false;
        private string ParseDataFile(string DataDotTxtFile)
        {
            string[] loadedInfo;
            List<char> checkedInfo = new List<char>();
            string combinedData = "";
            
            if (File.Exists(DataDotTxtFile))
            {
                loadedInfo = File.ReadAllLines(DataDotTxtFile);
                
                foreach (string line in loadedInfo)
                {
                    if (line == "" || line[0] == '/') // faster to check for unwanted lines
                    {
                        //do nothing
                    }
                    else 
                    {
                        foreach (char c in line)
                        {
                            if (c == '"')
                            {
                                loadingString = !loadingString;
                            }
                            else if (c != ' ' || loadingString)
                            {
                                checkedInfo.Add(c);
                            }
                        }
                    }
                }
            }
            else { throw new InvalidProgramException("Data file is invalid, missing or corrupt"); }
            combinedData = string.Join("", checkedInfo);

            return combinedData;
        }
        private string ConfigLabelRemover(string configLine)
        {
            string[] labelRemover = configLine.Split(':');

            if (labelRemover.Length > 1)
                return labelRemover[1]; // returns data that isn't the label
            else
                return labelRemover[0];
        }
        #endregion

        //public methods
        public void LoadAllFromFiles()
        {
            //PUT A CHECK TO LOAD CORRECT INFO OTHERWISE RETURN DETAILED AS POSSIBLE ERR
            L_WeaponInfo();
            L_ItemInfo();
            L_EnemyInfo();
        }
        

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
}