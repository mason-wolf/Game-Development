//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using MonoGame.Extended;
//using MonoGame.Extended.Tiled;
//using MonoGame.Extended.ViewportAdapters;
//using MonoGame.Extended.Tiled.Renderers;
//using Demo.Scenes;
//using MonoGame.Extended.TextureAtlases;
//using MonoGame.Extended.Animations.SpriteSheets;
//using MonoGame.Extended.Collisions;
//using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Audio;
//using System;
//using MonoGame.Extended.Shapes;
//using Humper;
//using Humper.Responses;

//namespace Demo.Scenes
//{
//    class BuildMode : SceneManager2
//    {
//        private TiledMap map;
//        public static ViewportAdapter viewPortAdapter;
//        private Queue<string> maps;
     

//        Camera2D camera;
//        Texture2D gridLine;
//        Texture2D horizontal_wall_wood;
//        Texture2D buildMenu;
//        Texture2D cursor;

//        MouseState mouseState;
//        Vector2 mousePosition = new Vector2(250, 250);
//        List<BuildingComponent> buildingComponents = new List<BuildingComponent>();
   

//        KeyboardState oldState;
//        KeyboardState newState;

//        public BuildMode(Game game, GameWindow window) : base(game)
//        {
//            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
//            camera = new Camera2D(viewPortAdapter);
//            mapRenderer = new FullMapRenderer(GraphicsDevice);
//            base.Initialize();
//        }

//        protected override void LoadContent()
//        {
//            maps = new Queue<string>(new[] { @"maps\build_mode" });
//            map = LoadNextMap();
//            mapRenderer.SwapMap(map);
//            gridLine = Content.Load<Texture2D>(@"tilesets\gridbox");
//            horizontal_wall_wood = Content.Load<Texture2D>(@"tilesets\horizontal_wall_wood");
//            buildMenu = Content.Load<Texture2D>(@"interface\buildMenu");
//            cursor = Content.Load<Texture2D>(@"interface\cursor");

//            Mouse.SetPosition((int)mousePosition.X, (int)mousePosition.Y);
    
      
//            int x = 100;
//            int y = 50;

//            for (int i = 0; i < 15; ++i)
//            {
//                for (int j = 0; j < 15; ++j)
//                {
//                    BuildingComponent xComponent = new BuildingComponent(16, 16, gridLine, new Vector2(x, y));
//                    buildingComponents.Add(xComponent);
//                    x += map.TileWidth - 1;
//                }

//                x = 100;

//                BuildingComponent yComponent = new BuildingComponent(16, 16, gridLine, new Vector2(x, y));
//                buildingComponents.Add(yComponent);
//                y += map.TileWidth - 1;

//            }

    
//            var collisionLayer = map.GetLayer<TiledTileLayer>("Collision");
            
//            collisionTiles = new List<Vector2>();


//            world = new World(map.Width * 16, map.Height * 16);

//            foreach (TiledTile tile in collisionLayer.Tiles)
//            {
//                if(!tile.IsBlank)
//                {
//                    collisionTiles.Add(new Vector2(tile.Position.X * 16, tile.Position.Y * 16));
//                    world.Create(tile.Position.X * 16, tile.Position.Y * 16, 16, 16);
//                }

//            }

            
//            playerCollision = world.Create(100, 100, 16, 16);
//            camera.Position = new Vector2(playerCollision.X, playerCollision.Y);
//            base.LoadContent();
//        }

//        List<Vector2> collisionTiles;


//        private TiledMap LoadNextMap()
//        {
//            var name = maps.Dequeue();
//            map = Content.Load<TiledMap>(name);
//            maps.Enqueue(name);
//            return map;
//        }



//        Rectangle mouseTexture;
//        Rectangle mouseBounds;
//        Rectangle buildMenuBounds;
//        World world;
//        IBox playerCollision;
  
//        public Vector2 Input()
//        {
//            Vector2 motion = new Vector2(playerCollision.X, playerCollision.Y);

//            int speed = 4;

//            if (newState.IsKeyDown(Keys.W))
//            {
//                if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.D))
//                {
//                    motion.Y -= speed;

