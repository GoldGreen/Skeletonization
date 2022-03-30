using Emgu.CV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using D = Skeletonization.BusinessLayer.Data;

namespace Skeletonization.PresentationLayer.Shared.Extensions
{
    public static class MatExtensionsMethods
    {
        public static Mat GetRoi(this Mat parent, IEnumerable<D.Point> points)
        {
            if (!points.Any())
            {
                return null;
            }

            var matPoints = points.Select(p => new D.Point(p.X * parent.Width, p.Y * parent.Height))
                                  .ToList();

            var (startX, startY, endX, endY) =
                (
                    matPoints.Min(p => p.X),
                    matPoints.Min(p => p.Y),
                    matPoints.Max(p => p.X),
                    matPoints.Max(p => p.Y)
                );

            if (startX == endX || startY == endY)
            {
                return null;
            }

            Rectangle rect = new((int)startX, (int)startY, (int)(endX - startX), (int)(endY - startY));
            Mat roi = new(parent, rect);

            return roi;
        }

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
