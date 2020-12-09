using Demo.Engine;
using Demo;
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
        Texture2D transitionTexture;
        bool fadeIn;
        public Color color;
        List<MapObject> mapObjects;

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
            mapObjects = map.GetMapObjects();
            this.content = content;
            collisionWorld = map.GenerateCollisionWorld();
            transitionTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            transitionTexture.SetData(new Color[] { Color.Black });
            color = new Color(255, 255, 255, 255);
            fadeIn = false;
        }

        public Map() { }

        public IBox GetCollisionWorld()
        {
            return collisionWorld;
        }

        public World GetWorld()
        {
            return map.GetWorld();
        }

        public void FadeIn()
        {
            fadeIn = true;
        }

        public List<MapObject> GetMapObjects()
        {
            return map.GetMapObjects();
        }

        public void LoadScene(Scene scene)
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
        /// <summary>
        /// Adds an object to the collision world on the map.
        /// </summary>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public void AddCollidable(float x, float y, int width, int height)
        {
            World world = map.GetWorld();
            world.Create(x, y, width, height);
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
                spriteBatch.Draw(transitionTexture, new Rectangle(0, 0, 1080, 1800), color);
            }
        }
    }
}
