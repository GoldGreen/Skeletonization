using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using Skeletonization.CrossLayer.Extensions;
using System.Collections.Generic;
using System.Linq;
using Dr = System.Drawing;

namespace Skeletonization.BusinessLayer.Implementation.Detection
{
    internal class Drawer : IDrawer
    {
        public IHumanConverter HumanConverter { get; }

        public Drawer(IHumanConverter humanConverter)
        {
            HumanConverter = humanConverter;
        }

        public void Draw(Mat mat, IEnumerable<Human> humans)
        {
            var points = HumanConverter.Convert(humans);

            for (int i = 0; i < points.GetLength(0); i++)
            {
                var personPoints = points.GetArray(i);

                //Нарисовать скелет.
                for (BodyPart part = 0; (int)part < personPoints.Length; part++)
                {
                    if (!personPoints[(int)part].IsEmpty() && part.HasLinks())
                    {
                        //Проход по всем смежным точкам тела.
                        foreach (int id in part.GetLinkedBodyParts())
                        {
                            if (!personPoints[id].IsEmpty())
                            {
                                //Нарисовать часть тела - кость.
                                CvInvoke.Line(mat, personPoints[(int)part], personPoints[id], new MCvScalar(0, 165, 255), 2);
                            }
                        }
                    }
                }

                for (BodyPart part = 0; (int)part < personPoints.Length; part++)
                {
                    if (!personPoints[(int)part].IsEmpty())
                    {
                        //Нарисовать точку.
                        CvInvoke.Circle(mat, personPoints[(int)part], 5, new MCvScalar(100, 100, 60), -1);
                    }
                }

                var valuablePersonPoints = personPoints.Where(x => !x.IsEmpty());

                //Нарисовать рамку каждого скелета.
                int padding = 10;

                int minX = valuablePersonPoints.Min(x => x.X);
                int minY = valuablePersonPoints.Min(x => x.Y);

                int maxX = valuablePersonPoints.Max(x => x.X);
                int maxY = valuablePersonPoints.Max(x => x.Y);

                var rect = new Dr.Rectangle(minX - padding, minY - padding, padding + maxX - minX, padding + maxY - minY);
                CvInvoke.Rectangle(mat, rect, new MCvScalar(0, 165, 265), 2);
                CvInvoke.PutText(mat, i.ToString(), new Dr.Point(minX, minY), FontFace.HersheyComplex, 1, new MCvScalar(100, 100, 60), 2);
            }
        }
    }
}
