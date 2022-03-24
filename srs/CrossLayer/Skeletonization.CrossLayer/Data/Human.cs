using System.Collections.Generic;

namespace Skeletonization.CrossLayer.Data
{
    public record Human(string Name, IReadOnlyList<BodyPartPoint> Points);
}
