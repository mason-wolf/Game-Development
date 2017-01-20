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
        public TextureAtlas palyerAtlas;
        public SpriteSheetAnimationFactory StandardAnimations;
        public SpriteSheetAnimationFactory CombatAnimations;

        public bool isAttacking;

        protected Texture2D StatusBar;
        protected Texture2D HealthBar;
        protected Texture2D StaminaBar;

        public static double health = 150;
        public static double stamina = 150;
        public static double attackCost = 20;

        public double Health
        {
            get { return health; }
            set { health = value; }
        }

        public double Stamina
        {
            get { return stamina; }
            set { stamina = value; }
        }

        public void LoadContent(ContentManager content)
        {

            StatusBar = content.Load<Texture2D>(@"interface\statusbar");
            HealthBar = content.Load<Texture2D>(@"interface\healthbar");
            StaminaBar = content.Load<Texture2D>(@"interface\staminabar");

            /*
            Texture = Content.Load<Texture2D>("CHARACTER_1");
            Atlas = TextureAtlas.Create(Texture, 16, 16);
            StandardAnimations = new SpriteSheetAnimationFactory(Atlas);
            StandardAnimations.Add("idle", new SpriteSheetAnimationData(new[] { 1 }));
            StandardAnimations.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, isLooping: true));
            StandardAnimations.Add("walkWest", new SpriteSheetAnimationData(new[] { 3, 4, 5 }, isLooping: true));
            StandardAnimations.Add("idleWest", new SpriteSheetAnimationData(new[] { 5 }));
            StandardAnimations.Add("walkEast", new SpriteSheetAnimationData(new[] { 6, 7, 8 }, isLooping: true));
            StandardAnimations.Add("idleEast", new SpriteSheetAnimationData(new[] { 6 }));
            StandardAnimations.Add("walkNorth", new SpriteSheetAnimationData(new[] { 9, 10, 11 }, isLooping: true));
            StandardAnimations.Add("idleNorth", new SpriteSheetAnimationData(new[] { 10 }));
            */

            playerTexture = content.Load<Texture2D>(@"player\player");
            palyerAtlas = TextureAtlas.Create(playerTexture, 32, 32);
            CombatAnimations = new SpriteSheetAnimationFactory(palyerAtlas);

            CombatAnimations.Add("idle", new SpriteSheetAnimationData(new[] { 0 }));
            CombatAnimations.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, (float)(0.1), isLooping: true));
            CombatAnimations.Add("attackSouth", new SpriteSheetAnimationData(new[] { 3, 4, 5, 6, 7, 8}, (float)(0.03), isLooping: true));

            CombatAnimations.Add("walkWest", new SpriteSheetAnimationData(new[] { 9, 10, 11}, (float)(0.1), isLooping: true));
            CombatAnimations.Add("attackWest", new SpriteSheetAnimationData(new[] { 12, 13, 14, 15, 16, 17, 17 }, (float)(0.03), isLooping: true));
            CombatAnimations.Add("idleWest", new SpriteSheetAnimationData(new[] { 9 }));

            CombatAnimations.Add("walkEast", new SpriteSheetAnimationData(new[] { 18, 19, 20 }, (float)(0.1), isLooping: true));
            CombatAnimations.Add("attackEast", new SpriteSheetAnimationData(new[] { 21, 22, 23, 24, 25, 26, 26}, (float)(0.03), isLooping: true));
            CombatAnimations.Add("idleEast", new SpriteSheetAnimationData(new[] { 18 }));

            CombatAnimations.Add("walkNorth", new SpriteSheetAnimationData(new[] { 27, 28, 29 }, (float)(0.1), isLooping: true));
            CombatAnimations.Add("attackNorth", new SpriteSheetAnimationData(new[] { 30, 31, 32, 33, 34 ,35 }, (float)(0.03), isLooping: true));
            CombatAnimations.Add("idleNorth", new SpriteSheetAnimationData(new[] { 27 }));

        }


        public void DrawHUD(SpriteBatch spriteBatch, Vector2 position)
        {
            if (stamina < 150)
            {
                stamina += .2;
            }

            Color transparent = Color.White;
            transparent.A = 150;
            float x = position.X + 410;
            float y = position.Y + 275;
            Vector2 healthPosition = new Vector2(x, y);
            Vector2 staminaPosition = new Vector2(healthPosition.X, healthPosition.Y + 3);
            spriteBatch.Draw(StatusBar, healthPosition, new Rectangle(10, 10, 150, 2), Color.Black);
            spriteBatch.Draw(HealthBar, healthPosition, new Rectangle(10, 10, Convert.ToInt32(Health), 2), transparent);
            spriteBatch.Draw(StatusBar, staminaPosition, new Rectangle(10, 10, 150, 2), Color.Black);
            spriteBatch.Draw(StaminaBar, staminaPosition, new Rectangle(0, 0, Convert.ToInt32(Stamina), 2), transparent);
        }

        public void HandleInput(GameTime gameTime, Entity player, bool inDialog, KeyboardState newState, KeyboardState oldState)
        {
            Vector2 motion = new Vector2();

            motion = player.Position;
            int speed = 1;

            // if player isn't moving but presses attack button, show attack animations
            if (stamina > 10)
            {
                if (player.State == Action.IdleWest && newState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                {
                    stamina -= attackCost;
                    isAttacking = true;
                    player.State = Action.AttackWest;
                    Village.swordSFX.Play();
                }

                if (player.State == Action.IdleEast && newState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                {
                    stamina -= attackCost;
                    isAttacking = true;
                    player.State = Action.AttackEast;
                    Village.swordSFX.Play();
                }

                if (player.State == Action.IdleNorth && newState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                {
                    stamina -= attackCost;
                    isAttacking = true;
                    player.State = Action.AttackNorth;
                    Village.swordSFX.Play();
                }

                if (player.State == Action.Idle && newState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                {
                    stamina -= attackCost;
                    isAttacking = true;
                    player.State = Action.AttackSouth;
                    Village.swordSFX.Play();
                }
            }
            // as long as player isn't in dialog, allow movement (governs attack and non-attack state)

            if (!inDialog)
            {

                // if player is pressing up-right while attacking, show eastern animation
                /*
                if (newState.IsKeyDown(Keys.Up))
                {
                    if (newState.IsKeyDown(Keys.Right) && newState.IsKeyDown(Keys.A))
                    {
                        stamina -= .5;
                        isAttacking = true;
                        player.State = Action.AttackEast;
                        motion.Y -= speed;
                        motion.X += speed;
                        player.Position = motion;
                    }

                    // if player is pressing up-left while attacking, show western animation

                    else if (newState.IsKeyDown(Keys.Left))
                    {
                        isAttacking = true;
                        player.State = Action.AttackWest;
                        motion.Y -= speed;
                        player.Position = motion;
                    }
                }
            */
       
            // if player isn't attacking, show normal movement 

                if (!isAttacking)
                { 
                        if (newState.IsKeyDown(Keys.Up))
                    {
                        if (newState.IsKeyDown(Keys.Up) && newState.IsKeyDown(Keys.Right))
                        {
                            motion.Y -= speed;
                            player.Position = motion;
                            player.State = Action.WalkEast;
                        }
                        else if (newState.IsKeyDown(Keys.Up) && newState.IsKeyDown(Keys.Left))
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
                }

                /*
                if (newState.IsKeyDown(Keys.Down) && newState.IsKeyDown(Keys.A))
                {

                    // if player is pressing down-right show eastern animation

                    if (newState.IsKeyDown(Keys.Right))
                    {
                        isAttacking = true;
                        player.State = Action.WalkEast;
                        motion.Y += speed;
                        player.Position = motion;
                    }

                    // if player is pressing down-left show western animation

                    else if (newState.IsKeyDown(Keys.Left))
                        {
                            isAttacking = true;
                        player.State = Action.WalkWest;
                            motion.Y += speed;
                            player.Position = motion;
                        }
                        else
                    {
                        isAttacking = true;
                        player.State = Action.WalkSouth;
                        motion.Y += speed;
                        player.Position = motion;
                    }
                }

                /*
                if (newState.IsKeyDown(Keys.Left) && newState.IsKeyDown(Keys.A))
                {
                    isAttacking = true;
                    player.State = Action.AttackWest;
                    motion.X -= speed;
                    player.Position = motion;
                }


                if (newState.IsKeyDown(Keys.Right) && newState.IsKeyDown(Keys.A))
                {
                    isAttacking = true;
                    player.State = Action.AttackEast;
                    motion.X += speed;
                    player.Position = motion;
                }
                */

                if (!isAttacking)
                {
                    if (newState.IsKeyDown(Keys.Down))
                    {
                        if (newState.IsKeyDown(Keys.Down) && newState.IsKeyDown(Keys.Right))
                        {
                            motion.Y += speed;
                            player.Position = motion;
                            player.State = Action.WalkEast;

                        }
                        else if (newState.IsKeyDown(Keys.Down) && newState.IsKeyDown(Keys.Left))
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

                    if (newState.IsKeyDown(Keys.Right))
                    {
                        motion.X += speed;
                        player.Position = motion;
                        player.State = Action.WalkEast;
                    }

                    if (newState.IsKeyDown(Keys.Left))
                    {
                        motion.X -= speed;
                        player.Position = motion;
                        player.State = Action.WalkWest;
                    }
                }
            }
        }
    }
}