//                }
//                else if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.A))
//                {
//                    motion.Y -= speed;

//                }
//                else
//                {
//                    motion.Y -= speed;
//                }
//            }

//            if (newState.IsKeyDown(Keys.S))
//            {
//                if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.D))
//                {
//                    motion.Y += speed;

//                }
//                else if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.A))
//                {
//                    motion.Y += speed;
//                }
//                else
//                {
//                    motion.Y += speed;
//                }
//            }

//            if (newState.IsKeyDown(Keys.D))
//            {
//                motion.X += speed;
//            }

//            if (newState.IsKeyDown(Keys.A))
//            {
//                motion.X -= speed;
//            }

//            return motion;
//        }

//        public override void Update(GameTime gameTime)
//        {

//             Vector2 position = Input();
//             playerCollision.Move(position.X, position.Y, (collision) => CollisionResponses.Slide);

   
//            mouseState = Mouse.GetState();
//            newState = Keyboard.GetState();
   
//            camera.Zoom = 3;
         

         
//            mouseBounds = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 1, 1);
//            buildMenuBounds = new Rectangle((int)position.X - 180, (int)position.Y + 70, buildMenu.Width, buildMenu.Height);

//            if (mouseState.LeftButton == ButtonState.Pressed)
//            {
//                foreach (BuildingComponent component in buildingComponents)
//                {
//                    Rectangle bounds = new Rectangle(mouseBounds.X, mouseBounds.Y, mouseBounds.Width / 2, mouseBounds.Height);
//                    if (component.Bounds.Intersects(bounds))
//                    {
//                        component.Tile = horizontal_wall_wood;
//                    }
//                }
//            }

//            if (mouseState.RightButton == ButtonState.Pressed)
//            {
//                foreach (BuildingComponent component in buildingComponents)
//                {
//                    Rectangle bounds = new Rectangle(mouseBounds.X, mouseBounds.Y, mouseBounds.Width / 2, mouseBounds.Height);
//                    if (component.Bounds.Intersects(bounds))
//                    {
//                        component.Tile = gridLine;
//                    }
//                }
//            }


//            if (mousePosition.X > 0 && mousePosition.X < viewPortAdapter.ViewportWidth && mousePosition.Y > 0 && mousePosition.Y < viewPortAdapter.ViewportHeight)
//            {
//                mouseTexture = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 16, 16);
//            }
     


//            mousePosition.X = mouseState.X;
//            mousePosition.Y = mouseState.Y;

//            camera.LookAt(new Vector2(playerCollision.X, playerCollision.Y));
//            mapRenderer.Update(gameTime);
//            oldState = newState;
            

//            base.Update(gameTime);
//        }

//        public override void Draw(GameTime gameTime)
//        {

//            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
//            mapRenderer.Draw(camera.GetViewMatrix());
     
//            Vector2 menuPosition = new Vector2(camera.Position.X + 360 , camera.Position.Y + 430);    
 
//            foreach (BuildingComponent component in buildingComponents)
//            {
//                spriteBatch.Draw(component.Tile, component.Position);
//            }

//            spriteBatch.Draw(buildMenu, menuPosition);
  
//            if (mouseTexture.Intersects(buildMenuBounds))
//            {
//                spriteBatch.Draw(cursor, mouseTexture, Color.White);
//            }
//            else
//            {
//                spriteBatch.Draw(horizontal_wall_wood, mouseTexture, Color.White);
//            }
           
           
//            spriteBatch.End();
//            base.Draw(gameTime);
//        }

//        public override void Show()
//        {
//            base.Show();
//            Enabled = true;
//            Visible = true;
//        }

//        public override void Hide()
//        {
//            base.Hide();
//            Enabled = false;
//            Visible = false;
//        }
//    }

//    public class BuildingComponent 
//    {
//        public Rectangle Bounds { get; set; }
//        public Texture2D Tile { get; set; }
//        public Vector2 Position { get; set; }

//        public BuildingComponent(int width, int height, Texture2D tile, Vector2 position)
//        {
//            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
//            Tile = tile;
//            Position = position;
//        }
//    }
//}

