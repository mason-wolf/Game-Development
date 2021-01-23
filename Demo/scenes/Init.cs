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
    class Init : SceneManager
    {
        public static ViewportAdapter viewPortAdapter;
        // Monitor keyboard states.
        public static KeyboardState KeyBoardOldState;
        public static KeyboardState KeyBoardNewState;
        public static Player Player;
        public static Camera2D Camera;
        // Store the maps.
        public static Map StartingAreaMap;
        public static Map Level_1Map;
        public static SpriteFont Font;
        private Vector2 playerStartingPosition = new Vector2(408, 894);

        public EscapeMenu escapeMenu;
        public static SaveMenu saveMenu;
        public static LoadMenu loadMenu;

        public Inventory inventory;

        // Create teleporters.
        private List<Teleporter> teleporterList = new List<Teleporter>();
        private Teleporter teleporterToLevel_1;
        private Teleporter teleporterToStartingLevel;

        private GameWindow window;
        public static Scene SelectedScene { get; set; }
        public static Map SelectedMap { get; set; }
        public static Vector2 SavedGamePosition;
        public static string SavedGameLocation;
        DialogBox dialogBox;

        // Create entities for this map.
        Entity sittingWarriorEntity;
        AnimatedSprite campfire;

        // Store the player's collision state to pass to different scenes.
        IBox playerCollision;

        public static bool Reloaded = false;
        public Init(Game game, GameWindow window) : base(game)
        {
            this.window = window;
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            Camera = new Camera2D(viewPortAdapter);
            Player = new Player();
            Player.LoadContent(Content);
            Player.sprite = new AnimatedSprite(Player.playerAnimation);
            Player.State = Action.IdleSouth1;
            Player.MaxHealth = 100;
            Player.CurrentHealth = 100;
            Player.AttackDamage = 3.5;
            Player.Position = playerStartingPosition;
            base.Initialize();
        }

        // Store a list of scenes to switch to.
        public enum Scene
        {
            EscapeMenu,
            SaveMenu,
            LoadMenu,
            Inventory,
            StartingArea,
            Level_1
        }

        protected override void LoadContent()
        {
            StartingAreaMap = new Map(Content, "Content/maps/StartingArea.tmx");
            Level_1Map = new Map(Content, "Content/maps/level_1.tmx");
            Font = Content.Load<SpriteFont>(@"interface\font");
            Player.LoadContent(Content);
            Player.sprite = new AnimatedSprite(Player.playerAnimation);
            Player.State = Action.IdleSouth1;
            Player.Position = playerStartingPosition;
            Sprites sprites = new Sprites();
            sprites.LoadContent(Content);

            sittingWarriorEntity = new Entity(Sprites.sittingWarriorAnimation);
            Level_1Map.LoadScene(new Level_1());

            sittingWarriorEntity.CurrentHealth = 1;
            sittingWarriorEntity.Position = new Vector2(Player.Position.X - 100, Player.Position.Y + 65);
            sittingWarriorEntity.State = Action.IdleEast1;
            StartingAreaMap.AddCollidable(sittingWarriorEntity.Position.X, sittingWarriorEntity.Position.Y - 16, 16, 31);

            escapeMenu = new EscapeMenu(game, window, Content);
            saveMenu = new SaveMenu(game, window, Content);
            loadMenu = new LoadMenu(game, window, Content);

            string[] items = { "Continue", "Save", "Load", "Quit" };
            escapeMenu.SetMenuItems(items);

            inventory = new Inventory(Content);

            dialogBox = new DialogBox(game, Font)
            {
                Text = "\n" + 
                       "Oh..are you here to ascend the mount?\n" +
                       "Hmph..you'll be needing some help then.\n" +
                       "I had some friends enter the beast a few days ago.\n" +
                       "Help me find them and we'll be sure to repay you.\n"
            };

            dialogBox.Position = new Vector2(Player.Position.X - 175, Player.Position.Y + 100);

            campfire = Sprites.campfireSprite;
            campfire.Position = new Vector2(300, 260);
            StartingAreaMap.AddCollidable(campfire.Position.X, campfire.Position.Y, 8, 8);

            teleporterToLevel_1 = new Teleporter(new Rectangle(340, 115, 8, 1), Scene.StartingArea.ToString());
            teleporterToStartingLevel = new Teleporter(new Rectangle(407, 915, 8, 1), Scene.Level_1.ToString());
            teleporterList.Add(teleporterToLevel_1);
            teleporterList.Add(teleporterToStartingLevel);
            SelectedScene = Scene.Level_1;
            playerCollision = StartingAreaMap.GetCollisionWorld();
            Item chickenItem = new Item();
            chickenItem.HealthAmount = 10;
            chickenItem.ItemTexture = Sprites.chickenTexture;
            //Player.InventoryList[0] = chickenItem;
            //Player.InventoryList[1] = chickenItem;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            escapeMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);
            saveMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);
            loadMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);

            // If save was loaded, create transition effects, assign the player's saved scene and position.
            if (Reloaded)
            {
                Player.EnemyList.Clear();
                Content.Unload();
                LoadContent();
                SelectedScene = (Init.Scene)Enum.Parse(typeof(Init.Scene), SavedGameLocation);

                switch (SelectedScene)
                {
                    case (Scene.StartingArea):
                        FadeInMap(StartingAreaMap);
                        break;
                    case (Scene.Level_1):
                        FadeInMap(Level_1Map);
                        break;
                }
                Player.Position = SavedGamePosition;
                Reloaded = false;
            }

            // To Level 1
            if (Player.BoundingBox.Intersects(teleporterToLevel_1.GetRectangle()) && teleporterToLevel_1.Enabled)
            {
                FadeInMap(Level_1Map);
                Player.Position = new Vector2(422, 828);
                SelectedScene = Scene.Level_1;
            }

            // To Starting level
            if (Player.BoundingBox.Intersects(teleporterToStartingLevel.GetRectangle()) && teleporterToStartingLevel.Enabled)
            {
                FadeInMap(StartingAreaMap);
                Player.EnemyList.Clear();
                Content.Unload();
                LoadContent();
                SelectedScene = Scene.StartingArea;
                Player.Position = new Vector2(335, 177);
            }

            KeyBoardNewState = Keyboard.GetState();

            HandleDialog();

            switch (SelectedScene)
            {
                case Scene.EscapeMenu:
                    escapeMenu.Update(gameTime);
                    break;
                case Scene.SaveMenu:
                    saveMenu.Update(gameTime);
                    break;
                case Scene.LoadMenu:
                    loadMenu.Update(gameTime);
                    break;
                case Scene.StartingArea:
                    playerCollision = StartingAreaMap.GetCollisionWorld();
                    sittingWarriorEntity.Update(gameTime);
                    StartingAreaMap.Update(gameTime);
                    SelectedMap = StartingAreaMap;
                    break;
                case Scene.Level_1:
                    playerCollision = Level_1Map.GetCollisionWorld();
                    Level_1Map.Update(gameTime);
                    SelectedMap = Level_1Map;
                    break;
            }

            ToggleTeleporters(SelectedScene);
            dialogBox.Update();
            inventory.Update(gameTime);

            // Handle player's collision.
            playerCollision.Move(Player.Position.X, Player.Position.Y, (collision) => CollisionResponses.Slide);
            Player.Update(gameTime);

            campfire.Update(gameTime);

            Camera.Zoom = 3;

            if (!inDialog)
            {
                Player.HandleInput(gameTime, Player, playerCollision, KeyBoardNewState, KeyBoardOldState);
            }
            Camera.LookAt(Player.Position);

            KeyBoardOldState = KeyBoardNewState;
            KeyBoardNewState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());

            // Draw the selected screen.
            switch (SelectedScene)
            {
                case Scene.StartingArea:
                    StartingAreaMap.Draw(spriteBatch);
                    campfire.Draw(spriteBatch);
                    sittingWarriorEntity.Draw(spriteBatch);
                    break;
                case Scene.Level_1:
                    if (!Reloaded)
                    {
                        Level_1Map.Draw(spriteBatch);
                    }
                    break;
            }

            // Escape menu.
            if (SelectedScene == Scene.EscapeMenu)
            {
                escapeMenu.Draw(spriteBatch);
            }
            // Save menu.
            else if (SelectedScene == Scene.SaveMenu)
            {
                saveMenu.Draw(spriteBatch);
            }
            // Load menu.
            else if (SelectedScene == Scene.LoadMenu)
            {
                if (!Reloaded)
                {
                    loadMenu.Draw(spriteBatch);
                }
            }
            else
            {
                Vector2 playerHealthPosition = new Vector2(Player.Position.X - 170, Player.Position.Y - 110);

                Player.Draw(spriteBatch);
                Player.DrawHUD(spriteBatch, playerHealthPosition, true);

                int health = (int)Player.CurrentHealth;
                Vector2 healthStatus = new Vector2(playerHealthPosition.X + 32, playerHealthPosition.Y);
                spriteBatch.DrawString(Font, health.ToString() + " / 100", healthStatus, Color.White);
                dialogBox.Draw(spriteBatch);
                inventory.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        bool inDialog = false;

        public void HandleDialog()
        {
            if (Player.BoundingBox.Intersects(sittingWarriorEntity.BoundingBox) && KeyBoardNewState.IsKeyDown(Keys.E) && KeyBoardOldState.IsKeyUp(Keys.E))
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
        public static void FadeInMap(Map map)
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

        public static void OpenSaveMenu(Game game, GameWindow window, ContentManager content)
        {
            saveMenu = new SaveMenu(game, window, content);
        }

        public static void OpenLoadMenu(Game game, GameWindow window, ContentManager content)
        {
            loadMenu = new LoadMenu(game, window, content);
        }
        public override void Show()
        {
            base.Show();
            Enabled = true;
            Visible = true;
        }

        /// <summary>
        /// Enables/Disables teleporters depending on which scene is selected.
        /// </summary>
        /// <param name="selectedScene">Which scene to enable teleporters</param>
        public void ToggleTeleporters(Scene selectedScene)
        {
            foreach (Teleporter teleporter in teleporterList)
            {
                if (teleporter.GetScene() != selectedScene.ToString())
                {
                    teleporter.Enabled = false;
                }
                else
                {
                    teleporter.Enabled = true;
                }
            }
        }

        public static void Reload()
        {
            Reloaded = true;
        }
        public override void Hide()
        {
            base.Hide();
            Enabled = false;
            Visible = false;
        }


    }


}