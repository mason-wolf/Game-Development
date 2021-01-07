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
    class SaveMenu : EscapeMenu
    {

        Game game;
        int selectedIndex = 0;
        int[] savedSlots = new int[3];
        Texture2D floppy;

        public static bool GameSaved = false;
        new int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = MathHelper.Clamp(value, 0, 2);
            }
        }

        public SaveMenu(Game game, GameWindow window, ContentManager content) : base (game, window, content)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }

        public override void LoadContent(ContentManager content)
        {
            floppy = content.Load<Texture2D>(@"interface\floppy");
            base.LoadContent(content);
            CheckSaves();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textPosition = Position;
            Rectangle buttonRectangle = new Rectangle((int)Position.X - 50, (int)Position.Y + 75, 125, 25);
            Rectangle floppyRectangle = new Rectangle((int)Position.X - 50, (int)Position.Y + 75, 32, 32);
            Color color = Color.White;

            Rectangle[] saveSlots = new Rectangle[3];

            Vector2 titlePosition = new Vector2(Position.X - 50, Position.Y + 55);
            Vector2 gameSavedMessagePosition = new Vector2(Position.X - 50, Position.Y + 175);
            spriteBatch.DrawString(spriteFont, "Save Game", titlePosition, color);

            for (int i = 0; i < 3; i++)
            {
                if (i == SelectedIndex)
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
                saveSlots[i] = buttonRectangle;
                saveSlots[i].Width = 16;
                saveSlots[i].Height = 16;
                saveSlots[i].X = buttonRectangle.X + 100;
                saveSlots[i].Y = buttonRectangle.Y + 5;
                buttonRectangle.Y += 25;
                buttonRectangle.Y += 5;
            }

            for (int i = 0; i < savedSlots.Length; i++)
            {
                if (savedSlots[i] == 1)
                {
                    spriteBatch.Draw(floppy, saveSlots[i], Color.White);
                }
            }

            if (GameSaved)
            {
                spriteBatch.DrawString(spriteFont, "Game saved", gameSavedMessagePosition, Color.GreenYellow);
            }
        }

        public override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

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
            }

            if (SelectedIndex == 1 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                SaveGame(2);
            }

            if (SelectedIndex == 2 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                SaveGame(3);
            }

            oldState = newState;
        }

        /// <summary>
        /// Checks if saves exists and assigns the slots to 1 (True) if they do.
        /// </summary>
        void CheckSaves()
        {
            for (int i = 0; i < savedSlots.Length; i++)
            {
                if (File.Exists("Save_" + (i + 1) + ".txt"))
                {
                    savedSlots[i] = 1;
                }
            }
        }

        void SaveGame(int saveSlot)
        {
            using (StreamWriter streamWriter = new StreamWriter("Save_" + saveSlot + ".txt"))
            {
                streamWriter.WriteLine("player_health="+Init.Player.CurrentHealth);
                streamWriter.WriteLine("arrows=" + Inventory.TotalArrows);
                streamWriter.WriteLine("location=" + Player.CurrentLevel);
                streamWriter.WriteLine("player_position=" + Init.Player.Position.X + "," + Init.Player.Position.Y);
                foreach(Item item in Inventory.itemList)
                {
                    if (item.Name != "")
                    {
                        streamWriter.WriteLine("inventory_item=" + item.Name + "," + item.Quantity);
                    }
                }
            }
            savedSlots[saveSlot - 1] = 1;
            GameSaved = true;
        }
    }
}
