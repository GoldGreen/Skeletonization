using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.CrossLayer.Data;
using Skeletonization.PresentationLayer.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Skeletonization.PresentationLayer.Shared.Data
{
    public class Zone : ReactiveObject, IChecker<Human>
    {
        public IEnumerable<Human> FailedCheckingElements { get; set; }

        [Reactive] public Point LeftTop { get; set; }
        [Reactive] public Point RightTop { get; set; }
        [Reactive] public Point RightBot { get; set; }
        [Reactive] public Point LeftBot { get; set; }
        [Reactive] public byte[] FrameRoiBytes { get; set; }

        public extern IEnumerable<Point> Points { [ObservableAsProperty]get; }

        public Zone(double startX, double startY, double width, double height)
        {
            LeftTop = new(startX, startY);
            RightTop = new(startX + width, startY);
            RightBot = new(startX + width, startY + height);
            LeftBot = new(startX, startY + height);

            this.WhenAnyValue(x => x.LeftTop, x => x.RightTop, x => x.RightBot, x => x.LeftBot)
                .Select(_ => GetPoints())
                .ToPropertyEx(this, x => x.Points);

            ValidatePoint(this.WhenAnyValue(x => x.LeftTop))
                .Subscribe(p => LeftTop = p);

            ValidatePoint(this.WhenAnyValue(x => x.RightTop))
                .Subscribe(p => RightTop = p);

            ValidatePoint(this.WhenAnyValue(x => x.RightBot))
                .Subscribe(p => RightBot = p);

            ValidatePoint(this.WhenAnyValue(x => x.LeftBot))
                .Subscribe(p => LeftBot = p);
        }

        public IEnumerable<Point> GetPoints()
        {
            yield return LeftTop;
            yield return RightTop;
            yield return RightBot;
            yield return LeftBot;
        }

        public void Check(IEnumerable<Human> elements)
        {
            FailedCheckingElements = elements.Where(x => Check(x))
                                             .ToList();
        }

        public bool Check(Human el)
        {
            foreach (var point in el.Points.Select(x => x.Point))
            {
                if (Check(point))
                {
                    return true;
                }
            }

            return false;
        }

        private bool Check(Point point)
        {
            var poly = Points.ToArray();

            bool inside = false;
            for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
            {
                if (poly[i].Y > point.Y != poly[j].Y > point.Y
                    && point.X < (poly[j].X - poly[i].X) * (point.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) + poly[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private IObservable<Point> ValidatePoint(IObservable<Point> point)
        {
            return point.Where(p => p.X is < 0 or > 1 || p.Y is < 0 or > 1)
                        .Select(p => new Point(ValidateCoordinate(p.X), ValidateCoordinate(p.Y)));
        }

        private double ValidateCoordinate(double c)
        {
            if (c < 0)
            {
                c = 0;
            }
            else if (c > 1)
            {
                c = 1;
            }

            return c;
        }

     
    }
}
