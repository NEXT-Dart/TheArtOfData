using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData
{
    class ImageDataWriter
    {
        #region "Fields"

        private string currentByte;
        private List<DataColors> colors;

        #endregion

        #region "Constructors"

        public ImageDataWriter()
        {
            colors = new List<DataColors>();
            currentByte = "";
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public void AddByte(byte data)
        {
            currentByte += GetBinaryFromByte(data);

            ParseColors();
        }

        private void ParseColors()
        {
            while (currentByte.Length >= 3)
            {
                DataColors color = Convert.ToInt32(currentByte.Substring(0, 3), 2);
                currentByte = currentByte.Remove(0, 3);

                colors.Add(color);
            }
        }

        public void AddString(string data)
        {
            AddBytes(Encoding.ASCII.GetBytes(data));
        }

        public void AddBytes(byte[] data)
        {
            foreach (byte b in data)
            {
                AddByte(b);
            }
        }

        private string GetBinaryFromByte(byte data)
        {
            string binary = Convert.ToString(data, 2);
            while (binary.Length < 8)
            {
                binary = "0" + binary;
            }

            return binary;
        }

        private void Flush()
        {
            while (currentByte.Length % 3 != 0)
            {
                currentByte += "0";
            }

            ParseColors();
        }

        public Image GetImage(int colorsPerRow)
        {
            // Flush the remaining data first
            Flush();

            // Create a bitmap
            // max. 150x212
            // A4 = 2480 X 3508 pixels
            // remove margin: 2200 X 3300 pixels
            //const int documentWidth = 700;
            //const int documentHeight = 1025;
            const int documentWidth = 300;
            const int documentHeight = 300;
            int colorSize = documentWidth / colorsPerRow;
            Bitmap bitmap = new Bitmap(documentWidth + 100, documentHeight + 10);

            // Generate the colors onto the bitmap
            int w = 0, h = 0;
            try
            {
                foreach (DataColors color in colors)
                {
                    for (int i = 0; i < colorSize; i++)
                    {
                        for (int j = 0; j < colorSize; j++)
                        {
                            bitmap.SetPixel(w * colorSize + i, h * colorSize + j, color);
                        }
                    }

                    w++;
                    if (w == colorsPerRow)
                    {
                        h++;
                        w = 0;
                    }
                }
            }
            catch (Exception)
            {

            }

            return bitmap;
        }

        private int Ceil(float value)
        {
            return (int)(value + 1);
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
