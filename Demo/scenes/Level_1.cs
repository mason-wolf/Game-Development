
using Demo.Engine;
using Demo;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Microsoft.Xna.Framework.Input;
using Demo.Interface;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Demo.Scenes
{
    public class Level_1 : SceneLogic
    {
        Song levelThemeSong;
        public Level_1()
        {

        }

        public override List<MapObject> MapObjects { get; set; }
        public override ContentManager ContentManager { get; set;  }
        public override Map Map { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
        //    throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content)
        {
            levelThemeSong = content.Load<Song>(@"music\level_1");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(levelThemeSong);
        }

        public override void Update(GameTime gameTime)
        {
     //       throw new NotImplementedException();
        }
    }
}
