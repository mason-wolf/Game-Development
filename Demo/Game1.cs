using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demo.Scenes;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Demo
{
    // Monogame Version 3.5.1.1679
    // http://www.monogame.net/2016/03/17/monogame-3-5/
    // Visual C++ Redistributable for Visual Studio 2012 Update 4 
    // https://www.microsoft.com/en-nz/download/confirmation.aspx?id=30679

    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont font;
        public static SceneManager start;
        public static SceneManager testMap;
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static SpriteBatch tileSpriteBatch;
        public static Camera2D Camera;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;   
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }


        public static bool CheckKey(Keys theKey)
        {
            return oldState.IsKeyDown(theKey) && newState.IsKeyUp(theKey);
        }

        public static SpriteBatch TileSpriteBatch
        {
            get { return tileSpriteBatch; }
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileSpriteBatch = new SpriteBatch(GraphicsDevice);

            Services.AddService(typeof(SpriteBatch), spriteBatch);
            Services.AddService(typeof(ContentManager), Content);
 
            font = Content.Load<SpriteFont>(@"interface\font");
            start = new Start(this);
            Components.Add(start);
            start.Hide();

            testMap = new TestMap(this, Window);
            Components.Add(testMap);
            testMap.Show();

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
