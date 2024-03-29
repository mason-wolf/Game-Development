﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Item
    {
        public Texture2D ItemTexture { get; set; }
        public int HealthAmount { get; set; }
        public Rectangle ItemRectangle { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
