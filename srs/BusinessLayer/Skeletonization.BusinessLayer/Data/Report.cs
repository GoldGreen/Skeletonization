using Emgu.CV;
using System.Collections.Generic;

namespace Skeletonization.BusinessLayer.Data
{
    public record Report(string Title, string Description, IEnumerable<Human> Humans, Mat Mat);
}
