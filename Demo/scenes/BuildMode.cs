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
using MonoGame.Extended.Shapes;

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
            camera.LookAt(mousePosition);


            base.LoadContent();
        }


        private TiledMap LoadNextMap()
        {
            var name = maps.Dequeue();
            map = Content.Load<TiledMap>(name);
            maps.Enqueue(name);
            return map;
        }

        Rectangle bounds = new Rectangle(100, 100, 2000, 1000);

        public static float GetHorizontalIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;

            // Calculate centers.
            float centerA = rectA.Left + halfWidthA;
            float centerB = rectB.Left + halfWidthB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA - centerB;
            float minDistanceX = halfWidthA + halfWidthB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX)
                return 0f;

            // Calculate and return intersection depths.
            return distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            camera.Zoom = 3;

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (camera.BoundingRectangle.Intersects(bounds.ToRectangleF()))
                {
                    Console.WriteLine("true");

                    //float x =  GetHorizontalIntersectionDepth(camera.BoundingRectangle.ToRectangle(), bounds);
                    // camera.Position = new Vector2(x, camera.Position.Y);

                }
                else
                {
                    camera.Position = Vector2.Lerp(camera.Position, mousePosition, 0.025f);
                }

            }

            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;
            mapRenderer.Update(gameTime);
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
                    //    spriteBatch.Draw(gridLine, new Vector2(x, y), Color.White);
                    x += map.TileWidth - 1;
                }

                x = 900;

                //   spriteBatch.Draw(gridLine, new Vector2(x, y), Color.White);
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

