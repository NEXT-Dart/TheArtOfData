using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ImageProcessor;
using System.Windows.Forms;
using System.IO;
using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;

namespace TheArtOfDecoding
{
    class ImageParser
    {
        private int pixelsPerRow = 10;

        public Image image { get; private set; }

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
                ChangeBrightnessAndContrast();
                //Straighten();
                pixelsPerRow = GetPixelsFromImage(image);
                InterlaceData.INSTANCE.PixelsPerRow = pixelsPerRow;
                Read();
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private int GetPixelsFromImage(Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            List<int> runningMedian = new List<int>();

            // Plot a surface profile from left to right
            List<int> LTR = new List<int>();
            int height = 50;
            int lastValue = -1;
            for (int i = 0; i < bitmap.Width; i++)
            {
                Color c = bitmap.GetPixel(i, height);
                runningMedian.Add(lastValue - (c.R + c.G + c.B));
                lastValue = (c.R + c.G + c.B);

                if (runningMedian.Count > 10)
                {
                    runningMedian.RemoveAt(0);
                    LTR.Add((int)runningMedian.Average());
                }
            }

            // Find the extremes
            Dictionary<int, int> extremes = new Dictionary<int, int>();
            for (int i = 0; i < LTR.Count; i++)
            {
                if (LTR[i] > 5 || LTR[i] < -5)
                {
                    extremes.Add(i, LTR[i]);
                    while (i < LTR.Count && LTR[i] != 0)
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
            differences.Add(LTR.Count - indexes[indexes.Length - 1]);

            int pixelWidth = Median(differences);

            string[] lines = differences.Select(x => x.ToString()).ToArray();
            File.WriteAllLines(@"D:\Users\Bas\Desktop\Knipsel.txt", lines);

            //go from top to bottom. Check most common heigth
            return (int)Math.Round((double)image.Width / pixelWidth);
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
            //ImageFactory imageFactory = new ImageFactory();
            //imageFactory.Load(image);
            //imageFactory.EntropyCrop();
            //image = imageFactory.Image;


            image = ImageCrop.Crop(image);


            image.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "crop.bmp"));
            //image = Image.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "crop.bmp"));
            //Display();


        }

        private void ChangeBrightnessAndContrast()
        {

            ImageFactory imageFactory = new ImageFactory();
            imageFactory.Load(image);

            imageFactory.Brightness(5);
            imageFactory.Contrast(5);

            image = imageFactory.Image;

            imageFactory.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "resize.bmp"));
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

            ImageFromList(pixels);
        }

        private Color correctColor(Color color)
        {
            Color colorToReturn = Color.White;
            List<Color> colors = new List<Color>();
            colors.Add(DataColors.Red);
            colors.Add(DataColors.Blue);
            colors.Add(DataColors.Green);
            colors.Add(DataColors.Black);
            colors.Add(DataColors.Magenta);
            colors.Add(DataColors.Cyan);
            colors.Add(DataColors.Orange);
            colors.Add(DataColors.Gray);
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

        private void ImageFromList(List<Color> list)
        {
            Bitmap img = new Bitmap(pixelsPerRow, ((int)(list.Count / pixelsPerRow)));

            int pixel = 0;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    img.SetPixel(x, y, list.ElementAt(pixel));
                    if (pixel == list.Count - 1) break;
                    else pixel++;
                }
                if (pixel > list.Count) break;
            }

            image = img;
            image.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "i.bmp"));
        }

        private void Display(Image i)
        {
            Form form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(800, 600);
            form.Text = "Debug image";

            PictureBox pb = new PictureBox();
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Dock = DockStyle.Fill;
            pb.Image = i;

            form.Controls.Add(pb);
            form.ShowDialog();
        }
    }
}
