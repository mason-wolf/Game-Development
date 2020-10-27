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
using Demo.Interface;

namespace Demo.Scenes
{
    class StartArea : SceneManager
    {
        public static ViewportAdapter viewPortAdapter;
        // Monitor keyboard states.
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static Player player;
        public static Camera2D camera;
        // Store the maps.
        public static Map startingAreaMap;
        public static Map level_1Map;
        public static SpriteFont font;
        private Vector2 playerStartingPosition = new Vector2(350, 200);

        public EscapeMenu escapeMenu;
        public Inventory inventory;

        // Create teleporters.
        private Rectangle teleporterToLevel_1;
        private Rectangle teleporterToStartingLevel;

        private GameWindow window;
        public static Scene SelectedScene { get; set; }
        DialogBox dialogBox;

        // Create entities for this map.
        Entity sittingWarriorEntity;
        AnimatedSprite campfire;

        // Store the player's collision state to pass to different scenes.
        IBox playerCollision;

        public StartArea(Game game, GameWindow window) : base(game)
        {
            this.window = window;
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            base.Initialize();
        }

        // Store a list of scenes to switch to.
        public enum Scene
        {
            EscapeMenu,
            Inventory,
            StartingArea,
            Level_1
        }

        protected override void LoadContent()
        {
            startingAreaMap = new Map(Content, "Content/maps/StartingArea.tmx");
            level_1Map = new Map(Content, "Content/maps/level_1.tmx");
            font = Content.Load<SpriteFont>(@"interface\font");
            player = new Player();
            player.LoadContent(Content);

            Sprites sprites = new Sprites();
            sprites.LoadContent(Content);

            sittingWarriorEntity = new Entity(Sprites.sittingWarriorAnimation);
            level_1Map.AddScene(new Level_1());

            player.sprite = new AnimatedSprite(player.playerAnimation);
            player.Position = playerStartingPosition;
            player.State = Action.IdleSouth1;
            player.MaxHealth = 150;
            player.CurrentHealth = 150;
            player.AttackDamage = 2.5;

            sittingWarriorEntity.CurrentHealth = 1;
            sittingWarriorEntity.Position = new Vector2(player.Position.X - 100, player.Position.Y + 65);
            sittingWarriorEntity.State = Action.IdleEast1;
            startingAreaMap.AddCollidable(sittingWarriorEntity.Position.X, sittingWarriorEntity.Position.Y - 16, 16, 31);

            escapeMenu = new EscapeMenu(game, window, Content);
            string[] items = { "Continue", "Save", "Load", "Quit" };
            escapeMenu.SetMenuItems(items);

            inventory = new Inventory(Content);

            dialogBox = new DialogBox(game, font)
            {
                Text = "\n" + 
                       "Oh..are you here to ascend the mount?\n" +
                       "Hmph..you'll be needing some help then.\n" +
                       "I had some friends enter the beast a few days ago.\n" +
                       "Help me find them and we'll be sure to repay you.\n"
            };

            dialogBox.Position = new Vector2(player.Position.X - 175, player.Position.Y + 100);

            campfire = Sprites.campfireSprite;
            campfire.Position = new Vector2(300, 260);
            startingAreaMap.AddCollidable(campfire.Position.X, campfire.Position.Y, 8, 8);

            teleporterToLevel_1 = new Rectangle(340, 134, 8, 1);
            teleporterToStartingLevel = new Rectangle(407, 915, 8, 1);
            SelectedScene = Scene.Level_1;
            playerCollision = startingAreaMap.GetCollisionWorld();
            player.Position = new Vector2(808, 862);

            Item chickenItem = new Item();
            chickenItem.HealthAmount = 10;
            chickenItem.ItemTexture = Sprites.chickenTexture;
            //Player.InventoryList[0] = chickenItem;
            //Player.InventoryList[1] = chickenItem;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            escapeMenu.Position = new Vector2(player.Position.X, player.Position.Y - 125);

            // If player intersects the teleporter, transport to Level 1.
            if (player.BoundingBox.Intersects(teleporterToLevel_1) && SelectedScene != Scene.Level_1)
            {
                FadeInMap(level_1Map);
                SelectedScene = Scene.Level_1;
                player.Position = new Vector2(410, 812);
            }

            if (player.BoundingBox.Intersects(teleporterToStartingLevel))
            {
                FadeInMap(startingAreaMap);
                SelectedScene = Scene.StartingArea;
                player.Position = new Vector2(325, 150);
            }

            newState = Keyboard.GetState();

            HandleDialog();

            switch (SelectedScene)
            {
                case Scene.EscapeMenu:
                    escapeMenu.Update(gameTime);
                    break;
                case Scene.StartingArea:
                    playerCollision = startingAreaMap.GetCollisionWorld();
                    sittingWarriorEntity.Update(gameTime);
                    startingAreaMap.Update(gameTime);
                    break;
                case Scene.Level_1:
                    playerCollision = level_1Map.GetCollisionWorld();
                    level_1Map.Update(gameTime);
                    break;
            }

            dialogBox.Update();
            inventory.Update(gameTime);

            // Handle player's collision.
            playerCollision.Move(player.Position.X, player.Position.Y, (collision) => CollisionResponses.Slide);
            player.Update(gameTime);

            campfire.Update(gameTime);

            camera.Zoom = 3;

            if (!inDialog)
            {
                player.HandleInput(gameTime, player, playerCollision, newState, oldState);
            }
            camera.LookAt(player.Position);

            oldState = newState;
            newState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());

            // Draw the selected screen.
            switch (SelectedScene)
            {
                case Scene.StartingArea:
                    startingAreaMap.Draw(spriteBatch);
                    campfire.Draw(spriteBatch);
                    sittingWarriorEntity.Draw(spriteBatch);
                    break;
                case Scene.Level_1:
                    level_1Map.Draw(spriteBatch);
                    break;
            }

            if (SelectedScene == Scene.EscapeMenu)
            {
                escapeMenu.Draw(spriteBatch);
            }
            else
            {
                Vector2 playerHealthPosition = new Vector2(player.Position.X - 170, player.Position.Y - 110);

                player.Draw(spriteBatch);
                player.DrawHUD(spriteBatch, playerHealthPosition, true);

                int health = (int)player.CurrentHealth;
                Vector2 healthStatus = new Vector2(playerHealthPosition.X + 57, playerHealthPosition.Y);
                spriteBatch.DrawString(font, health.ToString() + " / 150", healthStatus, Color.White);
                dialogBox.Draw(spriteBatch);
                inventory.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        bool inDialog = false;

        public void HandleDialog()
        {
            if (player.BoundingBox.Intersects(sittingWarriorEntity.BoundingBox) && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                dialogBox.Show();
            }

            if (dialogBox.IsActive())
            {
                inDialog = true;
            }
            else
            {
                inDialog = false;
            }
        }
        /// <summary>
        /// Creates a transition effect on the map.
        /// </summary>
        /// <param name="map">Map to fade in.</param>
        public void FadeInMap(Map map)
        {
            // Fade in the screen.
            map.FadeIn();
            // The effect occured once, so set the trigger back to false and reset visibility color.
            if (map.hasFaded)
            {
                map.hasFaded = false;
                map.color = new Color(255, 255, 255, 255);
            }
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