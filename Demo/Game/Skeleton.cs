using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;
using RoyT.AStar;

namespace Demo.Engine
{

    public class Skeleton 
    {
        public Texture2D Texture;
        public TextureAtlas Atlas;
        public SpriteSheetAnimationFactory Animation;

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(@"spritesheets\Skeleton");
            Atlas = TextureAtlas.Create(Texture, 32, 32);
            Animation = new SpriteSheetAnimationFactory(Atlas);
            float animationSpeed = .2f;
            float attackSpeed = 0.2f;
            Animation.Add("idle", new SpriteSheetAnimationData(new[] { 0 }));
            Animation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            Animation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            Animation.Add("walkWest", new SpriteSheetAnimationData(new[] { 0, 1, 2}, animationSpeed, isLooping: true));
            Animation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            Animation.Add("idleWest", new SpriteSheetAnimationData(new[] { 0 }));
            Animation.Add("walkEast", new SpriteSheetAnimationData(new[] { 0, 1, 2}, animationSpeed, isLooping: true));
            Animation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            Animation.Add("idleEast", new SpriteSheetAnimationData(new[] { 0 }));
            Animation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, animationSpeed, isLooping: true));
            Animation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 0, 1, 2 }, attackSpeed, isLooping: true));
            Animation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 0 }));
            Animation.Add("dead", new SpriteSheetAnimationData(new[] { 3 }, .2f, isLooping: false));
        }

    }
}
