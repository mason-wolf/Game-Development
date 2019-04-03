using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Engine
{
    public class Layer
    {
        public List<int> Tiles = new List<int>();
        public string Name;

        public void AddTiles(string[] tileList)
        {
            for (int i = 0; i < tileList.Length; i++)
            {
                Tiles.Add(Int32.Parse(tileList[i]));
            }
        }
    }
}
