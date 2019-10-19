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
using System.Timers;
using Humper;

namespace Demo
{
    public class Player
    {
        public Texture2D playerTexture;
        public TextureAtlas playerAtlas;
        public SpriteSheetAnimationFactory animation;
        MouseState oldMouseState;
        MouseState newMouseState;
       
        public void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>(@"player\player");
            playerAtlas = TextureAtlas.Create(playerTexture, 32, 32);
            animation = new SpriteSheetAnimationFactory(playerAtlas);
            float animationSpeed = .09f;
            float attackSpeed = .04f;
            animation.Add("idle", new SpriteSheetAnimationData(new[] { 0}));
            animation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 1, 2}, .1f, isLooping: true));
            animation.Add("attackSouth", new SpriteSheetAnimationData(new[] { 3, 4, 5, 6, 7, 8 }, attackSpeed, isLooping: true));
            animation.Add("walkWest", new SpriteSheetAnimationData(new[] { 9, 10, 9, 11}, animationSpeed, isLooping: true));
            animation.Add("attackWest", new SpriteSheetAnimationData(new[] { 12, 13, 14, 15, 16, 17 }, attackSpeed, isLooping: true));
            animation.Add("idleWest", new SpriteSheetAnimationData(new[] { 9 }));
            animation.Add("walkEast", new SpriteSheetAnimationData(new[] { 20, 19, 20, 18 }, animationSpeed, isLooping: true));
            animation.Add("attackEast", new SpriteSheetAnimationData(new[] { 21, 22, 23, 24, 25, 26 }, attackSpeed, isLooping: true));
            animation.Add("idleEast", new SpriteSheetAnimationData(new[] { 20 }));
            animation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 27, 28, 29, 28 }, .07f, isLooping: true));
            animation.Add("attackNorth", new SpriteSheetAnimationData(new[] { 30, 31, 32, 33, 34, 35, 34, 33, 32, 31 }, attackSpeed, isLooping: true));
            animation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 28 }));
        }


        public void HandleInput(GameTime gameTime, Entity player, IBox playerCollision, KeyboardState newState, KeyboardState oldState)
        {

            Vector2 motion = new Vector2(playerCollision.X, playerCollision.Y);

            int speed = 1;

            newMouseState = Mouse.GetState();


            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkSouth ||
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.Idle)
            {
                player.State = Action.AttackSouth;            
            }
            else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkWest ||
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleWest)
            {
                player.State = Action.AttackWest;
            }
            else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkEast ||
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleEast)
            {

                player.State = Action.AttackEast;
            }
            else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkNorth ||
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleNorth)
            {

                player.State = Action.AttackNorth;
            }
            else
            {
                if (newState.IsKeyDown(Keys.W) && player.State != Action.AttackNorth)
                {
                    // Walk east if W and D are pressed
                    if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.D))
                    {
                        motion.Y -= speed;
                        player.Position = motion;
                        player.State = Action.WalkEast;
                    }
                    // Walk west if W and A are pressed.
                    else if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.A))
                    {
                        motion.Y -= speed;
                        player.Position = motion;
                        player.State = Action.WalkWest;
                    }
                    else
                    {
                        // Walk north.
                        motion.Y -= speed;
                        player.Position = motion;
                        player.State = Action.WalkNorth;
                    }
                }

                if (newState.IsKeyDown(Keys.S) && player.State != Action.AttackSouth)
                {
                    // Walk east if S and D are pressed.
                    if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.D))
                    {
                        motion.Y += speed;
                        player.Position = motion;
                        player.State = Action.WalkEast;

                    }

                    // Walk west if S and A are pressed.
                    else if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.A))
                    {
                        motion.Y += speed;
                        player.Position = motion;
                        player.State = Action.WalkWest;
                    }
                    else
                    {
                        // Walk south
                        motion.Y += speed;
                        player.Position = motion;
                        player.State = Action.WalkSouth;
                    }
                }

                // Walk east
                if (newState.IsKeyDown(Keys.D) && player.State != Action.AttackEast)
                {
                    motion.X += speed;
                    player.Position = motion;
                    player.State = Action.WalkEast;
                }

                // Walk west
                if (newState.IsKeyDown(Keys.A) && player.State != Action.AttackWest)
                {
                    motion.X -= speed;
                    player.Position = motion;
                    player.State = Action.WalkWest;
                }
            }


            oldMouseState = newMouseState;
        }
    }
}
