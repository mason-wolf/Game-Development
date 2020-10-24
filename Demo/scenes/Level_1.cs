
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
using Microsoft.Xna.Framework.Input;

namespace Demo.Scenes
{
    public class Level_1 : Scene
    {
        GameTime gameTime;
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
                        IBox barrelCollidable = StartArea.level_1Map.GetWorld().Create(barrelSprite.Position.X, barrelSprite.Position.Y - 4, 16, 16);
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

        float elapsedTime;

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            enemyAI.Update(gameTime);

            foreach(Entity e in enemyList)
            {
                e.Update(gameTime);
            }

            foreach(MapObject o in mapObjects)
            {
                o.Update(gameTime);
            }

            // Handle the player destroying objects.
            foreach(MapObject mapObject in mapObjects)
            {
                if (player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.isAttacking && mapObject.GetName() == "Barrel")
                {
                    if (!mapObject.IsDestroyed())
                    {
                        mapObject.GetSprite().Play("broken");
                        mapObject.Destroy();
                        StartArea.level_1Map.GetWorld().Remove(mapObject.GetCollisionBox());
                    }
                }
            }

        }

        float timer = 3;

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
                Vector2 AIHealthPosition = new Vector2(enemy.Position.X - 8, enemy.Position.Y - 20);
                enemy.DrawHUD(spriteBatch, AIHealthPosition, false);
            }

            timer -= elapsedTime;

            foreach (MapObject mapObject in mapObjects)
            {
                Item item = new Item();
                item.ItemTexture = Sprites.chickenTexture;
                item.Name = "Chicken";
                mapObject.SetContainedItem(item);
                mapObject.Draw(spriteBatch);

                if (mapObject.ItemPickedUp())
                {
                    spriteBatch.DrawString(StartArea.font, "You picked up", new Vector2(player.Position.X - 170, player.Position.Y + 100), Color.White);
                }
            }

        }
    }
}
