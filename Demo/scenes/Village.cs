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

namespace Demo
{
    class Village : SceneManager
    {

        private TiledMap map;
        public static ViewportAdapter viewPortAdapter;
        private IMapRenderer mapRenderer;

        private Queue<string> maps;

        /// <summary>
        /// Create player entity to initiate position, state and camera.
        /// </summary>

        public static Entity player;

        /// <summary>
        /// Create instance of player class to store animations and status display.
        /// </summary>
        public static Player playerData;

        Entity npc;
        Entity westernExit;
 //     Entity wolf;

        Camera2D camera;

        CollisionWorld collision;

        DialogBox dialog;

        public static KeyboardState oldState;
        public static KeyboardState newState;

        public static int enemyHitCount;
        public static int enemyMovementTimer;
        public static int enemyBehaviorTimer;
 //       public static bool enemyIsMoving;
 //       public static bool enemyIsAttacking;

        Song song;

        public static SoundEffect swordSFX;

        public Village(Game game, GameWindow window)
            : base(game)
        {
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            mapRenderer = new FullMapRenderer(GraphicsDevice);
            window.AllowUserResizing = true;
            inDialog = false;
            base.Initialize();
        }

        public static bool inDialog
        {
            get;
            set;
        }

        protected override void LoadContent()
        {
            maps = new Queue<string>(new[] { @"maps\village" });
            map = LoadNextMap();
            mapRenderer.SwapMap(map);
            song = Content.Load<Song>(@"music\chopping_block");
            swordSFX = Content.Load<SoundEffect>(@"sfx\sfx_sword");
            //          MediaPlayer.Play(song);

            playerData = new Player();
            playerData.LoadContent(Content);

            var npcTexture = Content.Load<Texture2D>(@"characters\character_1");
            var npcAtlas = TextureAtlas.Create(npcTexture, 16, 16);
            var npcAnimations = new SpriteSheetAnimationFactory(npcAtlas);
            npcAnimations.Add("idle", new SpriteSheetAnimationData(new[] { 1 }));
            npcAnimations.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, isLooping: true));
            npcAnimations.Add("walkWest", new SpriteSheetAnimationData(new[] { 3, 4, 5 }, isLooping: true));
            npcAnimations.Add("idleWest", new SpriteSheetAnimationData(new[] { 5 }));
            npcAnimations.Add("walkEast", new SpriteSheetAnimationData(new[] { 6, 7, 8 }, isLooping: true));
            npcAnimations.Add("idleEast", new SpriteSheetAnimationData(new[] { 6 }));
            npcAnimations.Add("walkNorth", new SpriteSheetAnimationData(new[] { 9, 10, 11 }, isLooping: true));
            npcAnimations.Add("idleNorth", new SpriteSheetAnimationData(new[] { 10 }));

