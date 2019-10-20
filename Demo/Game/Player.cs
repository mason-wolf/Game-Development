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
            animation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 3, 4, 5, 6, 7, 8, 7, 6, 5}, attackSpeed, isLooping: true));
            animation.Add("attackSouthPattern2", new SpriteSheetAnimationData(new[] { 9, 10, 11 }, attackSpeed, isLooping: true));
            animation.Add("walkWest", new SpriteSheetAnimationData(new[] { 12, 13, 12, 14}, animationSpeed, isLooping: true));
            animation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 15, 16, 17, 18, 19, 20, 18, 17, 16 }, attackSpeed, isLooping: true));
            animation.Add("attackWestPattern2", new SpriteSheetAnimationData(new[] { 21, 22, 23 }, attackSpeed, isLooping: true));
            animation.Add("idleWest", new SpriteSheetAnimationData(new[] { 12 }));
            animation.Add("walkEast", new SpriteSheetAnimationData(new[] { 26, 25, 26, 24 }, animationSpeed, isLooping: true));
            animation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 27, 28, 29, 30, 31, 32, 30, 29, 28}, attackSpeed, isLooping: true));
            animation.Add("attackEastPattern2", new SpriteSheetAnimationData(new[] { 33, 34, 35 }, attackSpeed, isLooping: true));
            animation.Add("idleEast", new SpriteSheetAnimationData(new[] { 26 }));
            animation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 36, 37, 38, 37}, .07f, isLooping: true));
            animation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 39, 40, 41, 42, 43, 44 }, attackSpeed, isLooping: true));
            animation.Add("attackNorthPattern2", new SpriteSheetAnimationData(new[] { 45, 46, 47 }, attackSpeed, isLooping: true));
            animation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 37 }));
        }

        Random random = new Random();
        public void HandleInput(GameTime gameTime, Entity player, IBox playerCollision, KeyboardState newState, KeyboardState oldState)
        {

            Vector2 motion = new Vector2(playerCollision.X, playerCollision.Y);

            int speed = 1;

            newMouseState = Mouse.GetState();

            // Attacking south
            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkSouth ||
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.Idle)
            {


                int attackPattern = random.Next(1, 4);
              //  Console.WriteLine(attackMotion.ToString());
              if (attackPattern < 3)
                {
                    motion.Y -= speed;
                    player.Position = motion;
                    player.State = Action.AttackSouthPattern1;
                }
              else
                {
                    player.State = Action.AttackSouthPattern2;
                }        
            }

            // Attacking West
            else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkWest ||
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleWest)
            {
                int attackPattern = random.Next(1, 4);

                if (attackPattern < 3)
                {

                    player.State = Action.AttackWestPattern1;
                }
                else
                {
                    player.State = Action.AttackWestPattern2;
                }
            }

            // Attacking East
            else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkEast ||
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleEast)
            {
                int attackPattern = random.Next(1, 4);

                if (attackPattern < 3)
                {
                    player.State = Action.AttackEastPattern1;
                }
                else
                {
                    player.State = Action.AttackEastPattern2;
                }
            }

            // Attacking North
            else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkNorth ||
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleNorth )
            {

                int attackPattern = random.Next(1, 4);

                if (attackPattern < 3)
                {
                    player.State = Action.AttackNorthPattern1;
                }
                else
                {
                    player.State = Action.AttackNorthPattern2;
                }
            }
            else
            {
                if (newState.IsKeyDown(Keys.W) && player.State != Action.AttackNorthPattern1)
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

                if (newState.IsKeyDown(Keys.S) && player.State != Action.AttackSouthPattern1)
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
                if (newState.IsKeyDown(Keys.D) && player.State != Action.AttackEastPattern1)
                {
                    motion.X += speed;
                    player.Position = motion;
                    player.State = Action.WalkEast;
                }

                // Walk west
                if (newState.IsKeyDown(Keys.A) && player.State != Action.AttackWestPattern1)
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
