
using Demo.Engine;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Scenes
{
    public class Level_1 : Scene
    {
        public static Skeleton skeletonEnemy;
        public static Bat batEnemy;
        public static FireBat fireBatEnemy;
        public static EnemyAI enemyAI;
        public static List<Entity> enemyList = new List<Entity>();
        public RoyT.AStar.Grid grid;
        Player player = StartArea.player;

        public override void LoadContent(ContentManager content)
        {
            skeletonEnemy = new Skeleton();
            batEnemy = new Bat();
            fireBatEnemy = new FireBat();
            skeletonEnemy.LoadContent(content);
            batEnemy.LoadContent(content);
            fireBatEnemy.LoadContent(content);

            // Create skeleton enemies
            for (int i = 0; i < 11; i++)
            {
                Entity skeletonEntity = new Entity(skeletonEnemy.Animation);
                skeletonEntity.LoadContent(content);
                skeletonEntity.ID = i;
                skeletonEntity.State = Action.Idle;
                skeletonEntity.MaxHealth = 15;
                skeletonEntity.CurrentHealth = 15;
                skeletonEntity.AttackDamage = 0.05;
                enemyList.Add(skeletonEntity);
            }

            for (int i = 0; i < 10; i++)
            {
                Entity batEntity = new Entity(batEnemy.Animation);
                batEntity.LoadContent(content);
                batEntity.ID = i;
                batEntity.State = Action.IdleEast;
                batEntity.MaxHealth = 15;
                batEntity.CurrentHealth = 15;
                batEntity.AttackDamage = 0.02;
                enemyList.Add(batEntity);
            }

            for (int i = 0; i < 3; i++)
            {
                Entity fireBatEntity = new Entity(fireBatEnemy.Animation);
                fireBatEntity.LoadContent(content);
                fireBatEntity.ID = i;
                fireBatEntity.State = Action.IdleEast;
                fireBatEntity.MaxHealth = 15;
                fireBatEntity.CurrentHealth = 15;
                fireBatEntity.AttackDamage = 0.08;
                enemyList.Add(fireBatEntity);
            }

            MapRenderer map = StartArea.level_1.map;
            grid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);

            enemyList[0].Position = new Vector2(112, 691);
            enemyList[1].Position = new Vector2(29, 468);
            enemyList[2].Position = new Vector2(200, 471);
            enemyList[3].Position = new Vector2(33, 159);
            enemyList[4].Position = new Vector2(355, 114);
            enemyList[5].Position = new Vector2(726, 690);
            enemyList[6].Position = new Vector2(903, 438);
            enemyList[7].Position = new Vector2(840, 116);
            enemyList[8].Position = new Vector2(498, 162);
            enemyList[9].Position = new Vector2(533, 293);
            enemyList[10].Position = new Vector2(448, 563);
            enemyList[11].Position = new Vector2(890, 701);
            enemyList[12].Position = new Vector2(712, 877);
            enemyList[13].Position = new Vector2(955, 232);
            enemyList[14].Position = new Vector2(955, 567);
            enemyList[15].Position = new Vector2(261, 686);
            enemyList[16].Position = new Vector2(253, 314);
            enemyList[17].Position = new Vector2(191, 378);
            enemyList[18].Position = new Vector2(336, 125);
            enemyList[19].Position = new Vector2(626, 562);
            enemyList[20].Position = new Vector2(498, 692);
            enemyList[21].Position = new Vector2(510, 692);


            StartArea.player.EnemyList = enemyList;

            // Block cells in the collision layer for path finding.
            foreach (Tile tile in map.GetCollisionLayer())
            {
                if (tile.TileID !=0)
                {
                    int x = (int)tile.Position.X;
                    int y = (int)tile.Position.Y;

                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            grid.BlockCell(new RoyT.AStar.Position(x, y));
                            x++;
                        }

                        x = (int)tile.Position.X;
                        grid.BlockCell(new RoyT.AStar.Position(x, y));
                        y++;
                    }
                }
            }
            enemyAI = new EnemyAI(grid, enemyList, StartArea.player);
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(player.Position);
            enemyAI.Update(gameTime);

            foreach(Entity e in enemyList)
            {
                e.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach(Entity e in enemyList)
            {
                e.Draw(spriteBatch);
                Vector2 AIHealthPosition = new Vector2(e.Position.X - 8, e.Position.Y - 20);
                e.DrawHUD(spriteBatch, AIHealthPosition, false);
            }
        }
    }
}
