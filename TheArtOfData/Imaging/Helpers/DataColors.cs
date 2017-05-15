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
        public static readonly DataColors Orange = new DataColors(1);
        public static readonly DataColors Yellow = new DataColors(2);
        public static readonly DataColors Green = new DataColors(3);
        public static readonly DataColors Cyan = new DataColors(4);
        public static readonly DataColors Blue = new DataColors(5);
        public static readonly DataColors Purple = new DataColors(6);
        public static readonly DataColors Magenta = new DataColors(7);

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

        public float Min
        {
            get { return min; }
        }

        public float Max
        {
            get { return max; }
        }

        #endregion

        #region "Methods"

        private void GetRange()
        {
            switch (value)
            {
                case 0:
                    min = 340;
                    max = 10;
                    break;
                case 1:
                    min = 10;
                    max = 40;
                    break;
                case 2:
                    min = 40;
                    max = 75;
                    break;
                case 3:
                    min = 75;
                    max = 150;
                    break;
                case 4:
                    min = 150;
                    max = 200;
                    break;
                case 5:
                    min = 200;
                    max = 245;
                    break;
                case 6:
                    min = 250;
                    max = 280;
                    break;
                case 7:
                    min = 280;
                    max = 340;
                    break;
                case 8:
                    break;
            }
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"

        public override string ToString()
        {
            switch(value)
            {
                case 0:
                    return "Red";
                case 1:
                    return "Orange";
                case 2:
                    return "Yellow";
                case 3:
                    return "Green";
                case 4:
                    return "Cyan";
                case 5:
                    return "Blue";
                case 6:
                    return "Purple";
                case 7:
                    return "Magenta";
                default:
                    return "";
            }
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"

        public static implicit operator Color(DataColors dc)
        {
            //if (dc == null)
            //    return Color.White;

            float s = 1, v = 1;
            switch (dc.value)
            {
                case 0:
                    return new HSVColor(0, s, v).ToRGB();
                case 1:
                    return new HSVColor(25, s, v).ToRGB();
                case 2:
                    return new HSVColor(60, s, v).ToRGB();
                case 3:
                    return new HSVColor(120, s, v).ToRGB();
                case 4:
                    return new HSVColor(175, s, v).ToRGB();
                case 5:
                    return new HSVColor(240, s, v).ToRGB();
                case 6:
                    return new HSVColor(275, s, v).ToRGB();
                case 7:
                    return new HSVColor(315, s, v).ToRGB();
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
            //if (dc == null)
            //    return -1;
            return dc.value;
        }

        public static implicit operator DataColors(float hue)
        {
            if (Between(hue, 340, 10))
                return 0;
            else if (Between(hue, 10, 40))
                return 1;
            else if (Between(hue, 40, 75))
                return 2;
            else if (Between(hue, 75, 150))
                return 3;
            else if (Between(hue, 150, 200))
                return 4;
            else if (Between(hue, 200, 245))
                return 5;
            else if (Between(hue, 245, 280))
                return 6;
            else if (Between(hue, 280, 340))
                return 7;
            else return -1;
        }

        private static bool Between(float value, float min, float max)
        {
            if (min > max)
                return value >= min || value <= max;
            return value >= min && value <= max;
        }

        public static bool operator ==(DataColors left, DataColors right)
        {
            return left.value == right.value;
        }

        public static bool operator !=(DataColors left, DataColors right)
        {
            return !(left.value == right.value);
        }

        public static bool operator ==(Color left, DataColors right)
        {
            return (DataColors)left == right;
        }

        public static bool operator !=(Color left, DataColors right)
        {
            return !(left == right);
        }

        #endregion
    }
}
