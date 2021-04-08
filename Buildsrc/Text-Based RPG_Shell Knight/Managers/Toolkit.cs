using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Text_Based_RPG_Shell_Knight
{
    class Toolkit
    {
        public int RandomNumBetween(int lowNumber, int highNumber) // used to find random numbers
        {
            Random rdm = new Random();
            int finalDamage = rdm.Next(lowNumber, highNumber);
            return finalDamage;
        }
        
    }
}
