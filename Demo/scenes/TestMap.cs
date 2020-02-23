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
        public static Entity allyEntity;
        public static Entity enemyEntity;
        public static Player player;
        public static Ally ally;
        public static Enemy enemy;
        public static Camera2D camera;
        public static Map map;
        public static List<Vector2> wayPoints;
        public static List<Entity> enemyList = new List<Entity>();
        public static List<Entity> allyList = new List<Entity>();
        public static RoyT.AStar.Grid grid;
        public static World collisionWorld;
        public static IBox playerCollision;
        public static IBox allyCollision;
        public static IBox enemyCollision;

        private SpriteFont font;
        // y: 490, 820
        Vector2 playerStartingPosition = new Vector2(971, 590);

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

            grid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);

            // Find the tiles in the collision layer and add them to the collision world.
            foreach (Tile tile in map.GetCollisionLayer())
            {
                if (tile.TileID != 0)
                {
                    collisionWorld.Create(tile.Position.X + 5, tile.Position.Y + 5, 16, 16);

                    int x = (int)tile.Position.X;
                    int y = (int)tile.Position.Y;

                    for (int i = 0; i < 16; ++i)
                    {
                        for (int j = 0; j < 16; ++j)
                        {
                            grid.BlockCell(new Position(x, y));
                            x++;
                        }

                        x = (int)tile.Position.X;

                        grid.BlockCell(new Position(x, y));

                        y++;

                    }
                }
            }


            // Create player to manage animations and controls.
            player = new Player();
            ally = new Ally();
            enemy = new Enemy();
            player.LoadContent(Content);
            ally.LoadContent(Content);
            enemy.LoadContent(Content);

            // Create player entity to manage interactions with AI.
            playerEntity = new Entity(player.playerAnimation);
            playerEntity.LoadContent(Content);
            playerEntity.Position = playerStartingPosition;
            playerEntity.State = Action.IdleNorth;
            playerEntity.MaxHealth = 150;
            playerEntity.CurrentHealth = 150;
            player.AttackDamage = 2;

            // Create enemies
            for (int i = 0;  i < 1; i++)
            {
                Entity enemyEntity = new Entity(enemy.militiaAnimation);
                enemyEntity.LoadContent(Content);
                enemyEntity.ID = i;
                enemyEntity.State = Action.Idle;
                enemyEntity.MaxHealth = 15;
                enemyEntity.CurrentHealth = 15;
                enemyEntity.AttackDamage = 0.1;
                enemyList.Add(enemyEntity);
            }

            // Create Allies
            for (int i = 0; i < 2; i++)
            {
                Entity npc = new Entity(ally.militiaAnimation);
                npc.LoadContent(Content);
                npc.State = Action.Idle;
                npc.MaxHealth = 15;
                npc.CurrentHealth = 15;
                npc.AttackDamage = 0.1;
                allyList.Add(npc);
            }

            enemyList[0].Position = new Vector2(778, 590);
  


            allyList[0].Position = new Vector2(456, 338);
            allyList[1].Position = new Vector2(505, 338);
            // Attach player to collision world.
            playerCollision = collisionWorld.Create(0, 0, 16, 16);
            allyCollision = collisionWorld.Create(0, 0, 16, 16);
            enemyCollision = collisionWorld.Create(0, 0, 16, 16);
            player.EnemyList = enemyList;

            font = Content.Load<SpriteFont>(@"interface\font");

   
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {

            Console.WriteLine(playerEntity.Position);
            newState = Keyboard.GetState();

            // Handle collision.
            playerCollision.Move(playerEntity.Position.X, playerEntity.Position.Y, (collision) => CollisionResponses.Slide);
            playerEntity.Update(gameTime);


            foreach (Entity enemy in enemyList)
            {
                float enemyDistance = Vector2.Distance(playerEntity.Position, enemy.Position);

                if (enemyDistance < 200 && enemy.State != Action.Dead)
                {
                    PathFinder enemyPathFinder = new PathFinder(grid);
                    enemyPathFinder.FindPathToUnit(enemyList, playerEntity);
                    enemyPathFinder.MoveUnits(enemyList, gameTime);
                    enemy.Attack(enemy, playerEntity);
                }

                enemy.Update(gameTime);
            }

            foreach (Entity ally in allyList)
            {

                foreach (Entity enemy in enemyList)
                {
                    float enemyDistance = Vector2.Distance(ally.Position, enemy.Position);

                    if (enemyDistance < 200 && enemy.State != Action.Dead && ally.State != Action.Dead)
                    {
                        PathFinder allyPathFinder = new PathFinder(grid);
                        allyPathFinder.FindPathToUnit(allyList, enemy);
                        allyPathFinder.MoveUnits(allyList, gameTime);
                        ally.Attack(ally, enemy);
                    }

                }

            //    allyCollision.Move(ally.Position.X, ally.Position.Y, (collision) => CollisionResponses.Slide);
                ally.Update(gameTime);
            }

            camera.Zoom = 3;

            player.HandleInput(gameTime, playerEntity, playerCollision, newState, oldState);
            camera.LookAt(playerEntity.Position);
            oldState = newState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());

            map.Draw(spriteBatch);

            Vector2 playerHealthPosition = new Vector2(playerEntity.Position.X - 170, playerEntity.Position.Y - 110);

            map.SortSprites(spriteBatch, playerEntity, enemyList, allyList);
            playerEntity.DrawHUD(spriteBatch, playerHealthPosition, true);

            int health = (int)playerEntity.CurrentHealth;
            Vector2 healthStatus = new Vector2(playerHealthPosition.X + 57, playerHealthPosition.Y);
            spriteBatch.DrawString(font, health.ToString() + " / 150", healthStatus, Color.White);


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