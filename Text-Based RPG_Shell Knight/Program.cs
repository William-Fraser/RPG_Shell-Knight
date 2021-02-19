using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Program
    {
        static void Main(string[] args)
        {
            GameMANAGE GameMAN = new GameMANAGE();

            while (true)
            {
                GameMAN.GameplayMap();
            }
        }
    }
}
