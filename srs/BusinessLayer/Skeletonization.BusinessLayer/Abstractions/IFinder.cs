using Emgu.CV;
using Skeletonization.CrossLayer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IFinder
    {
        Task<IReadOnlyList<Human>> Find(Mat input);
    }
}
