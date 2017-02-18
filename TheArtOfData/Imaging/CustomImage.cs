using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imaging
{
    public class CustomImage
    {
        #region "Fields"

        protected int width;
        protected int height;
        protected uint[] pixels;

        private uint transparent;

        #endregion

        #region "Constructors"

        protected CustomImage()
        {

        }

        /// <summary>
        /// Creates a new instance of an image
        /// </summary>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        public CustomImage(int width, int height)
        {
            pixels = new uint[width * height];
            this.width = width;
            this.height = height;

            transparent = ConvertColorToInt(Color.Transparent);
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = transparent;
            }
        }

        /// <summary>
        /// The copy constructor for the custom image class
        /// </summary>
        /// <param name="source">The original image to copy</param>
        public CustomImage(CustomImage source)
        {
            this.width = source.width;
            this.height = source.height;
            //this.pixels = source.pixels;
            this.pixels = new uint[source.pixels.Length];
            for (int i = 0; i < source.pixels.Length; i++)
            {
                pixels[i] = source.pixels[i];
            }
            transparent = ConvertColorToInt(Color.Transparent);
        }

        /// <summary>
        /// Creates a custom image class from a standard GDI+ image
        /// </summary>
        /// <param name="source">A standard GDI+ image</param>
        public CustomImage(Image source)
        {
            this.width = source.Width;
            this.height = source.Height;
            pixels = new uint[width * height];
            transparent = ConvertColorToInt(Color.Transparent);
            ParseGDImage(source);
        }

        #endregion

        #region "Properties"

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

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

        protected uint ConvertColorToInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B << 0));
        }

        protected Color ConvertIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        protected uint ConvertBytesToInt(byte r, byte g, byte b, byte a)
        {
            return (uint)((a << 24) | (r << 16) | (g << 8) | (b << 0));
        }

        /// <summary>
        /// Gets a System.Drawing.Image from the custom Image class
        /// </summary>
        /// <returns></returns>
        public Image GetDrawableImage()
        {
            if (width <= 0 || height <= 0)
                return new Bitmap(1, 1);

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
            //System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set the values of the bitmap
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = GetPixel(x, y);
                    rgbValues[index] = c.B;
                    rgbValues[index + 1] = c.G;
                    rgbValues[index + 2] = c.R;
                    rgbValues[index + 3] = 255;
                    index += 4;
                }
            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        /// <summary>
        /// Gets a System.Drawing.Image from the custom Image class. Each pixel is drawed as x pixels (if scale is 5 exery pixel of the image is drawn as a 5x5 square)
        /// </summary>
        /// <param name="scale">The scale of each pixel</param>
        /// <returns>A drawable image</returns>
        public Image GetDrawableImageScaled(int scale)
        {
            CustomImage newImage = new CustomImage(width * scale, height * scale);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int newX = x * scale; newX < x * scale + scale; newX++)
                    {
                        for (int newY = y * scale; newY < y * scale + scale; newY++)
                        {
                            //newImage.pixels[newY * newImage.width + newY] = pixels[y * width + x];
                            newImage.SetPixel(newX, newY, GetPixel(x, y));
                        }
                    }
                }
            }

            return newImage.GetDrawableImage();
        }

        public Image GetDrawableImageScaled(int maxWidth = -1, int maxHeight = -1)
        {
            if (maxWidth == -1 && maxHeight == -1)
                return GetDrawableImageScaled(1);

            // Calculate the size per pixel
            int pixelWidth = 0;
            int pixelHeight = 0;
            if (maxWidth != -1)
            {
                pixelWidth = maxWidth / width;
            }
            if (maxHeight != -1)
            {
                pixelHeight = maxHeight / height;
            }

            if (maxWidth == -1)
                pixelWidth = pixelHeight;
            else if (maxHeight == -1)
                pixelHeight = pixelWidth;

            // Compare the pixel width and height
            if (pixelWidth < pixelHeight)
                pixelHeight = pixelWidth;
            else if (pixelHeight < pixelWidth)
                pixelWidth = pixelHeight;

            return GetDrawableImageScaled(pixelWidth);
        }

        /// <summary>
        /// Generates a binary image from the current Image according to the treshold value. All under the treshold will be black; otherwise white.
        /// With the r,g,b parameters specific colors can be excluded from the treshold calculation
        /// </summary>
        /// <param name="threshold">The treshold value. Can be an integer value between 0 and 255. All under the treshold will be black; otherwise white</param>
        /// <param name="red">This parameters excludes the red colors from the treshold calculation</param>
        /// <param name="green">This parameters excludes the green colors from the treshold calculation</param>
        /// <param name="blue">This parameters excludes the blue colors from the treshold calculation</param>
        /// <returns>A new instance of an Image containing a binary image</returns>
        public CustomBinaryImage GetBinaryImage(int threshold, bool red = true, bool green = true, bool blue = true)
        {
            return CustomBinaryImage.CreateFromImage(this, threshold, red, green, blue);
        }

        /// <summary>
        /// Rotates the image with 90, 180 of 270 degrees
        /// </summary>
        /// <param name="rotation">The angle of the rotation</param>
        public void RotateImage(int rotation)
        {
            // Check if we can actually rotate the image
            if (rotation != 90 && rotation != 180 && rotation != 270)
            {
                return;
            }

            // Set new boundaries for the new image
            CustomImage newImage = CreateImageScales(this, rotation);

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

        private CustomImage CreateImageScales(CustomImage original, int rotation)
        {
            if (rotation == 90 || rotation == 270)
                return new CustomImage(original.height, original.width);

            return new CustomImage(original.width, original.height);
        }

        private void Perform90DegreesRotation(ref CustomImage image)
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

        private void Perform180DegreesRotation(ref CustomImage image)
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

        private void Perform270DegreesRotation(ref CustomImage image)
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

        private void ParseGDImage(Image source)
        {
            // Create a new bitmap.
            Bitmap bmp = new Bitmap(source);

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
            int index = 0, x = 0, y = 0;
            while (index < rgbValues.Length)
            {
                byte b = rgbValues[index];
                byte g = rgbValues[index + 1];
                byte r = rgbValues[index + 2];
                byte a = rgbValues[index + 3];
                pixels[y * width + x] = ConvertBytesToInt(r, g, b, a);
                index += 4;

                x++;
                if (x == width)
                {
                    x = 0;
                    y++;
                }
            }

            // Copy the RGB values back to the bitmap
            //System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
        }

        /// <summary>
        /// Optimizes the image (removes all transparent pixels where possible
        /// </summary>
        public void Optimize()
        {
            // Check the rows for transparency
            int top = 0, bottom = 0, left = 0, right = 0;
            bool _do0 = true, _do1 = true;
            for (int i = 0; i < height; i++)
            {
                if (_do0)
                {
                    _do0 = IsRowTransparent(i);
                    top += _do0 ? 1 : 0;
                }
                if (_do1)
                {
                    _do1 = IsRowTransparent(height - 1 - i);
                    bottom += _do1 ? 1 : 0;
                }
            }

            _do0 = true;
            _do1 = true;

            // Check the columns for transparency
            for (int i = 0; i < width; i++)
            {
                if (_do0)
                {
                    _do0 = IsColumnTransparent(i);
                    left += _do0 ? 1 : 0;
                }
                if (_do1)
                {
                    _do1 = IsColumnTransparent(width - 1 - i);
                    right += _do1 ? 1 : 0;
                }
            }

            // Crop the image
            Crop(top, bottom, left, right);
        }

        private bool IsRowTransparent(int row)
        {
            for (int x = 0; x < width; x++)
            {
                if (pixels[row * width + x] != transparent)
                    return false;
            }
            return true;
        }

        private bool IsColumnTransparent(int column)
        {
            for (int y = 0; y < height; y++)
            {
                if (pixels[y * width + column] != transparent)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Crops the image from each side with the given amount of pixels
        /// </summary>
        /// <param name="top">The amount of pixel to cut from the top</param>
        /// <param name="bottom">The amount of pixel to cut from the bottom</param>
        /// <param name="left">The amount of pixel to cut from the left</param>
        /// <param name="right">The amount of pixel to cut from the right</param>
        public void Crop(int top, int bottom, int left, int right)
        {
            CustomImage cropped = new CustomImage(width - left - right, height - top - bottom);
            int newX = 0, newY = 0;
            for (int y = top; y < height - bottom; y++)
            {
                for (int x = left; x < width - right; x++)
                {
                    cropped.pixels[newY * cropped.width + newX] = pixels[y * width + x];
                    newX++;
                    if (newX >= cropped.width)
                    {
                        newX = 0;
                        newY++;
                    }
                }
            }

            pixels = cropped.pixels;
            width = cropped.width;
            height = cropped.height;
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"

        public static implicit operator Image(CustomImage img)
        {
            return img.GetDrawableImage();
        }

        public static implicit operator Bitmap(CustomImage img)
        {
            return new Bitmap(img.GetDrawableImage());
        }

        #endregion
    }
}
