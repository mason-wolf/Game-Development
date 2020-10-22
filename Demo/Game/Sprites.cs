using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Sprites
    {
        // Bat Sprite
        public Texture2D batTexture;
        public TextureAtlas batAtlas;
        public static SpriteSheetAnimationFactory batAnimation;

        // FireBat Sprite
        public Texture2D fireBatTexture;
        public TextureAtlas fireBatAtlas;
        public static SpriteSheetAnimationFactory fireBatAnimation;

        // Skeleton Sprite
        public Texture2D skeletonTexture;
        public TextureAtlas skeletonAtlas;
        public static SpriteSheetAnimationFactory skeletonAnimation;

        // Sitting Warrior Sprite
        public Texture2D sittingWarriorTexture;
        public TextureAtlas sittingWarriorAtlas;
        public static SpriteSheetAnimationFactory sittingWarriorAnimation;

        // Campfire sprite
        public Texture2D campfireTexture;
        public TextureAtlas campfireAtlas;
        public SpriteSheetAnimationFactory campfireAnimation;
        public static AnimatedSprite campfireSprite;

        // Torch sprite
        public static Texture2D torchTexture;
        public TextureAtlas torchAtlas;
        public static SpriteSheetAnimationFactory torchAnimation;

        // Barrel sprite
        public Texture2D barrelTexture;
        public TextureAtlas barrelAtlas;
        public static SpriteSheetAnimationFactory barrelAnimation;

        // Chicken sprite
        public static Texture2D chickenTexture;

        public void LoadContent(ContentManager content)
        {
            //Bat
            batTexture = content.Load<Texture2D>(@"spritesheets\Bat");
            batAtlas = TextureAtlas.Create(batTexture, 32, 32);
            batAnimation = new SpriteSheetAnimationFactory(batAtlas);
            float animationSpeed = .2f;
            float attackSpeed = 0.2f;
            batAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            batAnimation.Add("walkWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            batAnimation.Add("idleWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            batAnimation.Add("walkEast", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            batAnimation.Add("idleEast", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            batAnimation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            batAnimation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            batAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 3 }, .2f, isLooping: false));

            // Fire Bat
            fireBatTexture = content.Load<Texture2D>(@"spritesheets\FireBat");
            fireBatAtlas = TextureAtlas.Create(fireBatTexture, 32, 32);
            fireBatAnimation = new SpriteSheetAnimationFactory(fireBatAtlas);
            fireBatAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            fireBatAnimation.Add("walkWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            fireBatAnimation.Add("idleWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            fireBatAnimation.Add("walkEast", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            fireBatAnimation.Add("idleEast", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            fireBatAnimation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            fireBatAnimation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            fireBatAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 3 }, .2f, isLooping: false));

            // Skeleton
            skeletonTexture = content.Load<Texture2D>(@"spritesheets\Skeleton");
            skeletonAtlas = TextureAtlas.Create(skeletonTexture, 32, 32);
            skeletonAnimation = new SpriteSheetAnimationFactory(skeletonAtlas);
            skeletonAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            skeletonAnimation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            skeletonAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            skeletonAnimation.Add("walkWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            skeletonAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            skeletonAnimation.Add("idleWest", new SpriteSheetAnimationData(new[] { 0 }));
            skeletonAnimation.Add("walkEast", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            skeletonAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            skeletonAnimation.Add("idleEast", new SpriteSheetAnimationData(new[] { 0 }));
            skeletonAnimation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            skeletonAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            skeletonAnimation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 0 }));
            skeletonAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 3 }, .2f, isLooping: false));

            // Sitting Warrior
            sittingWarriorTexture = content.Load<Texture2D>(@"spritesheets\Sitting Warrior");
            sittingWarriorAtlas = TextureAtlas.Create(sittingWarriorTexture, 32, 32);
            sittingWarriorAnimation = new SpriteSheetAnimationFactory(sittingWarriorAtlas);
            sittingWarriorAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            sittingWarriorAnimation.Add("walkWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            sittingWarriorAnimation.Add("idleWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            sittingWarriorAnimation.Add("walkEast", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            sittingWarriorAnimation.Add("idleEast", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 3, 3, 3, 3, 3, 3}, 0.7f, isLooping: true));
            sittingWarriorAnimation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            sittingWarriorAnimation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            sittingWarriorAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 0 }, .2f, isLooping: false));

            // Campfire
            campfireTexture = content.Load<Texture2D>(@"objects\campfire");
            campfireAtlas = TextureAtlas.Create(campfireTexture, 16, 32);
            campfireAnimation = new SpriteSheetAnimationFactory(campfireAtlas);
            campfireAnimation.Add("burning", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, .09f, isLooping: true));
            campfireSprite = new AnimatedSprite(campfireAnimation);
            campfireSprite.Play("burning");

            // Torch
            torchTexture = content.Load<Texture2D>(@"objects\torch");
            torchAtlas = TextureAtlas.Create(torchTexture, 32, 32);
            torchAnimation = new SpriteSheetAnimationFactory(torchAtlas);
            torchAnimation.Add("burning", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, 0.09f, isLooping: true));

            // Barrel
            barrelTexture = content.Load<Texture2D>(@"objects\barrel");
            barrelAtlas = TextureAtlas.Create(barrelTexture, 32, 32);
            barrelAnimation = new SpriteSheetAnimationFactory(barrelAtlas);
            barrelAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0 }, 0.09f, isLooping: false));
            barrelAnimation.Add("broken", new SpriteSheetAnimationData(new[] { 1, 2, 3}, 0.07f, isLooping: false));

            // Chicken
            chickenTexture = content.Load<Texture2D>(@"items\Chicken");
        }
    }
}
