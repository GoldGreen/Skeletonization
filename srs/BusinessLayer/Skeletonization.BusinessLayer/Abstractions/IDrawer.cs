using Emgu.CV;
using Skeletonization.BusinessLayer.Data;
using System.Collections.Generic;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IDrawer
    {
        void Draw(Mat mat, IEnumerable<Human> humans);
    }
}
