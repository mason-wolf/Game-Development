using Humper.Responses;
using Microsoft.Xna.Framework;
using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class PathFinder
    {
        Grid grid;

        public PathFinder(Grid grid)
        {
            this.grid = grid;
        }
        List<Vector2> wayPoints = new List<Vector2>();
        public List<WayPoint> wayPointsList = new List<WayPoint>();

 
        ///// Finds the closest path to the player.
        //public void FindPathToPlayer(Entity entity, Entity player)
        //{

        // //   Vector2 closestPath = ClosestEntity(entity, player);
        //    var movementPattern = new[] { new Offset(-1, 0), new Offset(0, -1), new Offset(1, 0), new Offset(0, 1) };

        //    Position nearestEntity = new Position((int)closestPath.X, (int)closestPath.Y);
        //    Position playerPosition = new Position((int)player.Position.X, (int)player.Position.Y);
        //    Position[] path = grid.GetPath(nearestEntity, playerPosition, movementPattern);

        //    foreach (Position position in path)
        //    {
        //        wayPoints.Add(new Vector2(position.X, position.Y));
        //    }
        //}

        public void FindPathToUnit(List<Entity> entityList, Entity unit)
        {

            Vector2 closestPath = ClosestEntity(entityList, unit);
            var movementPattern = new[] { new Offset(-1, 0), new Offset(0, -1), new Offset(1, 0), new Offset(0, 1) };

            Position nearestEntity = new Position((int)closestPath.X, (int)closestPath.Y);
            Position playerPosition = new Position((int)unit.Position.X, (int)unit.Position.Y);
            Position[] path = grid.GetPath(nearestEntity, playerPosition, movementPattern);

            foreach (Position position in path)
            {
                wayPoints.Add(new Vector2(position.X, position.Y));
            }

            WayPoint wayPoint = new WayPoint();
            wayPoint.Add = wayPoints;
            wayPointsList.Add(wayPoint);
        }

        public void MoveUnit(List<Entity> unitList, Entity unit, GameTime gameTime)
        {
            if (wayPoints.Count > 15 && unit.CurrentHealth > 0)
            {
                Avoid(gameTime, unitList, unit);
                unit.FollowPath(gameTime, unit, wayPoints, 0.05f);
            }
            else if (unit.CurrentHealth <= 0)
            {
                unit.State = Action.Dead;
                unit.Dead = true;
            }

            unit.Update(gameTime);
        }

        public void MoveUnits(List<Entity> unitList, GameTime gameTime)
        {

            foreach (Entity unit in unitList)
            {
                if (wayPoints.Count > 15 && unit.CurrentHealth > 0)
                {
                    Avoid(gameTime, unitList, unit);

                    float speed = 0;

                    if (unitList.Count > 1)
                    {
                        speed = .02f;
                    }
                    else
                    {
                        speed = 0.05f;
                    }

                    unit.FollowPath(gameTime, unit, wayPoints, speed);
                }
                else if (unit.CurrentHealth <= 0)
                {
                    unit.State = Action.Dead;
                    unit.Dead = true;
                }

                unit.Update(gameTime);

            }
        }
        public void Avoid(GameTime gameTime, List<Entity> Units, Entity entity)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                if (Units[i].BoundingBox.Intersects(entity.BoundingBox) && Units[i].State != Action.Dead)
                {
                    float Distance1 = Vector2.Distance(entity.Position, wayPoints[wayPoints.Count - 1]);
                    float Distance2 = Vector2.Distance(Units[i].Position, wayPoints[wayPoints.Count - 1]);

                    if (Distance1 > Distance2)
                    {
                        Vector2 OppositeDirection = Units[i].Position - entity.Position;
                        OppositeDirection.Normalize();
                        entity.Position -= OppositeDirection * (float)(0.05f * gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                }
            }
        }
        public Vector2 ClosestEntity(List<Entity> enemyList, Entity playerEntity)
        {
            Vector2 closest = new Vector2(0, 0);
            var closestDistance = float.MaxValue;

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].State != Action.Dead)
                {
                    var distance = Vector2.DistanceSquared(enemyList[i].Position, playerEntity.Position);
                    if (distance < closestDistance)
                    {
                        closest = enemyList[i].Position;
                        closestDistance = distance;
                    }
                }

            }

            return closest;
        }
    }
}
