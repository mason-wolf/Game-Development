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
using Humper;
using Humper.Responses;

namespace Demo.Scenes
{
    class TestMap : SceneManager
    {

        public static ViewportAdapter viewPortAdapter;
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static Entity playerEntity;
        public static Player player;
        public static Camera2D camera;
        public static Map map;
        public static Vector2 startingPosition = new Vector2(150, 150);

        public static World collisionWorld;
        public static IBox playerCollision;

        public TestMap(Game game, GameWindow window) : base(game)
        {
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            map = new Map();

            map.LoadMap(Content, "Content/maps/testMap.tmx");

            // Generate collision world.
            collisionWorld = new World(map.Width() * 16, map.Height() * 16);

            // Find the tiles in the collision layer and add them to the collision world.
            foreach (Tile tile in map.GetCollisionLayer())
            {
                if (tile.TileID != 0)
                {
                    collisionWorld.Create(tile.Position.X, tile.Position.Y, 16, 16);
                }

            }

            // Create player to manage animations and movement.
            player = new Player();
            player.LoadContent(Content);

            // Create player entity for movement and player states.
            playerEntity = new Entity(player.animation);
            playerEntity.Position = startingPosition;
            playerEntity.State = Action.Idle;

            // Attach player IBox to collision world.
            playerCollision = collisionWorld.Create(0, 0, 16, 16);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            newState = Keyboard.GetState();
            playerCollision.Move(playerEntity.Position.X, playerEntity.Position.Y, (collision) => CollisionResponses.Slide);
            playerEntity.Update(gameTime);

            camera.Zoom = 4;

            camera.LookAt(playerEntity.Position);
            player.HandleInput(gameTime, playerEntity, playerCollision, newState, oldState);
  
            oldState = newState;

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
            map.Draw(spriteBatch);
            playerEntity.Draw(spriteBatch);
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