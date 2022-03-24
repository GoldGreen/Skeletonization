namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IVideoService
    {
        bool ReversePause();
        void StartCamera(int cameraId, IVideoProcessingHandler handler);
        void StartFile(string filePath, IVideoProcessingHandler handler);
    }
}
