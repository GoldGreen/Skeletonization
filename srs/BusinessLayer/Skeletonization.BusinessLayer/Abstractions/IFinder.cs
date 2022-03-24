using Emgu.CV;
using Skeletonization.CrossfulLayer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IFinder
    {
        Task<IReadOnlyList<Human>> Find(Mat input, Mat drawed = null);
    }
}
