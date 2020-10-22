
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
        Vector2 position;
        Rectangle boundingBox;
        AnimatedSprite animatedSprite;
        bool destroyed = false;
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
            return boundingBox;
        }

        public void SetSprite(AnimatedSprite animatedSprite)
        {
            this.animatedSprite = animatedSprite;
        }

        public AnimatedSprite GetSprite()
        {
            return animatedSprite;
        }

        public void SetCollisionBox(IBox collisionBox)
        {
            this.collisionBox = collisionBox;
        }

        public IBox GetCollisionBox()
        {
            return collisionBox;
        }

        public bool isDestroyed()
        {
            return destroyed;
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
            if (animatedSprite != null)
            {
                animatedSprite.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (animatedSprite != null)
            {
                animatedSprite.Draw(spriteBatch);
            }
        }

        public MapObject(string objectName, Vector2 position)
        {
            this.name = objectName;
            this.position = position;
            this.boundingBox = new Rectangle((int)position.X, (int)position.Y - 5, 10, 10);
        }
    }
}
