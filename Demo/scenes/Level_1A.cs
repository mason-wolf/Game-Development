
using Demo.Engine;
using Demo;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Microsoft.Xna.Framework.Input;
using Demo.Interface;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Demo.Scenes
{
    public class Level_1A : SceneLogic
    {
        public override List<MapObject> MapObjects { get; set; }
        public override ContentManager ContentManager { get; set; }
        public override Map Map { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (MapObject mapObject in MapObjects)
            {
                if (Init.Player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.ActionButtonPressed && mapObject.GetName() == "Chest")
                {
                    if (!mapObject.ItemPickedUp())
                    {
                        mapObject.GetSprite().Play("Opened");
                        Init.Message = "You obtained a key";
                        Init.MessageEnabled = true;
                        mapObject.PickUpItem();
                        Inventory.TotalKeys = Inventory.TotalKeys += 1;
                    }
                }
            }
        }
    }
}
