using Demo.Engine;
using Humper;
using Humper.Responses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Scenes
{
    class Level_1 : SceneManager
    {
        public static Camera2D camera;
        public static ViewportAdapter viewPortAdapter;
        public static Map map;
        public  Player player;
        public static Entity playerEntity;
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static World collisionWorld;
        public static IBox playerCollision;
        private SpriteFont font;
        private static Rectangle teleporter;
        GameWindow window;

        public Level_1(Game game, GameWindow window) : base(game)
        {
            this.window = window;
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            base.Initialize();
        }

        protected override void LoadContent()
        {
           
            map = new Map();
            map.LoadMap(Content, "Content/maps/Level_1.tmx");

            collisionWorld = new World(map.Width() * 16, map.Height() * 16);
            font = Content.Load<SpriteFont>(@"interface\font");

            foreach (Tile tile in map.GetCollisionLayer())
            {
                if (tile.TileID != 0)
                {
                    collisionWorld.Create(tile.Position.X + 8, tile.Position.Y + 8, 16, 16);
                }
            }

            player = new Player();
            playerCollision = collisionWorld.Create(0, 0, 16, 16);
            player.LoadContent(Content);
            playerEntity = new Entity(player.playerAnimation);
            playerEntity.LoadContent(Content);
            playerEntity.Position = new Vector2(407, 986);
            playerEntity.State = Action.IdleNorth;
            playerEntity.MaxHealth = 150;
            playerEntity.CurrentHealth = 150;
            player.AttackDamage = 2;

            teleporter = new Rectangle(407, 1020, 16, 16);
            base.LoadContent();
        }

        bool previousLevel = false;

        public override void Update(GameTime gameTime)
        {
            if (playerEntity.BoundingBox.Intersects(teleporter) && previousLevel == false)
            {
                Content.Unload();
                previousLevel = true;
                StartingArea startingArea = new StartingArea(game, window);
                Components.Remove(this);
                Components.Add(startingArea);
                startingArea.Show();
            }

            if (previousLevel != true)
            {
                newState = Keyboard.GetState();
                playerCollision.Move(playerEntity.Position.X, playerEntity.Position.Y, (collision) => CollisionResponses.Slide);
                playerEntity.Update(gameTime);
                camera.Zoom = 3;
                player.HandleInput(gameTime, playerEntity, playerCollision, newState, oldState);
                camera.LookAt(playerEntity.Position);
                oldState = newState;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());

            map.Draw(spriteBatch);

            Vector2 playerHealthPosition = new Vector2(playerEntity.Position.X - 170, playerEntity.Position.Y - 110);

            playerEntity.Draw(spriteBatch);
            playerEntity.DrawHUD(spriteBatch, playerHealthPosition, true);

            int health = (int)playerEntity.CurrentHealth;
            Vector2 healthStatus = new Vector2(playerHealthPosition.X + 57, playerHealthPosition.Y);
            spriteBatch.DrawString(font, health.ToString() + " / 150", healthStatus, Color.White);

            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
