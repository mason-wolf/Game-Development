using Demo.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Scenes
{
    class EscapeMenu : SceneManager
    {
        public Menu escapeMenu;
        SpriteFont spriteFont;
        Texture2D buttonImage;
        GameWindow window;

        public EscapeMenu(Game game, GameWindow window) : base(game)
        {
            LoadContent();
            this.window = window;
            SceneManager.game = game;

            string[] items = { "Exit" };

            escapeMenu = new Menu(
                game, window, spriteFont, buttonImage);

            escapeMenu.SetMenuItems(items);
        }

        public int SelectedIndex
        {
            get { return escapeMenu.SelectedIndex; }
        }

        protected override void LoadContent()
        {
            buttonImage = Content.Load<Texture2D>(@"interface\menu");
            spriteFont = Content.Load<SpriteFont>(@"interface\font");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            escapeMenu.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            escapeMenu.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
