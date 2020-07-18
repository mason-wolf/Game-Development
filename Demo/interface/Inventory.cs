using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;

namespace Demo.Interface
{
    class Inventory : Scene
    {
        Texture2D inventoryTexture;
        Texture2D selectedItemTexture;
        Rectangle inventoryInterface;
        SpriteFont inventoryFont;
        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;
        int selectedItem = 0;
        List<Item> itemList = Player.InventoryList;
        public static bool InventoryOpen { get; set; }
        Vector2 Position { get; set; }

        public Inventory(ContentManager content)
        {
            LoadContent(content);
        }

        public override void LoadContent(ContentManager content)
        {
            inventoryTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            inventoryFont = content.Load<SpriteFont>(@"interface\font");
            selectedItemTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            Color selectedItemTextureColor = new Color(1f, 1f, 1f, 1f);
            Color interfaceColor = new Color(0f, 0f, 0f, 0.8f);
            inventoryTexture.SetData(new[] { interfaceColor });
            selectedItemTexture.SetData(new[] { selectedItemTextureColor });
            InventoryOpen = false;
        }

        public override void Update(GameTime gameTime)
        {
            newKeyboardState = Keyboard.GetState();

            Position = new Vector2(StartArea.player.Position.X - 150, StartArea.player.Position.Y - 90);
            inventoryInterface = new Rectangle((int)Position.X, (int)Position.Y, 300, 200);

            // Handle item selection in inventory menu.
            if (newKeyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D))
            {
                if (selectedItem != itemList.Count)
                {
                    selectedItem++;

                    // Ignore and skip the duplicate Rectangles from grid population.
                    if (selectedItem == 7 || selectedItem == 16 || selectedItem == 25 || selectedItem == 34 || selectedItem == 43)
                    {
                        selectedItem += 2;
                    }
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A))
            {
                if (selectedItem != 0)
                {
                    selectedItem--;

                    // Ignore and skip the duplicate Rectangles from grid population.
                    if (selectedItem == 7 || selectedItem == 16 || selectedItem == 25 || selectedItem == 34 || selectedItem == 43)
                    {
                        selectedItem -= 2;
                    }
                }
            }

            if (selectedItem == 42)
            {
                selectedItem = 0;
            }


            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InventoryOpen)
            {
                spriteBatch.Draw(inventoryTexture, null, inventoryInterface);
                spriteBatch.DrawString(inventoryFont, "Inventory", new Vector2(Position.X + 25, Position.Y + 10), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                DrawSelectedItem(spriteBatch);
            }
        }

        public void DrawSelectedItem(SpriteBatch spriteBatch)
        {
            int x = (int)Position.X + 25;
            int y = (int)Position.Y + 30;

            Rectangle itemSlot;

            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    itemSlot = new Rectangle(x, y, 32, 32);
                    Item item1 = new Item();
                    item1.ItemRectangle = itemSlot;
                    itemList.Add(item1);
                    x += 31;
                }

                x = (int)Position.X + 25;
                itemSlot = new Rectangle(x, y, 32, 32);
                Item item2 = new Item();
                item2.ItemRectangle = itemSlot;
                itemList.Add(item2);
                y += 31;
            }

            Item chicken = new Item();
            chicken.ItemTexture = Sprites.chickenTexture;
            itemList[0].ItemTexture = chicken.ItemTexture;
            itemList[1].ItemTexture = chicken.ItemTexture;

            int itemPositionX = itemList[selectedItem].ItemRectangle.X;
            int itemPositionY = itemList[selectedItem].ItemRectangle.Y;
            int itemWidth = itemList[selectedItem].ItemRectangle.Width;
            int itemHeight = itemList[selectedItem].ItemRectangle.Height;

            for (int i = 0; i < itemList.Count; i++)
            {
                if (i < 45 && itemList[i].ItemTexture != null)
                {
                    spriteBatch.Draw(itemList[i].ItemTexture, new Rectangle(itemList[i].ItemRectangle.X, itemList[i].ItemRectangle.Y, 32, 32), Color.White);
                }
            }

            spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY, 1, itemHeight + 1), Color.White);
            spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY, itemWidth + 1, 1), Color.White);
            spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX + itemWidth, itemPositionY, 1, itemHeight + 1), Color.White);
            spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY + itemHeight, itemWidth + 1, 1), Color.White);


            if (itemList.Count > 100)
            {
                itemList.Clear();
            }
        }
    }
}
