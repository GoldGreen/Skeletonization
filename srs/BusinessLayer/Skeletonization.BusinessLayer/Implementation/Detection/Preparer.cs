using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using System;
using static Skeletonization.CrossLayer.Extensions.PointsExtensionsMethods;
using Dr = System.Drawing;

namespace Skeletonization.BusinessLayer.Implementation.Detection
{
    internal class Preparer : IPreparer
    {
        public Dr.Point[,] Prepare(Dr.Point[,] points)
        {
            var result = new Dr.Point[points.GetLength(0), Enum.GetValues<BodyPart>().Length];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = points[i, j];
                }

                //Из глаз и ушей сделать точку головы.
                result[i, (int)BodyPart.Head] = Average(points[i, 14], points[i, 15], points[i, 16], points[i, 17]);

                //Из бедер сделать точку нижнего тела.
                result[i, (int)BodyPart.Hip] = Average(points[i, 8], points[i, 11]);
            }

            return result;
        }
    }
}
