using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Demo.Engine
{
    public class Enemy 
    {

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
