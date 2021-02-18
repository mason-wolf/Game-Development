using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Demo.Scenes;
using Humper;
using Humper.Responses;
using Demo.Engine;
using Microsoft.Xna.Framework.Audio;
using Demo.Interface;

namespace Demo
{
    public enum Action
    {
        IdleSouth1,
        IdleSouth2,
        WalkSouthPattern1,
        WalkSouthPattern2,
        AttackSouthPattern1,
        AttackSouthPattern2,
        WalkWestPattern1,
        WalkWestPattern2,
        AttackWestPattern1,
        AttackWestPattern2,
        IdleWest1,
        IdleWest2,
        WalkEastPattern1,
        WalkEastPattern2,
        AttackEastPattern1,
        AttackEastPattern2,
        IdleEast1,
        IdleEast2,
        WalkNorthPattern1,
        WalkNorthPattern2,
        AttackNorthPattern1,
        AttackNorthPattern2,
        IdleNorth1,
        IdleNorth2,
        Dead
    }

    /// <summary>
    /// Entity class for player, enemy or NPC.
    /// </summary>
    public class Entity : IUpdate, IActorTarget
    {
        public AnimatedSprite sprite;
 
        private Action state;
        public RectangleF BoundingBox => sprite.BoundingRectangle;

        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        // Create textures to display health.
        public Texture2D statusBar;
        public Texture2D healthBar;
        public Texture2D staminaBar;
        List<SoundEffect> soundEffects;
        public int ID { get; set; } = 0;
        public double MaxHealth { get; set; } = 0;
        public double CurrentHealth { get; set; } = 0;
        public double AttackDamage { get; set; } = 0;
        public bool Dead { get; set; } = false;
        public bool Aggroed { get; set; } = false;
        public string Name { get; set; }
        public void LoadContent(ContentManager content)
        {
            statusBar = content.Load<Texture2D>(@"interface\statusbar");
            healthBar = content.Load<Texture2D>(@"interface\healthbar");
            staminaBar = content.Load<Texture2D>(@"interface\staminabar");
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\dead-bat"));
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\dead-skeleton"));
        }

        // Create standard animation states for the entity.
        public Action State
        {
            get { return state; }

             set
            {
                if (state != value)
                {
                    state = value;

                    switch (state)
                    {
                        case Action.IdleSouth1:
                            sprite.Play("idleSouth1");
                            break;
                        case Action.IdleSouth2:
                            sprite.Play("idleSouth2");
                            break;
                        case Action.WalkSouthPattern1:
                            sprite.Play("walkSouthPattern1", () => State = Action.IdleSouth1);
                            break;
                        case Action.WalkSouthPattern2:
                            sprite.Play("walkSouthPattern2", () => State = Action.IdleSouth2);
                            break;
                        case Action.AttackSouthPattern1:
                            sprite.Play("attackSouthPattern1", () => State = Action.IdleSouth1);
                            break;
                        case Action.AttackSouthPattern2:
                            sprite.Play("attackSouthPattern2", () => State = Action.IdleSouth2);
                            break;
                        case Action.WalkWestPattern1:
                            sprite.Play("walkWestPattern1", () => State = Action.IdleWest1);
                            break;
                        case Action.WalkWestPattern2:
                            sprite.Play("walkWestPattern2", () => State = Action.IdleWest2);
                            break;
                        case Action.AttackWestPattern1:
                            sprite.Play("attackWestPattern1", () => State = Action.IdleWest1);
                            break;
                        case Action.AttackWestPattern2:
                            sprite.Play("attackWestPattern2", () => State = Action.IdleWest2);
                            break;
                        case Action.IdleWest1:
                            sprite.Play("idleWest1");
                            break;
                        case Action.IdleWest2:
                            sprite.Play("idleWest2");
                            break;
                        case Action.WalkEastPattern1:
                            sprite.Play("walkEastPattern1", () => State = Action.IdleEast1);
                            break;
                        case Action.WalkEastPattern2:
                            sprite.Play("walkEastPattern2", () => State = Action.IdleEast2);
                            break;
                        case Action.AttackEastPattern1:
                            sprite.Play("attackEastPattern1", () => State = Action.IdleEast1);
                            break;
                        case Action.AttackEastPattern2:
                            sprite.Play("attackEastPattern2", () => State = Action.IdleEast2);
                            break;
                        case Action.IdleEast1:
                            sprite.Play("idleEast1");
                            break;
                        case Action.IdleEast2:
                            sprite.Play("idleEast2");
                            break;
                        case Action.WalkNorthPattern1:
                            sprite.Play("walkNorthPattern1", () => State = Action.IdleNorth1);
                            break;
                        case Action.WalkNorthPattern2:
                            sprite.Play("walkNorthPattern2", () => State = Action.IdleNorth2);
                            break;
                        case Action.AttackNorthPattern1:
                            sprite.Play("attackNorthPattern1", () => State = Action.IdleNorth1);
                            break;
                        case Action.AttackNorthPattern2:
                            sprite.Play("attackNorthPattern2", () => State = Action.IdleNorth2);
                            break;
                        case Action.IdleNorth1:
                            sprite.Play("idleNorth1");
                            break;
                        case Action.IdleNorth2:
                            sprite.Play("idleNorth2");
                            break;
                        case Action.Dead:
                            sprite.Play("dead");
                            break;
                    }
                }
            }
        }

