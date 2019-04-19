﻿using System.Collections.Generic;
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

namespace Demo.Scenes
{
    class TestMap : SceneManager
    {

        public static ViewportAdapter viewPortAdapter;
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static Entity playerEntity;
        public static Player playerData;
        public static Camera2D camera;
        public static Map map;
        public static Vector2 startingPosition = new Vector2(0, 0);


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

            playerData = new Player();
            playerData.LoadContent(Content);
            playerEntity = new Entity(playerData.animation);
            playerEntity.Position = startingPosition;
            playerEntity.State = Action.IdleWest;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

            playerEntity.Update(gameTime);
     
            camera.Zoom = 4;
            camera.LookAt(playerEntity.Position);
           
            playerData.HandleInput(gameTime, playerEntity, false, newState, oldState);

            oldState = newState;

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
            map.Draw(spriteBatch);
         //   playerEntity.Draw(spriteBatch);
            //   playerData.DrawHUD(spriteBatch, camera.Position);
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