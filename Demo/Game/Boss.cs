using Demo.Scenes;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Shapes;
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
        int stunTimer = 0;
        AttackPattern attackPattern;
        AnimatedSprite pickaxe;
        Vector2 pickaxePosition;
        // Static field to be modified when player uses dynamite to stun the boss.
        public static bool Stunned = false;
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
            pickaxe = new AnimatedSprite(Sprites.pickaxeAnimation);
        }

        bool attackDecided = false;
        bool attackTypeDecided = false;
        bool attacking = false;
        public static bool bossEngaged = false;

        public new void Update(GameTime gameTime)
        {
            float distanceToBoss = Vector2.Distance(Init.Player.Position, Position);

            // Start the fight when close to the boss.
            if (distanceToBoss < 120)
            {
                bossEngaged = true;
                Level_1B.chainedGateSprite.Play("idle");
             //   IBox chainedGateCollidable = Init.Level_1BMap.GetWorld().Create(Level_1B.chainedGateSprite.Position.X - 10, Level_1B.chainedGateSprite.Position.Y, 32, 16);
            }

            pickaxe.Update(gameTime);

            if (!attacking)
            {
                pickaxePosition.X = Position.X + 12;
                pickaxePosition.Y = Position.Y + 4;
                pickaxe.Position = pickaxePosition;
            }

            if (attackDecided == false)
            {
                randomFrame = random.Next(50, 190);
                attackDecided = true;
            }

            // Randomly select an attack set.
            if (stepsLeft == randomFrame && frames < 20 && bossEngaged && !Dead)
            {
                if (attackTypeDecided == false)
                {
                    randomAttack = random.Next(1, 3);
                    if (randomAttack == 1)
                    {
                        pickaxePosition.Y = Position.Y + 10;
                        pickaxePosition.X = Position.X - 5;
                        pickaxe.Position = pickaxePosition;
                        attackPattern = AttackPattern.Attack1;
                        pickaxe.Play("Attack1");
                        DetectHit(pickaxe.BoundingRectangle);
                    }
                    else if (randomAttack == 2)
                    {
                        pickaxePosition.X = Position.X;
                        pickaxePosition.Y = Position.Y + 20;
                        pickaxe.Position = pickaxePosition;
                        attackPattern = AttackPattern.Attack2;
                        pickaxe.Play("Attack2");
                        DetectHit(pickaxe.BoundingRectangle);
                    }
                    attackTypeDecided = true;
                    attacking = true;
                }

                frames++;
            }
            else
            {
                attacking = false;
                attackPattern = AttackPattern.Walk;
                frames = 0;
                attackDecided = false;
                attackTypeDecided = false;
            }

            Vector2 newPosition = new Vector2(Position.X, Position.Y);

            // Switch attack types and move to the left and right.
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

                    if (Stunned == false && !Dead)
                    {
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
                    }

                    if (Stunned)
                    {
                        stunTimer++;
                    }

                    if (stunTimer >= 125)
                    {
                        stunTimer = 0;
                        Stunned = false;
                    }

                    break;
            }

            // Damage the boss
            if (Player.IsAttacking && Init.Player.BoundingBox.Intersects(BoundingBox))
            {
                CurrentHealth = CurrentHealth - 0.05;
            }

            if (CurrentHealth <= 0)
            {
                Dead = true;
            }

            sprite.Update(gameTime);
        }

        /// <summary>
        /// Damage the player if a hit is detected.
        /// </summary>
        /// <param name="boundingBox"></param>
        private void DetectHit(RectangleF boundingBox)
        {
            if (Init.Player.BoundingBox.Intersects(boundingBox))
            {
                Init.Player.CurrentHealth -= 25;
            }

        }

        int flickerTicker = 0;
        int flickerTimer = 0;
        public new void Draw(SpriteBatch spriteBatch)
        {
            // Play flicker effect when boss is defeated.
            if (flickerTimer < 200 && Dead)
            {
                if (flickerTicker < 10)
                {
                    sprite.Draw(spriteBatch);
                    flickerTicker++;
                }
                else
                {
                    flickerTicker--;
                }

                flickerTimer++;
            }
            else if (Dead)
            {
                flickerTicker = 120;
                flickerTicker = 10;
            }
            else
            {
                sprite.Draw(spriteBatch);
            }

            if (!Dead)
            {
                // Draw boss weapon and health bar
                pickaxe.Draw(spriteBatch);

                Vector2 bossHealthBarPosition = new Vector2(Init.Player.Position.X - 100, Init.Player.Position.Y + 90);

                if (CurrentHealth > 0 && bossEngaged)
                {
                    spriteBatch.DrawString(Init.Font, "The Prospector", new Vector2(bossHealthBarPosition.X, bossHealthBarPosition.Y - 10), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(statusBar, bossHealthBarPosition, new Rectangle(0, 0, Convert.ToInt32(MaxHealth) * 5, 10), Color.Black);
                    spriteBatch.Draw(healthBar, bossHealthBarPosition, new Rectangle(10, 10, Convert.ToInt32(CurrentHealth) * 5, 10), Color.White);
                }
            }

        }
    }
}
