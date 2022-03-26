using Skeletonization.CrossfulLayer.Data;
using Skeletonization.CrossLayer.Data;
using System.Threading.Tasks;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IVideoProcessingHandler
    {
        Task HandleFrame(FrameInfo frameInfo);
        void HandleVideoInformation(VideoInfo size);
    }
}
