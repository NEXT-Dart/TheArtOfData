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
        public static int totalTints;
        public const int bits = 4;

        public static Color Get(int value)
        {
            CalculateTotalTints();

            float step = 128f / (totalTints / 2);
            int g = 0;
            int rb = 0;
            if (value <= totalTints / 2)
            {
                g = (int)step * value+ 127;
            }
            else
            {
                rb = (int)step * (value - (int)(totalTints / 2));
                g = 255;
            }
            return Color.FromArgb(rb, g, rb);
        }

        private static void CalculateTotalTints()
        {
            string bytes = "";
            for (int i = 0; i < bits; i++)
            {
                bytes += "1";
            }

            totalTints = Convert.ToInt32(bytes, 2) + 1;
        }
    }
}
