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
using RoyT.AStar;

namespace Demo.Scenes
{
    class TestMap : SceneManager
    {

        public static ViewportAdapter viewPortAdapter;
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static Entity playerEntity;
        public static Entity AIEntity;
        public static Player player;
        public static Camera2D camera;
        public static Map map;
        public static Vector2 startingPosition = new Vector2(120, 170);
        // Stores pathfinding waypoints.
        List<Vector2> AIWayPoints;
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

            var AIMovementGrid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);

            // Find the tiles in the collision layer and add them to the collision world.
            foreach (Tile tile in map.GetCollisionLayer())
            {
                if (tile.TileID != 0)
                {
                    collisionWorld.Create(tile.Position.X, tile.Position.Y, 16, 16);

                    // Populate the AI movement grid to avoid obstacles.
                    int x = (int)tile.Position.X;
                    int y = (int)tile.Position.Y;

                    for (int i = 0; i < 16; ++i)
                    {
                        for (int j = 0; j < 16; ++j)
                        {
                            AIMovementGrid.BlockCell(new Position(x, y));
                            x++;
                        }

                        x = (int)tile.Position.X;

                        AIMovementGrid.BlockCell(new Position(x, y));

                        y++;

                    }
                }
            }


            // Create player to manage animations and movement.
            player = new Player();
            player.LoadContent(Content);

            // Create player entity for movement and player states.
            playerEntity = new Entity(player.animation);
            playerEntity.Position = startingPosition;
            playerEntity.State = Action.Idle;

            AIEntity = new Entity(player.animation);
            AIEntity.Position = new Vector2(200, 250);
            AIEntity.State = Action.Idle;

            // Attach player IBox to collision world.
            playerCollision = collisionWorld.Create(0, 0, 16, 16);

            // Find closest path to the player.
 
            Position[] path = AIMovementGrid.GetPath(new Position((int)AIEntity.Position.X, (int)AIEntity.Position.Y), new Position((int)playerEntity.Position.X, (int)playerEntity.Position.Y));

            AIWayPoints = new List<Vector2>();

            foreach (Position position in path)
            {
                AIWayPoints.Add(new Vector2(position.X, position.Y));
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            newState = Keyboard.GetState();

            playerCollision.Move(playerEntity.Position.X, playerEntity.Position.Y, (collision) => CollisionResponses.Slide);
            playerEntity.Update(gameTime);
            
            if (AIWayPoints.Count > 0)
            {
                AIEntity.MoveTo(gameTime, AIEntity, AIWayPoints, 0.1f);
            }
            

            AIEntity.Update(gameTime);

            camera.Zoom = 3;

            player.HandleInput(gameTime, playerEntity, playerCollision, newState, oldState);
            camera.LookAt(playerEntity.Position);
            oldState = newState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D collision;
            Texture2D path;

            collision = new Texture2D(GraphicsDevice, 1, 1);
            collision.SetData(new Color[] { Color.Blue });

            path = new Texture2D(GraphicsDevice, 1, 1);
            path.SetData(new Color[] { Color.Red });



            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());


            map.Draw(spriteBatch);
            playerEntity.Draw(spriteBatch);
            AIEntity.Draw(spriteBatch);


            //foreach (Tile t in map.GetCollisionLayer())
            //{
            //    if (t.TileID != 0)
            //    {
            //        spriteBatch.Draw(collision, new Rectangle((int)t.Position.X, (int)t.Position.Y, 1, 1), Color.White);

            //        int x = (int)t.Position.X;
            //        int y = (int)t.Position.Y;

            //        for (int i = 0; i < 16; ++i)
            //        {
            //            for (int j = 0; j < 16; ++j)
            //            {
            //                spriteBatch.Draw(collision, new Rectangle((int)x, (int)y, 1, 1), Color.White);
            //                x++;
            //            }

            //            x = (int)t.Position.X;

            //            spriteBatch.Draw(collision, new Rectangle((int)x, (int)y, 1, 1), Color.White);

            //            y++;

            //        }
            //    }
            //}

            //foreach (Vector2 v in vectorList)
            //{
            //    spriteBatch.Draw(path, new Rectangle((int)v.X, (int)v.Y, 1, 1), Color.White);
            //}

 
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