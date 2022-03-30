using System.Collections.Generic;

namespace Skeletonization.BusinessLayer.Data
{
    public record Human(string Name, IReadOnlyList<BodyPartPoint> Points);
}
