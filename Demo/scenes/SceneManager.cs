using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;


namespace Demo.Scenes
{

    public class SceneManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected List<GameComponent> childComponents;
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;
        protected Game game;

        public SceneManager(Game game)
            : base(game)
        {
            this.game = game;
            spriteBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            Content =
                (ContentManager)Game.Services.GetService(typeof(ContentManager));
            childComponents = new List<GameComponent>();
            Visible = false;
            Enabled = false;
        }

        public List<GameComponent> Components
        {
            get { return childComponents; }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent child in childComponents)
            {
                if (child.Enabled)
                {
                    child.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent child in childComponents)
            {
                if ((child is DrawableGameComponent) &&
                    ((DrawableGameComponent)child).Visible)
                {
                    ((DrawableGameComponent)child).Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }
        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }
    }
}
