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
    public class Map 
    {
        public ContentManager content;
        public MapRenderer map;
        public string mapName;
        IBox collisionWorld;
        Scene scene;
        Texture2D splash;
        bool fadeIn;
        public Color color;

        /// <summary>
        /// Loads and renders a map. Every map has collision and a basic screen transition effect.
        /// </summary>
        /// <param name="content">Content Manager</param>
        /// <param name="mapName">Map Name</param>
        public Map(ContentManager content, string mapName)
        {
            this.mapName = mapName;
            map = new MapRenderer();
            map.LoadMap(content, mapName);
            this.content = content;
            collisionWorld = map.GenerateCollisionWorld();
            splash = content.Load<Texture2D>(@"interface/titlescreen");
            color = new Color(255, 255, 255, 255);
            fadeIn = false;
        }

        public Map() { }

        public IBox GetCollisionWorld()
        {
            return collisionWorld;
        }

        public void FadeIn()
        {
            fadeIn = true;
        }

        // Adds a scene associated with this map.
        public void AddScene(Scene scene)
        {
            this.scene = scene;

            if (scene != null)
            {
                scene.LoadContent(content);
            }
        }

        public bool hasFaded = false;

        public void Update(GameTime gameTime)
        {
            if (fadeIn && hasFaded == false)
            {
                color.A -= 5;
                color.B -= 5;
                color.G -= 5;

                if (color.A == 0)
                {
                    hasFaded = true;
                }
            }

            if (scene != null)
            {
                scene.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            map.Draw(spriteBatch);

            if (scene != null)
            {
                scene.Draw(spriteBatch);
            }

            if (fadeIn == true)
            {
                spriteBatch.Draw(splash, new Rectangle(0, 0, 1080, 1800), color);
            }


        }
    }
}
