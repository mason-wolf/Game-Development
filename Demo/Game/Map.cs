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
        ContentManager content;
        MapRenderer map;
        string mapName;
        IBox collisionWorld;
        Scene scene;
        Texture2D transitionTexture;
        bool fadeIn;
        Color transitionColor;
        static List<MapObject> mapObjects;
        bool hasFaded = false;

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
            transitionColor = new Color(255, 255, 255, 255);
            fadeIn = false;

            foreach (MapObject mapObject in mapObjects)
            {
                if (mapObject.GetObjectType() == "teleporter")
                {
                    Rectangle teleporterRect = new Rectangle((int)mapObject.GetPosition().X, (int)mapObject.GetPosition().Y, 8, 1);
                    List<string> targetPosition = mapObject.GetCustomProperties();
                    float x = 0;
                    float y = 0;
                    foreach (string p in targetPosition)
                    {
                        if (p != string.Empty)
                        {
                             x = float.Parse(targetPosition[0]);
                             y = float.Parse(targetPosition[1]);
                        }
                    }
                    Teleporter teleporter = new Teleporter(teleporterRect, mapObject.GetName(), mapName, new Vector2(x, y));
                    teleporter.Enabled = true;
                    Init.teleporterList.Add(teleporter);
                }
            }
        }

        public Map() { }

        /// <summary>
        /// Returns the collision world of a map to set boundaries the player cannot cross.
        /// </summary>
        /// <returns></returns>
        public IBox GetCollisionWorld()
        {
            return collisionWorld;
        }

        /// <summary>
        /// Returns the world of a map to allow creation and destruction of collidable objects.
        /// </summary>
        /// <returns></returns>
        public World GetWorld()
        {
            return map.GetWorld();
        }

        public void FadeIn()
        {
            fadeIn = true;
        }

        /// <summary>
        /// Returns a list of map objects that are in the object layer of a tmx map.
        /// </summary>
        /// <returns></returns>
        public List<MapObject> GetMapObjects()
        {
            return map.GetMapObjects();
        }

        /// <summary>
        /// Returns a list of all tiles in the collision layer of a tmx map.
        /// </summary>
        /// <returns></returns>
        public List<Tile> GetCollisionTiles()
        {
            return map.GetCollisionLayer();
        }

        /// <summary>
        /// Sets the color of the transition texture that occurs upon map entry and exit.
        /// </summary>
        /// <param name="color"></param>
        public void SetTransitionColor(Color color)
        {
            this.transitionColor = color;
        }

        /// <summary>
        /// Signals whether the map has finished transitioning and the fade effect has finished.
        /// </summary>
        /// <returns></returns>
        public bool HasFaded()
        {
            return hasFaded;
        }

        public void HasFaded(bool trueFalse)
        {
            hasFaded = trueFalse;
        }

        public string GetMapName()
        {
            return this.mapName;
        }

        /// <summary>
        /// Generates and returns a path finding grid of collidable tiles based upon the collision layer in a tiled map.
        /// </summary>
        /// <returns></returns>
        public RoyT.AStar.Grid GenerateAStarGrid()
        {
            RoyT.AStar.Grid grid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);

            // Block cells in the collision layer for path finding.
            foreach (Tile tile in map.GetCollisionLayer())
            {
                if (tile.TileID != 0)
                {
                    int x = (int)tile.Position.X;
                    int y = (int)tile.Position.Y;

                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            grid.BlockCell(new RoyT.AStar.Position(x, y));
                            x++;
                        }

                        x = (int)tile.Position.X;
                        grid.BlockCell(new RoyT.AStar.Position(x, y));
                        y++;
                    }
                }
            }

            return grid;
        }

        public void SetScene(Scene scene)
        {
            this.scene = scene;
        }

        public void LoadScene()
        {
            if (scene != null )
            {
                scene.LoadContent(content);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (fadeIn && hasFaded == false)
            {
                transitionColor.A -= 5;
                transitionColor.B -= 5;
                transitionColor.G -= 5;

                if (transitionColor.A <= 0)
                {
                    hasFaded = true;
                }
            }

            if (scene != null)
            {
             //   scene.Update(gameTime);
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
                spriteBatch.Draw(transitionTexture, new Rectangle(-1000, -500, 4000, 2000), transitionColor);
            }
        }
    }
}
