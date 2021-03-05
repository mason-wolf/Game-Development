
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
    public class Level_1B : SceneLogic
    {
        public static AnimatedSprite chainedGateSprite;
        Boss bossEntity;
        Song levelThemeSong;
        Song bossThemeSong;
        bool objectsCreated = false;
        bool bossThemePlaying = false;

        public override List<MapObject> MapObjects { get; set; }
        public override ContentManager ContentManager { get; set; }
        public override Map Map { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (bossEntity != null)
            {
                bossEntity.Draw(spriteBatch);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            bossThemeSong = content.Load<Song>(@"music\boss_fight");
            levelThemeSong = content.Load<Song>(@"music\level_1");

            if (bossEntity != null)
            {
                bossEntity.LoadContent(content);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Wait for level class to load and once finished load content for this level.
            if (MapObjects != null && objectsCreated == false)
            {
                foreach (MapObject mapObject in MapObjects)
                {
                    if (mapObject.GetName() == "Boss")
                    {
                        bossEntity = new Boss(Sprites.prospectorAnimation);
                        bossEntity.MaxHealth = 40;
                        bossEntity.CurrentHealth = 40;
                        bossEntity.AttackDamage = 0.06;
                        bossEntity.Position = mapObject.GetPosition();
                        bossEntity.Name = "The Prospector";
                    }
                    if (mapObject.GetName() == "Gate")
                    {
                        chainedGateSprite = new AnimatedSprite(Sprites.chainedGateAnimation);
                        chainedGateSprite.Play("idle");
                        chainedGateSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(chainedGateSprite);
                        IBox chainedGateCollidable = Map.GetWorld().Create(chainedGateSprite.Position.X - 10, chainedGateSprite.Position.Y, 32, 16);
                        mapObject.SetCollisionBox(chainedGateCollidable);
                    }

                }

                LoadContent(ContentManager);
                objectsCreated = true;
            }

            if (MapObjects != null)
            {
                bossEntity.Update(gameTime);

                if (Boss.bossEngaged && bossThemePlaying == false)
                {
                    MediaPlayer.Play(bossThemeSong);
                    bossThemePlaying = true;
                }
                else if (bossEntity.Dead)
                {
                    MediaPlayer.Stop();
                }
                else if (Init.Player.Dead)
                {
                    MediaPlayer.Play(levelThemeSong);
                }

                foreach (MapObject mapObject in MapObjects)
                {
                    if (Init.Player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.ActionButtonPressed && mapObject.GetName() == "Chest")
                    {
                        if (!mapObject.ItemPickedUp())
                        {
                            mapObject.GetSprite().Play("Opened");
                            Init.Message = "You obtained dynamite.";
                            Init.MessageEnabled = true;
                            mapObject.PickUpItem();
                            Inventory.TotalDynamite = Inventory.TotalDynamite += 10;
                        }
                    }

                    if (Init.Player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.ActionButtonPressed && mapObject.GetName() == "Gate")
                    {
                        if (Inventory.TotalKeys == 0)
                        {
                            Init.Message = "Gate is locked. You need a key.";
                            Init.MessageEnabled = true;
                        }
                        else
                        {
                            Inventory.TotalKeys = Inventory.TotalKeys - 1;
                            mapObject.GetSprite().Play("open");
                            Map.GetWorld().Remove(mapObject.GetCollisionBox());
                        }
                    }
                }
            }            
        }
    }
}
