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
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Content;

namespace Demo.Scenes
{
    class StartingArea : SceneManager
    {

        public static ViewportAdapter viewPortAdapter;
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static Entity playerEntity;
        public static Player player;
        public static Ally ally;
        public static Enemy enemy;
        public static EnemyAI enemyAI;
        public static Camera2D camera;
        public static Map map;
        public static List<Entity> enemyList = new List<Entity>();
        public static List<Entity> allyList = new List<Entity>();
        public static RoyT.AStar.Grid grid;
        public static World collisionWorld;
        public static IBox playerCollision;
        public static IBox allyCollision;
        public static IBox enemyCollision;

        private SpriteFont font;
        Vector2 playerStartingPosition = new Vector2(350, 220);

        public Texture2D campfireTexture;
        public TextureAtlas campfireAtlas;
        public SpriteSheetAnimationFactory campfireAnimation;
        public AnimatedSprite campfire;
        Rectangle teleporter;
        GameWindow window;

        public StartingArea(Game game, GameWindow window) : base(game)
        {
            this.window = window;
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            map = new Map();

            map.LoadMap(Content, "Content/maps/StartingArea.tmx");

            // Generate collision world.
            collisionWorld = new World(map.Width() * 16, map.Height() * 16);

            // Create path finding grid.
            grid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);

            // Find the tiles in the collision layer and add them to the collision world.
            foreach (Tile tile in map.GetCollisionLayer())
            {
                if (tile.TileID != 0)
                {
                    collisionWorld.Create(tile.Position.X + 8, tile.Position.Y + 8, 16, 16);
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
            for (int i = 0; i < 6; i++)
            {
                Entity enemyEntity = new Entity(enemy.Animation);
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

            enemyList[0].Position = new Vector2(789, 663);
            enemyList[1].Position = new Vector2(789, 376);
            enemyList[2].Position = new Vector2(581, 459);
            enemyList[3].Position = new Vector2(800, 663);
            enemyList[4].Position = new Vector2(825, 376);
            enemyList[5].Position = new Vector2(850, 459);

            // Attach entities to collision world.
            playerCollision = collisionWorld.Create(0, 0, 16, 16);
            allyCollision = collisionWorld.Create(0, 0, 16, 16);
            enemyCollision = collisionWorld.Create(0, 0, 16, 16);
            player.EnemyList = enemyList;

            font = Content.Load<SpriteFont>(@"interface\font");

            enemyAI = new EnemyAI(grid, enemyList, playerEntity);

            campfireTexture = Content.Load<Texture2D>(@"objects\campfire");
            campfireAtlas = TextureAtlas.Create(campfireTexture, 16, 32);
            campfireAnimation = new SpriteSheetAnimationFactory(campfireAtlas);
            campfireAnimation.Add("burning", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, .09f, isLooping: true));
            campfire = new AnimatedSprite(campfireAnimation);
            campfire.Play("burning");
            campfire.Position = new Vector2(300, 260);
            collisionWorld.Create(campfire.Position.X, campfire.Position.Y- 1, 4, 4);
            collisionWorld.Create(campfire.Position.X, campfire.Position.Y, 16, 16);

            teleporter = new Rectangle(340, 134, 16, 13);

            base.LoadContent();
        }

        bool nextLevel = false;

        public override void Update(GameTime gameTime)
        {

            if (playerEntity.BoundingBox.Intersects(teleporter) && nextLevel == false)
            {
                Content.Unload();
                nextLevel = true;
                Level_1 level_1 = new Level_1(game, window);
                Components.Add(level_1);
                level_1.Show();
            }

            if (nextLevel == false)
            {
                newState = Keyboard.GetState();

                // Handle collision.
                playerCollision.Move(playerEntity.Position.X, playerEntity.Position.Y, (collision) => CollisionResponses.Slide);

                playerEntity.Update(gameTime);
                enemyAI.Update(gameTime);
                campfire.Update(gameTime);
                camera.Zoom = 3;

                player.HandleInput(gameTime, playerEntity, playerCollision, newState, oldState);
                camera.LookAt(playerEntity.Position);
                oldState = newState;
                newState = Keyboard.GetState();

            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (nextLevel == false)
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());

                map.Draw(spriteBatch);

                spriteBatch.Draw(campfire);

                Vector2 playerHealthPosition = new Vector2(playerEntity.Position.X - 170, playerEntity.Position.Y - 110);

                map.SortSprites(spriteBatch, playerEntity, enemyList);
                playerEntity.DrawHUD(spriteBatch, playerHealthPosition, true);

                int health = (int)playerEntity.CurrentHealth;
                Vector2 healthStatus = new Vector2(playerHealthPosition.X + 57, playerHealthPosition.Y);
                spriteBatch.DrawString(font, health.ToString() + " / 150", healthStatus, Color.White);

                spriteBatch.End();
            }

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