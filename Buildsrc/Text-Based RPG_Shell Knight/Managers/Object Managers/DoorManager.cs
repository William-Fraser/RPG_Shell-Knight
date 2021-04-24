using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class DoorManager:ObjectManager
    {
        public List<Door> Init(string[] doorInfo) // initalize door group within a for loop
        {
            List<Door> doors = new List<Door>();

            for (int i = 0; i < doorInfo.Length; i++)
            {
                Door listObject = new Door(doorInfo[i]);
                doors.Add(listObject);
            }

            return doors;
        }
        public void Draw(List<Door> doors, Camera camera) // draw every door within a for loop
        {
            for (int i = 0; i < doors.Count; i++)
            {
                doors[i].Draw(camera);
            }
        }
    }
}
