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
        Die
    }

    public class Entity : IUpdate, IActorTarget
    {
        private readonly AnimatedSprite sprite;
 
        private Action state;
        public RectangleF BoundingBox => sprite.BoundingRectangle;

        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        Texture2D statusBar;
        Texture2D healthBar;
        Texture2D staminaBar;

        public double MaxHealth { get; set; } = 0;
        public double CurrentHealth { get; set; } = 0;
        public double AttackDamage { get; set; } = 0;

        public void LoadContent(ContentManager content)
        {
            statusBar = content.Load<Texture2D>(@"interface\statusbar");
            healthBar = content.Load<Texture2D>(@"interface\healthbar");
            staminaBar = content.Load<Texture2D>(@"interface\staminabar");
        }

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
                        case Action.Die:
                            sprite.Play("die");
                            break;
                    }
                }
            }
        }

        public Vector2 Velocity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public int WayPointIndex;
        public bool ReachedDestination;

        /// <summary>
        /// Moves an entity through a list of vectors.
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="entity">Entity to move.</param>
        /// <param name="DestinationWaypoint">List of vectors to move through (destination).</param>
        /// <param name="Speed">Speed of movement.</param>

        public void MoveTo(GameTime gameTime, Entity entity, List<Vector2> DestinationWaypoint, float Speed)
        {
            if (DestinationWaypoint.Count > 0)
            {
                if (!ReachedDestination)
                {
                    float Distance = Vector2.Distance(entity.Position, DestinationWaypoint[WayPointIndex]);
                    Vector2 Direction = DestinationWaypoint[WayPointIndex] - entity.Position;
                    Direction.Normalize();
                    Double angle = Math.Atan2(Direction.X, Direction.Y);
                    double rotation = (float)(angle * (180 / Math.PI));

                
                    if (rotation < -179 || rotation == 180)
                    {
                        entity.State = Action.WalkNorth;
                    }

                    if (rotation >= 90 && rotation < 180)
                    {
                        entity.State = Action.WalkEast;
                    }

                    if (rotation <= -90 && rotation > -179)
                    {
                        entity.State = Action.WalkWest;
                    }

                    if (rotation == 0)
                    {
                        entity.State = Action.WalkSouth;
                    }

                    if (rotation < -0 && rotation > -90)
                    {
                        entity.State = Action.WalkSouth;
                    }

                    if (Distance > Direction.Length())
                        entity.Position += Direction * (float)(Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                    else
                    {
                        if (WayPointIndex >= DestinationWaypoint.Count - 1)
                        {
                            entity.Position += Direction;
                            ReachedDestination = true;
                        }
                        else
                            WayPointIndex++;
                    }
                }
            }
        }

        public Entity(SpriteSheetAnimationFactory animations)
        {
            sprite = new AnimatedSprite(animations);
        }

        public void Update(GameTime gameTime)
        {
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