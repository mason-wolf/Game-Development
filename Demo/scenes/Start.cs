using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Demo.Scenes;
using Demo.Interface;
using Microsoft.Xna.Framework.Input;

namespace Demo
{
    class Start : SceneManager
    {
        public Menu buttonMenu;
        SpriteFont spriteFont;
        Texture2D background;
        Texture2D buttonImage;
        GameWindow window;


        public Start(Game game, GameWindow window)
            : base(game)
        {
            LoadContent();

            this.window = window;
            this.game = game;

            string[] items = { "START", "QUIT" };

            buttonMenu = new Menu(
                game,
                window,
                spriteFont,
                buttonImage);

            buttonMenu.SetMenuItems(items);
            
            Components.Add(buttonMenu);
            buttonMenu.Show();
        }

        public int SelectedIndex
        {
            get { return buttonMenu.SelectedIndex; }
        }

        protected override void LoadContent()
        {
            background = Content.Load<Texture2D>(@"interface\titlescreen");
            buttonImage = Content.Load<Texture2D>(@"interface\menu");
            spriteFont = Content.Load<SpriteFont>(@"interface\font");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter) && SelectedIndex == 0)
            {
                buttonMenu.Hide();
                TestMap testMap = new TestMap(game, window);
                Components.Add(testMap);
                testMap.Show();
            }

            base.Update(gameTime);
        }

        public override void Show()
        {
            buttonMenu.Position = new Vector2((Game.Window.ClientBounds.Width -
                                        buttonMenu.Width) / 2, 450);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