            var wolfTexture = Content.Load<Texture2D>(@"enemies\wolf");
            var wolfAtlas = TextureAtlas.Create(wolfTexture, 48, 32);
            var wolfAnimations = new SpriteSheetAnimationFactory(wolfAtlas);
            wolfAnimations.Add("idle", new SpriteSheetAnimationData(new[] { 1 }));
            wolfAnimations.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, isLooping: true));
            wolfAnimations.Add("attackSouth", new SpriteSheetAnimationData(new[] { 3, 4, 5 }, isLooping: true));
            wolfAnimations.Add("walkWest", new SpriteSheetAnimationData(new[] { 6, 7, 8, 7 }, isLooping: true));
            wolfAnimations.Add("attackWest", new SpriteSheetAnimationData(new[] { 9, 10, 11 }, isLooping: true));
            wolfAnimations.Add("idleWest", new SpriteSheetAnimationData(new[] { 7 }));
            wolfAnimations.Add("walkEast", new SpriteSheetAnimationData(new[] { 12, 13, 14, 13 }, isLooping: true));
            wolfAnimations.Add("attackEast", new SpriteSheetAnimationData(new[] { 15, 16, 17 }, isLooping: true));
            wolfAnimations.Add("idleEast", new SpriteSheetAnimationData(new[] { 13 }));
            wolfAnimations.Add("walkNorth", new SpriteSheetAnimationData(new[] { 18, 19, 20, 19 }, isLooping: true));
            wolfAnimations.Add("idleNorth", new SpriteSheetAnimationData(new[] { 19 }));
            wolfAnimations.Add("hurtSouth", new SpriteSheetAnimationData(new[] { 21, 26 }, frameDuration: (float)0.01));
            wolfAnimations.Add("hurtWest", new SpriteSheetAnimationData(new[] { 24, 25 }, frameDuration: (float)0.01));
            wolfAnimations.Add("hurtEast", new SpriteSheetAnimationData(new[] { 22, 23 }, frameDuration: (float)0.01));

            var westernExitTexture = Content.Load<Texture2D>(@"objects\teleporter");
            var westernExitAtlas = TextureAtlas.Create(westernExitTexture, 16, 16);
            var westernExitAnimations = new SpriteSheetAnimationFactory(westernExitAtlas);
            westernExitAnimations.Add("idle", new SpriteSheetAnimationData(new[] { 1 }));

            player = new Entity(playerData.CombatAnimations);
            player.Position = new Vector2(850, 500);
            player.State = Action.Idle;

            npc = new Entity(npcAnimations);
            npc.Position = new Vector2(800, 500);
            npc.State = Action.Idle;
            
            westernExit = new Entity(westernExitAnimations);
            westernExit.Position = new Vector2(710, 525);
            westernExit.State = Action.Idle;

   //         wolf = new Entity(wolfAnimations);
   //         wolf.Position = new Vector2(600, 300);
  //          wolf.State = Action.Idle;

            collision = new CollisionWorld(new Vector2(0));
            collision.CreateGrid(map.GetLayer<TiledTileLayer>("Collision"));
            collision.CreateActor(player);
            collision.CreateActor(npc);
  //          collision.CreateActor(wolf);

            dialog = new DialogBox(game) { Text = "" };

            base.LoadContent();
        }


        protected override void UnloadContent()
        {
            map.Dispose();
        }


        private TiledMap LoadNextMap()
        {
            var name = maps.Dequeue();
            map = Content.Load<TiledMap>(name);
            maps.Enqueue(name);
            return map;
        }


 //               private float DirectionTimer;
//                private float MovementTimer;
 
        public override void Update(GameTime gameTime)
        {
            enemyHitCount++;
            enemyMovementTimer++;
            enemyBehaviorTimer++; 

            newState = Keyboard.GetState();
            player.Update(gameTime);
            npc.Update(gameTime);
   //         wolf.Update(gameTime);
            collision.Update(gameTime);

            camera.Zoom = 4;
            camera.LookAt(player.Position);

            CollisionHandler player_npc_collision = new CollisionHandler();
            player_npc_collision.CharacterCollision(player, npc);

            CollisionHandler player_westExit_collision = new CollisionHandler();

            bool enteredWestExit = player_westExit_collision.TransitionerCollision(player, westernExit);

            if(enteredWestExit)
            {
                Game1.area_1.Show();
                this.Hide();

            }

            Player controls = new Player();
            controls.HandleInput(gameTime, player, inDialog, newState, oldState);
          /*
            EnemyAI wolfAI = new EnemyAI(player, wolf);
            
            Random random = new Random();

            if(EnemyIsMoving)
            {
                wolfAI.RandomMovement(gameTime, wolf);
            }

            if(EnemyIsAttacking)
            {
                wolfAI.Attack(gameTime, player, wolf, controls);
            }

            if (EnemyBehaviorTimer > 50)
            {
                EnemyIsMoving = false;
                EnemyIsAttacking = false;

                switch (random.Next(0, 3))
                {
                    case 0:
                        EnemyIsMoving = true;
                        break;
                    case 1:
                        EnemyIsAttacking = true;
                        break;
                    case 2:
                        EnemyIsAttacking = true;
                        break;
                }
                EnemyBehaviorTimer = 0;
            }
            */

            float player_npc_distance = Vector2.Distance(player.Position, npc.Position);

            if (!inDialog) {
                if (player_npc_distance < 25 && newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                {
                    inDialog = true;
                    dialog = new DialogBox(game)
                    {
                        Text = "test"
                    };
                    Components.Add(dialog);
                    dialog.Show();
                    this.Hide();
                }
            }

            dialog.Update();

            oldState = newState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
            mapRenderer.Draw(camera.GetViewMatrix());
            player.Draw(spriteBatch);
            npc.Draw(spriteBatch);
     //       wolf.Draw(spriteBatch);
            playerData.DrawHUD(spriteBatch, camera.Position);
            dialog.Draw(spriteBatch);
            westernExit.Draw(spriteBatch);
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
