using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imaging;
using System.Drawing;

namespace TheArtOfData.Art
{
    class OctagonArtGenerator : ArtGenerator
    {
        #region "Fields"

        private string currentByte;
        private List<DataColors> colors;

        #endregion

        #region "Constructors"

        public OctagonArtGenerator()
        {
            colors = new List<DataColors>();
            currentByte = "";
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        private void AddByte(byte data)
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

        public virtual CustomImage GetImage(int colorsPerRow)
        {
            // Flush the remaining data first
            Flush();

            // Create a bitmap
            CustomImage bitmap = new CustomImage(colorsPerRow, Ceil(colors.Count / colorsPerRow));

            // Generate the colors onto the bitmap
            int w = 0, h = 0;
            foreach (DataColors color in colors)
            {
                bitmap.SetPixel(w, h, color);

                w++;
                if (w == colorsPerRow)
                {
                    h++;
                    w = 0;
                }
            }

            bitmap.Optimize();
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

        public override void AddBytes(byte[] data)
        {
            foreach (byte b in data)
            {
                AddByte(b);
            }
        }

        public override CustomImage GetImage(object[] values)
        {
            int colorsPerRow = (int)values[0];
            CustomImage original = GetImage(colorsPerRow);

            int maxWidth = (int)values[1];
            int maxHeight = (int)values[2];

            if (maxWidth > maxHeight)
                maxWidth = maxHeight;

            while (maxWidth % 6 != 0 && maxWidth % 3 != 0)
                maxWidth--;

            int squareSize = maxWidth / colorsPerRow;
            int spaceingWidth = squareSize / 6 / 3;


            original.Scale(squareSize);

            int angleWidth = squareSize / 3;

            for (int x = 0; x < original.Width; x++)
            {
                for (int y = 0; y < original.Height; y++)
                {
                    if (x % squareSize < spaceingWidth || x % squareSize > squareSize - spaceingWidth || y % squareSize < spaceingWidth || y % squareSize > squareSize - spaceingWidth)
                    {
                        original.SetPixel(x, y, Color.Transparent);
                    }
                    if (x % squareSize < angleWidth - y % squareSize || x % squareSize > (squareSize - angleWidth) + y % squareSize ||
                       x % squareSize < -angleWidth * 2 + y % squareSize || x % squareSize > (squareSize + angleWidth * 2) - y % squareSize)
                    {
                        original.SetPixel(x, y, Color.Transparent);
                    }
                }
            }

            original.Optimize();
            return original;
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
