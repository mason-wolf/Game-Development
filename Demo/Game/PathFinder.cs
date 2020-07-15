using Microsoft.Xna.Framework;
using RoyT.AStar;
using System;
using System.Collections.Generic;

namespace Demo
{
    class PathFinder
    {
        Grid movementGrid;
        List<Entity> unitList;
        GameTime gameTime;

        /// <summary>
        /// Creates a path finding instance to track units on a movement grid.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="movementGrid">The area in which the units are allowed to move.</param>
        /// <param name="unitList">The list of units to move.</param>

        public PathFinder(GameTime gameTime, Grid movementGrid, List<Entity> unitList)
        {
            this.movementGrid = movementGrid;
            this.unitList = unitList;
            this.gameTime = gameTime;
        }

        List<Vector2> wayPoints = new List<Vector2>();
        public List<WayPoint> wayPointsList = new List<WayPoint>();

        public void FindPathToUnit(Entity target)
        {
    
            Vector2 closestPath = GetNearestUnit(unitList, target);
            var movementPattern = new[] { new Offset(-1, 0), new Offset(0, -1), new Offset(1, 0), new Offset(0, 1) };

            Position nearestEntity = new Position((int)closestPath.X, (int)closestPath.Y);
            Position targetPosition = new Position((int)target.Position.X, (int)target.Position.Y);
            Position[] path = movementGrid.GetPath(nearestEntity, targetPosition, movementPattern);

                foreach (Position position in path)
                {
                    wayPoints.Add(new Vector2(position.X, position.Y));
                }


            WayPoint wayPoint = new WayPoint();
            wayPoint.Add = wayPoints;
            wayPointsList.Add(wayPoint);
        }


        public void MoveUnit(Entity unit, float speed, int distanceLimit, GameTime gameTime)
        {
 
                if (wayPoints.Count > distanceLimit && unit.CurrentHealth > 0)
                {
                    Avoid(gameTime, unit);
                    unit.FollowPath(gameTime, unit, wayPoints, speed);
                }
                unit.Update(gameTime);        
        }


        public void Avoid(GameTime gameTime, Entity unit)
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i].BoundingBox.Intersects(unit.BoundingBox) && unitList[i].State != Action.Dead)
                {
                    float unitDistanceToDestination = Vector2.Distance(unit.Position, wayPoints[wayPoints.Count - 1]);
                    float movingUnitsDistanceToDestination = Vector2.Distance(unitList[i].Position, wayPoints[wayPoints.Count - 1]);

                    if (unitDistanceToDestination > movingUnitsDistanceToDestination)
                    {
                        Vector2 oppositeDirection = unitList[i].Position - unit.Position;
                        oppositeDirection.Normalize();
                        unit.Position -= oppositeDirection * (float)(0.01f * gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                }
            }
        }

        // Returns the closest distance between a list of units and the target.
        public static Vector2 GetNearestUnit(List<Entity> movingUnits, Entity target)
        {
            Vector2 closest = new Vector2(0, 0);
            var closestDistance = float.MaxValue;

            for (int i = 0; i < movingUnits.Count; i++)
            {
                if (movingUnits[i].State != Action.Dead)
                {
                    var distance = Vector2.DistanceSquared(movingUnits[i].Position, target.Position);
                    if (distance < closestDistance)
                    {
                        closest = movingUnits[i].Position;
                        closestDistance = distance;
                    }
                }

            }
            return closest;
        }

        // Find the closest unit.
        public static Entity GetNearestEntity(List<Entity> movingUnits, Entity target)
        {
            Vector2 closest = new Vector2(0, 0);
            Entity closestEntity = new Entity();

            var closestDistance = float.MaxValue;

            for (int i = 0; i < movingUnits.Count; i++)
            {
                if (movingUnits[i].State != Action.Dead)
                {
                    var distance = Vector2.DistanceSquared(movingUnits[i].Position, target.Position);
                    if (distance < closestDistance)
                    {
                        closest = movingUnits[i].Position;
                        closestEntity = movingUnits[i];
                        closestDistance = distance;
                    }
                }

            }
            return closestEntity;
        }
    }
}
