using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData.Art
{
    class TintedColor
    {
        public const int totalTints = 8;
        public const int bits = 3;

        public static Color Get(int value)
        {
            float step = 255f / totalTints;
            int current = (int)(step * value);
            return Color.FromArgb(0, current, 0);
        }
    }
}
