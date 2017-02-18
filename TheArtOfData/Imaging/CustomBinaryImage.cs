using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imaging
{
    class CustomBinaryImage : CustomImage
    {
        #region "Fields"

        private uint black;
        private uint white;

        #endregion

        #region "Constructors"

        protected CustomBinaryImage(CustomImage source) : base(source)
        {
            black = ConvertColorToInt(Color.Black);
            white = ConvertColorToInt(Color.White);
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        private void CreateBinary(int threshold, bool red = true, bool green = true, bool blue = true)
        {
            int r = red ? 1 : 0;
            int g = green ? 1 : 0;
            int b = blue ? 1 : 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color c = ConvertIntToColor(pixels[y * width + x]);
                    int gray = (c.R * r + c.G * g + c.B * b) / (r + g + b);

                    pixels[y * width + x] = gray < threshold ? black : white;
                }
            }
        }

        /// <summary>
        /// Merges 2 images of the same size together
        /// </summary>
        /// <param name="merge">The image to merge with this one</param>
        /// <param name="mergeBlack">Indicator to merge te black or white pixels</param>
        /// <returns>A new CustomBinaryImage with the merged pixels</returns>
        public CustomBinaryImage Merge(CustomBinaryImage merge, bool mergeBlack = true)
        {
            if (merge.width != this.width || merge.height != this.height)
                return this;

            CustomBinaryImage merged = new CustomBinaryImage(this);

            for (int x = 0; x < merged.width; x++)
            {
                for (int y = 0; y < merged.height; y++)
                {
                    if (mergeBlack)
                    {
                        if(merged.pixels[y * merged.width + x] != white)
                        {
                            merged.pixels[y * merged.width + x] = merge.pixels[y * merged.width + x];
                        }
                    }
                    else
                    {
                        if(merged.pixels[y * merged.width + x] != black)
                        {
                            merged.pixels[y * merged.width + x] = merge.pixels[y * merged.width + x];
                        }
                    }
                }
            }

            return merged;
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"

        public static CustomBinaryImage CreateFromImage(CustomImage source, int threshold, bool red = true, bool green = true, bool blue = true)
        {
            CustomBinaryImage binary = new CustomBinaryImage(source);

            binary.CreateBinary(threshold, red, green, blue);

            return binary;
        }

        #endregion

        #region "Operators"



        #endregion
    }
}
