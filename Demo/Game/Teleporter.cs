using Demo.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Teleporter
    {
        private Rectangle rectangle;
        private string destinationMap;
        private string sourceMap;

        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        public string GetDestinationMap()
        {
            return destinationMap;
        }

        public string GetSourceMap()
        {
            return sourceMap;
        }
        public bool Enabled { get; set; }

        /// <summary>
        /// Creates a rectangle for the player to intersect and transport to another area.
        /// </summary>
        /// <param name="rectangle">Rectangle dimensions and position</param>
        /// <param name="destinationMap">Name of the scene the teleporter is assigned to</param>
        public Teleporter(Rectangle rectangle, string destinationMap, string sourceMap)
        {
            this.rectangle = rectangle;
            this.destinationMap = destinationMap;
            this.sourceMap = sourceMap;
        }
    }
}
