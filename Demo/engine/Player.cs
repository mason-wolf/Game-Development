using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demo.Scenes;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations.SpriteSheets;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Particles;
using MonoGame.Extended.SceneGraphs;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Audio;

namespace Demo
{
    public class Player
    {
        public Texture2D playerTexture;
        public TextureAtlas playerAtlas;
        public SpriteSheetAnimationFactory animation;

        public void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>(@"player\player");
            playerAtlas = TextureAtlas.Create(playerTexture, 32, 32);
   
            animation = new SpriteSheetAnimationFactory(playerAtlas);
            animation.Add("idle", new SpriteSheetAnimationData(new[] { 0 }));
            animation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, (float)(0.1), isLooping: true));
            animation.Add("walkWest", new SpriteSheetAnimationData(new[] { 9, 10, 11 }, (float)(0.1), isLooping: true));
            animation.Add("idleWest", new SpriteSheetAnimationData(new[] { 9 }));
            animation.Add("walkEast", new SpriteSheetAnimationData(new[] { 18, 19, 20 }, (float)(0.1), isLooping: true));
            animation.Add("idleEast", new SpriteSheetAnimationData(new[] { 18 }));
            animation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 27, 28, 29 }, (float)(0.1), isLooping: true));
            animation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 27 }));
        }

        public void HandleInput(GameTime gameTime, Entity player, bool inDialog, KeyboardState newState, KeyboardState oldState)
        {
            Vector2 motion = new Vector2();

            motion = player.Position;
            int speed = 1;

            if (newState.IsKeyDown(Keys.W))
            {
                if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.D))
                {
                    motion.Y -= speed;
                    player.Position = motion;
                    player.State = Action.WalkEast;
                }
                else if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.A))
                {
                    motion.Y -= speed;
                    player.Position = motion;
                    player.State = Action.WalkWest;
                }
                else
                {
                    motion.Y -= speed;
                    player.Position = motion;
                    player.State = Action.WalkNorth;
                }
            }

            if (newState.IsKeyDown(Keys.S))
            {
                if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.D))
                {
                    motion.Y += speed;
                    player.Position = motion;
                    player.State = Action.WalkEast;

                }
                else if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.A))
                {
                    motion.Y += speed;
                    player.Position = motion;
                    player.State = Action.WalkWest;
                }
                else
                {
                    motion.Y += speed;
                    player.Position = motion;
                    player.State = Action.WalkSouth;
                }
            }

            if (newState.IsKeyDown(Keys.D))
            {
                motion.X += speed;
                player.Position = motion;
                player.State = Action.WalkEast;
            }

            if (newState.IsKeyDown(Keys.A))
            {
                motion.X -= speed;
                player.Position = motion;
                player.State = Action.WalkWest;
            }
        }
    }
}
