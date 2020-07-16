using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Scenes
{
    class ScreenManager : DrawableGameComponent
    {
        protected Game game;
        protected ContentManager content;
        protected SpriteBatch spriteBatch;
        Start startMenu;
        StartArea startArea;

        public enum Scene
        {
            StartMenu,
            StartArea
        }

        public Scene SelectedScene { get; set; }

        public ScreenManager(Game game) : base(game)
        {
            this.game = game;
            content = (ContentManager)Game.Services.GetService(typeof(ContentManager));
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            startMenu = new Start(game, Game.Window);

            //   startMenu = new StartMenu(game, window, content, spriteBatch);
            // startArea = new StartArea(game, window);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            switch (SelectedScene)
            {
                case Scene.StartMenu:
                    startMenu.Update(gameTime);
                    break;
                case Scene.StartArea:
                    startArea.Update(gameTime);
                    break;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            switch (SelectedScene)
            {
                case Scene.StartMenu:
                    startMenu.Draw(gameTime);
                    break;
                case Scene.StartArea:
                    startArea.Draw(gameTime);
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
