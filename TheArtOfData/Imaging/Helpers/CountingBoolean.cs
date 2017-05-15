using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imaging.Helpers
{
    class CountingBoolean
    {
        #region "Fields"

        private int amount;

        #endregion

        #region "Constructors"

        public CountingBoolean()
        {
            amount = 0;
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

        public static implicit operator bool(CountingBoolean cb)
        {
            return cb.amount > 0;
        }

        public static CountingBoolean operator ++(CountingBoolean right)
        {
            right.amount++;
            return right;
        }

        public static CountingBoolean operator --(CountingBoolean right)
        {
            right.amount--;
            return right;
        }

        #endregion
    }
}
