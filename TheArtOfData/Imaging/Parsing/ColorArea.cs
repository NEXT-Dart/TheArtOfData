using Imaging.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imaging.Parsing
{
    class ColorArea
    {
        #region "Fields"

        private HashSet<Point> containingPixels;
        private DataColors color;
        private CustomImage image;

        private int width;
        private int height;
        private Point start;

        #endregion

        #region "Constructors"

        public ColorArea(CustomImage image, int x, int y)
        {
            this.image = image;

            containingPixels = new HashSet<Point>();
            containingPixels.Add(new Point(x, y));

            Color pixel = image.GetPixel(x, y);
            color = pixel.GetHue();
        }

        public ColorArea(DataColors color)
        {
            this.color = color;
            containingPixels = new HashSet<Point>();
        }

        #endregion

        #region "Properties"

        public Point[] Pixels
        {
            get { return containingPixels.ToArray(); }
        }

        public DataColors Color
        {
            get { return color; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public Point Start
        {
            get { return start; }
        }

        #endregion

        #region "Methods"

        public void Parse()
        {
            List<Point> alreadyChecked = new List<Point>();
            List<Point> neighboursToCheck = new List<Point>(containingPixels);

            while (neighboursToCheck.Count > 0)
            {
                // Get the first pixel from the todo list
                Point current = neighboursToCheck[0];
                neighboursToCheck.RemoveAt(0);

                // Calculate the neighbours
                Point[] neighbours = new Point[4];
                neighbours[0] = new Point(current.X, current.Y - 1);
                neighbours[1] = new Point(current.X, current.Y + 1);
                neighbours[2] = new Point(current.X - 1, current.Y);
                neighbours[3] = new Point(current.X + 1, current.Y);

                // Check each of the pixels
                foreach (Point neighbour in neighbours)
                {
                    if (!alreadyChecked.Contains(neighbour) && !neighboursToCheck.Contains(neighbour) && !containingPixels.Contains(neighbour))
                    {
                        if (neighbour.X < 0 || neighbour.X >= image.Width || neighbour.Y < 0 || neighbour.Y >= image.Height)
                            continue;

                        Color pixel = image.GetPixel(neighbour.X, neighbour.Y);
                        if (pixel == System.Drawing.Color.FromArgb(255, 255, 255))
                            continue;

                        float hue = pixel.GetHue();
                        if (color.Min > color.Max && (hue >= color.Min || hue <= color.Max))
                        {
                            containingPixels.Add(neighbour);
                            neighboursToCheck.Add(neighbour);
                        }
                        if (hue >= color.Min && hue <= color.Max)
                        {
                            containingPixels.Add(neighbour);
                            neighboursToCheck.Add(neighbour);
                        }
                    }

                    alreadyChecked.Add(neighbour);
                }
            }
        }

        public void AddPixel(Point pixel)
        {
            containingPixels.Add(pixel);
        }

        public void DrawOnImage(ref CustomImage image)
        {
            foreach (Point p in containingPixels)
            {
                image.SetPixel(p.X, p.Y, color);
            }
        }

        public ColorArea[] Optimize()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //// Remove pixels in the center
            //HashSet<Point> borders = new HashSet<Point>();
            //foreach (Point p in containingPixels)
            //{
            //    Point[] neighbours = new Point[4];
            //    neighbours[0] = new Point(p.X + 1, p.Y);
            //    neighbours[1] = new Point(p.X - 1, p.Y);
            //    neighbours[2] = new Point(p.X, p.Y + 1);
            //    neighbours[3] = new Point(p.X, p.Y - 1);

            //    foreach(Point n in neighbours)
            //    {
            //        if(!containingPixels.Contains(n))
            //        {
            //            borders.Add(p);
            //            break;
            //        }
            //    }
            //}

            //containingPixels = borders;

            //containingPixels = containingPixels.OrderBy(y => y.Y).ThenBy(x => x.X).ToList();
            List<ColorArea> foundAreas = new List<ColorArea>();
            while (containingPixels.Count > 0)
            {
                Point start = containingPixels.First();
                containingPixels.Remove(start);
                foundAreas.Add(FindArea(start));

            }

            sw.Stop();
            Debug.WriteLine("Single color: " + sw.ElapsedMilliseconds + " ms");

            foreach (ColorArea area in foundAreas)
            {
                area.CalculateWH();
            }

            return foundAreas.ToArray();
        }

        private void CalculateWH()
        {
            Point top = containingPixels.OrderBy(y => y.Y).ThenBy(x => x.X).First();
            Point bottom = containingPixels.OrderByDescending(y => y.Y).ThenByDescending(x => x.X).First();
            Point left = containingPixels.OrderBy(x => x.X).ThenBy(y => y.Y).First();
            Point right = containingPixels.OrderByDescending(x => x.X).ThenByDescending(y => y.Y).First();

            int pixelW = right.X - top.X;
            int pixelH = right.Y - top.Y;
            width = (int)Math.Sqrt(Math.Pow(pixelW, 2) + Math.Pow(pixelH, 2));

            pixelW = right.X - bottom.X;
            pixelH = right.Y - bottom.Y;
            height = (int)Math.Sqrt(Math.Pow(pixelW, 2) + Math.Pow(pixelH, 2));

            int lowX = top.X;
            if (left.X < lowX)
                lowX = left.X;
            if (bottom.X < lowX)
                lowX = bottom.X;

            int lowY = top.Y;
            if (left.Y < lowY)
                lowY = left.Y;
            if (right.Y < lowY)
                lowY = right.Y;

            start = new Point(lowX, lowY);
        }

        public bool IsInRectangle(Helpers.Rectangle rect)
        {
            foreach(Point p  in containingPixels)
            {
                if (rect.IsInRectangle(p))
                    return true;
            }
            return false;
        }

        private ColorArea FindArea(Point start)
        {
            // Set some start conditions
            Point current;
            ColorArea newArea = new ColorArea(color);
            newArea.containingPixels.Add(start);
            Queue<Point> todo = new Queue<Point>(new Point[] { start });
            List<Point> done = new List<Point>(new Point[] { start });

            // Run through the image
            while (todo.Count > 0)
            {
                // Get the first object from the queue
                current = todo.Dequeue();

                // Generate the points for the neighbours
                Point[] neighbours = new Point[4];
                neighbours[0] = new Point(current.X, current.Y - 1);
                neighbours[1] = new Point(current.X, current.Y + 1);
                neighbours[2] = new Point(current.X - 1, current.Y);
                neighbours[3] = new Point(current.X + 1, current.Y);

                // Loop through the neighbours
                foreach (Point n in neighbours)
                {
                    // Check if the neighbour is actually in this area
                    if (!containingPixels.Contains(n))
                        continue;

                    // If the neighbour is already on todo or done, skip it
                    if (todo.Contains(n) || done.Contains(n))
                        continue;

                    newArea.containingPixels.Add(n);
                    containingPixels.Remove(n);
                    todo.Enqueue(n);
                    done.Add(n);
                }
            }

            return newArea;
        }

        private ColorArea FindAreaNew(Point start)
        {
            ColorArea newArea = new ColorArea(color);
            List<Point> next = new List<Point>(new Point[] { start });

            while (next.Count > 0)
            {
                Point current = next[0];
                next.RemoveAt(0);
                newArea.AddPixel(current);

                Queue<Point> nextPixel = new Queue<Point>(new Point[] { current });
                while (nextPixel.Count > 0)
                {
                    current = nextPixel.Dequeue();

                    Point down = new Point(current.X, current.Y + 1);
                    if (containingPixels.Contains(down))
                        next.Add(down);

                    Point rightNeighbour = new Point(current.X + 1, current.Y);
                    Point leftNeighbour = new Point(current.X - 1, current.Y);

                    // Check if the pixels to the right exists
                    if (containingPixels.Contains(rightNeighbour))
                    {
                        containingPixels.Remove(rightNeighbour);
                        next.Remove(rightNeighbour);
                        newArea.AddPixel(rightNeighbour);
                        nextPixel.Enqueue(rightNeighbour);
                    }

                    if (containingPixels.Contains(leftNeighbour))
                    {
                        containingPixels.Remove(leftNeighbour);
                        next.Remove(leftNeighbour);
                        newArea.AddPixel(leftNeighbour);
                        nextPixel.Enqueue(leftNeighbour);
                    }
                }
            }

            return newArea;
        }

        public bool HasNeighbour(Point p)
        {
            if (containingPixels.Contains(new Point(p.X - 1, p.Y)))
                return true;
            else if (containingPixels.Contains(new Point(p.X, p.Y - 1)))
                return true;
            else if (containingPixels.Contains(new Point(p.X + 1, p.Y)))
                return true;
            else if (containingPixels.Contains(new Point(p.X, p.Y + 1)))
                return true;
            else return false;
        }

        public void Merge(ColorArea area)
        {
            //containingPixels.AddRange(area.containingPixels);
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"

        public override string ToString()
        {
            return color.ToString() + " -> " + containingPixels.Count + " (" + width + "x" + height + ")";
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
