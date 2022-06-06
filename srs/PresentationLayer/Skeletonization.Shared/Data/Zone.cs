using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.BusinessLayer.Data;
using Skeletonization.CrossLayer.Data;
using Skeletonization.PresentationLayer.Shared.Abstractions;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Media;

namespace Skeletonization.PresentationLayer.Shared.Data
{
    public class Zone : ReactiveObject, IChecker<Human, HumanCheckResult>
    {
        [Reactive] public IEnumerable<HumanCheckResult> FailedCheckingElements { get; set; }

        [Reactive] public Point LeftTop { get; set; }
        [Reactive] public Point RightTop { get; set; }
        [Reactive] public Point RightBot { get; set; }
        [Reactive] public Point LeftBot { get; set; }
        public extern IEnumerable<Point> Points { [ObservableAsProperty]get; }

        [Reactive] public int MinCount { get; set; }
        [Reactive] public int Delay { get; set; }
        [Reactive] public IEnumerable<Selectable<BodyPart>> BodyParts { get; set; }
        [Reactive] public bool CheckInside { get; set; }

        [Reactive] public ImageSource ZoneRoiSource { get; set; }

        [Reactive] public string Name { get; set; }
        [Reactive] public string Color { get; set; }
        [Reactive] public double Opacity { get; set; }
        public Guid Guid { get; }

        private double _currentMillisecond;

        public Zone(double startX, double startY, double width, double height, Guid guid)
        {
            Guid = guid;

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

            this.WhenAnyValue(x => x.Delay)
                .Subscribe(_ => _currentMillisecond = 0)
                .Cashe();

            Observable.Interval(TimeSpan.FromMilliseconds(15))
                      .TimeInterval()
                      .ObserveOnDispatcher()
                      .Select(x => x.Interval.TotalMilliseconds)
                      .Where(x => _currentMillisecond < Delay)
                      .Subscribe(mil => _currentMillisecond += mil)
                      .Cashe();
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
            var checkResult = CheckAndConvert(elements).ToList();

            if (checkResult.Count == 0)
            {
                _currentMillisecond = 0;
            }

            bool check = checkResult.Count > MinCount && _currentMillisecond >= Delay;

            FailedCheckingElements = check ? checkResult : null;
        }

        private IEnumerable<HumanCheckResult> CheckAndConvert(IEnumerable<Human> elements)
        {
            foreach (var el in elements)
            {
                if (Check(el, out var res))
                {
                    yield return res;
                }
            }
        }

        public bool Check(Human el, out HumanCheckResult res)
        {
            var selectedParts = BodyParts.Where(x => x.IsSelected)
                                          .Select(x => x.Value);

            bool result = false;
            List<BodyPart> bodyParts = new();

            foreach (var bodyPartPoint in el.Points.Where(x => selectedParts.Contains(x.BodyPart)))
            {
                if (Check(bodyPartPoint.Point) ^ !CheckInside)
                {
                    result = true;
                    bodyParts.Add(bodyPartPoint.BodyPart);
                }
            }

            res = bodyParts.Count > 0 ? new HumanCheckResult(el, bodyParts) : null;
            return result;
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

        private static double ValidateCoordinate(double c)
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
