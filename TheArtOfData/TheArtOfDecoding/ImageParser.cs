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
            Crop();
            ChangeBrightnessAndContrast();
            //Straighten();
            //pixelsPerRow = GetPixelsFromImage(image);
            Read();
            return image;
        }

        private int GetPixelsFromImage(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            //go from left to right. Check most common width.
            for (int i = 0; i < image.Width; i++)
            {
                List<Color> pixels = new List<Color>();
                int x = 1;
                int y = x;


                while (true)
                {
                    pixels.Add(correctColor(bmp.GetPixel(x, y)));
                    x++;
                    if (x >= bmp.Width)
                    {
                        break;
                    }
                }

                int currentPixelsFromPixelInImage = 0;
                int minimalPixelsFromPixelInImage = 999999;

                Color tempC = Color.White;
                foreach (Color c in pixels)
                {
                    //change current to minimal or something
                    if (c.Equals(tempC))
                    {
                        currentPixelsFromPixelInImage++;
                    }
                    else
                    {
                        if(currentPixelsFromPixelInImage < minimalPixelsFromPixelInImage)
                        {
                            minimalPixelsFromPixelInImage = currentPixelsFromPixelInImage;
                        }
                    }

                    tempC = c;
                }


                return bmp.Width / minimalPixelsFromPixelInImage;
            }

            //go from top to bottom. Check most common heigth
            return 0;
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
                if (steps > 9)
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
