using Demo.Engine;
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
    class LoadMenu : EscapeMenu
    {
        Game game;
        GameWindow window;
        ContentManager content;
        int selectedIndex = 0;
        int[] savedSlots = new int[3];
        Texture2D floppy;
        SavedGame savedGame;
        Texture2D transitionTexture;
        bool fadeIn = false;
        bool hasFaded = false;
        public Color transitionColor;

        public static bool GameLoaded { get; set; }
        new int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(value, 0, 2);
            }
        }

        public LoadMenu(Game game, GameWindow window, ContentManager content) : base(game, window, content)
        {
            this.game = game;
            this.window = window;
            this.content = content;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            floppy = content.Load<Texture2D>(@"interface\floppy");
            LoadContent(content);
            CheckSaves();
            transitionTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            transitionTexture.SetData(new Color[] { Color.Black });
            transitionColor = new Color(0, 0, 0, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textPosition = Position;
            Rectangle buttonRectangle = new Rectangle((int)Position.X - 50, (int)Position.Y + 75, 125, 25);
            Rectangle floppyRectangle = new Rectangle((int)Position.X - 50, (int)Position.Y + 75, 32, 32);
            Color color = Color.White;
            Vector2 titlePosition = new Vector2(Position.X - 50, Position.Y + 55);
            spriteBatch.DrawString(spriteFont, "Load Game", titlePosition, color);

            Rectangle[] saveSlots = new Rectangle[3];

            // Draw three save slots.
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

            // Draw the saved slots for the files found.
            for (int i = 0; i < savedSlots.Length; i++)
            {
                if (savedSlots[i] == 1)
                {
                    spriteBatch.Draw(floppy, saveSlots[i], Color.White);
                }
            }

            if (fadeIn == true)
            {
                spriteBatch.Draw(transitionTexture, new Rectangle(0, 0, 1080, 1800), transitionColor);
            }
        }

        /// <summary>
        /// Checks if saves exists.
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

        int selectedSave = 0;
        public override void Update(GameTime gameTime)
        {
            if (fadeIn && hasFaded == false)
            {
                transitionColor.A += 15;
                transitionColor.B += 15;
                transitionColor.G += 15;

                if (transitionColor.A >= 255)
                {
                    hasFaded = true;
                }
            }

            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.S) && oldState.IsKeyUp(Keys.S))
            {
                SelectedIndex++;
            }

            if (newState.IsKeyDown(Keys.W) && oldState.IsKeyUp(Keys.W))
            {
                SelectedIndex--;
            }
            
            // Save Slot 1
            if (SelectedIndex == 0 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                fadeIn = true;
                selectedSave = 1;
            }

            // Save Slot 2
            if (SelectedIndex == 1 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                fadeIn = true;
                selectedSave = 2;
            }

            // Save Slot 3
            if (SelectedIndex == 2 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                fadeIn = true;
                selectedSave = 3;
            }

            if (hasFaded)
            {
                fadeIn = false;
                hasFaded = false;
                transitionColor = new Color(0, 0, 0, 0);
                Init.Reload();
                LoadGame(selectedSave);
            }
            oldState = newState;
        }


        /// <summary>
        /// Load saved game from file.
        /// </summary>
        /// <param name="saveSlot"></param>
        void LoadGame(int saveSlot)
        {
            savedGame = new SavedGame();

            using (StreamReader streamReader = new StreamReader("Save_" + saveSlot + ".txt"))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    string value = line.Split('=').First();
                    switch(value)
                    {
                        case ("player_health"):
                            value = line.Split('=').Last();
                            savedGame.PlayerHealth = (int)Math.Round(Double.Parse(value));
                            break;
                        case ("arrows"):
                            value = line.Split('=').Last();
                            savedGame.Arrows = Int32.Parse(value);
                            break;
                        case ("location"):
                            value = line.Split('=').Last();
                            savedGame.Location = value;
                            break;
                        case ("player_position"):
                            value = line.Split('=').Last();
                            string[] vector = value.Split(',');
                            savedGame.Position = new Vector2(float.Parse(vector[0]), float.Parse(vector[1]));
                            break;
                        case ("inventory_item"):
                            value = line.Split('=').Last();
                            string[] tempItem = value.Split(',');
                            Item item = new Item();
                            item.Name = tempItem[0];
                            item.Quantity = Int32.Parse(tempItem[1]);
                            savedGame.InventoryList.Add(item);
                            break;
                    }
                }

                Init.SavedGamePosition = savedGame.Position;
                Init.Player.CurrentHealth = savedGame.PlayerHealth;
                Inventory.SavedGameLoaded = true;
                Inventory.TotalArrows = savedGame.Arrows;
                Init.SavedGameLocation = savedGame.Location;

                Init.Player.InMenu = false;
                foreach (Item item in savedGame.InventoryList)
                {
                    switch(item.Name)
                    {
                        case ("chicken"):
                            Inventory.TotalChickens = item.Quantity;
                            break;
                    }
                }

            }
        }
    }
}
