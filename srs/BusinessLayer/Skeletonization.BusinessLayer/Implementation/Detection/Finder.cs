using Emgu.CV;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeletonization.BusinessLayer.Implementation.Detection
{
    internal class Finder : IFinder
    {
        public IDetector Detector { get; }
        public IPreparer Preparer { get; }
        public IDrawer Drawer { get; }
        public IHumanConverter HumanConverter { get; }

        public Finder(IDetector detector, IPreparer preparer, IDrawer drawer, IHumanConverter humanConverter)
        {
            Detector = detector;
            Preparer = preparer;
            Drawer = drawer;
            HumanConverter = humanConverter;
        }

        public async Task<IReadOnlyList<Human>> Find(Mat input, Mat drawed = null)
        {
            var points = Preparer.Prepare(await Task.Run(() => Detector.Detect(input)));
            if (drawed is not null)
            {
                Drawer.Draw(drawed, points);
            }
            return HumanConverter.Convert(points).ToList();
        }
    }
}
