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
using MonoGame.Extended.Shapes;


namespace Demo.Scenes
{
    class BuildMode : SceneManager
    {
        private TiledMap map;
        public static ViewportAdapter viewPortAdapter;
        private Queue<string> maps;
     

        Camera2D camera;
        Texture2D gridLine;
        Texture2D horizontal_wall_wood;
        Texture2D buildMenu;
        Texture2D cursor;

        MouseState mouseState;
        Vector2 mousePosition = new Vector2(1050, 500);
        List<BuildingComponent> buildingComponents = new List<BuildingComponent>();
        CollisionWorld collision;
        Entity player;
        Player playerData;
        KeyboardState oldState;
        KeyboardState newState;

        public BuildMode(Game game, GameWindow window) : base(game)
        {
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            camera = new Camera2D(viewPortAdapter);
            mapRenderer = new FullMapRenderer(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            maps = new Queue<string>(new[] { @"maps\build_mode" });
            map = LoadNextMap();
            mapRenderer.SwapMap(map);
            gridLine = Content.Load<Texture2D>(@"tilesets\gridbox");
            horizontal_wall_wood = Content.Load<Texture2D>(@"tilesets\horizontal_wall_wood");
            buildMenu = Content.Load<Texture2D>(@"interface\buildMenu");
            cursor = Content.Load<Texture2D>(@"interface\cursor");

            Mouse.SetPosition((int)mousePosition.X, (int)mousePosition.Y);
            camera.LookAt(mousePosition);
            int x = 900;
            int y = 400;

            for (int i = 0; i < 15; ++i)
            {
                for (int j = 0; j < 15; ++j)
                {
                    BuildingComponent xComponent = new BuildingComponent(16, 16, gridLine, new Vector2(x, y));
                    buildingComponents.Add(xComponent);
                    x += map.TileWidth - 1;
                }

                x = 900;

                BuildingComponent yComponent = new BuildingComponent(16, 16, gridLine, new Vector2(x, y));
                buildingComponents.Add(yComponent);
                y += map.TileWidth - 1;

            }

            playerData = new Player();
            playerData.LoadContent(Content);
            var playerTexture = Content.Load<Texture2D>(@"tilesets\null");
            var playerAtlas = TextureAtlas.Create(playerTexture, 16, 16);
            var playerAnimations = new SpriteSheetAnimationFactory(playerAtlas);
            playerAnimations.Add("idle", new SpriteSheetAnimationData(new[] { 1 }));
            player = new Entity(playerAnimations);
            player.Position = mousePosition;
            collision = new CollisionWorld(new Vector2(0));
            collision.CreateGrid(map.GetLayer<TiledTileLayer>("Collision"));
            collision.CreateActor(player);

            base.LoadContent();
        }


        private TiledMap LoadNextMap()
        {
            var name = maps.Dequeue();
            map = Content.Load<TiledMap>(name);
            maps.Enqueue(name);
            return map;
        }



        Rectangle mouseTexture;
        Rectangle mouseBounds;
        Rectangle buildMenuBounds;

        public override void Update(GameTime gameTime)
        {

            
            mouseState = Mouse.GetState();
            newState = Keyboard.GetState();
            player.Update(gameTime);
            camera.Zoom = 3;
           // camera.LookAt(player.Position);
            Player controls = new Player();
            controls.HandleInput(gameTime, player, false, newState, oldState);

            mouseTexture = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 16, 16);
            mouseBounds = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 4, 4);
            buildMenuBounds = new Rectangle((int)player.Position.X - 180, (int)player.Position.Y + 70, buildMenu.Width, buildMenu.Height);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                foreach (BuildingComponent component in buildingComponents)
                {
                    if (component.Bounds.Intersects(mouseBounds))
                    {
                        component.Tile = horizontal_wall_wood;
                    }
                }
            }

            if (newState.IsKeyDown(Keys.W) || newState.IsKeyDown(Keys.A) || newState.IsKeyDown(Keys.S) || newState.IsKeyDown(Keys.D))
            {
                Mouse.SetPosition((int)player.Position.X, (int)player.Position.Y);
            }

            mousePosition.Y = mouseState.Y;
                mousePosition.X = mouseState.X;
            
        
            //  camera.Position = Vector2.Lerp(new Vector2(camera.Position.X - 50, camera.Position.Y - 50), player.Position, 0.1f);
            camera.LookAt(player.Position);

            mapRenderer.Update(gameTime);
            collision.Update(gameTime);
            oldState = newState;
            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
            mapRenderer.Draw(camera.GetViewMatrix());
            player.Draw(spriteBatch);
            Vector2 menuPosition = new Vector2(player.Position.X - 180, player.Position.Y + 70);
            

            foreach (BuildingComponent component in buildingComponents)
            {
                spriteBatch.Draw(component.Tile, component.Position);
            }

            spriteBatch.Draw(buildMenu, menuPosition);
  
            if (mouseTexture.Intersects(buildMenuBounds))
            {
                Console.WriteLine("true");
                spriteBatch.Draw(cursor, mouseTexture, Color.White);
            }
            else
            {
                spriteBatch.Draw(horizontal_wall_wood, mouseTexture, Color.White);
            }
           
           
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

    public class BuildingComponent 
    {
        public Rectangle Bounds { get; set; }
        public Texture2D Tile { get; set; }
        public Vector2 Position { get; set; }

        public BuildingComponent(int width, int height, Texture2D tile, Vector2 position)
        {
            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            Tile = tile;
            Position = position;
        }
    }
}

