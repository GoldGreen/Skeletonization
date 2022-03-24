using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using Skeletonization.CrossLayer.Extensions;
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
                        (p, i) => p.IsEmpty() ? null : new BodyPartPoint
                        {
                            Point = new Point(p.X, p.Y),
                            BodyPart = (BodyPart)i
                        }
                    ).ToList()
                );

                yield return human;
            }
        }
    }
}
