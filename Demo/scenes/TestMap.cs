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
        public static Entity enemyEntity;
        public static Player player;
        public static Enemy enemy;
        public static Camera2D camera;
        public static Map map;
        // Stores pathfinding waypoints.
        public static List<Vector2> AIWayPoints;
        public static List<Entity> enemyList = new List<Entity>();
        public static RoyT.AStar.Grid AIMovementGrid;
        public static World collisionWorld;
        public static IBox playerCollision;
        public static IBox enemyCollision;

        private SpriteFont font;
        
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

            AIMovementGrid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);

            // Find the tiles in the collision layer and add them to the collision world.
            foreach (Tile tile in map.GetCollisionLayer())
            {
                if (tile.TileID != 0)
                {
                    collisionWorld.Create(tile.Position.X + 1, tile.Position.Y + 1, 16, 16);

                    // Populate the AI movement grid to avoid obstacles.
                    int x = (int)tile.Position.X - 1;
                    int y = (int)tile.Position.Y - 1;

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


            // Create player to manage animations and controls.
            player = new Player();
            enemy = new Enemy();
            player.LoadContent(Content);
            enemy.LoadContent(Content);

            Vector2 enemyStartingPosition = new Vector2(525, 628);
            // Create player entity to manage interactions with AI.
            playerEntity = new Entity(player.playerAnimation);
            playerEntity.LoadContent(Content);
            playerEntity.Position = new Vector2(525,660);
            playerEntity.State = Action.Idle;
            playerEntity.MaxHealth = 150;
            playerEntity.CurrentHealth = 150;
            player.AttackDamage = 2;

            enemyEntity = new Entity(enemy.militiaAnimation);
            enemyEntity.LoadContent(Content);
            enemyEntity.Position = new Vector2(525, 628);
            enemyEntity.State = Action.Idle;
            enemyEntity.MaxHealth = 15;
            enemyEntity.CurrentHealth = 15;
            enemyEntity.AttackDamage = .09;

            enemyList.Add(enemyEntity);

            // Create five enemies.
            for (int i = 0; i < 5; i++)
            {
                Entity e = new Entity(enemy.militiaAnimation);
                e.LoadContent(Content);
                e.State = Action.Idle;
                e.MaxHealth = 15;
                e.CurrentHealth = 15;
                e.AttackDamage = 0.9;
                enemyList.Add(e);
            }

            // Assign enemy positions.
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] != enemyList[0])
                {
                    enemyList[i].Position = new Vector2(enemyList[i - 1].Position.X + 25, enemyList[i - 1].Position.Y);
                }
            }


            // Attach player to collision world.
            playerCollision = collisionWorld.Create(0, 0, 16, 16);
            enemyCollision = collisionWorld.Create(0, 0, 16, 16);
            player.EnemyList = enemyList;

            font = Content.Load<SpriteFont>(@"interface\font");

            base.LoadContent();
        }

        Position[] path;

        public override void Update(GameTime gameTime)
        {
           // Find AI's closest path to the player.

            var movementPattern = new[] { new Offset(-1, 0), new Offset(0, -1), new Offset(1, 0), new Offset(0, 1) };
            path = AIMovementGrid.GetPath(new Position((int)enemyEntity.Position.X, (int)enemyEntity.Position.Y), new Position((int)playerEntity.Position.X, (int)playerEntity.Position.Y), movementPattern);

            // Add way points for AI.
            AIWayPoints = new List<Vector2>();

            foreach (Position position in path)
            {
                AIWayPoints.Add(new Vector2(position.X, position.Y));
            }

            newState = Keyboard.GetState();

            // Handle collision.
            playerCollision.Move(playerEntity.Position.X, playerEntity.Position.Y, (collision) => CollisionResponses.Slide);
          //  enemyCollision.Move(enemyEntity.Position.X, enemyEntity.Position.Y, (collision) => CollisionResponses.Slide);

            playerEntity.Update(gameTime);

            // AI to follow player.
            if (AIWayPoints.Count > 15 && enemyEntity.CurrentHealth > 0)
            {
             //        enemyEntity.MoveTo(gameTime, enemyEntity, AIWayPoints, .05f);
            }
            else if (enemyEntity.CurrentHealth <= 0)
            {
                enemyEntity.State = Action.Die;
            }

            enemy.Attack(enemyEntity, playerEntity);

            enemyEntity.Update(gameTime);

            camera.Zoom = 3;

            player.HandleInput(gameTime, playerEntity, playerCollision, newState, oldState);
            camera.LookAt(playerEntity.Position);
            oldState = newState;

            base.Update(gameTime);
        }

        public void SortSprites(SpriteBatch spriteBatch, Entity playerEntity, List<Entity> enemyList)
        {
            foreach (Entity e in enemyList)
            {
                Vector2 AIHealthPosition = new Vector2(e.Position.X - 8, e.Position.Y - 20);
                e.DrawHUD(spriteBatch, AIHealthPosition, false);

                Vector2 destination = playerEntity.Position - e.Position;
                destination.Normalize();
                Double angle = Math.Atan2(destination.X, destination.Y);
                double direction = Math.Ceiling(angle);


                if (direction == -3 || direction == 4 || direction == -2)
                {
                    playerEntity.Draw(spriteBatch);
                    e.Draw(spriteBatch); ;
                }
                else if (direction == 0 || direction == 1)
                {
                    e.Draw(spriteBatch);
                    playerEntity.Draw(spriteBatch);
                }
                else if (e.CurrentHealth <= 0)
                {
                    e.Draw(spriteBatch);
                    playerEntity.Draw(spriteBatch);
                }
                else
                {
                    playerEntity.Draw(spriteBatch);
                    e.Draw(spriteBatch);
                }
            }
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

            Vector2 playerHealthPosition = new Vector2(playerEntity.Position.X - 170, playerEntity.Position.Y - 110);

            SortSprites(spriteBatch, playerEntity, enemyList);
            playerEntity.DrawHUD(spriteBatch, playerHealthPosition, true);

            int health = (int)playerEntity.CurrentHealth;
            Vector2 healthStatus = new Vector2(playerHealthPosition.X + 57, playerHealthPosition.Y);
            spriteBatch.DrawString(font, health.ToString() + " / 150", healthStatus, Color.White);
       
            //foreach (Tile t in map.GetCollisionLayer())
            //{
            //    if (t.TileID != 0)
            //    {

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

            //foreach (Vector2 v in AIWayPoints)
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