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
using Demo.Engine;

namespace Demo.Scenes
{
    class Area_1 : SceneManager
    {
        //  private TiledMap map;
        Map map;
        public static ViewportAdapter viewPortAdapter;
        private Queue<string> maps;

        public static Entity player;
        public static Player playerData;

        Camera2D camera;

        CollisionWorld collision;

        public static KeyboardState oldState;
        public static KeyboardState newState;
        Texture2D gridLine;
        MouseState mouseState;
        Vector2 startingPosition = new Vector2(1050, 500);
        Vector2 viewportPosition;

        public Area_1(Game game, GameWindow window) : base(game)
        {
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            mapRenderer = new FullMapRenderer(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            map = new Map();
            map.LoadMap("Content/maps/area_1.tmx");
       //     map = Map.Load("Content/maps/area_1.tmx", Content);
            maps = new Queue<string>(new[] { @"maps\area_1" });
          //  map = LoadNextMap();
         //   mapRenderer.SwapMap(map);
            playerData = new Player();
            playerData.LoadContent(Content);
            player = new Entity(playerData.CombatAnimations);
            player.Position = new Vector2(1050, 500);
            player.State = Action.IdleWest;
            collision = new CollisionWorld(new Vector2(0));
          //  collision.CreateGrid(map.GetLayer<TiledTileLayer>("Collision"));
            collision.CreateActor(player);
            Mouse.SetPosition((int)player.Position.X, (int)player.Position.Y);
         //   gridLine = new Texture2D(spriteBatch.GraphicsDevice, map.TileWidth, map.TileHeight);
         gridLine = Content.Load<Texture2D>(@"tilesets\gridbox");
            //   gridLine.SetData<Color>(new Color[] { Color.White });
            Game.IsMouseVisible = true;
            base.LoadContent();
        }


        //private TiledMap LoadNextMap()
        //{
        //    var name = maps.Dequeue();
        //  //  map = Content.Load<TiledMap>(name);
        //    maps.Enqueue(name);
        //    return map;
        //}

        public override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();
            player.Update(gameTime);
            collision.Update(gameTime);       
            camera.Zoom = 4;
            camera.LookAt(new Vector2(1000, 575));
            Player controls = new Player();
            controls.HandleInput(gameTime, player, false, newState, oldState);
            oldState = newState;
            mapRenderer.Update(gameTime);
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
      //      map.Draw(spriteBatch, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), viewportPosition);
            player.Draw(spriteBatch);
          //  CreateBorder(gridLine, .01f, Color.White);






            //   playerData.DrawHUD(spriteBatch, camera.Position);
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
