
using Demo.Interface;
using Demo.Scenes;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class MapObject : Scene
    {
        string name;
        GameTime gameTime;
        Vector2 position;
        Rectangle objectBoundingBox;
        AnimatedSprite animatedSprite;
        Item containedItem;
        Rectangle containedItemBoundingBox;
        bool destroyed = false;
        bool itemPickedUp = false;
        IBox collisionBox;

        public string GetName()
        {
            return name;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public Rectangle GetBoundingBox()
        {
            return objectBoundingBox;
        }

        public void SetSprite(AnimatedSprite animatedSprite)
        {
            this.animatedSprite = animatedSprite;
        }

        public AnimatedSprite GetSprite()
        {
            return animatedSprite;
        }

        public void SetContainedItem(Item item)
        {
            this.containedItem = item;
        }

        public void SetCollisionBox(IBox collisionBox)
        {
            this.collisionBox = collisionBox;
        }

        public IBox GetCollisionBox()
        {
            return collisionBox;
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }

        public bool ItemPickedUp()
        {
            return itemPickedUp;
        }

        public void Destroy()
        {
            destroyed = true;
        }

        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            if (animatedSprite != null)
            {
                animatedSprite.Update(gameTime);
            }

            // If the player picks up an item, add it to the inventory.
            if (containedItem != null && Init.Player.BoundingBox.Intersects(containedItemBoundingBox) && !itemPickedUp)
            {
                switch (containedItem.Name)
                {
                    case "Chicken":
                        Inventory.TotalChickens = Inventory.TotalChickens + 1;
                        break;
                }

                itemPickedUp = true;
            }
        }

        bool itemDrawn = false;

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (animatedSprite != null)
            {
                animatedSprite.Draw(spriteBatch);
            }

            // If the object is destroyed, drop an item. (Draw and create a bounding box).
            if (destroyed && !itemPickedUp)
            {
                itemDrawn = true;

                int x = 0;
                int y = 0;

                // Spawn the item to the left or right of the container depending on the direction the player intersects.
                if (position.X < Init.Player.Position.X && itemDrawn)
                {
                    x = (int)position.X - 5;
                    y = (int)position.Y;
                }
                else if (position.X > Init.Player.Position.X && itemDrawn)
                {
                    x = (int)position.X + 3;
                    y = (int)position.Y;
                }
                spriteBatch.Draw(containedItem.ItemTexture, new Rectangle(x, y, 16, 16), Color.White);
                containedItemBoundingBox = new Rectangle(x, y, 1, 1);
            }
        }

        public MapObject(string objectName, Vector2 position)
        {
            this.name = objectName;
            this.position = position;
            this.objectBoundingBox = new Rectangle((int)position.X, (int)position.Y - 5, 10, 10);
        }
    }
}
