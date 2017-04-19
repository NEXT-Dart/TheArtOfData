using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imaging.Helpers
{
    public class DataColors
    {
        #region "Fields"

        public static readonly DataColors Red = new DataColors(0);
        public static readonly DataColors Blue = new DataColors(1);
        public static readonly DataColors Green = new DataColors(2);
        //public static readonly DataColors Black = new DataColors(3);
        public static readonly DataColors Yellow = new DataColors(3);
        public static readonly DataColors Magenta = new DataColors(4);
        public static readonly DataColors Cyan = new DataColors(5);
        public static readonly DataColors Orange = new DataColors(6);
        public static readonly DataColors Purple = new DataColors(7);
        //public static readonly DataColors Gray = new DataColors(7);
        //public static readonly DataColors Grey = new DataColors(7);

        private int value;
        private float min;
        private float max;

        #endregion

        #region "Constructors"

        private DataColors(int value)
        {
            this.value = value;

            GetRange();
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        private void GetRange()
        {
            switch (value)
            {
                case 0:
                    min = 360 - 15;
                    max = 15;
                    break;
                case 1:
                    min = 360 - 135;
                    max = 360 - 105;
                    break;
                case 2:
                    min = 95;
                    max = 95 + 30;
                    break;
                case 3:

                    break;
                case 4:
                    min = 360 - 65;
                    max = 360 - 35;
                    break;
                case 5:
                    min = 360 - 155;
                    max = 165;
                    break;
                case 6:
                    min = 50;
                    max = 70;
                    break;
                case 7:

                    break;
                case 8:
                    break;
            }
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"

        public static implicit operator Color(DataColors dc)
        {
            if (dc == null)
                return Color.White;

            float s = 1, v = 1;
            switch (dc.value)
            {
                case 0:
                    return new HSVColor(0, s, v).ToRGB();
                case 1:
                    return new HSVColor(240, s, v).ToRGB();
                case 2:
                    return new HSVColor(120, s, v).ToRGB();
                case 3:
                    return new HSVColor(60, s, v).ToRGB();
                case 4:
                    return new HSVColor(315, s, v).ToRGB();
                case 5:
                    return new HSVColor(175, s, v).ToRGB();
                case 6:
                    return new HSVColor(25, s, v).ToRGB();
                case 7:
                    return new HSVColor(275, s, v).ToRGB();
                default:
                    return Color.White;
            }
        }

        public static implicit operator DataColors(Color c)
        {
            if (c == Color.FromArgb(255, 0, 0))
                return new DataColors(0);
            else if (c == Color.FromArgb(0, 0, 255))
                return new DataColors(1);
            else if (c == Color.FromArgb(0, 255, 0))
                return new DataColors(2);
            else if (c == Color.FromArgb(255,255, 0))
                return new DataColors(3);
            else if (c == Color.FromArgb(255, 0, 191))
                return new DataColors(4);
            else if (c == Color.FromArgb(0, 255, 234))
                return new DataColors(5);
            else if (c == Color.FromArgb(255, 106, 0))
                return new DataColors(6);
            else if (c == Color.FromArgb(149, 0, 255))
                return new DataColors(7);
            else
                return new DataColors(-1);
        }

        public static implicit operator DataColors(int value)
        {
            return new DataColors(value);
        }

        public static implicit operator int(DataColors dc)
        {
            if (dc == null)
                return -1;
            return dc.value;
        }

        public static implicit operator DataColors(float hue)
        {
            if (Between(hue, 345, 9))
                return 0;
            else if (Between(hue, 218, 251))
                return 1;
            else if (Between(hue, 83, 143))
                return 2;
            else if (Between(hue, 47, 65))
                return 3;
            else if (Between(hue, 287, 332))
                return 4;
            else if (Between(hue, 151, 190))
                return 5;
            else if (Between(hue, 19, 66))
                return 6;
            else if (Between(hue, 265, 285))
                return 7;
            else return -1;
        }

        private static bool Between(float value, float min, float max)
        {
            if (min > max)
                return value >= min || value <= max;
            return value >= min && value <= max;
        }

        #endregion
    }
}
