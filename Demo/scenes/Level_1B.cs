
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
using Demo.Scenes;

namespace Demo
{
    public class Level_1B : Scene
    {
        public static EnemyAI enemyAI;
        public static List<Entity> enemyList = new List<Entity>();
        public RoyT.AStar.Grid grid;
        Player player = Init.Player;
        List<MapObject> mapObjects = Init.Level_1BMap.GetMapObjects();
        AnimatedSprite torchSprite;
        AnimatedSprite barrelSprite;
        AnimatedSprite chestSprite;
        AnimatedSprite rockSprite;
        public static AnimatedSprite chainedGateSprite;
        Boss bossEntity;
        Texture2D arrowsSprite;
        List<SoundEffect> soundEffects;
        Song song;
        int frameCount = 0;
        string message = "";
        bool messageEnabled = false;
        bool objectsPopulated = false;
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
                    case ("Fire Bat"):
                        Entity fireBatEntity = new Entity(Sprites.fireBatAnimation);
                        fireBatEntity.LoadContent(content);
                        fireBatEntity.State = Action.IdleEast1;
                        fireBatEntity.MaxHealth = 15;
                        fireBatEntity.CurrentHealth = 15;
                        fireBatEntity.AttackDamage = 0.05;
                        fireBatEntity.Position = mapObject.GetPosition();
                        fireBatEntity.Name = "Fire Bat";
                        enemyList.Add(fireBatEntity);
                        break;
                    case ("Goblin"):
                        Entity goblinEntity = new Entity(Sprites.goblinAnimation);
                        goblinEntity.LoadContent(content);
                        goblinEntity.State = Action.IdleEast1;
                        goblinEntity.MaxHealth = 20;
                        goblinEntity.CurrentHealth = 20;
                        goblinEntity.AttackDamage = 0.06;
                        goblinEntity.Position = mapObject.GetPosition();
                        goblinEntity.Name = "Goblin";
                        enemyList.Add(goblinEntity);
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
                        IBox barrelCollidable = Init.Level_1BMap.GetWorld().Create(barrelSprite.Position.X, barrelSprite.Position.Y - 4, 16, 16);
                        mapObject.SetCollisionBox(barrelCollidable);
                        break;
                    case ("Chest"):
                        chestSprite = new AnimatedSprite(Sprites.chestAnimation);
                        chestSprite.Play("Unopened");
                        chestSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(chestSprite);
                        IBox chestCollidable = Init.Level_1BMap.GetWorld().Create(chestSprite.Position.X, chestSprite.Position.Y, 16, 16);
                        mapObject.SetCollisionBox(chestCollidable);
                        break;
                    case ("Rock"):
                        rockSprite = new AnimatedSprite(Sprites.rockAnimation);
                        rockSprite.Play("idle");
                        rockSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(rockSprite);
                        IBox rockCollidable = Init.Level_1BMap.GetWorld().Create(rockSprite.Position.X, rockSprite.Position.Y, 16, 16);
                        mapObject.SetCollisionBox(rockCollidable);
                        break;
                    case ("Boss"):
                        bossEntity = new Boss(Sprites.prospectorAnimation);
                        bossEntity.MaxHealth = 40;
                        bossEntity.CurrentHealth = 40;
                        bossEntity.AttackDamage = 0.06;
                        bossEntity.Position = mapObject.GetPosition();
                        bossEntity.Name = "The Prospector";
                        break;
                    case ("Gate"):
                        chainedGateSprite = new AnimatedSprite(Sprites.chainedGateAnimation);
                        chainedGateSprite.Play("idle");
                        chainedGateSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(chainedGateSprite);
                        IBox chainedGateCollidable = Init.Level_1BMap.GetWorld().Create(chainedGateSprite.Position.X - 10, chainedGateSprite.Position.Y, 32, 16);
                        mapObject.SetCollisionBox(chainedGateCollidable);
                        break;
                }
            }

            grid = Init.Level_1BMap.GenerateAStarGrid();
            enemyAI = new EnemyAI(grid, enemyList, Init.Player);
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\destroyed-barrel"));
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\dead-bat"));
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\dead-skeleton"));
            soundEffects.Add(content.Load<SoundEffect>(@"sounds\dead-goblin"));
            arrowsSprite = content.Load<Texture2D>(@"objects\arrows");
            song = content.Load<Song>(@"music\level_1");
            bossEntity.LoadContent(content);
            //   MediaPlayer.Play(song);
        }

        public override void Update(GameTime gameTime)
        {
            bossEntity.Update(gameTime);
            enemyAI.Update(gameTime);

            foreach (Entity enemy in enemyList)
            {
                enemy.Update(gameTime);

                // If enemy dies
                if (enemy.CurrentHealth <= 0 && enemy.Dead == false)
                {
                    enemy.State = Action.Dead;
                    enemy.Dead = true;

                    switch (enemy.Name)
                    {
                        case ("Bat"):
                            soundEffects[1].Play();
                            break;
                        case ("Skeleton"):
                            soundEffects[2].Play();
                            break;
                        case ("Goblin"):
                            soundEffects[3].Play();
                            break;
                    }
                }
            }

            foreach (MapObject mapObject in mapObjects)
            {
                mapObject.Update(gameTime);
            }

            // Handle the player interacting with objects.
            foreach (MapObject mapObject in mapObjects)
            {
                // Destroying barrels
                if (player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.IsAttacking && mapObject.GetName() == "Barrel")
                {
                    if (!mapObject.IsDestroyed())
                    {
                        mapObject.GetSprite().Play("broken");
                        mapObject.Destroy();
                        soundEffects[0].Play();
                        Init.Level_1BMap.GetWorld().Remove(mapObject.GetCollisionBox());
                    }
                }

                if (player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.ActionButtonPressed && mapObject.GetName() == "Chest")
                {
                    if (!mapObject.ItemPickedUp())
                    {
                        mapObject.GetSprite().Play("Opened");
                        message = "You obtained dynamite.";
                        messageEnabled = true;
                        mapObject.PickUpItem();
                        Inventory.TotalDynamite = Inventory.TotalDynamite += 10;
                    }
                }

                if (player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.ActionButtonPressed && mapObject.GetName() == "Gate")
                {
                    if (Inventory.TotalKeys == 0)
                    {
                        message = "Gate is locked. You need a key.";
                        messageEnabled = true;
                    }
                    else
                    {
                        Inventory.TotalKeys = Inventory.TotalKeys - 1;
                        mapObject.GetSprite().Play("open");
                        Init.Level_1BMap.GetWorld().Remove(mapObject.GetCollisionBox());
                    }
                }
            }
        }

        public void ShowMessage(string message, SpriteBatch spriteBatch)
        {
            if (frameCount < 12000)
            {
                spriteBatch.DrawString(Init.Font, message, new Vector2(Init.Player.Position.X - 165, Init.Player.Position.Y + 105), Color.White);
                frameCount++;
            }
            else
            {
                messageEnabled = false;
                frameCount = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            bossEntity.Draw(spriteBatch);
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

                if (messageEnabled)
                {
                    ShowMessage(message, spriteBatch);
                }
                mapObject.Draw(spriteBatch);
            }

            objectsPopulated = true;
        }
    }
}
