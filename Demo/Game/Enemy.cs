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

    public class Enemy 
    {
        public Texture2D Texture;
        public TextureAtlas Atlas;
        public SpriteSheetAnimationFactory Animation;

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(@"spritesheets\militia2");
            Atlas = TextureAtlas.Create(Texture, 32, 32);
            Animation = new SpriteSheetAnimationFactory(Atlas);
            float animationSpeed = .2f;
            float attackSpeed = 0.09f;
            Animation.Add("idle", new SpriteSheetAnimationData(new[] { 0 }));
            Animation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 1, 2 }, animationSpeed, isLooping: true));
            Animation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 3, 4, 5, 6, 7, 8, 7, 6, 5, 9, 10, 11, 10 }, attackSpeed, isLooping: true));
            Animation.Add("walkWest", new SpriteSheetAnimationData(new[] { 12, 13, 12, 14 }, animationSpeed, isLooping: true));
            Animation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 15, 16, 17, 18, 19, 20, 18, 17, 16, 21, 22, 23 }, attackSpeed, isLooping: true));
            Animation.Add("idleWest", new SpriteSheetAnimationData(new[] { 12 }));
            Animation.Add("walkEast", new SpriteSheetAnimationData(new[] { 26, 25, 26, 24 }, animationSpeed, isLooping: true));
            Animation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 27, 28, 29, 30, 31, 32, 30, 29, 28, 33, 34, 35 }, attackSpeed, isLooping: true));
            Animation.Add("idleEast", new SpriteSheetAnimationData(new[] { 26 }));
            Animation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 36, 38 }, animationSpeed, isLooping: true));
            Animation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 39, 40, 41, 42, 43, 42, 41, 42, 45, 46, 47, 46 }, attackSpeed, isLooping: true));
            Animation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 37 }));
            Animation.Add("dead", new SpriteSheetAnimationData(new[] { 48, 49, 50 }, .2f, isLooping: false));
        }

    }
}
