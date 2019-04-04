using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Demo.Engine
{
    public class Map
    {
        Texture2D mapTexture;
        TextureAtlas mapAtlas;
        List<Layer> layers;
        int mapWidth;
        int mapHeight;
        int tileWidth = 16;
        int tileHeight = 16;

        public void LoadMap(ContentManager content, string filePath)
        {
            layers = new List<Layer>();

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.LocalName == "map")
                    {
                        if (reader.GetAttribute("width") != null && reader.GetAttribute("height") != null)
                        {
                            mapWidth = Int32.Parse(reader.GetAttribute("width"));
                            mapHeight = Int32.Parse(reader.GetAttribute("height"));
                        }
                    }
     
                    if (reader.LocalName == "image")
                    {
                        string tilesetFilePath = "tilesets/" + Path.GetFileNameWithoutExtension(reader.GetAttribute("source"));
                        mapTexture = content.Load<Texture2D>(tilesetFilePath);
                        mapAtlas = TextureAtlas.Create(mapTexture, 16, 16);
                    }

                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "layer")
                    {
                        Layer newLayer = new Layer();
                        newLayer.Name = reader.GetAttribute("name");
                        reader.ReadToFollowing("data");
                        string[] tiles = reader.ReadElementContentAsString().Split(',');
                        newLayer.AddTiles(tiles);
                        layers.Add(newLayer);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (int tile in layers[0].Tiles)
            {
   
                        TextureRegion2D region = mapAtlas.GetRegion(tile);
                        Rectangle sourceRectangle = region.Bounds;
                        Rectangle destinationRectangle = new Rectangle(0, 0, region.Width, region.Height);
                        spriteBatch.Draw(region.Texture, destinationRectangle, sourceRectangle, Color.White);

            }

        }
    }
}

