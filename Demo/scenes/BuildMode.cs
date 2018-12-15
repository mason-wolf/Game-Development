using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Tiled.Renderers;
using Demo.Scenes;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Collisions;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Demo.Scenes
{
    class BuildMode : SceneManager
    {
        private TiledMap map;
        public static ViewportAdapter viewPortAdapter;
        private Queue<string> maps;


        Camera2D camera;


        Texture2D gridLine;
        MouseState mouseState;
        Vector2 mousePosition = new Vector2(1050, 500);

        public BuildMode(Game game, GameWindow window) : base(game)
        {
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            mapRenderer = new FullMapRenderer(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            maps = new Queue<string>(new[] { @"maps\build_mode" });
            map = LoadNextMap();
            mapRenderer.SwapMap(map);
            gridLine = Content.Load<Texture2D>(@"tilesets\gridbox");
            Game.IsMouseVisible = true;
            Mouse.SetPosition((int)mousePosition.X, (int)mousePosition.Y);
        
            base.LoadContent();
        }


        private TiledMap LoadNextMap()
        {
            var name = maps.Dequeue();
            map = Content.Load<TiledMap>(name);
            maps.Enqueue(name);
            return map;
        }

        public List<Tuple<TimeSpan, Vector2>> Points { get; set; }

        public Vector2 CurrentPoint { get; set; }
        
     
        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
        
         
            //camera.Zoom = 3;

            //camera.LookAt(mousePosition);


            //if (mouseState.Position.X != mousePosition.X)
            //{
            //    Points.Add(new Tuple<TimeSpan, Vector2>(TimeSpan.FromSeconds(1), mousePosition));
            //    foreach (var point in Points)
            //    {
            //        point -= gameTime.ElapsedTimeSpan;

            //        if (point.Item1 < TimeSpan.Zero) point
            //             CurrentPoint = point.Item2;
            //    }
            //}

            //mousePosition.X = mouseState.X;
            //mousePosition.Y = mouseState.Y;

            //mapRenderer.Update(gameTime);


           

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
            mapRenderer.Draw(camera.GetViewMatrix());


            int x = 900;
            int y = 400;

            for (int i = 0; i < 15; ++i)
            {
                for (int j = 0; j < 15; ++j)
                {
                    spriteBatch.Draw(gridLine, new Vector2(x, y), Color.White);
                    x += map.TileWidth - 1;
                }

                x = 900;
 
                spriteBatch.Draw(gridLine, new Vector2(x, y), Color.White);
                y += map.TileWidth - 1;
                
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Show()
        {
            base.Show();
            Enabled = true;
            Visible = true;
        }

        public override void Hide()
        {
            base.Hide();
            Enabled = false;
            Visible = false;
        }
    }
}
