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

namespace Demo.Engine
{

    public class Enemy 
    {
        public Texture2D militiaTexture;
        public TextureAtlas militiaAtlas;
        public SpriteSheetAnimationFactory militiaAnimation;

        public void LoadContent(ContentManager content)
        {
            militiaTexture = content.Load<Texture2D>(@"spritesheets\militia");
            militiaAtlas = TextureAtlas.Create(militiaTexture, 32, 32);
            militiaAnimation = new SpriteSheetAnimationFactory(militiaAtlas);
            float animationSpeed = .09f;
            float attackSpeed = 0.06f;
            militiaAnimation.Add("idle", new SpriteSheetAnimationData(new[] { 0 }));
            militiaAnimation.Add("walkSouth", new SpriteSheetAnimationData(new[] { 1, 2 }, animationSpeed, isLooping: true));
            militiaAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 3, 4, 5, 6, 7, 8, 7, 6, 5 }, attackSpeed, isLooping: true));
            militiaAnimation.Add("attackSouthPattern2", new SpriteSheetAnimationData(new[] { 9, 10, 11, 10 }, attackSpeed, isLooping: true));
            militiaAnimation.Add("walkWest", new SpriteSheetAnimationData(new[] { 12, 13, 12, 14 }, animationSpeed, isLooping: true));
            militiaAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 15, 16, 17, 18, 19, 20, 18, 17, 16 }, attackSpeed, isLooping: true));
            militiaAnimation.Add("attackWestPattern2", new SpriteSheetAnimationData(new[] { 21, 22, 23 }, attackSpeed, isLooping: true));
            militiaAnimation.Add("idleWest", new SpriteSheetAnimationData(new[] { 12 }));
            militiaAnimation.Add("walkEast", new SpriteSheetAnimationData(new[] { 26, 25, 26, 24 }, animationSpeed, isLooping: true));
            militiaAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 27, 28, 29, 30, 31, 32, 30, 29, 28 }, attackSpeed, isLooping: true));
            militiaAnimation.Add("attackEastPattern2", new SpriteSheetAnimationData(new[] { 33, 34, 35 }, attackSpeed, isLooping: true));
            militiaAnimation.Add("idleEast", new SpriteSheetAnimationData(new[] { 26 }));
            militiaAnimation.Add("walkNorth", new SpriteSheetAnimationData(new[] { 36, 38 }, animationSpeed, isLooping: true));
            militiaAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 39, 40, 41, 42, 43, 44 }, attackSpeed, isLooping: true));
            militiaAnimation.Add("attackNorthPattern2", new SpriteSheetAnimationData(new[] { 45, 46, 47, 46 }, attackSpeed, isLooping: true));
            militiaAnimation.Add("idleNorth", new SpriteSheetAnimationData(new[] { 37 }));
        }

        public void Attack(Entity entity, Entity target)
        {
    
            float distance = Vector2.Distance(entity.Position, target.Position);
           
            if (distance < 15)
            {

                Vector2 destination = entity.Position - target.Position;
                destination.Normalize();
                Double angle = Math.Atan2(destination.X, destination.Y);                                           
                double direction = Math.Ceiling(angle);
     

                if (direction == -3 || direction == 4 || direction == -2)
                {
                    entity.State = Action.AttackSouthPattern2;
                }

                if (direction == -1)
                {
                    entity.State = Action.AttackEastPattern2;
                    target.CurrentHealth -= .09;
                }

                if (direction == 0 || direction == 1)
                {
                    entity.State = Action.AttackNorthPattern2;
                }

                if (direction == 2 || direction == 3)
                {
                    entity.State = Action.AttackWestPattern2;
                }
            }
        }

    }
}
