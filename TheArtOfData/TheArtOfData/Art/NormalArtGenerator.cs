using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imaging;

namespace TheArtOfData.Art
{
    class NormalArtGenerator : ArtGenerator
    {
        #region "Fields"

        private string currentByte;
        private List<DataColors> colors;

        #endregion

        #region "Constructors"

        public NormalArtGenerator()
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
            return GetImage((int)values[0]);
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
