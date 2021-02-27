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

        // Goblin Sprite
        public Texture2D goblinTexture;
        public TextureAtlas goblinAtlas;
        public static SpriteSheetAnimationFactory goblinAnimation;

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

        // Chest sprite
        public Texture2D chestTexture;
        public TextureAtlas chestAtlas;
        public static SpriteSheetAnimationFactory chestAnimation;

        // Chicken sprite
        public static Texture2D chickenTexture;

        // Arrow sprite
        public static Texture2D arrowTexture;
        public TextureAtlas arrowAtlas;
        public static SpriteSheetAnimationFactory arrowAnimation;

        // Dynamite sprite
        public static Texture2D dynamiteTexture;
        public TextureAtlas dynamiteAtlas;
        public static SpriteSheetAnimationFactory dynamiteAnimation;

        // Explosion sprite
        public static Texture2D explosionTexture;
        public TextureAtlas explosionAtlas;
        public static SpriteSheetAnimationFactory explosionAnimation;

        // Single dynamite sprite
        public static Texture2D singleDynamiteTexture;

        // Rock sprite
        public static Texture2D rockTexture;
        public TextureAtlas rockAtlas;
        public static SpriteSheetAnimationFactory rockAnimation;

        // Prospector sprite
        public static Texture2D prospectorTexture;
        public TextureAtlas prospectorAtlas;
        public static SpriteSheetAnimationFactory prospectorAnimation;

        public void LoadContent(ContentManager content)
        {
            //Bat
            batTexture = content.Load<Texture2D>(@"spritesheets\Bat");
            batAtlas = TextureAtlas.Create(batTexture, 32, 32);
            batAnimation = new SpriteSheetAnimationFactory(batAtlas);
            float animationSpeed = .2f;
            float attackSpeed = 0.2f;
            batAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("walkSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            batAnimation.Add("walkWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            batAnimation.Add("idleWest1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            batAnimation.Add("walkEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            batAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            batAnimation.Add("walkNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            batAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            batAnimation.Add("idleNorth1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            batAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 3 }, .2f, isLooping: false));

            // Fire Bat
            fireBatTexture = content.Load<Texture2D>(@"spritesheets\FireBat");
            fireBatAtlas = TextureAtlas.Create(fireBatTexture, 32, 32);
            fireBatAnimation = new SpriteSheetAnimationFactory(fireBatAtlas);
            fireBatAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("walkSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            fireBatAnimation.Add("walkWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            fireBatAnimation.Add("idleWest1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            fireBatAnimation.Add("walkEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            fireBatAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            fireBatAnimation.Add("walkNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            fireBatAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            fireBatAnimation.Add("idleNorth1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            fireBatAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 3 }, .2f, isLooping: false));

            // Skeleton
            skeletonTexture = content.Load<Texture2D>(@"spritesheets\Skeleton");
            skeletonAtlas = TextureAtlas.Create(skeletonTexture, 32, 32);
            skeletonAnimation = new SpriteSheetAnimationFactory(skeletonAtlas);
            skeletonAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            skeletonAnimation.Add("walkSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4 }, animationSpeed, isLooping: true));
            skeletonAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4}, attackSpeed, isLooping: true));
            skeletonAnimation.Add("walkWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4}, animationSpeed, isLooping: true));
            skeletonAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4}, attackSpeed, isLooping: true));
            skeletonAnimation.Add("idleWest1", new SpriteSheetAnimationData(new[] { 0 }));
            skeletonAnimation.Add("walkEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4}, animationSpeed, isLooping: true));
            skeletonAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4}, attackSpeed, isLooping: true));
            skeletonAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 0 }));
            skeletonAnimation.Add("walkNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4 }, animationSpeed, isLooping: true));
            skeletonAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4 }, attackSpeed, isLooping: true));
            skeletonAnimation.Add("idleNorth1", new SpriteSheetAnimationData(new[] { 0 }));
            skeletonAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 5 }, .2f, isLooping: false));

            // Goblin
            goblinTexture = content.Load<Texture2D>(@"spritesheets\Goblin");
            goblinAtlas = TextureAtlas.Create(goblinTexture, 32, 32);
            goblinAnimation = new SpriteSheetAnimationFactory(goblinAtlas);
            goblinAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, animationSpeed, isLooping: true));
            goblinAnimation.Add("walkSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, animationSpeed, isLooping: true));
            goblinAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, attackSpeed, isLooping: true));
            goblinAnimation.Add("walkWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, animationSpeed, isLooping: true));
            goblinAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, attackSpeed, isLooping: true));
            goblinAnimation.Add("idleWest1", new SpriteSheetAnimationData(new[] { 0 }));
            goblinAnimation.Add("walkEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, animationSpeed, isLooping: true));
            goblinAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, attackSpeed, isLooping: true));
            goblinAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 0 }));
            goblinAnimation.Add("walkNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, animationSpeed, isLooping: true));
            goblinAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, attackSpeed, isLooping: true));
            goblinAnimation.Add("idleNorth1", new SpriteSheetAnimationData(new[] { 0 }));
            goblinAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 4 }, .2f, isLooping: false));

            // Sitting Warrior
            sittingWarriorTexture = content.Load<Texture2D>(@"spritesheets\Sitting Warrior");
            sittingWarriorAtlas = TextureAtlas.Create(sittingWarriorTexture, 32, 32);
            sittingWarriorAnimation = new SpriteSheetAnimationFactory(sittingWarriorAtlas);
            sittingWarriorAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 0 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            sittingWarriorAnimation.Add("walkWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            sittingWarriorAnimation.Add("idleWest", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            sittingWarriorAnimation.Add("walkEast", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            sittingWarriorAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            sittingWarriorAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 3, 3, 3, 3, 3, 3 }, 0.7f, isLooping: true));
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
            barrelAnimation.Add("broken", new SpriteSheetAnimationData(new[] { 1, 2, 3 }, 0.07f, isLooping: false));

            // Chicken
            chickenTexture = content.Load<Texture2D>(@"items\Chicken");

            // Arrow
            arrowTexture = content.Load<Texture2D>(@"objects\Arrow");
            arrowAtlas = TextureAtlas.Create(arrowTexture, 32, 32);
            arrowAnimation = new SpriteSheetAnimationFactory(arrowAtlas);
            arrowAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0 }, 0.09f, isLooping: false));
            arrowAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 1 }, 0.07f, isLooping: false));
            arrowAnimation.Add("idleNorth1", new SpriteSheetAnimationData(new[] { 2 }, 0.07f, isLooping: false));
            arrowAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 1 }, 0.07f, isLooping: false));
            arrowAnimation.Add("idleWest1", new SpriteSheetAnimationData(new[] { 0 }, 0.07f, isLooping: false));
            arrowAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 3 }, 0.07f, isLooping: false));
            arrowAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 2 }, 0.07f, isLooping: false));
            arrowAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 3 }, 0.07f, isLooping: false));

            // Chest
            chestTexture = content.Load<Texture2D>(@"objects\Chest");
            chestAtlas = TextureAtlas.Create(chestTexture, 32, 32);
            chestAnimation = new SpriteSheetAnimationFactory(chestAtlas);
            chestAnimation.Add("Unopened", new SpriteSheetAnimationData(new[] { 0 }, isLooping: false));
            chestAnimation.Add("Opened", new SpriteSheetAnimationData(new[] { 1 }, isLooping: false));

            // Dynamite
            dynamiteTexture = content.Load<Texture2D>(@"objects\Dynamite");
            dynamiteAtlas = TextureAtlas.Create(dynamiteTexture, 16, 16);
            dynamiteAnimation = new SpriteSheetAnimationFactory(dynamiteAtlas);
            dynamiteAnimation.Add("burning", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }, 0.03f, isLooping: true));

            // Single dynamite
            singleDynamiteTexture = content.Load<Texture2D>(@"items\Dynamite");

            // Explosion 
            explosionTexture = content.Load<Texture2D>(@"objects\Explosion");
            explosionAtlas = TextureAtlas.Create(explosionTexture, 32, 32);
            explosionAnimation = new SpriteSheetAnimationFactory(explosionAtlas);
            explosionAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0 }, .1f, isLooping: false));
            explosionAnimation.Add("explosion", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4 }, .1f, isLooping: false ));

            // Rock
            rockTexture = content.Load<Texture2D>(@"objects\BigRock");
            rockAtlas = TextureAtlas.Create(rockTexture, 32, 32);
            rockAnimation = new SpriteSheetAnimationFactory(rockAtlas);
            rockAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0 }, .1f, isLooping: false));
            rockAnimation.Add("broken", new SpriteSheetAnimationData(new[] { 1, 2, 3 }, .08f, isLooping: false));

            // Prospector
            prospectorTexture = content.Load<Texture2D>(@"spritesheets\Prospector");
            prospectorAtlas = TextureAtlas.Create(prospectorTexture, 32, 42);
            prospectorAnimation = new SpriteSheetAnimationFactory(prospectorAtlas);
            prospectorAnimation.Add("walking", new SpriteSheetAnimationData(new[] { 0, 1 }, 0.08f, isLooping: true));
            prospectorAnimation.Add("Attack1", new SpriteSheetAnimationData(new[] { 1, 2 }, 0.4f, isLooping: true));
            prospectorAnimation.Add("Attack2", new SpriteSheetAnimationData(new[] { 3, 4 }, 0.4f, isLooping: true));
        }
    }
}
