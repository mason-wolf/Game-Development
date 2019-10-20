using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Engine
{
    public class Tile
    {
        public int TileID { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Rectangle { get; set; }
        public Vector2 Position { get; set; }

        public Tile(int tileId)
        {
            this.TileID = tileId;
        }

        public Tile(Vector2 position)
        {
            this.Position = position;
        }

        public Tile(int tileId, Vector2 position)
        {
            this.TileID = tileId;
            Position = position;
        }

        public Tile(Texture2D texture, Rectangle rectangle)
        {
            this.Texture = texture;
            this.Rectangle = rectangle;
        }

    }
}
