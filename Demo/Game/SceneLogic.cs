using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Engine
{
    public abstract class SceneLogic : Scene
    {
        public abstract Map Map { get; set; }
        public abstract ContentManager ContentManager { get; set; }
        public abstract List<MapObject> MapObjects { get; set; }
    }
}
