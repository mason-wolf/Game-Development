
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
using Demo.Interface;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Demo.Scenes
{
    public class Level_1A : Scene
    {
        public static EnemyAI enemyAI;
        public static List<Entity> enemyList = new List<Entity>();
        public RoyT.AStar.Grid grid;
        Player player = Init.Player;
        List<MapObject> mapObjects = Init.Level_1AMap.GetMapObjects();
        AnimatedSprite torchSprite;
        AnimatedSprite barrelSprite;
        Texture2D arrowsSprite;
        List<SoundEffect> soundEffects;
        Song song;

        public override void LoadContent(ContentManager content)
        {
            foreach (MapObject mapObject in mapObjects)
            {
                switch (mapObject.GetName())
                {
                    case ("Skeleton"):
                        Entity skeletonEntity = new Entity(Sprites.skeletonAnimation);
                        skeletonEntity.LoadContent(content);
                        skeletonEntity.State = Action.IdleEast1;
                        skeletonEntity.MaxHealth = 15;
                        skeletonEntity.CurrentHealth = 15;
                        skeletonEntity.AttackDamage = 0.05;
                        skeletonEntity.Position = mapObject.GetPosition();
                        skeletonEntity.Name = "Skeleton";
                        enemyList.Add(skeletonEntity);
                        break;
                    case ("Bat"):
                        Entity batEntity = new Entity(Sprites.batAnimation);
                        batEntity.LoadContent(content);
                        batEntity.State = Action.IdleEast1;
                        batEntity.MaxHealth = 15;
                        batEntity.CurrentHealth = 15;
                        batEntity.AttackDamage = 0.05;
                        batEntity.Position = mapObject.GetPosition();
                        batEntity.Name = "Bat";
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
                        IBox barrelCollidable = Init.Level_1AMap.GetWorld().Create(barrelSprite.Position.X, barrelSprite.Position.Y - 4, 16, 16);
                        mapObject.SetCollisionBox(barrelCollidable);
                        break;
                }
            }

            grid = Init.Level_1AMap.GenerateAStarGrid();
            enemyAI = new EnemyAI(grid, enemyList, Init.Player);
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\destroyed-barrel"));
            arrowsSprite = content.Load<Texture2D>(@"objects\arrows");
            song = content.Load<Song>(@"music\level_1");
            //   MediaPlayer.Play(song);
        }

        public override void Update(GameTime gameTime)
        {
            enemyAI.Update(gameTime);

            foreach (Entity e in enemyList)
            {
                e.Update(gameTime);
            }

            foreach (MapObject o in mapObjects)
            {
                o.Update(gameTime);
            }

            // Handle the player destroying objects.
            foreach (MapObject mapObject in mapObjects)
            {
                if (player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.IsAttacking && mapObject.GetName() == "Barrel")
                {
                    if (!mapObject.IsDestroyed())
                    {
                        mapObject.GetSprite().Play("broken");
                        mapObject.Destroy();
                        soundEffects[0].Play();
                        Init.Level_1AMap.GetWorld().Remove(mapObject.GetCollisionBox());
                    }
                }
            }

        }

        bool objectsPopulated = false;
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
                Vector2 AIHealthPosition = new Vector2(enemy.Position.X - 8, enemy.Position.Y - 20);
                enemy.DrawHUD(spriteBatch, AIHealthPosition, false);
            }

            Random random = new Random();

            foreach (MapObject mapObject in mapObjects)
            {
                Item item = new Item();

                if (objectsPopulated == false)
                {
                    int lootChance = random.Next(1, 4);

                    switch (lootChance)
                    {
                        case (1):
                            item.ItemTexture = Sprites.chickenTexture;
                            item.Name = "Chicken";
                            item.Width = 16;
                            item.Height = 16;
                            break;
                        case (2):
                            item.ItemTexture = arrowsSprite;
                            item.Name = "Arrow";
                            item.Width = 13;
                            item.Height = 19;
                            break;
                    }

                    if (item != null)
                    {
                        mapObject.SetContainedItem(item);
                    }
                }
                mapObject.Draw(spriteBatch);
            }

            objectsPopulated = true;
        }
    }
}
