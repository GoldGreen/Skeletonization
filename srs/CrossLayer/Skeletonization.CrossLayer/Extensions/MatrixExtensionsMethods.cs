using Emgu.CV;
using Emgu.CV.Util;
using System.Runtime.CompilerServices;

namespace Skeletonization.CrossLayer.Extensions
{
    public static class MatrixExtensionsMethods
    {
        public static byte[] ToBytes(this Mat mat)
        {
            using VectorOfByte output = new();
            CvInvoke.Imencode(".png", mat, output);
            return output.ToArray();
        }

        public static T[] GetArray<T>(this T[,] matrix, int index)
        {
            var array = new T[matrix.GetLength(1)];

            for (int j = 0; j < array.Length; j++)
            {
                array[j] = matrix[index, j];
            }

            return array;
        }

        public static unsafe ref T At<T>(this Mat mat, int i0, int i1)
          where T : unmanaged
        {
            var p = mat.GetDataPointer(i0, i1);
            return ref Unsafe.AsRef<T>(p.ToPointer());
        }
    }
}
