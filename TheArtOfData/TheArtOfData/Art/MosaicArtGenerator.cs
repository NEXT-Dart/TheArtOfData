using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData.Art
{
    class MosaicArtGenerator : ArtGenerator
    {
        #region "Fields"

        private List<byte> data;

        #endregion

        #region "Constructors"

        public MosaicArtGenerator()
        {
            data = new List<byte>();
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"



        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"

        public override void AddBytes(byte[] data)
        {
            this.data.AddRange(data);
        }

        public override Image GetImage()
        {

            return new Bitmap(1, 1);
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
