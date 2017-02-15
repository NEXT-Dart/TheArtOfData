using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData.Imaging
{
    public class Image
    {
        #region "Fields"

        private int width;
        private int height;
        private uint[] pixels;

        #endregion

        #region "Constructors"

        /// <summary>
        /// Creates a new instance of an image
        /// </summary>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        public Image(int width, int height)
        {
            pixels = new uint[width * height];
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// The copy constructor for the Image class
        /// </summary>
        /// <param name="source">The original image to copy</param>
        public Image(Image source)
        {
            this.width = source.width;
            this.height = source.height;
            this.pixels = source.pixels;
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        /// <summary>
        /// Sets the color of the pixel at the specified location
        /// </summary>
        /// <param name="x">The horizontal position of the pixel</param>
        /// <param name="y">Tje verticla position of the pixel</param>
        /// <param name="color">The new color for the pixel</param>
        public void SetPixel(int x, int y, Color color)
        {
            uint iColor = ConvertColorToInt(color);
            pixels[y * width + x] = iColor;
        }

        /// <summary>
        /// Gets the color of the pixel at the specified location
        /// </summary>
        /// <param name="x">The horizontal position of the pixel</param>
        /// <param name="y">The vertical position of the pixel</param>
        /// <returns>The color at the specified pixel</returns>
        public Color GetPixel(int x, int y)
        {
            return ConvertIntToColor(pixels[y * width + x]);
        }

        private uint ConvertColorToInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) |
                    (color.G << 8) | (color.B << 0));
        }

        private Color ConvertIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Gets a System.Drawing.Image from the custom Image class
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Image GetDrawableImage()
        {
            // Create a new bitmap.
            Bitmap bmp = new Bitmap(width, height);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set the values of the bitmap
            int index = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color c = GetPixel(i, j);
                    rgbValues[index] = c.R;
                    index++;
                    rgbValues[index] = c.G;
                    index++;
                    rgbValues[index] = c.B;
                    index++;
                    rgbValues[index] = 255;
                    index++;
                }
            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        /// <summary>
        /// Generates a binary image from the current Image according to the treshold value. All under the treshold will be black; otherwise white.
        /// With the r,g,b parameters specific colors can be excluded from the treshold calculation
        /// </summary>
        /// <param name="treshold">The treshold value. Can be an integer value between 0 and 255. All under the treshold will be black; otherwise white</param>
        /// <param name="red">This parameters excludes the red colors from the treshold calculation</param>
        /// <param name="green">This parameters excludes the green colors from the treshold calculation</param>
        /// <param name="blue">This parameters excludes the blue colors from the treshold calculation</param>
        /// <returns>A new instance of an Image containing a binary image</returns>
        public Image GetBinaryImage(int treshold, bool red = true, bool green = true, bool blue = true)
        {
            Image binary = new Image(this);
            int r = red ? 1 : 0;
            int g = green ? 1 : 0;
            int b = blue ? 1 : 0;

            uint white = ConvertColorToInt(Color.White), black = ConvertColorToInt(Color.Black);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color c = ConvertIntToColor(pixels[y * width + x]);
                    int gray = (c.R * r + c.G * g + c.B * b) / (r + g + b);

                    binary.pixels[y * width + x] = gray < treshold ? white : black;
                }
            }

            return binary;
        }

        /// <summary>
        /// Rotates the image with 90, 180 of 270 degrees
        /// </summary>
        /// <param name="rotation">The angle of the rotation</param>
        public void RotateImage(int rotation)
        {
            // Check if we can actually rotate the image
            if (rotation != 90 || rotation != 180 || rotation != 270)
            {
                return;
            }

            // Set new boundaries for the new image
            Image newImage = CreateImageScales(this, rotation);

            // Perform the rotation
            switch (rotation)
            {
                case 90:
                    Perform90DegreesRotation(ref newImage);
                    break;
                case 180:
                    Perform180DegreesRotation(ref newImage);
                    break;
                case 270:
                    Perform270DegreesRotation(ref newImage);
                    break;
            }

            // Set the new image as the current
            pixels = newImage.pixels;
            width = newImage.width;
            height = newImage.height;
        }

        private Image CreateImageScales(Image original, int rotation)
        {
            if (rotation == 90 || rotation == 270)
                return new Image(original.height, original.width);

            return new Image(original.width, original.height);
        }

        private void Perform90DegreesRotation(ref Image image)
        {
            // Start reading from left to right in the top left corner and put it back top to down from the top right corner
            int x = 0, y = 0;
            for (int newX = image.width - 1; newX >= 0; newX--)
            {
                for (int newY = 0; newY < image.height; newY++)
                {
                    image.pixels[newY * image.width + newX] = pixels[y * width + x];
                    x++;

                    if (x >= width)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
        }

        private void Perform180DegreesRotation(ref Image image)
        {
            // Start reading from left to right in the top left corner and put it back right to left from the bottom right corner
            int x = 0, y = 0;
            for (int newY = image.height - 1; newY >= 0; newY--)
            {
                for (int newX = image.width - 1; newX >= 0; newX--)
                {
                    image.pixels[newY * image.width + newX] = pixels[y * width + x];
                    x++;

                    if (x >= width)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
        }

        private void Perform270DegreesRotation(ref Image image)
        {
            // Start reading from left to right in the top left corner and put it back bottom to top from the bottom left corner
            int x = 0, y = 0;
            for (int newX = 0; newX >= image.width; newX++)
            {
                for (int newY = image.height - 1; newY >= 0; newY--)
                {
                    image.pixels[newY * image.width + newX] = pixels[y * width + x];
                    x++;

                    if (x >= width)
                    {
                        x = 0;
                        y++;
                    }
                }
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

        public static implicit operator System.Drawing.Image(Image img)
        {
            return img.GetDrawableImage();
        }

        #endregion
    }
}
