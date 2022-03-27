using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using Skeletonization.CrossLayer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Dr = System.Drawing;

namespace Skeletonization.BusinessLayer.Implementation.Detection
{
    internal class HumanConverter : IHumanConverter
    {
        public IEnumerable<Human> Convert(Dr.Point[,] points)
        {
            for (int i = 0; i < points.GetLength(0); i++)
            {
                var humanPoints = points.GetArray(i);
                var human = new Human
                (
                    $"Человек {i}",
                    humanPoints.Select
                    (
                        (p, i) => p.IsEmpty() ? null : new BodyPartPoint((BodyPart)i, new(p.X, p.Y))
                    ).Where(x => x is not null).ToList()
                );

                yield return human;
            }
        }

        public Dr.Point[,] Convert(IEnumerable<Human> humans)
        {
            var humansList = humans.ToList();
            var points = new Dr.Point[humansList.Count, 18];

            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    points[i, j] = new(-1, -1);
                }
            }

            for (int i = 0; i < humansList.Count; i++)
            {
                for (int j = 0; j < humansList[i].Points.Count; j++)
                {
                    var point = humansList[i].Points[j];
                    points[i, (int)point.BodyPart] = new((int)point.Point.X, (int)point.Point.Y);
                }
            }

            return points;
        }
    }
}
