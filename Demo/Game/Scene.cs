using Demo.Engine;
using Demo.Scenes;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Scene 
    {
        public Map map;
        public string mapName;
        IBox collisionWorld;
        Texture2D splash;
        bool fadeIn;
        Color color;

        public Scene(ContentManager content, string mapName)
        {
            this.mapName = mapName;
            map = new Map();
            map.LoadMap(content, mapName);
            collisionWorld = map.GenerateCollisionWorld();
            splash = content.Load<Texture2D>(@"interface/titlescreen");
            color = new Color(0, 0, 0, 0);
        }

        public IBox GetCollisionWorld()
        {
            return collisionWorld;
        }

        public void FadeIn()
        {
            fadeIn = true;
        }

        bool hasFaded = false;

        public void Update(GameTime gameTime)
        {
            if (fadeIn && hasFaded == false)
            {
                color.A += 5;
                color.B += 5;
                color.G += 5;

                if (color.A == 255)
                {
                    hasFaded = true;
                }
            }

            if (hasFaded)
            {
                color.A -= 5;
                color.B -= 5;
                color.G -= 5;

                if (color.A == 0)
                {
                    hasFaded = false;
                    fadeIn = false;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
            spriteBatch.Draw(splash, new Rectangle(0, 0, 1080, 1800), color);
        }
    }
}
