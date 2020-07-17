
using Demo.Engine;
using Demo;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Demo.Scenes
{
    public class Level_1 : Scene
    {
        public static EnemyAI enemyAI;
        public static List<Entity> enemyList = new List<Entity>();
        public RoyT.AStar.Grid grid;
        Player player = StartArea.player;
        List<MapObject> mapObjects = StartArea.level_1Map.GetMapObjects();
        AnimatedSprite torchSprite;
        AnimatedSprite barrelSprite;

        public override void LoadContent(ContentManager content)
        {
            foreach (MapObject mapObject in mapObjects)
            {
                switch (mapObject.GetName())
                {
                    case ("Skeleton"):
                        Entity skeletonEntity = new Entity(Sprites.skeletonAnimation);
                        skeletonEntity.LoadContent(content);
                        skeletonEntity.State = Action.IdleEast;
                        skeletonEntity.MaxHealth = 15;
                        skeletonEntity.CurrentHealth = 15;
                        skeletonEntity.AttackDamage = 0.05;
                        skeletonEntity.Position = mapObject.GetPosition();
                        enemyList.Add(skeletonEntity);
                        break;
                    case ("Bat"):
                        Entity batEntity = new Entity(Sprites.batAnimation);
                        batEntity.LoadContent(content);
                        batEntity.State = Action.IdleEast;
                        batEntity.MaxHealth = 15;
                        batEntity.CurrentHealth = 15;
                        batEntity.AttackDamage = 0.05;
                        batEntity.Position = mapObject.GetPosition();
                        enemyList.Add(batEntity);
                        break;
                    case ("Torch"):
                        torchSprite = new AnimatedSprite(Sprites.torchAnimation);
                        torchSprite.Play("burning");
                        torchSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(torchSprite);
                        break;
                    case ("Barrel"):
                        barrelSprite = new AnimatedSprite(Sprites.barrelAnimation);
                        barrelSprite.Play("idle");
                        barrelSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(barrelSprite);
                        IBox barrelCollidable = StartArea.level_1Map.GetWorld().Create(barrelSprite.Position.X, barrelSprite.Position.Y - 6, 16, 16);
                        mapObject.SetCollisionBox(barrelCollidable);
                        break;
                }
            }
 
            MapRenderer map = StartArea.level_1Map.map;
            grid = new RoyT.AStar.Grid(map.Width() * 16, map.Height() * 16, 1);

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
            enemyAI.Update(gameTime);

            foreach(Entity e in enemyList)
            {
                e.Update(gameTime);
            }

            foreach(MapObject o in mapObjects)
            {
                o.Update(gameTime);
            }

            foreach(MapObject o in mapObjects)
            {
                if (player.BoundingBox.Intersects(o.GetBoundingBox()) && Player.isAttacking && o.GetName() == "Barrel")
                {
                    if (!o.isDestroyed())
                    {
                        o.GetSprite().Play("broken");
                        o.Destroy();
                        StartArea.level_1Map.GetWorld().Remove(o.GetCollisionBox());
                    }
                }
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

            foreach (MapObject o in mapObjects)
            {
                o.Draw(spriteBatch);
            }
        }
    }
}
