using Imaging;
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
        private List<int> colors;
        private string currentByte;

        #endregion

        #region "Constructors"

        public MosaicArtGenerator()
        {
            data = new List<byte>();
            colors = new List<int>();
            currentByte = "";
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

            foreach (byte b in data)
            {
                string binary = Convert.ToString(b, 2);
                while (binary.Length < 8)
                {
                    binary = "0" + binary;
                }
                currentByte += binary;
            }

            while (currentByte.Length > TintedColor.bits)
            {
                string bits = currentByte.Substring(0, TintedColor.bits);
                currentByte = currentByte.Remove(0, TintedColor.bits);
                colors.Add(Convert.ToInt32(bits, 2));
            }
        }

        public override CustomImage GetImage()
        {
            const int bitmapSize = 500;
            int width = (int)(Math.Sqrt(data.Count * 8 / TintedColor.bits) + 1);
            int height = width + 1;
            int pixelSize = bitmapSize / width;
            Bitmap bitmap = new Bitmap(width * pixelSize, height * pixelSize);

            int x = 0, y = 0;
            int margin = (int)(pixelSize * 0.05f);
            foreach (int tint in colors)
            {
                for (int i = margin; i < pixelSize - margin; i++)
                {
                    for (int j = margin; j < pixelSize - margin; j++)
                    {
                        bitmap.SetPixel(x * pixelSize + i, y * pixelSize + j, TintedColor.Get(tint));
                    }
                }
                x++;
                if (x == width)
                {
                    x = 0;
                    y++;
                }
            }

            return new CustomImage(bitmap);// ImageDataWriter.TrimBitmap(bitmap);
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
