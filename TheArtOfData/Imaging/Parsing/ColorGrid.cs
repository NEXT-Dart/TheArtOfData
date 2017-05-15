using Imaging.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imaging.Parsing
{
    class ColorGrid
    {
        #region "Fields"

        private List<ColorArea> areas;
        private CustomImage image;
        private List<Point> pixelsToDo;
        private Point rotation;

        #endregion

        #region "Constructors"

        public ColorGrid(CustomImage image, Point rotation)
        {
            areas = new List<ColorArea>();
            pixelsToDo = new List<Point>();
            image.Optimize(Color.White);
            this.rotation = rotation;
            this.image = image;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    pixelsToDo.Add(new Point(x, y));
                }
            }
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public void ParseImage()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int y = 0; y < image.Height; y++)
            {
                Stopwatch sw1 = new Stopwatch();
                sw1.Start();
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    if (pixel == Color.FromArgb(255, 255, 255))
                        continue;

                    DataColors pixelColor = pixel.GetHue();
                    ColorArea[] areas = this.areas.Where(a => a.Color == pixelColor).Where(b => b.HasNeighbour(new Point(x, y))).ToArray();
                    if (areas.Length == 0)
                    {
                        ColorArea area = new ColorArea(pixelColor);
                        this.areas.Add(area);
                        area.AddPixel(new Point(x, y));
                    }
                    else if (areas.Length == 1)
                    {
                        areas[0].AddPixel(new Point(x, y));
                    }
                    else
                    {
                        for (int i = 1; i < areas.Length; i++)
                        {
                            areas[0].Merge(areas[i]);
                        }
                    }
                }
                sw1.Stop();
                Debug.WriteLine(sw1.ElapsedMilliseconds);
            }

            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds + " ms");

            CustomImage newImg = new CustomImage(image.Width, image.Height);

            //foreach (ColorArea area in areas)
            {
                areas[0].DrawOnImage(ref newImg);
            }

            newImg.GetDrawableImageScaled(10).Save(@"D:\Developments\Git\TheArtOfData\Test images\parsedArea.png");
        }

        private void AddColorArea(int x, int y)
        {
            ColorArea ca = new ColorArea(image, x, y);
            ca.Parse();
            areas.Add(ca);

            foreach (Point p in ca.Pixels)
            {
                pixelsToDo.Remove(p);
            }
        }

        public void ParseImageNew()
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color c = image.GetPixel(x, y);
                    if (c == Color.FromArgb(255, 255, 255))
                    {
                        continue;
                    }

                    float hue = c.GetHue();
                    GetArea(hue).AddPixel(new Point(x, y));
                }
            }

            // Optimze the areas
            List<ColorArea> optimizedAreas = new List<ColorArea>();
            foreach (ColorArea area in areas)
            {
                ColorArea[] optimized = area.Optimize();
                optimizedAreas.AddRange(optimized);
            }

            optimizedAreas.RemoveAll(x => x.Pixels.Length <= 2);
            optimizedAreas = optimizedAreas.OrderByDescending(x => x.Pixels.Length).ToList();
            double median = optimizedAreas.Take(optimizedAreas.Count / 3).Average(x => x.Pixels.Length);
            optimizedAreas.RemoveAll(x => x.Pixels.Length < median);
            optimizedAreas = optimizedAreas.OrderBy(y => y.Start.Y).ThenBy(x => x.Start.X).ToList();

            areas = optimizedAreas;
        }

        private ColorArea GetArea(DataColors color)
        {
            ColorArea area = areas.FirstOrDefault(x => x.Color == color);

            if (area == null)
            {
                area = new ColorArea(color);
                areas.Add(area);
            }
            return area;
        }

        public CustomImage CreateGrid(out int width, out int height)
        {
            // Calculate Width and Height by finding the (5) most occuring values for width and height and get an average from it
            Dictionary<int, int> wOccurence = new Dictionary<int, int>();
            foreach (ColorArea area in areas)
            {
                if (!wOccurence.ContainsKey(area.Width))
                    wOccurence.Add(area.Width, 1);
                else
                    wOccurence[area.Width] = wOccurence[area.Width] + 1;
            }
            width = (int)wOccurence.OrderByDescending(x => x.Value).Take(5).Average(a => a.Key);

            Dictionary<int, int> hOccurence = new Dictionary<int, int>();
            foreach (ColorArea area in areas)
            {
                if (!hOccurence.ContainsKey(area.Height))
                    hOccurence.Add(area.Height, 1);
                else
                    hOccurence[area.Height] = hOccurence[area.Height] + 1;
            }
            height = (int)hOccurence.OrderByDescending(x => x.Value).Take(5).Average(a => a.Key);

            //Point tl = new Point(binimg.Width, binimg.Height);
            //Point tr = new Point(0, binimg.Height);
            //Point bl = new Point(binimg.Width, 0);
            //Point br = new Point(0, 0);
            //binimg.FindCorners(rotation, ref tl, ref tr, ref bl, ref br);

            CustomImage newImg = new CustomImage(image.Width, image.Height);

            foreach (ColorArea area in areas)
            {
                area.DrawOnImage(ref newImg);
            }

            //newImg.SetPixel(tl.X, tl.Y, Color.Black);
            //newImg.SetPixel(tr.X, tr.Y, Color.Black);
            //newImg.SetPixel(bl.X, bl.Y, Color.Black);
            //newImg.SetPixel(br.X, br.Y, Color.Black);

            newImg.GetDrawableImage().Save(@"D:\Developments\Git\TheArtOfData\Test images\parsedArea.png");

            return newImg;
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
