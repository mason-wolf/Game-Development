using Demo.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Scenes
{
    class SaveMenu : Scene
    {
        SpriteFont spriteFont;
        Texture2D menuItem;
        Texture2D menuItemSelected;
        SpriteBatch spriteBatch;
        Game game;
        Vector2 textPosition;
        int selectedIndex = 0;
        Color normalColor = Color.Yellow;
        Color selectedColor = Color.White;
        KeyboardState oldState = Keyboard.GetState();
        public Vector2 Position { get; set; } = new Vector2();
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(value, 0, 3 - 1);
            }
        }

        public SaveMenu(Game game, GameWindow window, ContentManager content)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textPosition = Position;
            Rectangle buttonRectangle = new Rectangle((int)Position.X - 50, (int)Position.Y + 75, 125, 25);
            Color color = Color.White;

            for (int i = 0; i < 3; i++)
            {
                if (i == selectedIndex)
                {
                    spriteBatch.Draw(menuItemSelected, buttonRectangle, Color.White);
                }
                else
                {
                    spriteBatch.Draw(menuItem, buttonRectangle, Color.White);
                }
                textPosition = new Vector2(buttonRectangle.X + (menuItem.Width / 2), buttonRectangle.Y + (menuItem.Height / 2) + 5);
                Vector2 textSize = spriteFont.MeasureString("File");
                textPosition.X -= textSize.X / 2;
                textPosition.Y -= spriteFont.LineSpacing / 3;
                spriteBatch.DrawString(spriteFont, "File " + (i + 1), textPosition, color);
                buttonRectangle.Y += 25;
                buttonRectangle.Y += 5;
            }
          }

        public override void LoadContent(ContentManager content)
        {
            menuItem = content.Load<Texture2D>(@"interface\menu");
            menuItemSelected = content.Load<Texture2D>(@"interface\menuItemSelected");
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

            if (SelectedIndex == 0 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                SaveGame(1);
                // Sends a signal to player object that player selected 'Continue' option.
           //     Player.pressedContinued = true;
            }

            if (SelectedIndex == 1 && newState.IsKeyDown(Keys.E))
            {
            //    Init.SelectedScene = Init.Scene.SaveMenu;
            }

            if (SelectedIndex == 2 && newState.IsKeyDown(Keys.E))
            {
            //    game.Exit();
            }

            oldState = newState;
        }

        void SaveGame(int saveSlot)
        {
            using (StreamWriter streamWriter = new StreamWriter("Save_" + saveSlot + ".txt"))
            {
                streamWriter.WriteLine("player_health="+Init.player.CurrentHealth);
                streamWriter.WriteLine("arrows=" + Inventory.TotalArrows);
                streamWriter.WriteLine("location=" + Init.SelectedScene);
                streamWriter.WriteLine("player_postiion=" + Init.player.Position);
                foreach(Item item in Inventory.itemList)
                {
                    if (item.Name != "")
                    {
                        streamWriter.WriteLine("inventory_item=" + item.Name);
                        streamWriter.WriteLine("item_quantity=" + item.Quantity);
                    }
                }
            }
        }
    }
}
