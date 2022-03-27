using Emgu.CV;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Skeletonization.PresentationLayer.Shared.Extensions
{
    public static class MatExtensionsMethods
    {
        public static ImageSource ToImageSource(this Mat image)
        {
            using var source = image.ToBitmap();
            var ptr = source.GetHbitmap();

            var bs = Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr);
            return bs;
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
    }
}
