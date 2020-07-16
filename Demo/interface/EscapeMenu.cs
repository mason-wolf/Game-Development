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
    public class EscapeMenu : Scene
    {
        SpriteFont spriteFont;
        Texture2D buttonImage;

        SpriteBatch spriteBatch;

        Color normalColor = Color.Yellow;
        Color selectedColor = Color.White;
        int selectedIndex = 0;
        Game game;
        private StringCollection menuItems = new StringCollection();

        public EscapeMenu(Game game, GameWindow window, ContentManager content)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            LoadContent(content);
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(value, 0, menuItems.Count - 1);
            }
        }

        public Vector2 Position { get; set; } = new Vector2();

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);

            CalculateBounds();
        }

        private void CalculateBounds()
        {
            Width = buttonImage.Width;
            Height = 0;
            foreach (string item in menuItems)
            {
                Height += 5;
                Height += buttonImage.Height;
            }
        }

        KeyboardState oldState = Keyboard.GetState();

        public override void LoadContent(ContentManager content)
        {
            buttonImage = content.Load<Texture2D>(@"interface\menu");
            spriteFont = content.Load<SpriteFont>(@"interface\font");
        }

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

            if (SelectedIndex == 0 && newState.IsKeyDown(Keys.E))
            {
                // Sends a signal to player object that player selected 'Continue' option.
                Player.pressedContinued = true;
            }

            if (SelectedIndex == 3 && newState.IsKeyDown(Keys.E))
            {
                game.Exit();
            }

            oldState = newState;
        }

        Vector2 textPosition;

        public override void Draw(SpriteBatch spriteBatch)
        {

            textPosition = Position;
            Rectangle buttonRectangle = new Rectangle((int)Position.X - 20, (int)Position.Y + 75, buttonImage.Width, buttonImage.Height);
            Color color;

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == selectedIndex)
                {
                    color = normalColor;
                }
                else
                {
                    color = selectedColor;
                }

                spriteBatch.Draw(buttonImage, buttonRectangle, Color.White);
                textPosition = new Vector2(buttonRectangle.X + (buttonImage.Width / 2), buttonRectangle.Y + (buttonImage.Height / 2));
                Vector2 textSize = spriteFont.MeasureString(menuItems[i]);
                textPosition.X -= textSize.X / 2;
                textPosition.Y -= spriteFont.LineSpacing / 3;
                spriteBatch.DrawString(spriteFont, menuItems[i], textPosition, color);
                buttonRectangle.Y += buttonImage.Height;
                buttonRectangle.Y += 5;
            }
        }
    }
}
