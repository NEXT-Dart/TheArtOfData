using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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
            const int documentWidth = 300 * 2;
            const int documentHeight = 430 * 2;
            int colorSize = documentWidth / colorsPerRow;
            //Bitmap bitmap = new Bitmap(documentWidth + 100, documentHeight + 10);
            Imaging.CustomImage bitmap = new Imaging.CustomImage(documentWidth + 100, documentHeight + 10);

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

            bitmap.Optimize();
            bitmap.GetDrawableImage().Save(@"D:\Users\Bas\Desktop\output.png");
            return bitmap;
        }

        public static Bitmap TrimBitmap(Bitmap source)
        {
            Rectangle srcRect = default(Rectangle);
            BitmapData data = null;
            try
            {
                data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] buffer = new byte[data.Height * data.Stride];
                Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
                int xMin = int.MaxValue;
                int xMax = 0;
                int yMin = int.MaxValue;
                int yMax = 0;
                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++)
                    {
                        byte alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha != 0)
                        {
                            if (x < xMin) xMin = x;
                            if (x > xMax) xMax = x;
                            if (y < yMin) yMin = y;
                            if (y > yMax) yMax = y;
                        }
                    }
                }
                if (xMax < xMin || yMax < yMin)
                {
                    // Image is empty...
                    return null;
                }
                srcRect = Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
            }
            finally
            {
                if (data != null)
                    source.UnlockBits(data);
            }

            Bitmap dest = new Bitmap(srcRect.Width, srcRect.Height);
            Rectangle destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            using (Graphics graphics = Graphics.FromImage(dest))
            {
                graphics.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return dest;
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
