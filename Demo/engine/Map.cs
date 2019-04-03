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
     
        public void LoadMap(string filePath)
        {
            List<Layer> layers = new List<Layer>();

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.LocalName == "image")
                    {
                        Console.WriteLine(reader.GetAttribute("source"));
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

            foreach (Layer layer in layers)
            {
                Console.WriteLine(layer.Name);
                Console.WriteLine(layer.Tiles);
            }
            
        }
    }
}
