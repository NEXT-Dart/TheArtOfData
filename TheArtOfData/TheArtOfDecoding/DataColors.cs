using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfDecoding
{
    class DataColors
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

        private int value;

        #endregion

        #region "Constructors"

        private DataColors(int value)
        {
            this.value = value;
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"



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
            else
                return new DataColors(-1);
        }

        public static implicit operator DataColors(int value)
        {
            return new DataColors(value);
        }

        public static implicit operator int(DataColors dc)
        {
            return dc.value;
        }

        #endregion
    }
}
