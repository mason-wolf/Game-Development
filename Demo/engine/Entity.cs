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
        AttackSouth,
        WalkWest,
        AttackWest,
        IdleWest,
        WalkEast,
        AttackEast,
        IdleEast,
        WalkNorth,
        AttackNorth,
        IdleNorth,
        HurtSouth,
        HurtEast,
        HurtWest
    }

    public class Entity : IUpdate, IActorTarget
    {
        private readonly AnimatedSprite sprite;
   
        List<Vector2> path;

        private Action state;
        public RectangleF BoundingBox => sprite.BoundingRectangle;

        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
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
                        case Action.AttackSouth:
                            sprite.Play("attackSouth", () => State = Action.Idle);
                            break;
                        case Action.WalkWest:
                            sprite.Play("walkWest", () => State = Action.IdleWest);
                            break;
                        case Action.AttackWest:
                            sprite.Play("attackWest", () => State = Action.IdleWest);
                            break;
                        case Action.IdleWest:
                            sprite.Play("idleWest");
                            break;
                        case Action.WalkEast:
                            sprite.Play("walkEast", () => State = Action.IdleEast);
                            break;
                        case Action.AttackEast:
                            sprite.Play("attackEast", () => State = Action.IdleEast);
                            break;
                        case Action.IdleEast:
                            sprite.Play("idleEast");
                            break;
                        case Action.WalkNorth:
                            sprite.Play("walkNorth", () => State = Action.IdleNorth);
                            break;
                        case Action.AttackNorth:
                            sprite.Play("attackNorth", () => State = Action.IdleNorth);
                            break;
                        case Action.IdleNorth:
                            sprite.Play("idleNorth");
                            break;
                    }
                }
            }
        }

        public Vector2 Velocity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public int WayPointIndex;
        public bool ReachedDestination;

        public void MoveTo(GameTime gameTime, Entity unit, List<Vector2> DestinationWaypoint, float Speed)
        {
            if (DestinationWaypoint.Count > 0)
            {
                if (!ReachedDestination)
                {
                    float Distance = Vector2.Distance(unit.Position, DestinationWaypoint[WayPointIndex]);
                    Vector2 Direction = DestinationWaypoint[WayPointIndex] - unit.Position;
                    Direction.Normalize();

                    if (Distance > Direction.Length())
                        unit.Position += Direction * (float)(Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                    else
                    {
                        if (WayPointIndex >= DestinationWaypoint.Count - 1)
                        {
                            unit.Position += Direction;
                            ReachedDestination = true;
                            Console.WriteLine("true");
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

        public void OnCollision(CollisionInfo collisionInfo)
        {
            throw new NotImplementedException();
        }
    }
}