
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
        public static Enemy enemy;
        public static EnemyAI enemyAI;
        public static List<Entity> enemyList = new List<Entity>();
        public RoyT.AStar.Grid grid;
        Player player = StartArea.player;

        public override void LoadContent(ContentManager content)
        {
            enemy = new Enemy();
            enemy.LoadContent(content);

            // Create enemies
            for (int i = 0; i < 6; i++)
            {
                Entity enemyEntity = new Entity(enemy.Animation);
                enemyEntity.LoadContent(content);
                enemyEntity.ID = i;
                enemyEntity.State = Action.Idle;
                enemyEntity.MaxHealth = 15;
                enemyEntity.CurrentHealth = 15;
                enemyEntity.AttackDamage = 0.1;
                enemyList.Add(enemyEntity);
            }

            MapRenderer map = StartArea.level_1.map;
            grid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);

            enemyList[0].Position = new Vector2(100, 764);
            enemyList[1].Position = new Vector2(170, 617);
            enemyList[2].Position = new Vector2(95, 687);
            enemyList[3].Position = new Vector2(95, 521);
            enemyList[4].Position = new Vector2(95, 380);
            enemyList[5].Position = new Vector2(373, 373);

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
