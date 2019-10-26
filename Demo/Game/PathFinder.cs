using Microsoft.Xna.Framework;
using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class PathFinder
    {
        public void Update(GameTime gameTime)
        {

        }

        public void FindPathToPlayer(Grid grid, Entity entity, Entity playerEntity)
        {
                var movementPattern = new[] { new Offset(-1, 0), new Offset(0, -1), new Offset(1, 0), new Offset(0, 1) };
                Position[] path = grid.GetPath(new Position((int)entity.Position.X, (int)entity.Position.Y), new Position((int)playerEntity.Position.X, (int)playerEntity.Position.Y), movementPattern);
        }
    }
}
