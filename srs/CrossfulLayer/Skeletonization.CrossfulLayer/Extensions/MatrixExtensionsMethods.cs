using Emgu.CV;
using Emgu.CV.Util;

namespace Skeletonization.CrossfulLayer.Extensions
{
    public static class MatrixExtensionsMethods
    {
        public static byte[] ToBytes(this Mat mat)
        {
            using VectorOfByte output = new();
            CvInvoke.Imencode(".png", mat, output);
            return output.ToArray();
        }
    }
}
