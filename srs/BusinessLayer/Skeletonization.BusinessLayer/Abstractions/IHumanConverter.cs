using Skeletonization.CrossLayer.Data;
using System.Collections.Generic;
using Dr = System.Drawing;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IHumanConverter
    {
        IEnumerable<Human> Convert(Dr.Point[,] points);
        Dr.Point[,] Convert(IEnumerable<Human> humans);
    }
}
