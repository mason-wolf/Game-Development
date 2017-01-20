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
        private float _direction = -1.0f;
        private Action _state;
        public RectangleF BoundingBox => sprite.BoundingRectangle;

        public bool IsOnGround { get; private set; }

        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }


        public Action State
        {
            get { return _state; }

             set
            {
                if (_state != value)
                {
                    _state = value;

                    switch (_state)
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
                        case Action.HurtSouth:
                            sprite.Play("hurtSouth");
                            break;
                        case Action.HurtWest:
                            sprite.Play("hurtWest");
                            break;
                        case Action.HurtEast:
                            sprite.Play("hurtEast");
                            break;
                    }
                }
            }
        }
        

        public Vector2 Velocity { get; set; }

        public Entity(SpriteSheetAnimationFactory animations)
        {
            sprite = new AnimatedSprite(animations);
            IsOnGround = false;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            IsOnGround = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite);
        } 

        public void Walk(float direction)
        {
            sprite.Effect = _direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _direction = direction;
            Velocity = new Vector2(200f * _direction, Velocity.Y);
        }

        public void OnCollision(CollisionInfo c)
        {
            Position -= c.PenetrationVector;
        }
    }
}