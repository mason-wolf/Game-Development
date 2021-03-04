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
using Demo.Interface;
using Demo.Engine;

namespace Demo
{
    public class Player : Entity
    {
        public Texture2D playerTexture;
        public TextureAtlas playerAtlas;
        public SpriteSheetAnimationFactory playerAnimation;
        MouseState oldMouseState;
        MouseState newMouseState;
        Entity playerEntity;

        public List<Entity> EnemyList { get; set; }
        public static List<Item> InventoryList = new List<Item>();
        public static bool PressedContinue = false;
        public static bool IsAttacking = false;
        public static bool ActionButtonPressed = false;
        public static Vector2 MotionVector;
        public static string CurrentLevel { get; set; }
        private string EquipedWeapon = "Sword";
        AnimatedSprite arrow;
        public bool InMenu = false;
        // Store currently running scene to revert back after exiting escape menu.
        Init.Scene currentScene = Init.SelectedScene;
        Random random = new Random();
        List<SoundEffect> soundEffects;

        public new void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>(@"spritesheets\Player");
            playerAtlas = TextureAtlas.Create(playerTexture, 32, 32);
            playerAnimation = new SpriteSheetAnimationFactory(playerAtlas);
            Sprites sprites = new Sprites();
            sprites.LoadContent(content);
            arrow = new AnimatedSprite(Sprites.arrowAnimation);
            float animationSpeed = .09f;
            float attackSpeed = 0.03f;
            playerAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 0 }));
            playerAnimation.Add("idleSouth2", new SpriteSheetAnimationData(new[] { 9 }));
            playerAnimation.Add("walkSouthPattern1", new SpriteSheetAnimationData(new[] { 1, 2 }, animationSpeed, isLooping: true));
            playerAnimation.Add("walkSouthPattern2", new SpriteSheetAnimationData(new[] { 9, 10, 11, 10 }, animationSpeed, isLooping: true));
            playerAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 3, 4, 5, 6, 7, 8, 7, 6, 5 }, attackSpeed, isLooping: true));
            playerAnimation.Add("attackSouthPattern2", new SpriteSheetAnimationData(new[] { 21, 22, 23 }, .1f, isLooping: true));
            playerAnimation.Add("walkWestPattern1", new SpriteSheetAnimationData(new[] { 12, 13, 12, 14 }, animationSpeed, isLooping: true));
            playerAnimation.Add("walkWestPattern2", new SpriteSheetAnimationData(new[] { 33, 34, 33, 35 }, animationSpeed, isLooping: true));
            playerAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 15, 16, 17, 18, 19, 20, 19, 18, 17 }, attackSpeed, isLooping: true));
            playerAnimation.Add("attackWestPattern2", new SpriteSheetAnimationData(new[] { 45, 46, 47}, .1f, isLooping: true));
            playerAnimation.Add("idleWest1", new SpriteSheetAnimationData(new[] { 12 }));
            playerAnimation.Add("idleWest2", new SpriteSheetAnimationData(new[] { 33 }));
            playerAnimation.Add("walkEastPattern1", new SpriteSheetAnimationData(new[] { 26, 25, 26, 24 }, animationSpeed, isLooping: true));
            playerAnimation.Add("walkEastPattern2", new SpriteSheetAnimationData(new[] { 59, 57, 59, 58 }, animationSpeed, isLooping: true));
            playerAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 27, 28, 29, 30, 31, 32, 30, 29, 28 }, attackSpeed, isLooping: true));
            playerAnimation.Add("attackEastPattern2", new SpriteSheetAnimationData(new[] { 69, 70, 71 }, .1f, isLooping: true));
            playerAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 26 }));
            playerAnimation.Add("idleEast2", new SpriteSheetAnimationData(new[] { 59 }));
            playerAnimation.Add("walkNorthPattern1", new SpriteSheetAnimationData(new[] { 36, 38 }, animationSpeed, isLooping: true));
            playerAnimation.Add("walkNorthPattern2", new SpriteSheetAnimationData(new[] { 81, 83 }, .1f, isLooping: true));
            playerAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 39, 40, 41, 42, 43, 44, 43, 42, 41 }, attackSpeed, isLooping: true));
            playerAnimation.Add("attackNorthPattern2", new SpriteSheetAnimationData(new[] { 93, 94, 95}, .1f, isLooping: true));
            playerAnimation.Add("idleNorth1", new SpriteSheetAnimationData(new[] { 37 }));
            playerAnimation.Add("idleNorth2", new SpriteSheetAnimationData(new[] { 82 }));
            playerAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 48, 49, 50 }, 0.08f, isLooping: false));
            statusBar = content.Load<Texture2D>(@"interface\statusbar");
            healthBar = content.Load<Texture2D>(@"interface\healthbar");
            staminaBar = content.Load<Texture2D>(@"interface\staminabar");

            soundEffects = new List<SoundEffect>();
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\sword-swing"));
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\bow-shoot"));
        }

        /// <summary>
        /// Calculate the direction in which a target is hit.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public double GetHitDirection(Entity entity)
        {
            Vector2 destination = Position - entity.Position;
            destination.Normalize();
            Double angle = Math.Atan2(destination.X, destination.Y);
            double direction = Math.Ceiling(angle);
            return direction;
        }

        public bool IntersectsCollidable(Entity entity)
        {
            bool intersected = false;
            foreach (Tile tile in Init.SelectedMap.GetCollisionTiles())
            {
                if (entity.BoundingBox.Intersects(tile.Rectangle))
                {
                    intersected = true;
                }
            }

            return intersected;
        }
        // Loop through list of enemies and do damage if close.

        public void Attack()
        {
            if (EnemyList != null)
            {
                foreach (Entity enemy in EnemyList)
                {
                    if (playerEntity.BoundingBox.Intersects(enemy.BoundingBox) && enemy.State != Action.Dead)
                    {
                        double directionHit = GetHitDirection(enemy);
                        int distance = 20;

                        enemy.CurrentHealth -= AttackDamage;

                        // Hitting enemy from the north 
                        if (directionHit == -3 || directionHit == 4 || directionHit == -2)
                        {
                            if (!IntersectsCollidable(enemy))
                            {
                                enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + distance);
                            }
                        }

                        // Hitting enemy from the west
                        if (directionHit == -1)
                        {
                            if (!IntersectsCollidable(enemy))
                            {
                                enemy.Position = new Vector2(enemy.Position.X + distance, enemy.Position.Y);
                            }
                        }

                        // Hitting enemy from the east
                        if (directionHit == 2)
                        {
                            if (!IntersectsCollidable(enemy))
                            {
                                enemy.Position = new Vector2(enemy.Position.X - distance, enemy.Position.Y);
                            }
                        }

                        // Hitting enemy from the south
                        if (directionHit == 1)
                        {
                            if (!IntersectsCollidable(enemy))
                            {
                                enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y - distance);
                            }
                            else
                            {
                                enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + 10);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Method to check status of the player. Death, status effects, etc.
        /// </summary>
        /// <param name="gameTime"></param>
        public void CheckPlayerStatus(GameTime gameTime)
        {
            if (CurrentHealth <= 0)
            {
                Dead = true;
                sprite.Play("dead");
            }
        }

        // Handle attacking and movement animations.
        public void HandleInput(GameTime gameTime, Entity player, IBox playerCollisionBox, KeyboardState newState, KeyboardState oldState)
        {
            playerEntity = player;
            MotionVector = new Vector2(playerCollisionBox.X, playerCollisionBox.Y);

            float speed = 0;
            ActionButtonPressed = false;

            // If action button 'E' is pressed.
            if (newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                ActionButtonPressed = true;
            }

            // If run button 'Shift' is held down.
            if (newState.IsKeyDown(Keys.LeftShift) && CurrentStamina >= 0)
            {
                speed = 2.2f;
                CurrentStamina -= .4;
            }
            else if (newState.IsKeyDown(Keys.LeftShift) && currentScene <= 0)
            {
                speed = 1.4f; 
            }
            else
            {
                speed = 1.4f;
                if (CurrentStamina <= MaxStamina)
                {
                    CurrentStamina += 0.5;
                }
            }

            newMouseState = Mouse.GetState();

            // Handle escape menu.
            if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape) || PressedContinue == true)
            {
                // Exit the menu if Escape is pressed or if player pressed continue.
                if (InMenu)
                {
                    Init.SelectedScene = currentScene;
                    InMenu = false;
                    PressedContinue = false;
                }
                else
                {
                    // Open the menu if escape is pressed.
                    currentScene = Init.SelectedScene;
                    // Store the current level to save progress later.
                    CurrentLevel = currentScene.ToString();
                    Init.SelectedScene = Init.Scene.EscapeMenu;

                    InMenu = true;
                    SaveMenu.GameSaved = false;
                }
            }

            // Handle inventory
            if (newState.IsKeyDown(Keys.I) && oldState.IsKeyUp(Keys.I))
            {
                if (Inventory.InventoryOpen)
                {
                    Inventory.InventoryOpen = false;
                }
                else
                {
                    Inventory.InventoryOpen = true;
                }
            }

            // Switch weapons when pressing 1 or 2.
            if (newState.IsKeyDown(Keys.D1) && oldState.IsKeyDown(Keys.D1))
            {
                EquipedWeapon = "Sword";

                switch (State)
                {
                    case (Action.IdleEast2):
                        State = Action.IdleEast1;
                        break;
                    case (Action.IdleWest2):
                        State = Action.IdleWest1;
                        break;
                    case (Action.IdleNorth2):
                        State = Action.IdleNorth1;
                        break;
                    case (Action.IdleSouth2):
                        State = Action.IdleSouth1;
                        break;
                }
            }

            // Handle animations when bow is equipped.
            if (newState.IsKeyDown(Keys.D2) && oldState.IsKeyDown(Keys.D2))
            {
                EquipedWeapon = "Bow";

                switch (State)
                {
                    case (Action.IdleEast1):
                        State = Action.IdleEast2;
                        break;
                    case (Action.IdleWest1):
                        State = Action.IdleWest2;
                        break;
                    case (Action.IdleNorth1):
                        State = Action.IdleNorth2;
                        break;
                    case (Action.IdleSouth1):
                        State = Action.IdleSouth2;
                        break;
                }
            }

            if (!InMenu && !Inventory.InventoryOpen)
            {
                // Attacking south
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkSouthPattern1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleSouth1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleSouth2 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkSouthPattern2)
                {
                    if (EquipedWeapon == "Sword")
                    {
                        player.State = Action.AttackSouthPattern1;
                        soundEffects[0].Play();
                    }
                    else if (EquipedWeapon == "Bow")
                    {
                        if (Inventory.TotalArrows > 0)
                        {
                            player.State = Action.AttackSouthPattern2;
                            ShootProjectile(arrow, "south");
                            Inventory.TotalArrows -= 1;
                            soundEffects[1].Play();
                        }
                    }
                    Attack();
                }

                // Attacking West
                else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkWestPattern1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleWest1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleWest2 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkWestPattern2)
                {
                    if (EquipedWeapon == "Sword")
                    {
                        player.State = Action.AttackWestPattern1;
                        soundEffects[0].Play();
                    }
                    else if (EquipedWeapon == "Bow")
                    {
                        if (Inventory.TotalArrows > 0)
                        {
                            player.State = Action.AttackWestPattern2;
                            ShootProjectile(arrow, "west");
                            Inventory.TotalArrows -= 1;
                            soundEffects[1].Play();
                        }
                    }
                    Attack();
                }

                // Attacking East
                else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkEastPattern1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleEast1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkEastPattern2 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleEast2)
                {
                    if (EquipedWeapon == "Sword")
                    {
                        player.State = Action.AttackEastPattern1;
                        soundEffects[0].Play();
                    }
                    else if (EquipedWeapon == "Bow")
                    {
                        if (Inventory.TotalArrows > 0)
                        {
                            player.State = Action.AttackEastPattern2;
                            ShootProjectile(arrow, "east");
                            Inventory.TotalArrows -= 1;
                            soundEffects[1].Play();
                        }
                    }
                    Attack();
                }

                // Attacking North
                else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkNorthPattern1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleNorth1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkNorthPattern2 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleNorth2)
                {
                    if (EquipedWeapon == "Sword")
                    {
                        player.State = Action.AttackNorthPattern1;
                        soundEffects[0].Play();
                    }
                    else if (EquipedWeapon == "Bow")
                    {
                        if (Inventory.TotalArrows > 0)
                        {
                            player.State = Action.AttackNorthPattern2;
                            ShootProjectile(arrow, "north");
                            Inventory.TotalArrows -= 1;
                            soundEffects[1].Play();
                        }
                    }
                        Attack();
                }
                else
                {
                    if (newState.IsKeyDown(Keys.W) && player.State != Action.AttackNorthPattern1 && player.State != Action.AttackNorthPattern2)
                    {
                        // Walk east if W and D are pressed
                        if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.D))
                        {
                            MotionVector.Y -= speed;
                            player.Position = MotionVector;

                            if (EquipedWeapon == "Sword")
                            {
                                player.State = Action.WalkEastPattern1;
                            }
                            else if (EquipedWeapon == "Bow")
                            {
                                player.State = Action.WalkEastPattern2;
                            }
                        }
                        // Walk west if W and A are pressed.
                        else if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.A))
                        {
                            MotionVector.Y -= speed;
                            player.Position = MotionVector;

                            if (EquipedWeapon == "Sword")
                            {
                                player.State = Action.WalkWestPattern1;
                            }
                            else if (EquipedWeapon == "Bow")
                            {
                                player.State = Action.WalkWestPattern2;
                            }
                        }
                        else
                        {
                            // Walk north.
                            MotionVector.Y -= speed;
                            player.Position = MotionVector;
                            if (EquipedWeapon == "Sword")
                            {
                                player.State = Action.WalkNorthPattern1;
                            }
                            else if (EquipedWeapon == "Bow")
                            {
                                player.State = Action.WalkNorthPattern2;
                            }
                        }
                    }

                    if (newState.IsKeyDown(Keys.S) && player.State != Action.AttackSouthPattern1 && player.State != Action.AttackSouthPattern2)
                    {
                        // Walk east if S and D are pressed.
                        if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.D))
                        {
                            MotionVector.Y += speed;
                            player.Position = MotionVector;

                            if (EquipedWeapon == "Sword")
                            {
                                player.State = Action.WalkEastPattern1;
                            }
                            else if (EquipedWeapon == "Bow")
                            {
                                player.State = Action.WalkEastPattern2;
                            }

                        }

                        // Walk west if S and A are pressed.
                        else if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.A))
                        {
                            MotionVector.Y += speed;
                            player.Position = MotionVector;

                            if (EquipedWeapon == "Sword")
                            {
                                player.State = Action.WalkWestPattern1;
                            }
                            else if (EquipedWeapon == "Bow")
                            {
                                player.State = Action.WalkWestPattern2;
                            }
                        }
                        else
                        {
                            // Walk south
                            MotionVector.Y += speed;
                            player.Position = MotionVector;

                            if (EquipedWeapon == "Sword")
                            {
                                player.State = Action.WalkSouthPattern1;
                            }
                            else if (EquipedWeapon == "Bow")
                            {
                                player.State = Action.WalkSouthPattern2;
                            }
                        }
                    }

                    // Walk east
                    if (newState.IsKeyDown(Keys.D) && player.State != Action.AttackEastPattern1 && player.State != Action.AttackEastPattern2)
                    {
                        MotionVector.X += speed;
                        player.Position = MotionVector;

                        if (EquipedWeapon == "Sword")
                        {
                            player.State = Action.WalkEastPattern1;
                        }
                        else if (EquipedWeapon == "Bow")
                        {
                            player.State = Action.WalkEastPattern2;
                        }
                    }

                    // Walk west
                    if (newState.IsKeyDown(Keys.A) && player.State != Action.AttackWestPattern1 && player.State != Action.AttackWestPattern2)
                    {
                        MotionVector.X -= speed;
                        player.Position = MotionVector;

                        if (EquipedWeapon == "Sword")
                        {
                            player.State = Action.WalkWestPattern1;
                        }
                        else if (EquipedWeapon == "Bow")
                        {
                            player.State = Action.WalkWestPattern2;
                        }
                    }
                }

                switch(player.State)
                {
                    case (Action.AttackEastPattern1):
                        IsAttacking = true;
                        break;
                    case (Action.AttackEastPattern2):
                        IsAttacking = true;
                        break;
                    case (Action.AttackNorthPattern1):
                        IsAttacking = true;
                        break;
                    case (Action.AttackNorthPattern2):
                        IsAttacking = true;
                        break;
                    case (Action.AttackSouthPattern1):
                        IsAttacking = true;
                        break;
                    case (Action.AttackSouthPattern2):
                        IsAttacking = true;
                        break;
                    case (Action.AttackWestPattern1):
                        IsAttacking = true;
                        break;
                    case (Action.AttackWestPattern2):
                        IsAttacking = true;
                        break;
                    default:
                        IsAttacking = false;
                        break;
                }
            }
            oldMouseState = newMouseState;
        }
    }
}
