using Skeletonization.BusinessLayer.Data;
using Skeletonization.CrossLayer.Data;
using System.Collections.Generic;

namespace Skeletonization.PresentationLayer.Shared.Data
{
    public record HumanCheckResult(Human Human, IEnumerable<BodyPart> DetectedBodyParts);
}