        public Vector2 Velocity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  
        public Entity()
        {
        }

        public Entity(SpriteSheetAnimationFactory animations)
        {
            sprite = new AnimatedSprite(animations);
        }

        public int WayPointIndex;
        public bool ReachedDestination;

        // Method to make an entity follow a path of waypoints.
        public void FollowPath(GameTime gameTime, Entity entity, List<Vector2> DestinationWaypoint, float Speed)
        {
            if (DestinationWaypoint.Count > 0)
            {
                if (!ReachedDestination)
                {
                    Vector2 Direction = DestinationWaypoint[WayPointIndex] - entity.Position;
                    Direction.Normalize();
                    Double angle = Math.Atan2(Direction.X, Direction.Y);
                    double rotation = (float)(angle * (180 / Math.PI));

                    if (rotation < -179 || rotation == 180)
                    {
                        entity.State = Action.WalkNorthPattern1;
                    }
                    else if (rotation >= 89 && rotation < 180)
                    {
                        entity.State = Action.WalkEastPattern1;
                    }
                    else if (rotation <= -90 && rotation > -179)
                    {

                        entity.State = Action.WalkWestPattern1;
                    }
                    else if (rotation >= 0 && rotation <= 1)
                    {
                        entity.State = Action.WalkSouthPattern1;
                    }

                    if (rotation < -0 && rotation > -90)
                    {
                        entity.State = Action.WalkSouthPattern1;
                    }

                    float Distance = Vector2.Distance(entity.Position, DestinationWaypoint[WayPointIndex]);

                    if (Distance > Direction.Length())
                        entity.Position += Direction * (float)(Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                    else
                    {
                        if (WayPointIndex >= DestinationWaypoint.Count - 1)
                        {
                            entity.Position += Direction;
                            ReachedDestination = true;
                        }
                            if(WayPointIndex < 3)
                            WayPointIndex++;
                    }
                }
            }
        }


        public void Attack(Entity target)
        {

            Vector2 currentPosition = Position;

            float distance = Vector2.Distance(Position, target.Position);

            if (distance < 20 && CurrentHealth > 0)
            {

                Vector2 destination = Position - target.Position;
                destination.Normalize();
                Double angle = Math.Atan2(destination.X, destination.Y);
                double direction = Math.Ceiling(angle);

                if (direction == -3 || direction == 4 || direction == -2)
                {
                    State = Action.AttackSouthPattern1;
                    target.CurrentHealth -= AttackDamage;
                }

                if (direction == -1)
                {
                    State = Action.AttackEastPattern1;
                    target.CurrentHealth -= AttackDamage;
                }

                if (direction == 0 || direction == 1)
                {
                    State = Action.AttackNorthPattern1;
                    target.CurrentHealth -= AttackDamage;
                }

                if (direction == 2 || direction == 3)
                {
                    State = Action.AttackWestPattern1;
                    target.CurrentHealth -= AttackDamage;
                }
            }
        }

        Entity projectile;
        Vector2 projectilePosition;
        Vector2 projectileStartingPosition;
        bool targetHit = false;
        string direction = "";
        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="sprite">AnimatedSprite of the projectile.</param>
        /// <param name="direction">Direction (North, South, East, West)</param>
        public void ShootProjectile(AnimatedSprite sprite, string direction)
        {
            projectile = new Entity();
            projectile.sprite = sprite;
            projectilePosition = new Vector2(Init.Player.Position.X, Init.Player.Position.Y - 5);
            projectile.Position = projectilePosition;
            projectileStartingPosition = projectilePosition;
            projectile.MaxHealth = 10;
            projectile.CurrentHealth = 10;
            this.direction = direction;
            targetHit = false;
        }

        /// <summary>
        /// Checks to see if the projectile intersects any of the collision tiles on the current map.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        RectangleF projectileBoundingBox;
        bool ProjectileCollision(Entity projectile)
        {
            bool collided = false;
            Vector2 offsetPosition = new Vector2(projectilePosition.X + 5, projectilePosition.Y + 5);
            projectileBoundingBox = new RectangleF(offsetPosition, new SizeF(2, 2));
            foreach (Tile tile in Init.SelectedMap.GetCollisionTiles())
            {
                if (projectileBoundingBox.Intersects(tile.Rectangle))
                {
                    collided = true;
                }
            }
            return collided;
        }
        public void Update(GameTime gameTime)
        {
            // If enemy dies
           if (CurrentHealth <= 0 && Dead == false)
            {
                State = Action.Dead;
                Dead = true;

                switch(Name)
                {
                    case ("Bat"):
                        soundEffects[0].Play();
                        break;
                    case ("Skeleton"):
                        soundEffects[1].Play();
                        break;
                }
            }

            if (projectile != null && !ProjectileCollision(projectile))
            {
                int speed = 7;

                if (direction == "north")
                {
                    projectile.State = Action.IdleNorth1;
                    projectilePosition.Y -= speed;
                    projectile.Position = projectilePosition;
                }

                if (direction == "south")
                {
                    projectile.State = Action.AttackSouthPattern1;
                    projectilePosition.Y += speed;
                    projectile.Position = projectilePosition;
                }

                if (direction == "east")
                {
                    ProjectileCollision(projectile);
                    projectile.State = Action.IdleEast1;
                    projectilePosition.X += speed;
                    projectile.Position = projectilePosition;
                }

                if (direction == "west")
                {
                    projectile.State = Action.IdleWest1;
                    projectilePosition.X -= speed;
                    projectile.Position = projectilePosition;
                }

                foreach (Entity entity in Init.Player.EnemyList)
                {
                    if (projectile.BoundingBox.Intersects(entity.BoundingBox) && targetHit == false && entity.state != Action.Dead)
                    {
                        entity.CurrentHealth -= 4;
                        entity.Aggroed = true;
                        targetHit = true;
                    }
                }

                projectile.Update(gameTime);
            }

            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite);

            if (projectile != null && targetHit == false)
            {
                spriteBatch.Draw(projectile.sprite);
            }
        }

        /// <summary>
        /// Draws HUD depending on entity type.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="position">Entity's position</param>
        /// <param name="isPlayer">Flag for player</param>

        public void DrawHUD(SpriteBatch spriteBatch, Vector2 position, bool isPlayer)
        {
            if (isPlayer)
            {
                if (CurrentHealth > 0)
                {
                    spriteBatch.Draw(statusBar, position, new Rectangle(0, 0, Convert.ToInt32(MaxHealth), 9), Color.Black);
                    spriteBatch.Draw(healthBar, position, new Rectangle(10, 10, Convert.ToInt32(CurrentHealth), 9), Color.White);
                }
            }
            else
            {
                if (CurrentHealth > 0)
                {
                    spriteBatch.Draw(statusBar, position, new Rectangle(0, 0, Convert.ToInt32(MaxHealth), 2), Color.Black);
                    spriteBatch.Draw(staminaBar, position, new Rectangle(10, 10, Convert.ToInt32(CurrentHealth), 2), Color.White);
                }
            }
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            throw new NotImplementedException();
        }
    }
}