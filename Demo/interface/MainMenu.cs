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
using System.Collections.Specialized;
using Demo.Scenes;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended;

namespace Demo.Interface
{
    public class MainMenu : SceneManager
    {
        private BoxingViewportAdapter viewPortAdapter;
        private Camera2D camera;
        SpriteFont spriteFont;
        Texture2D buttonImage;
        Texture2D background;

        Color normalColor = Color.Yellow;
        Color hiliteColor = Color.White;

        Vector2 position = new Vector2();

        int selectedIndex = 0;

        private StringCollection menuItems = new StringCollection();

        int width, height;

        public MainMenu(Game game, GameWindow window, SpriteFont spriteFont, Texture2D buttonImage, Texture2D background)
            : base(game)
        {
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            this.spriteFont = spriteFont;
            this.buttonImage = buttonImage;
            this.background = background;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }

        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = MathHelper.Clamp( value, 0, 2);
            }
        }

        public Color NormalColor
        {
            get { return normalColor; }
            set { normalColor = value; }
        }

        public Color HiliteColor
        {
            get { return hiliteColor; }
            set { hiliteColor = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);

            CalculateBounds();
        }

        private void CalculateBounds()
        {
            width = buttonImage.Width;
            height = 0;
            foreach (string item in menuItems)
            {
                height += 5;
                height += buttonImage.Height;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        KeyboardState oldState = Keyboard.GetState();

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.S) && oldState.IsKeyUp(Keys.S))
            {
                SelectedIndex++;
            }

            if (newState.IsKeyDown(Keys.W) && oldState.IsKeyUp(Keys.W))
            {
                SelectedIndex--;
            }

            camera.Zoom = 4;
            Vector2 cameraPosition = new Vector2(textPosition.X, textPosition.Y - 50);
            camera.LookAt(cameraPosition);
            oldState = newState;
            base.Update(gameTime);
        }

        Vector2 textPosition;

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
            textPosition = Position;
            Rectangle buttonRectangle = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                buttonImage.Width,
                buttonImage.Height);

            Color myColor;

            spriteBatch.Draw(background, new Vector2(textPosition.X - 121, textPosition.Y - 106), Color.White);

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                    myColor = normalColor;
                else
                    myColor = hiliteColor;

                spriteBatch.Draw(buttonImage,
                    buttonRectangle,
                    Color.White);

                textPosition = new Vector2(
                    buttonRectangle.X + (buttonImage.Width / 2),
                    buttonRectangle.Y + (buttonImage.Height / 2));

                Vector2 textSize = spriteFont.MeasureString(menuItems[i]);
                textPosition.X -= textSize.X / 2;
                textPosition.Y -= spriteFont.LineSpacing / 3;

                spriteBatch.DrawString(spriteFont,
                    menuItems[i],
                    textPosition,
                    myColor);
                buttonRectangle.Y += buttonImage.Height;
                buttonRectangle.Y += 5;
            }
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
