using Skeletonization.BusinessLayer.Data;
using System.Collections.Generic;
using System.Windows.Media;

namespace Skeletonization.PresentationLayer.Shared.Data
{
    public record HumanWithRoi(ImageSource HumanRoi, string Name, IReadOnlyList<BodyPartPoint> Points) 
                : Human(Name, Points);
}
