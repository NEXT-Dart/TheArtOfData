using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData
{
    class DataColors
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

        private static Random rand = new Random();

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
            float s = rand.Next(600, 1000) / 1000f;
            float v = rand.Next(600, 1000) / 1000f;
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
                    return new HSVColor(315, 1, 1).ToRGB();
                case 5:
                    return new HSVColor(175, s, v).ToRGB();
                case 6:
                    return new HSVColor(25, s, v).ToRGB();
                case 7:
                    return new HSVColor(275, 1, 1).ToRGB();
                default:
                    return Color.White;
            }
        }

        public static implicit operator DataColors(int value)
        {
            return new DataColors(value);
        }

        #endregion
    }
}
