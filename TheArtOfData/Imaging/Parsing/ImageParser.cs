using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ImageProcessor;
using System.IO;
using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
using Imaging.Helpers;
using System.Drawing.Drawing2D;

namespace Imaging.Parsing
{
    public class ImageParser
    {
        private int pixelsPerRow = 10;
        private CustomImage customImage;
        private CustomBinaryImage binImg;

        //public Image image { get; private set; }

        public Image image
        {
            get { return customImage; }
            set { customImage = value; }
        }

        public ImageParser(string filename)
        {
            image = Image.FromFile(filename);
        }

        public ImageParser(Image image)
        {
            this.image = image;
        }

        public Image Run()
        {
            try
            {
                Crop();
                //ChangeBrightnessAndContrast();
                Straighten();
                //pixelsPerRow = GetPixelsFromImage(image);
                InterlaceData.INSTANCE.PixelsPerRow = pixelsPerRow;
                Read_new();
                return image;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }

        private int GetPixelsFromImage(Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            // Get the pixel width from left to right
            int pixelWidth = GetPixelWidth(new Point(0, 20), new Point(1, 0), bitmap);
            // Get the pixel width from top to bottom
            int pixelHeight = GetPixelWidth(new Point(20, 0), new Point(0, 1), bitmap);

            int pixelSize = (pixelWidth + pixelHeight) / 2;

            // Get the pixel width from left to right
            pixelWidth = GetPixelWidth(new Point(0, pixelSize), new Point(1, 0), bitmap);
            // Get the pixel width from top to bottom
            pixelHeight = GetPixelWidth(new Point(pixelSize, 0), new Point(0, 1), bitmap);

            pixelSize = (pixelWidth + pixelHeight) / 2;

            return (int)Math.Round((double)image.Width / pixelSize);
        }

        private int GetPixelWidth(Point start, Point direction, Bitmap bitmap)
        {
            List<int> runningMedian = new List<int>();

            // Plot a surface profile
            List<int> surface = new List<int>();

            Point nextPoint = start;
            int lastValue = -1;
            while (nextPoint.X < bitmap.Width && nextPoint.Y < bitmap.Height)
            {
                Color c = bitmap.GetPixel(nextPoint.X, nextPoint.Y);
                runningMedian.Add(lastValue - (c.R + c.G + c.B));
                lastValue = (c.R + c.G + c.B);

                if (runningMedian.Count > 10)
                {
                    runningMedian.RemoveAt(0);
                    surface.Add((int)runningMedian.Average());
                }

                nextPoint = new Point(nextPoint.X + direction.X, nextPoint.Y + direction.Y);
            }

            // Find the extremes
            Dictionary<int, int> extremes = new Dictionary<int, int>();
            for (int i = 0; i < surface.Count; i++)
            {
                if (surface[i] > 5 || surface[i] < -5)
                {
                    extremes.Add(i, surface[i]);
                    while (i < surface.Count && surface[i] != 0)
                    {
                        i++;
                    }
                }
            }

            // Get the indexes
            int[] indexes = extremes.Select(x => x.Key).ToArray();

            // Calculate the differences between each color
            List<int> differences = new List<int>();
            if (indexes.Length > 0)
                differences.Add(indexes[0]);
            for (int i = 1; i < indexes.Length; i++)
            {
                differences.Add(indexes[i] - indexes[i - 1]);
            }
            if (indexes.Length > 0)
                differences.Add(surface.Count - indexes[indexes.Length - 1]);

            return Median(differences);
        }

        private int Median(List<int> range)
        {
            range.Sort();
            int mid = range.Count / 2;
            if (range.Count % 2 == 0)
                return range[mid];
            else
                return (range[mid] + range[mid + 1]) / 2;
        }

        private void Crop()
        {
            // Create a color corrected version of the image by setting the V (value) from the HSV colors to 0.5f
            CustomImage colorCorrection = new CustomImage(customImage.Width, customImage.Height);
            for (int x = 0; x < customImage.Width; x++)
            {
                for (int y = 0; y < customImage.Height; y++)
                {
                    Color rgb_color = customImage.GetPixel(x, y);

                    HSLColor hsl_color = rgb_color.ToHSL();
                    if (hsl_color.Luminosity <= 0.05f)
                        colorCorrection.SetPixel(x, y, Color.Black);
                    else
                    {
                        HSVColor hsv_color = rgb_color.ToHSV();
                        hsv_color.Value = 0.5f;
                        colorCorrection.SetPixel(x, y, hsv_color.ToRGB().ToGrayscale());
                    }
                }
            }

            // Get a binary image from the color corrected image (102 = 0.4f)
            binImg = colorCorrection.GetBinaryImage(102);

            // Find the highest and lowest pixels on the x and y axis
            int top, bottom, left, right;
            binImg.FindOutline(out top, out bottom, out left, out right);

            // Crop the actual image
            customImage.Crop(top, customImage.Height - bottom, left, customImage.Width - right);
            customImage.GetDrawableImage().Save(@"D:\Developments\Git\TheArtOfData\Test images\output.jpg");

            binImg.Crop(top, binImg.Height - bottom, left, binImg.Width - right);
            //binImg.GetDrawableImage().Save(@"D:\Developments\Git\TheArtOfData\Test images\outputbin.jpg");

            // Set some debug information
            InterlaceData.INSTANCE.StartPosition = new Point(left, top);
            InterlaceData.INSTANCE.TotalImageWidth = right - left;
            InterlaceData.INSTANCE.CropInfo = new Rectangle(left, top, right - left, bottom - top);

            //image = customImage;
        }

        private void Straighten()
        {
            List<Point> blackPixelsFirstRow = new List<Point>();

            for (int x = 0; x < binImg.Width; x++)
            {
                string name = binImg.GetPixel(x, 0).Name;
                if (binImg.GetPixel(x, 0).Name == "ff000000")
                {
                    blackPixelsFirstRow.Add(new Point(x, 0));
                }
            }

            if (blackPixelsFirstRow.Count == 0)
                return;

            int horizontal = 0, vertical = 0;
            if (blackPixelsFirstRow[0].X < binImg.Width / 2)
            {
                GetRotationHeightRight(out horizontal, out vertical);
            }
            else if (blackPixelsFirstRow[blackPixelsFirstRow.Count - 1].X > binImg.Width / 2)
            {
                GetRotationHeightLeft(out horizontal, out vertical);
            }



            customImage.Shear(horizontal, vertical);
            Crop();

            //customImage.GetDrawableImage().Save(@"D:\Developments\Git\TheArtOfData\Test images\output.jpg");
        }

        private void GetRotationHeightRight(out int h, out int v)
        {
            Color current = Color.FromArgb(255, 255, 255);
            int vertical = 0;
            while (current.Name == "ffffffff")
            {
                current = binImg.GetPixel(binImg.Width - binImg.Width / 20, vertical);
                vertical++;
            }

            current = Color.FromArgb(255, 255, 255);
            int horizontal = 0;
            while (current.Name == "ffffffff")
            {
                current = binImg.GetPixel(horizontal, binImg.Width / 20);
                horizontal++;
            }
            v = -vertical;
            h = -horizontal;
        }

        private void GetRotationHeightLeft(out int h, out int v)
        {
            Color current = Color.FromArgb(255, 255, 255);
            int vertical = 0;
            while (current.Name == "ffffffff")
            {
                current = binImg.GetPixel(binImg.Width / 20, vertical);
                vertical++;
            }

            current = Color.FromArgb(255, 255, 255);
            int horizontal = 0;
            while (current.Name == "ffffffff")
            {
                current = binImg.GetPixel(horizontal, binImg.Height - binImg.Width / 20);
                horizontal++;
            }
            v = vertical;
            h = horizontal;
        }

        private void ChangeBrightnessAndContrast()
        {

            ImageFactory imageFactory = new ImageFactory();
            imageFactory.Load(image);

            imageFactory.Brightness(5);
            imageFactory.Contrast(5);

            image = imageFactory.Image;

            //imageFactory.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "resize.bmp"));
            //image = Image.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "resize.bmp"));
            //Display();
        }

        private void Read()
        {
            Bitmap bmp = new Bitmap(image);
            List<Color> pixels = new List<Color>();
            int step = bmp.Width / pixelsPerRow;
            int steps = 0;
            int x = step / 2;
            int y = x;

            while (true)
            {
                pixels.Add(correctColor(bmp.GetPixel(x, y)));
                x += step;
                steps++;
                if (steps > pixelsPerRow - 1)
                {
                    y += step;
                    x = step / 2;
                    steps = 0;
                }
                if (y > bmp.Height) break;
            }

            ImageFromList(pixels, 0);
        }

        private void Read_new()
        {
            //Bitmap bmp = new Bitmap(image);
            List<Color> pixels = new List<Color>();
            List<Color> pixels2 = new List<Color>();
            int step = customImage.Width / pixelsPerRow;
            int steps = 0;
            int x = step / 2;
            int y = x;

            while (true)
            {
                Color color = customImage.GetPixel(x, y);
                pixels.Add(correctColor(color));
                float b = color.GetBrightness(), s = color.GetSaturation();
                if (b == 1f && s == 0f)
                    pixels2.Add(Color.White);
                else
                {
                    DataColors col = color.GetHue();
                    pixels2.Add(col);
                }

                x += step;
                steps++;
                if (steps > pixelsPerRow - 1)
                {
                    y += step;
                    x = step / 2;
                    steps = 0;
                }
                if (y > customImage.Height) break;
            }

            ImageFromList(pixels, 0);
            ImageFromList(pixels2, 1);

        }

        private Color correctColor(Color color)
        {
            Color colorToReturn = Color.White;
            List<Color> colors = new List<Color>();
            colors.Add(DataColors.Red);
            colors.Add(DataColors.Blue);
            colors.Add(DataColors.Green);
            colors.Add(DataColors.Yellow);
            //colors.Add(DataColors.Black);
            colors.Add(DataColors.Magenta);
            colors.Add(DataColors.Cyan);
            colors.Add(DataColors.Orange);
            //colors.Add(DataColors.Gray);
            colors.Add(DataColors.Purple);
            colors.Add(Color.White);


            Rgb colorrgb = new Rgb { R = color.R, G = color.G, B = color.B };

            double deltaC = 99999999999;

            foreach (Color c in colors)
            {
                Rgb cRgb = new Rgb { R = c.R, G = c.G, B = c.B };

                double deltaE = cRgb.Compare(colorrgb, new Cie1976Comparison());
                if (deltaE < deltaC)
                {
                    deltaC = deltaE;

                    if (c.Equals(Color.White))
                    {
                        colorToReturn = Color.Transparent;
                        continue;
                    }
                    colorToReturn = c == DataColors.Purple ? (Color)DataColors.Blue : c;
                }

            }

            return colorToReturn;
        }



        private int doubleToIntCeiling(double v)
        {
            return (int)(v + 1);
        }

        private void ImageFromList(List<Color> list, int i)
        {
            CustomImage img = new CustomImage(pixelsPerRow, list.Count / pixelsPerRow);

            int pixel = 0;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    if (pixel < list.Count)
                    {
                        img.SetPixel(x, y, list[pixel]);
                        pixel++;
                    }
                    else
                        img.SetPixel(x, y, Color.Transparent);
                }
            }

            img.GetDrawableImageScaled(10).Save(@"D:\Developments\Git\TheArtOfData\Test images\putput" + i + ".jpg");
            //Bitmap img = new Bitmap(pixelsPerRow, ((int)(list.Count / pixelsPerRow)));

            //int pixel = 0;
            //for (int y = 0; y < img.Height; y++)
            //{
            //    for (int x = 0; x < img.Width; x++)
            //    {
            //        img.SetPixel(x, y, list.ElementAt(pixel));
            //        if (pixel == list.Count - 1) break;
            //        else pixel++;
            //    }
            //    if (pixel > list.Count) break;
            //}

            image = img;
            //image.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "i.bmp"));
        }

        //private void Display(Image i)
        //{
        //    Form form = new Form();
        //    form.StartPosition = FormStartPosition.CenterScreen;
        //    form.Size = new Size(800, 600);
        //    form.Text = "Debug image";

        //    PictureBox pb = new PictureBox();
        //    pb.SizeMode = PictureBoxSizeMode.Zoom;
        //    pb.Dock = DockStyle.Fill;
        //    pb.Image = i;

        //    form.Controls.Add(pb);
        //    form.ShowDialog();
        //}
    }
}
