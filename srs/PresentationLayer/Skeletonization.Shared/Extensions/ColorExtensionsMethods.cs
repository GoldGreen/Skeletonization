using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Skeletonization.PresentationLayer.Shared.Extensions
{
    public static class ColorExtensionsMethods
    {
        public static IEnumerable<string> Names { get; }
        public static IEnumerable<Brush> Colors { get; }

        private static readonly Dictionary<string, Brush> _colorsDictionary;
        private static readonly Dictionary<Brush, string> _namesDictionary;

        static ColorExtensionsMethods()
        {
            var brushes = typeof(Brushes).GetProperties()
                                         .Where(x => x.Name.Count(x => char.IsUpper(x)) <= 2)
                                         .Select(x => (name: x.Name, brush: x.GetValue(null) as Brush))
                                         .GroupBy(x => x.brush)
                                         .Select(x => x.FirstOrDefault())
                                         .ToList();

            Names = brushes.Select(x => x.name).ToList();
            Colors = brushes.Select(x => x.brush).ToList();

            _colorsDictionary = brushes.ToDictionary(x => x.name, x => x.brush);
            _namesDictionary = brushes.ToDictionary(x => x.brush, x => x.name);
        }

        public static string ToName(this Brush brush)
        {
            return _namesDictionary[brush];
        }

        public static Brush ToBrush(this string name)
        {
            return _colorsDictionary[name];
        }
    }
}
