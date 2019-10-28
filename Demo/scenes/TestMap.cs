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
                    collisionWorld.Create(tile.Position.X + 1, tile.Position.Y + 1, 16, 16);

                    // Populate the AI movement grid to avoid obstacles.
                    int x = (int)tile.Position.X - 1;
                    int y = (int)tile.Position.Y - 1;

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
            playerEntity.Position = new Vector2(525,725);
            playerEntity.State = Action.IdleNorth;
            playerEntity.MaxHealth = 150;
            playerEntity.CurrentHealth = 150;
            player.AttackDamage = 4;

            Vector2 enemyStartingPosition = new Vector2(525, 628);
            enemyEntity = new Entity(enemy.militiaAnimation);
            enemyEntity.LoadContent(Content);
            enemyEntity.ID = 0;
            enemyEntity.Position = enemyStartingPosition;
            enemyEntity.State = Action.Idle;
            enemyEntity.MaxHealth = 15;
            enemyEntity.CurrentHealth = 15;
            enemyEntity.AttackDamage = .1;

            Vector2 allyStartingPosition = new Vector2(525, 700);
            allyEntity = new Entity(ally.militiaAnimation);
            allyEntity.LoadContent(Content);
            allyEntity.ID = 0;
            allyEntity.Position = allyStartingPosition;
            allyEntity.State = Action.IdleNorth;
            allyEntity.MaxHealth = 15;
            allyEntity.CurrentHealth = 15;
            allyEntity.AttackDamage = 0.1;


            enemyList.Add(enemyEntity);
            allyList.Add(allyEntity);

            // Create five enemies.
            for (int i = 1; i < 10; i++)
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

            // Assign enemy positions.
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] != enemyList[0])
                {
                    enemyList[i].Position = new Vector2(enemyList[i - 1].Position.X + 25, enemyList[i - 1].Position.Y);
                }
            }

            // Create five allies.
            for (int i = 1; i < 10; i++)
            {
                allyEntity = new Entity(ally.militiaAnimation);
                allyEntity.LoadContent(Content);
                allyEntity.ID = i;
                allyEntity.State = Action.IdleNorth;
                allyEntity.MaxHealth = 15;
                allyEntity.CurrentHealth = 15;
                allyEntity.AttackDamage = 0.1;
                allyList.Add(allyEntity);
            }

            // Assign ally positions.
            for (int i = 0; i < allyList.Count; i++)
            {
                if (allyList[i] != allyList[0])
                {
                    allyList[i].Position = new Vector2(allyList[i - 1].Position.X + 25, allyList[i - 1].Position.Y);
                }
            }


            // Attach player to collision world.
            playerCollision = collisionWorld.Create(0, 0, 16, 16);
            allyCollision = collisionWorld.Create(0, 0, 16, 16);
            enemyCollision = collisionWorld.Create(0, 0, 16, 16);
            player.EnemyList = enemyList;

            font = Content.Load<SpriteFont>(@"interface\font");

   
            base.LoadContent();
        }


        List<Entity> enemyDeaths = new List<Entity>();
        bool gameOver = false;

        public override void Update(GameTime gameTime)
        {

            newState = Keyboard.GetState();

            // Handle collision.
            playerCollision.Move(playerEntity.Position.X, playerEntity.Position.Y, (collision) => CollisionResponses.Slide);
          
            playerEntity.Update(gameTime);

            PathFinder enemyPathFinder = new PathFinder();
            PathFinder allyPathFinder = new PathFinder();

            foreach (Entity enemy in enemyList)
            {
                if (enemy.Dead)
                {
                    if (!enemyDeaths.Contains(enemy))
                    {
                        enemyDeaths.Add(enemy);
                    }
                }

            }

            if (!gameOver)
            {
                enemyPathFinder.FindPathToUnit(grid, enemyList, playerEntity);
                enemyPathFinder.MoveUnits(enemyList, gameTime);
                allyPathFinder.FindPathToUnit(grid, allyList, enemyList[5]);
                allyPathFinder.MoveUnits(allyList, gameTime);
            }
            else
            {
                allyPathFinder.FindPathToUnit(grid, allyList, playerEntity);
                allyPathFinder.MoveUnits(allyList, gameTime);
            }

            if (enemyDeaths.Count == enemyList.Count)
            {
                gameOver = true;
            }

            foreach (Entity enemy in enemyList)
            {
                foreach (Entity ally in allyList)
                {
                    if (ally.State != Action.Dead)
                    {
                       // enemyCollision.Move(enemy.Position.X, enemy.Position.Y, (collision) => CollisionResponses.Slide);
                        enemy.Attack(enemy, ally);
                    }
                }

                enemy.Attack(enemy, playerEntity);
            }

            foreach (Entity ally in allyList)
            { 
                foreach (Entity enemy in enemyList)
                {
                    if (enemy.State != Action.Dead)
                    {
                       // allyCollision.Move(ally.Position.X, ally.Position.Y, (collision) => CollisionResponses.Slide);
                        ally.Attack(ally, enemy);
                    }
                }
            }


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

            Vector2 playerHealthPosition = new Vector2(playerEntity.Position.X - 170, playerEntity.Position.Y - 110);

            map.SortSprites(spriteBatch, playerEntity, enemyList, allyList);
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