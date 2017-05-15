using Imaging.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imaging.Parsing
{
    class ColorCount
    {
        #region "Fields"

        private int count;
        private DataColors color;

        #endregion

        #region "Constructors"

        public ColorCount(DataColors color)
        {
            this.color = color;
            this.count = 1;
        }

        #endregion

        #region "Properties"

        public int Count
        {
            get { return count; }
        }

        public DataColors Color
        {
            get { return color; }
        }

        #endregion

        #region "Methods"



        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"

        public override string ToString()
        {
            return color.ToString() + " -> " + count;
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"

        public static ColorCount operator ++(ColorCount cc)
        {
            cc.count++;
            return cc;
        }

        #endregion
    }
}
