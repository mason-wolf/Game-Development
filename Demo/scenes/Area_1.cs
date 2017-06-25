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
    class Area_1 : SceneManager
    {
        private TiledMap map;
        public static ViewportAdapter viewPortAdapter;
        private FullMapRenderer mapRenderer;
        private Queue<string> maps;

        public static Entity player;
        public static Player playerData;

        Camera2D camera;

        CollisionWorld collision;

        public static KeyboardState oldState;
        public static KeyboardState newState;

        Entity water;

        public Area_1(Game game, GameWindow window) : base(game)
        {
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            mapRenderer = new FullMapRenderer(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            maps = new Queue<string>(new[] { @"maps\area_1" });
            map = LoadNextMap();
            mapRenderer.SwapMap(map);
            playerData = new Player();
            playerData.LoadContent(Content);
            player = new Entity(playerData.CombatAnimations);
            player.Position = new Vector2(1050, 500);
            player.State = Action.IdleWest;
            collision = new CollisionWorld(new Vector2(0));
            collision.CreateGrid(map.GetLayer<TiledTileLayer>("Collision"));
            collision.CreateActor(player);

            var waterTexture = Content.Load<Texture2D>(@"objects\water");
            var waterAtlas = TextureAtlas.Create(waterTexture, 16, 16);
            var waterAnimation = new SpriteSheetAnimationFactory(waterAtlas);
            waterAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, (float)(.2), isLooping: true));
            waterAnimation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2}, (float)(.2), isLooping: true));

            water = new Entity(waterAnimation);
            water.Position = new Vector2(900, 500);
            water.State = Action.WalkSouth;

            base.LoadContent();
        }


        private TiledMap LoadNextMap()
        {
            var name = maps.Dequeue();
            map = Content.Load<TiledMap>(name);
            maps.Enqueue(name);
            return map;
        }

        public override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();
            player.Update(gameTime);
            collision.Update(gameTime);       
            camera.Zoom = 4;
            camera.LookAt(player.Position);
            Player controls = new Player();
            controls.HandleInput(gameTime, player, false, newState, oldState);
            oldState = newState;
            mapRenderer.Update(gameTime);
            water.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
            mapRenderer.Draw(camera.GetViewMatrix());
            player.Draw(spriteBatch);
            playerData.DrawHUD(spriteBatch, camera.Position);
            water.Draw(spriteBatch);
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
