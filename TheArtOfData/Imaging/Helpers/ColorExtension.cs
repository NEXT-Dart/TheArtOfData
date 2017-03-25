using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imaging.Helpers
{
    static class ColorExtension
    {
        public static int Grayscale(this Color color)
        {
            return (color.R + color.G + color.B) / 3;
        }

        public static Color ToGrayscale(this Color color)
        {
            int grayscale = color.Grayscale();
            return Color.FromArgb(grayscale, grayscale, grayscale);
        }

        public static HSLColor ToHSL(this Color color)
        {
            return HSLColor.FromRGB(color);
        }

        public static HSVColor ToHSV(this Color color)
        {
            return new HSVColor(color);
        }
    }
}
