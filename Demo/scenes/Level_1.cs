
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

        public override void LoadContent(ContentManager content)
        {
            enemy = new Enemy();
            enemy.LoadContent(content);

            // Create enemies
            for (int i = 0; i < 1; i++)
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

         //   grid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);
            enemyList[0].Position = new Vector2(407, 764);

         //   StartArea.playerObject.EnemyList = enemyList;
        }

        public override void Update(GameTime gameTime)
        {
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
            }
        }
    }
}
