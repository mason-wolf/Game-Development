using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Demo.Scenes
{
    public class ScreenManager : DrawableGameComponent
    {
        public Game game;
        public SpriteBatch spriteBatch;
        ContentManager content;
        List<Screen> screens = new List<Screen>();

        public ScreenManager(Game game) : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            content = (ContentManager)Game.Services.GetService(typeof(ContentManager));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            foreach (Screen screen in screens)
            {
                screen.LoadContent();
            }

            base.LoadContent();
        }

        public void AddScene(Screen screen)
        {
            screens.Add(screen);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Screen screen in screens)
            {
                if (screen.Enabled)
                {
                    screen.Draw(gameTime);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Screen screen in screens)
            {
                if (screen.Enabled)
                {
                    screen.Draw(gameTime);
                }
            }
            base.Update(gameTime);
        }
    }
}
