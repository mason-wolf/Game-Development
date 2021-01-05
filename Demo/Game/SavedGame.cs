using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Engine
{
    /// <summary>
    /// Represents a saved game. Loading a save creates a SavedGame object to load saved progress.
    /// </summary>
    class SavedGame
    {
        public int PlayerHealth { get; set; }
        public int Arrows { get; set; }
        public string Location { get; set; }
        public Vector2 Position { get; set; }
        public List<Item> InventoryList = new List<Item>();
    }
}
