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
        public static readonly DataColors Black = new DataColors(3);
        public static readonly DataColors Magenta = new DataColors(4);
        public static readonly DataColors Cyan = new DataColors(5);
        public static readonly DataColors Orange = new DataColors(6);
        public static readonly DataColors Gray = new DataColors(7);
        public static readonly DataColors Grey = new DataColors(7);
        public static readonly DataColors Purple = new DataColors(8);

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
            switch(value)
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
            switch (dc.value)
            {
                case 0:
                    return Color.FromArgb(255, 0, 0);
                case 1:
                    return Color.FromArgb(0, 0, 255);
                case 2:
                    return Color.FromArgb(0, 128, 0);
                case 3:
                    return Color.FromArgb(0, 0, 0);
                case 4:
                    return Color.FromArgb(255, 0, 255);
                case 5:
                    return Color.FromArgb(0, 255, 255);
                case 6:
                    return Color.FromArgb(255, 165, 0);
                case 7:
                    return Color.FromArgb(128, 128, 128);
                case 8:
                    return Color.FromArgb(160, 115, 255);
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
            else if (c == Color.FromArgb(0, 128, 0))
                return new DataColors(2);
            else if (c == Color.FromArgb(0, 0, 0))
                return new DataColors(3);
            else if (c == Color.FromArgb(255, 0, 255))
                return new DataColors(4);
            else if (c == Color.FromArgb(0, 255, 255))
                return new DataColors(5);
            else if (c == Color.FromArgb(255, 165, 0))
                return new DataColors(6);
            else if (c == Color.FromArgb(128, 128, 128))
                return new DataColors(7);
            else if (c == Color.FromArgb(160, 115, 255))
                return new DataColors(8);
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
            else if (Between(hue, 287, 332))
                return 4;
            else if (Between(hue, 151, 190))
                return 5;
            else if (Between(hue, 19, 66))
                return 6;
            else return 4;
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
