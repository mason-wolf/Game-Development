using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demo.Scenes;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Demo
{

    public class EnemyAI 
    {
        float player_wolf_distance;
        public static Vector2 direction;
        public static float attackSpeed = .4F;
        public static double angle;
        public static double enemyDamage = 2;

        public EnemyAI(Entity player, Entity enemy)
        {
            player_wolf_distance = Vector2.Distance(player.Position, enemy.Position);
            if (player_wolf_distance < 150 && player.State == Action.AttackEast || player.State == Action.AttackWest 
                || player.State == Action.AttackNorth || player.State == Action.AttackSouth)
            {
                if(enemy.State == Action.AttackSouth || enemy.State == Action.WalkSouth)
                {
                    enemy.State = Action.HurtSouth;
                }

                if(enemy.State == Action.AttackEast || enemy.State == Action.WalkEast)
                {
                    enemy.State = Action.HurtEast;
                }

                if (enemy.State == Action.AttackWest || enemy.State == Action.WalkWest)
                {
                    enemy.State = Action.HurtWest;
                }
            }
        }
     
        public void Attack(GameTime gameTime, Entity player, Entity enemy, Player playerStats)
        {
            direction = player.Position - enemy.Position;
            angle = Math.Atan2(direction.X, direction.Y);
            if (player_wolf_distance > 20)
            {
                direction.Normalize();
                enemy.Position += direction * attackSpeed;

                if (angle < 0 && angle > -2)
                {
                    enemy.State = Action.WalkWest;
                }

                if (angle > 0 && angle < 1)
                {
                    enemy.State = Action.WalkSouth;
                }

                if (angle > 1 && angle < 2)
                {
                    enemy.State = Action.WalkEast;
                }

                if (angle > 3 || angle < -3)
                {
                    enemy.State = Action.WalkNorth;
                }
            }
            else
            {

                direction = player.Position - enemy.Position;
                direction.Normalize();

                if (Village.enemyHitCount > 70)
                {
                    if (angle < 0 && angle > -2)
                    {
                        enemy.State = Action.AttackWest;
                        playerStats.Health -= enemyDamage;
                    }

                    if (angle > 0 && angle < 1)
                    {
                        enemy.State = Action.AttackSouth;
                        playerStats.Health -= enemyDamage;
                    }

                    if (angle > 1 && angle < 2)
                    {
                        enemy.State = Action.AttackEast;
                        playerStats.Health -= enemyDamage;
                    }

                    if (angle > 3 || angle < -3)
                    {
                        enemy.State = Action.AttackEast;
                        playerStats.Health -= enemyDamage;
                    }

                    if (angle == 0)
                    {
                        enemy.State = Action.AttackSouth;
                        playerStats.Health -= enemyDamage;
                    }

                    Village.enemyHitCount = 0;
                }
            }
        }

            public void RandomMovement(GameTime gameTime, Entity enemy) {

            Random random = new Random();
            if (Village.enemyMovementTimer > 50)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        enemy.State = Action.WalkSouth;
                        break;
                    case 1:
                        enemy.State = Action.WalkEast;
                        break;
                    case 2:
                        enemy.State = Action.WalkWest;
                        break;
                    case 3:
                        enemy.State = Action.WalkNorth;
                        break;
                }
                Village.enemyMovementTimer = 0;
            }

                if (enemy.State == Action.WalkSouth)
                {
                    Vector2 motion = new Vector2();
                    motion = enemy.Position;
                    motion.Y++;
                    enemy.Position = motion;
                }
                if (enemy.State == Action.WalkEast)
                {
                    Vector2 motion = new Vector2();
                    motion = enemy.Position;
                    motion.X++;
                    enemy.Position = motion;
                }
                if (enemy.State == Action.WalkWest)
                {
                    Vector2 motion = new Vector2();
                    motion = enemy.Position;
                    motion.X--;
                    enemy.Position = motion;
                }
                if (enemy.State == Action.WalkNorth)
                {
                    Vector2 motion = new Vector2();
                    motion = enemy.Position;
                    motion.Y--;
                    enemy.Position = motion;
                }
        }
    }
}

