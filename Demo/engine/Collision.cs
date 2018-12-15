using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demo.Scenes;

namespace Demo
{
    public class Collision
    {
        Vector2 motion = new Vector2();

        public bool TransitionerCollision(Entity player, Entity transitioner)
        {
            if(player.BoundingBox.Intersects(transitioner.BoundingBox))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CharacterCollision(Entity player, Entity npc)
        {
            motion = player.Position;

            if (player.BoundingBox.Left < npc.BoundingBox.Left && player.BoundingBox.Right > npc.BoundingBox.Left && player.BoundingBox.Intersects(npc.BoundingBox))
            {             
                motion.X--;
                player.Position = motion;
                npc.State = Action.IdleWest;
                Console.WriteLine("Left");
            }

            if (player.BoundingBox.Left < npc.BoundingBox.Left && player.BoundingBox.Right > npc.BoundingBox.Left && player.BoundingBox.Intersects(npc.BoundingBox)
                && player.BoundingBox.Top < npc.BoundingBox.Bottom && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y++;
                motion.X++;
                player.Position = motion;
                npc.State = Action.Idle;
                Console.WriteLine("bottom-left");
            }

            if (player.BoundingBox.Right > npc.BoundingBox.Right && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.X++;
                player.Position = motion;
                npc.State = Action.IdleEast;
                Console.WriteLine("Right");
            }

            if (player.BoundingBox.Right > npc.BoundingBox.Right && player.BoundingBox.Intersects(npc.BoundingBox) &&
            player.BoundingBox.Top < npc.BoundingBox.Bottom && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y++;
                motion.X--;
                player.Position = motion;
                npc.State = Action.Idle;
                Console.WriteLine("bottom-right");
            }

            if (player.BoundingBox.Top < npc.BoundingBox.Bottom && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y++;
                player.Position = motion;
                npc.State = Action.Idle;
                Console.WriteLine("Bottom");
            }

            if (player.BoundingBox.Bottom > npc.BoundingBox.Top && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y--;
                player.Position = motion;
                npc.State = Action.IdleNorth;
                Console.WriteLine("Top");
            }

            if (player.BoundingBox.Top < npc.BoundingBox.Bottom && player.BoundingBox.Intersects(npc.BoundingBox) &
                player.BoundingBox.Bottom > npc.BoundingBox.Top && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y--;
                player.Position = motion;
                npc.State = Action.IdleNorth;
                Console.WriteLine("top & bottom");
            }

            if(player.BoundingBox.Top < npc.BoundingBox.Bottom && player.BoundingBox.Intersects(npc.BoundingBox) &&
               player.BoundingBox.Right > npc.BoundingBox.Right && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y--;
                player.Position = motion;
                npc.State = Action.IdleNorth;
                Console.WriteLine("top-right");
            }

            if (player.BoundingBox.Top < npc.BoundingBox.Bottom && player.BoundingBox.Intersects(npc.BoundingBox) &&
                player.BoundingBox.Right > npc.BoundingBox.Right && player.BoundingBox.Intersects(npc.BoundingBox) &&
                player.BoundingBox.Right > npc.BoundingBox.Right && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y--;
           //     motion.X--;
                player.Position = motion;
                npc.State = Action.IdleNorth;
                Console.WriteLine("top-right & right");
            }

            if (player.BoundingBox.Top < npc.BoundingBox.Bottom && player.BoundingBox.Intersects(npc.BoundingBox) &&
                player.BoundingBox.Left < npc.BoundingBox.Left && player.BoundingBox.Right > npc.BoundingBox.Left && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y--;
                player.Position = motion;
                npc.State = Action.IdleNorth;
                Console.WriteLine("top-left");
            }

            if (player.BoundingBox.Top < npc.BoundingBox.Bottom && player.BoundingBox.Intersects(npc.BoundingBox) &&
                player.BoundingBox.Left < npc.BoundingBox.Left && player.BoundingBox.Right > npc.BoundingBox.Left && player.BoundingBox.Intersects(npc.BoundingBox) &&
                player.BoundingBox.Left < npc.BoundingBox.Left && player.BoundingBox.Right > npc.BoundingBox.Left && player.BoundingBox.Intersects(npc.BoundingBox))
            {
                motion.Y--;
                motion.X--;
                player.Position = motion;
                npc.State = Action.IdleWest;
                Console.WriteLine("top-left & left");
            }
        }
    }
}
