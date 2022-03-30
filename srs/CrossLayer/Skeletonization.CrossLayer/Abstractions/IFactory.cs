namespace Skeletonization.CrossfulLayer.Abstractions
{
    public interface IFactory<out T>
    {
        T Create();
    }
}
