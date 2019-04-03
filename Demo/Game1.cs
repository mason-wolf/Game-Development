using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demo.Scenes;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Demo
{

    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont font;
        public static SceneManager start;
        public static SceneManager main;
        public static SceneManager area_1;
        public static KeyboardState oldState;
        public static KeyboardState newState;

        static SpriteBatch tileSpriteBatch;
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

            main = new Village(this, Window);
            Components.Add(main);
            main.Hide();

            area_1 = new Area_1(this, Window);
            Components.Add(area_1);
           // area_1.Hide();
            area_1.Show();
          // main.Show();

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
 //           spriteBatch.Begin();
            base.Draw(gameTime);
 //           spriteBatch.End();
        }
    }
}
