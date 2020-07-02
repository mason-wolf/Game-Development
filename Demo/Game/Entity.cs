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

namespace Demo
{
    public enum Action
    {
        Idle,
        WalkSouth,
        AttackSouthPattern1,
        AttackSouthPattern2,
        WalkWest,
        AttackWestPattern1,
        AttackWestPattern2,
        IdleWest,
        WalkEast,
        AttackEastPattern1,
        AttackEastPattern2,
        IdleEast,
        WalkNorth,
        AttackNorthPattern1,
        AttackNorthPattern2,
        IdleNorth,
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

        public int ID { get; set; } = 0;
        public double MaxHealth { get; set; } = 0;
        public double CurrentHealth { get; set; } = 0;
        public double AttackDamage { get; set; } = 0;
        public bool Dead { get; set; } = false;
 

        public void LoadContent(ContentManager content)
        {
            statusBar = content.Load<Texture2D>(@"interface\statusbar");
            healthBar = content.Load<Texture2D>(@"interface\healthbar");
            staminaBar = content.Load<Texture2D>(@"interface\staminabar");
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
                        case Action.Idle:
                            sprite.Play("idle");
                            break;
                        case Action.WalkSouth:
                            sprite.Play("walkSouth", () => State = Action.Idle);
                            break;
                        case Action.AttackSouthPattern1:
                            sprite.Play("attackSouthPattern1", () => State = Action.Idle);
                            break;
                        case Action.AttackSouthPattern2:
                            sprite.Play("attackSouthPattern2", () => State = Action.Idle);
                            break;
                        case Action.WalkWest:
                            sprite.Play("walkWest", () => State = Action.IdleWest);
                            break;
                        case Action.AttackWestPattern1:
                            sprite.Play("attackWestPattern1", () => State = Action.IdleWest);
                            break;
                        case Action.AttackWestPattern2:
                            sprite.Play("attackWestPattern2", () => State = Action.IdleWest);
                            break;
                        case Action.IdleWest:
                            sprite.Play("idleWest");
                            break;
                        case Action.WalkEast:
                            sprite.Play("walkEast", () => State = Action.IdleEast);
                            break;
                        case Action.AttackEastPattern1:
                            sprite.Play("attackEastPattern1", () => State = Action.IdleEast);
                            break;
                        case Action.AttackEastPattern2:
                            sprite.Play("attackEastPattern2", () => State = Action.IdleEast);
                            break;
                        case Action.IdleEast:
                            sprite.Play("idleEast");
                            break;
                        case Action.WalkNorth:
                            sprite.Play("walkNorth", () => State = Action.IdleNorth);
                            break;
                        case Action.AttackNorthPattern1:
                            sprite.Play("attackNorthPattern1", () => State = Action.IdleNorth);
                            break;
                        case Action.AttackNorthPattern2:
                            sprite.Play("attackNorthPattern2", () => State = Action.IdleNorth);
                            break;
                        case Action.IdleNorth:
                            sprite.Play("idleNorth");
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
                        entity.State = Action.WalkNorth;
                    }
                    else if (rotation >= 89 && rotation < 180)
                    {
                        entity.State = Action.WalkEast;
                    }
                    else if (rotation <= -90 && rotation > -179)
                    {

                        entity.State = Action.WalkWest;
                    }
                    else if (rotation >= 0 && rotation <= 1)
                    {
                        entity.State = Action.WalkSouth;
                    }

                    if (rotation < -0 && rotation > -90)
                    {
                        entity.State = Action.WalkSouth;
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
        public void Update(GameTime gameTime)
        {
           if (CurrentHealth <= 0)
            {
                State = Action.Dead;
                Dead = true;
            }

            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite);
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