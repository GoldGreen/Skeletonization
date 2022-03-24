using Emgu.CV;
using Skeletonization.CrossfulLayer.Data;
using System.Threading.Tasks;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IVideoProcessingHandler
    {
        Task HandleFrame(Mat mat);
        void HandleVideoInformation(Size size);
    }
}
