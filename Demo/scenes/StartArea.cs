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
    class StartArea : SceneManager
    {
        public static ViewportAdapter viewPortAdapter;
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static Player player;
        public static Camera2D camera;
        public static Map startingArea;
        public static Map level_1;
        private SpriteFont font;
        Vector2 playerStartingPosition = new Vector2(350, 200);

        public Texture2D campfireTexture;
        public TextureAtlas campfireAtlas;
        public SpriteSheetAnimationFactory campfireAnimation;
        public AnimatedSprite campfire;

        Rectangle teleporter;
        Rectangle level_1_teleporter;
        GameWindow window;

        public StartArea(Game game, GameWindow window) : base(game)
        {
            this.window = window;
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            base.Initialize();
        }

        public enum Level
        {
            StartingArea,
            Level_1
        }

        public Level SelectedLevel { get; set; }

        protected override void LoadContent()
        {
            startingArea = new Map(Content, "Content/maps/StartingArea.tmx");
            level_1 = new Map(Content, "Content/maps/level_1.tmx");

            player = new Player();
            player.LoadContent(Content);

            level_1.AddScene(new Level_1());

            player.sprite = new AnimatedSprite(player.playerAnimation);
            player.Position = playerStartingPosition;
            player.State = Action.Idle;
            player.MaxHealth = 150;
            player.CurrentHealth = 150;
            player.AttackDamage = 2;


            font = Content.Load<SpriteFont>(@"interface\font");

            campfireTexture = Content.Load<Texture2D>(@"objects\campfire");
            campfireAtlas = TextureAtlas.Create(campfireTexture, 16, 32);
            campfireAnimation = new SpriteSheetAnimationFactory(campfireAtlas);
            campfireAnimation.Add("burning", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, .09f, isLooping: true));
            campfire = new AnimatedSprite(campfireAnimation);
            campfire.Play("burning");
            campfire.Position = new Vector2(300, 260);
            //     startingAreaCollisionWorld.Create(campfire.Position.X, campfire.Position.Y- 1, 4, 4);
            //      startingAreaCollisionWorld.Create(campfire.Position.X, campfire.Position.Y, 16, 16);

            teleporter = new Rectangle(340, 134, 8, 1);
            level_1_teleporter = new Rectangle(407, 915, 8, 1);
            SelectedLevel = Level.StartingArea;
          //  player.Position = new Vector2(407, 875);
            playerCollision = startingArea.GetCollisionWorld();
            base.LoadContent();
        }

        IBox playerCollision;

        public override void Update(GameTime gameTime)
        {
            //    Console.WriteLine(player.Position);

            if (player.BoundingBox.Intersects(teleporter))
            {
                level_1.FadeIn();

                if (level_1.hasFaded)
                {
                    level_1.hasFaded = false;
                    level_1.color = new Color(255, 255, 255, 255);
                }

                SelectedLevel = Level.Level_1;
                player.Position = new Vector2(407, 875);
            }

            if (player.BoundingBox.Intersects(level_1_teleporter))
            {
                startingArea.FadeIn();

                if (startingArea.hasFaded)
                {
                    startingArea.hasFaded = false;
                    startingArea.color = new Color(255, 255, 255, 255);
                }

                SelectedLevel = Level.StartingArea;
                player.Position = new Vector2(325, 150);
            }

            newState = Keyboard.GetState();

            switch (SelectedLevel)
            {
                case Level.StartingArea:
                    playerCollision = startingArea.GetCollisionWorld();
                    startingArea.Update(gameTime);
                    break;
                case Level.Level_1:
                    playerCollision = level_1.GetCollisionWorld();
                    level_1.Update(gameTime);
                    break;
            }

            // Handle collision.
            playerCollision.Move(player.Position.X, player.Position.Y, (collision) => CollisionResponses.Slide);
            player.Update(gameTime);
            campfire.Update(gameTime);

            camera.Zoom = 3;
            player.HandleInput(gameTime, player, playerCollision, newState, oldState);
            camera.LookAt(player.Position);

            oldState = newState;
            newState = Keyboard.GetState();

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());

            switch (SelectedLevel)
            {
                case Level.StartingArea:
                    startingArea.Draw(spriteBatch);
                    campfire.Draw(spriteBatch);
                    break;
                case Level.Level_1:
                    level_1.Draw(spriteBatch);
                    break;
            }

            Vector2 playerHealthPosition = new Vector2(player.Position.X - 170, player.Position.Y - 110);


            player.Draw(spriteBatch);
            player.DrawHUD(spriteBatch, playerHealthPosition, true);
            int health = (int)player.CurrentHealth;
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