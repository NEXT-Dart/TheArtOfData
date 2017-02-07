using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfDecoding
{
    class ImageDataReader
    {
        #region "Fields"

        private Bitmap img;
        private string currentByte;
        private List<byte> bytes;

        #endregion

        #region "Constructors"

        public ImageDataReader(Image img)
        {
            this.img = new Bitmap(img);
            bytes = new List<byte>();
            currentByte = "";
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public byte[] GetData()
        {
            for (int j = 0; j < img.Height; j++)
            {
                for (int i = 0; i < img.Width; i++)
                {
                    DataColors dc = img.GetPixel(i, j);
                    if (dc == -1)
                        continue;
                    string bits = Convert.ToString(dc, 2);
                    while (bits.Length < 3)
                        bits = "0" + bits;
                    currentByte += bits;
                    ParseBytes();
                }
            }

            return bytes.ToArray();
        }

        private void ParseBytes()
        {
            while (currentByte.Length >= 8)
            {
                string b = currentByte.Substring(0, 8);
                byte newByte = Convert.ToByte(b, 2);
                currentByte = currentByte.Remove(0, 8);
                bytes.Add(newByte);
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



        #endregion
    }
}
