using System.Collections.Generic;

namespace Skeletonization.CrossfulLayer.Data
{
    public record Human(string Name, IReadOnlyList<BodyPartPoint> Points);
}
