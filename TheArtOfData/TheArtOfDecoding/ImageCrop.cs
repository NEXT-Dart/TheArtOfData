using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfDecoding
{
    class ImageCrop
    {
        #region "Fields"

        private Image image;

        #endregion

        #region "Constructors"

        private ImageCrop(Image image)
        {
            this.image = image;
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        private Bitmap GetBinaryBitmap()
        {
            Bitmap bitmap = new Bitmap(image);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = data.Stride;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                // Check this is not a null area
                // Go through the draw area and set the pixels as they should be
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int b = ptr[(x * 3) + y * stride];
                        int g = ptr[(x * 3) + y * stride + 1];
                        int r = ptr[(x * 3) + y * stride + 2];

                        const int marge = 35;
                        int avg = (r + b + g) / 3;
                        if (r <= avg + marge && r >= avg - marge &&
                            g <= avg + marge && g >= avg - marge &&
                            b <= avg + marge && b >= avg - marge)
                            avg = 255;
                        else
                            avg = 0;

                        ptr[(x * 3) + y * stride] = (byte)avg;
                        ptr[(x * 3) + y * stride + 1] = (byte)avg;
                        ptr[(x * 3) + y * stride + 2] = (byte)avg;
                    }
                }
            }
            bitmap.UnlockBits(data);
            return bitmap;
        }

        private Image Crop()
        {
            // Get a binary image by removing all gray
            Bitmap binary = GetBinaryBitmap();

            // Get the highest and lowest black value
            BitmapData data = binary.LockBits(new Rectangle(0, 0, binary.Width, binary.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = data.Stride;
            int lowX = -1, lowY = -1, highX = -1, highY = -1;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                // Check this is not a null area
                // Go through the draw area and set the pixels as they should be
                for (int x = 0; x < binary.Width; x++)
                {
                    for (int y = 0; y < binary.Height; y++)
                    {
                        int b = ptr[(x * 3) + y * stride];
                        int g = ptr[(x * 3) + y * stride + 1];
                        int r = ptr[(x * 3) + y * stride + 2];

                        if (r != 255 && g != 255 && b != 255)
                        {
                            if (x < lowX || lowX == -1)
                                lowX = x;
                            if (x > highX)
                                highX = x;
                            if (y < lowY || lowY == -1)
                                lowY = y;
                            if (y > highY)
                                highY = y;
                        }
                    }
                }
            }
            binary.UnlockBits(data);

            // Crop the image
            Image img = CutEdges(lowY, image.Height - highY, lowX, image.Width - highX);

            return img;
        }

        private Image CutEdges(int top, int bottom, int left, int right)
        {
            Bitmap bmpImage = new Bitmap(image);
            return bmpImage.Clone(new Rectangle(left, top, image.Width - left - right, image.Height - top - bottom), bmpImage.PixelFormat);
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"

        public static Image Crop(Image image)
        {
            return new ImageCrop(image).Crop();
        }

        #endregion

        #region "Operators"



        #endregion
    }
}
