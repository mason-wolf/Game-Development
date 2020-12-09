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
        private string assignedScene;

        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        public string GetScene()
        {
            return assignedScene;
        }
        public bool Enabled { get; set; }

        /// <summary>
        /// Creates a rectangle for the player to intersect and transport to another area.
        /// </summary>
        /// <param name="rectangle">Rectangle dimensions and position</param>
        /// <param name="assignedScene">Name of the scene the teleporter is assigned to</param>
        public Teleporter(Rectangle rectangle, string assignedScene)
        {
            this.rectangle = rectangle;
            this.assignedScene = assignedScene;
        }
    }
}
