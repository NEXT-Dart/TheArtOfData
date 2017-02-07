using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData
{
    class ImageResult
    {
        #region "Fields"

        private Image image;
        private int colorsPerRow;
        private byte[] data;
        private ImageDataWriter idw;
        private DateTime timestamp;

        #endregion

        #region "Constructors"

        public ImageResult(byte[] data, int colorsPerRow)
        {
            this.data = data;
            this.colorsPerRow = colorsPerRow;
            this.timestamp = DateTime.Now;
        }

        #endregion

        #region "Properties"

        public DateTime TimeStamp
        {
            get { return timestamp; }
        }

        public Image Image
        {
            get { return image; }
        }

        #endregion

        #region "Methods"

        public void Execute()
        {
            idw = new ImageDataWriter();
            idw.AddBytes(data);
            image = idw.GetImage(colorsPerRow);
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
