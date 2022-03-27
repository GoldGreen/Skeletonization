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
        public IHumanConverter HumanConverter { get; }

        public Finder(IDetector detector, IPreparer preparer, IHumanConverter humanConverter)
        {
            Detector = detector;
            Preparer = preparer;
            HumanConverter = humanConverter;
        }

        public async Task<IReadOnlyList<Human>> Find(Mat input)
        {
            var points = Preparer.Prepare(await Task.Run(() => Detector.Detect(input)));
            return HumanConverter.Convert(points).ToList();
        }
    }
}
