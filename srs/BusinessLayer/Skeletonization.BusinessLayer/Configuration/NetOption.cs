namespace Skeletonization.BusinessLayer.Configuration
{
    internal class NetOption
    {
        public const string Net = nameof(Net);

        public string Config { get; set; }
        public string Model { get; set; }
        public bool UseCuda { get; set; }
    }
}
