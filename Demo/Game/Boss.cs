using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Boss : Entity
    {
        int stepsLeft = 0;
        int stepsRight = 0;
        int frames = 0;
        Random random = new Random();
        int randomFrame = 0;
        int randomAttack = 0;
        AttackPattern attackPattern;

        enum AttackPattern
        {
            Walk,
            Attack1,
            Attack2
        }
        public Boss(SpriteSheetAnimationFactory bossSprite)
        {
            sprite = new AnimatedSprite(bossSprite);
            sprite.Play("walking");
        }

        bool attackDecided = false;
        bool attackTypeDecided = false;
        public new void Update(GameTime gameTime)
        {

            if (attackDecided == false)
            {
                randomFrame = random.Next(10, 190);
                attackDecided = true;
            }

            if (stepsLeft == randomFrame && frames < 30)
            {
                if (attackTypeDecided == false)
                {
                    randomAttack = random.Next(1, 3);
                    Console.WriteLine(randomAttack);
                    if (randomAttack == 1)
                    {
                        attackPattern = AttackPattern.Attack1;
                    }
                    else if (randomAttack == 2)
                    {
                        attackPattern = AttackPattern.Attack2;
                    }

                    attackTypeDecided = true;
                }

                frames++;
            }
            else
            {
                attackPattern = AttackPattern.Walk;
                frames = 0;
                attackDecided = false;
                attackTypeDecided = false;
            }
            
            Vector2 newPosition = new Vector2(Position.X, Position.Y);

            //if (stepsLeft == 30 && frames < 60)
            //{
            //    attackPattern = AttackPattern.Attack1;
            //    frames++;
            //}
            //else if (stepsRight == 180 && frames < 60)
            //{
            //    attackPattern = AttackPattern.Attack1;
            //    frames++;
            //}
            //else
            //{
            //    attackPattern = AttackPattern.Walk;
            //    frames = 0;
            //}

            switch (attackPattern)
            {
                case (AttackPattern.Attack1):
                    sprite.Play("Attack1");
                    break;
                case (AttackPattern.Attack2):
                    sprite.Play("Attack2");
                    break;
                case (AttackPattern.Walk):
                    sprite.Play("walking");
                    if (stepsLeft < 100)
                    {
                        newPosition.X -= 1f;
                        Position = newPosition;
                        stepsLeft++;
                    }
                    else
                    {
                        if (stepsRight < 200)
                        {
                            newPosition.X += 1f;
                            Position = newPosition;
                            stepsRight++;
                        }
                        else
                        {
                            stepsLeft = -100;
                            stepsRight = 0;
                        }
                    }
                    break;
            }
            sprite.Update(gameTime);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
