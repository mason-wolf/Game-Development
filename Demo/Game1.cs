using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demo.Scenes;
using MonoGame.Extended;
using System.Collections.Generic;
using System;

namespace Demo
{
    // Monogame Version 3.5.1.1679
    // http://www.monogame.net/2016/03/17/monogame-3-5/
    // Visual C++ Redistributable for Visual Studio 2012 Update 4 
    // https://www.microsoft.com/en-nz/download/confirmation.aspx?id=30679
    // Tiled Version 1.2.1

    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SceneManager start;
        public static Vector2 CenterScreen => new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            Services.AddService(typeof(ContentManager), Content);
 
            // Show the start screen
            start = new Start(this, Window);

            Components.Add(start);
            start.Show();
                 
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(25, 25, 25));
            base.Draw(gameTime);
        }
    }
}
