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


namespace Demo.Interface
{

    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteFont spriteFont;
        SpriteBatch spriteBatch;
        Texture2D buttonImage;

        Color normalColor = Color.Black;
        Color hiliteColor = Color.White;

        Vector2 position = new Vector2();

        int selectedIndex = 0;

        private StringCollection menuItems = new StringCollection();

        int width, height;

        public Menu(Game game, SpriteFont spriteFont, Texture2D buttonImage)
            : base(game)
        {
            this.spriteFont = spriteFont;
            this.buttonImage = buttonImage;

            spriteBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
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
                selectedIndex = (int)MathHelper.Clamp(
                        value,
                        0,
                        menuItems.Count - 1);
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

        public override void Update(GameTime gameTime)
        {
            if (Game1.CheckKey(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Count)
                    selectedIndex = 0;
            }

            if (Game1.CheckKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex == -1)
                {
                    selectedIndex = menuItems.Count - 1;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Vector2 textPosition = Position;
            Rectangle buttonRectangle = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                buttonImage.Width,
                buttonImage.Height);

            Color myColor;

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                    myColor = HiliteColor;
                else
                    myColor = NormalColor;

                spriteBatch.Draw(buttonImage,
                    buttonRectangle,
                    Color.White);

                textPosition = new Vector2(
                    buttonRectangle.X + (buttonImage.Width / 2),
                    buttonRectangle.Y + (buttonImage.Height / 2));

                Vector2 textSize = spriteFont.MeasureString(menuItems[i]);
                textPosition.X -= textSize.X / 2;
                textPosition.Y -= spriteFont.LineSpacing / 2;

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
