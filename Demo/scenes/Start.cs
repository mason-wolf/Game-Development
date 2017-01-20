using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Demo.Scenes;
using Demo.Interface;

namespace Demo
{
    class Start : SceneManager
    {
        Menu buttonMenu;
        SpriteFont spriteFont;
        Texture2D background;
        Texture2D buttonImage;
        
        public Start(Game game)
            : base(game)
        {
            LoadContent();
            Components.Add(new Background(game, background, true));

            string[] items = { "start" };

            buttonMenu = new Menu(
                game,
                spriteFont,
                buttonImage);

            buttonMenu.SetMenuItems(items);
            Components.Add(buttonMenu);
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
