using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Cogs.Mvvm
{
    public static class ColorExtensionMethods
    {
        public static string ToHex(this Color color)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static Color FromHex(string hexColor)
        {
            return (Color)ColorConverter.ConvertFromString(hexColor);
        }
    }
}
