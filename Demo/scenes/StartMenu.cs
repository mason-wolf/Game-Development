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
        public MainMenu buttonMenu;
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

            buttonMenu = new MainMenu(
                game,
                window,
                spriteFont,
                buttonImage, background);

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

        bool gameStart = false;

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (!gameStart)
            {
                if (keyboardState.IsKeyDown(Keys.E) && SelectedIndex == 0)
                {
                    gameStart = true;
                    buttonMenu.Hide();
                    this.UnloadContent();
                    StartArea startingArea = new StartArea(game, window);
                    Components.Add(startingArea);
                    startingArea.Show();
                }
            }

            base.Update(gameTime);
        }

        public override void Show()
        {
            buttonMenu.Position = new Vector2((Game.Window.ClientBounds.Width - buttonMenu.Width) / 2, 450);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
