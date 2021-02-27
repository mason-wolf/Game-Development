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
        public static Map Level_1AMap;
        public static Map Level_1BMap;
        public static SpriteFont Font;
        private Vector2 playerStartingPosition = new Vector2(408, 894);

        public EscapeMenu escapeMenu;
        public static SaveMenu saveMenu;
        public static LoadMenu loadMenu;

        public Inventory inventory;
        private Texture2D HUDArrows;
        private Texture2D HUDKeys;
        // Create teleporters.
        public static List<Teleporter> teleporterList;

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
            Player.MaxStamina = 75;
            Player.CurrentStamina = 75;
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
            Level_1,
            Level_1A,
            Level_1B
        }

        protected override void LoadContent()
        {
            teleporterList = new List<Teleporter>();
            StartingAreaMap = new Map(Content, "Content/maps/StartingArea.tmx");
            Level_1Map = new Map(Content, "Content/maps/level_1.tmx");
            Level_1AMap = new Map(Content, "Content/maps/level_1a.tmx");
            Level_1BMap = new Map(Content, "Content/maps/level_1b.tmx");
            Font = Content.Load<SpriteFont>(@"interface\font");
            Player.LoadContent(Content);
            Player.sprite = new AnimatedSprite(Player.playerAnimation);
            Player.State = Action.IdleSouth1;
            Player.Position = playerStartingPosition;
            Sprites sprites = new Sprites();
            sprites.LoadContent(Content);

            sittingWarriorEntity = new Entity(Sprites.sittingWarriorAnimation);

            Level_1Map.LoadScene(new Level_1());
            Level_1AMap.LoadScene(new Level_1A());
            Level_1BMap.LoadScene(new Level_1B());
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
            SelectedScene = Scene.Level_1;
            playerCollision = StartingAreaMap.GetCollisionWorld();
            Item chickenItem = new Item();
            chickenItem.HealthAmount = 10;
            chickenItem.ItemTexture = Sprites.chickenTexture;
            HUDArrows = Content.Load<Texture2D>(@"objects\arrows");
            HUDKeys = Content.Load<Texture2D>(@"objects\key");
            //Player.InventoryList[0] = chickenItem;
            //Player.InventoryList[1] = chickenItem;
            base.LoadContent();
        }

        bool transitionState = false;
        int counter = 0;
        public override void Update(GameTime gameTime)
        {
            // If save was loaded, create transition effects, assign the player's saved scene and position.
            if (Reloaded)
            {
                SelectedScene = (Init.Scene)Enum.Parse(typeof(Init.Scene), SavedGameLocation);

                switch (SelectedScene)
                {
                    case (Scene.StartingArea):
                        FadeInMap(StartingAreaMap);
                        break;
                    case (Scene.Level_1):
                        FadeInMap(Level_1Map);
                        break;
                    case (Scene.Level_1A):
                        FadeInMap(Level_1AMap);
                        break;
                    case (Scene.Level_1B):
                        FadeInMap(Level_1BMap);
                        break;
                }
                Player.Position = SavedGamePosition;
                Reloaded = false;
            }

            foreach(Teleporter teleporter in teleporterList)
            {
                if (teleporter.Enabled)
                {
                    if (Player.BoundingBox.Intersects(teleporter.GetRectangle()) && teleporter.GetDestinationMap() == "StartingArea")
                    {
                        if (Player.EnemyList.Count > 0)
                        {
                            Player.EnemyList.Clear();
                        }
                        transitionState = true;
                        Content.Unload();
                        LoadContent();

                        FadeInMap(StartingAreaMap);
                        SelectedScene = Scene.StartingArea;
                        Player.Position = new Vector2(335f, 150f);
                    }

                    if (Player.BoundingBox.Intersects(teleporter.GetRectangle()) && teleporter.GetDestinationMap() == "Level_1South")
                    {
                        FadeInMap(Level_1Map);
                        SelectedScene = Scene.Level_1;
                        Player.Position = new Vector2(408f, 880f);
                    }

                    if (Player.BoundingBox.Intersects(teleporter.GetRectangle()) && teleporter.GetDestinationMap() == "Level_1NorthWest")
                    {
                        transitionState = true;
                        FadeInMap(Level_1Map);
                        SelectedScene = Scene.Level_1;
                        Player.Position = new Vector2(42f, 90f);
                    }

                    if (Player.BoundingBox.Intersects(teleporter.GetRectangle()) && teleporter.GetDestinationMap() == "Level_1NorthEast")
                    {
                        transitionState = true;
                        FadeInMap(Level_1Map);
                        SelectedScene = Scene.Level_1;
                        Player.Position = new Vector2(984f, 120f);
                    }

                    if (Player.BoundingBox.Intersects(teleporter.GetRectangle()) && teleporter.GetDestinationMap() == "Level_1A")
                    {
                        transitionState = true;
                        FadeInMap(Level_1AMap);
                        SelectedScene = Scene.Level_1A;
                        Player.Position = new Vector2(105f, 980f);
                    }

                    if (Player.BoundingBox.Intersects(teleporter.GetRectangle()) && teleporter.GetDestinationMap() == "Level_1B")
                    {
                        transitionState = true;
                        FadeInMap(Level_1BMap);
                        SelectedScene = Scene.Level_1B;
                        Player.Position = new Vector2(520f, 980f);
                    }
                }
            }

            // Create a short pause after transitioning to next level.
            counter++;

            if (counter > 70)
            {
                counter = 0;
                transitionState = false;
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
                    ToggleTeleporters(SelectedMap.MapName);
                    break;
                case Scene.Level_1:
                    playerCollision = Level_1Map.GetCollisionWorld();
                    Level_1Map.Update(gameTime);
                    SelectedMap = Level_1Map;
                    ToggleTeleporters(SelectedMap.MapName);
                    Player.EnemyList = Level_1.enemyList;
                    break;
                case Scene.Level_1A:
                    playerCollision = Level_1AMap.GetCollisionWorld();
                    Level_1AMap.Update(gameTime);
                    SelectedMap = Level_1AMap;
                    ToggleTeleporters(SelectedMap.MapName);
                    Player.EnemyList = Level_1A.enemyList;
                    break;
                case Scene.Level_1B:
                    playerCollision = Level_1BMap.GetCollisionWorld();
                    Level_1BMap.Update(gameTime);
                    SelectedMap = Level_1BMap;
                    ToggleTeleporters(SelectedMap.MapName);
                    Player.EnemyList = Level_1B.enemyList;
                    break;
            }
            dialogBox.Update();
            inventory.Update(gameTime);

            // Handle player's collision.
            playerCollision.Move(Player.Position.X, Player.Position.Y, (collision) => CollisionResponses.Slide);
            Player.Update(gameTime);

            campfire.Update(gameTime);

            Camera.Zoom = 3;

            if (!inDialog && !transitionState)
            {
                Player.HandleInput(gameTime, Player, playerCollision, KeyBoardNewState, KeyBoardOldState);
            }

            Camera.LookAt(Player.Position);
            KeyBoardOldState = KeyBoardNewState;
            KeyBoardNewState = Keyboard.GetState();
            escapeMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);
            saveMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);
            loadMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);

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
                    Level_1Map.Draw(spriteBatch);
                    break;
                case Scene.Level_1A:
                    Level_1AMap.Draw(spriteBatch);
                    break;
                case Scene.Level_1B:
                    Level_1BMap.Draw(spriteBatch);
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

                // Draw arrows
                spriteBatch.Draw(HUDArrows, new Vector2(Init.Player.Position.X + 145, Init.Player.Position.Y - 110), Color.White);
                spriteBatch.DrawString(Init.Font, Inventory.TotalArrows.ToString(), new Vector2(Init.Player.Position.X + 165, Init.Player.Position.Y - 105), Color.White);

                // Draw keys
                spriteBatch.Draw(HUDKeys, new Vector2(Init.Player.Position.X + 135, Init.Player.Position.Y - 90), Color.White);
                spriteBatch.DrawString(Init.Font, Inventory.TotalKeys.ToString(), new Vector2(Init.Player.Position.X + 165, Init.Player.Position.Y - 80), Color.White);

                //int health = (int)Player.CurrentHealth;
                //Vector2 healthStatus = new Vector2(playerHealthPosition.X + 32, playerHealthPosition.Y);
            //    spriteBatch.DrawString(Font, health.ToString() + " / 100", healthStatus, Color.White);
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
        public void ToggleTeleporters(string selectedMap)
        {
            foreach (Teleporter teleporter in teleporterList)
            {
                if (teleporter.GetSourceMap() == selectedMap)
                {
                    teleporter.Enabled = true;
                }
                else
                {
                    teleporter.Enabled = false;
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