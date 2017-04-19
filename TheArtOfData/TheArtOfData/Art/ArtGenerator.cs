using Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData.Art
{
    abstract class ArtGenerator
    {
        #region "Fields"



        #endregion

        #region "Constructors"

        protected ArtGenerator()
        {

        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"



        #endregion

        #region "Abstract/Virtual Methods"

        public abstract void AddBytes(byte[] data);

        public abstract CustomImage GetImage(object[] values);

        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
