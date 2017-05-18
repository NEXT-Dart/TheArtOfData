using Imaging.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imaging.Parsing
{
    class Grid
    {
        #region "Fields"

        private int width;
        private int height;
        private Point[] crossingPoints;
        private DataColors[] colors;

        #endregion

        #region "Constructors"

        public Grid(int width, int height, Point[] corners)
        {
            this.width = width;
            this.height = height;

            colors = new DataColors[width * height];
            crossingPoints = new Point[(width + 1) * (height + 1)];
            SetCorner(0, 0, corners[0]);
            SetCorner(width, 0, corners[1]);
            SetCorner(0, height, corners[2]);
            SetCorner(width, height, corners[3]);
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

        public void Calculate()
        {
            // Get the top line
            Point[] line = GetBresenhamLine(GetCorner(0, 0), GetCorner(width, 0));
            float stepSize = (float)line.Length / (float)width;
            for (int x = 1; x < width; x++)
            {
                SetCorner(x, 0, line[(int)(stepSize * x)]);
            }

            // Get the bottom line
            line = GetBresenhamLine(GetCorner(0, height), GetCorner(width, height));
            stepSize = (float)line.Length / (float)width;
            for (int x = 1; x < width; x++)
            {
                SetCorner(x, height, line[(int)(stepSize * x)]);
            }

            // Do the vertical lines
            for (int x = 0; x < width + 1; x++)
            {
                Point top = GetCorner(x, 0);
                Point bottom = GetCorner(x, height);
                line = GetBresenhamLine(top, bottom);
                stepSize = (float)line.Length / (float)height;

                for (int y = 1; y < height; y++)
                {
                    SetCorner(x, y, line[(int)(stepSize * y)]);
                }
            }
        }

        public void Draw(CustomImage image)
        {
            foreach (Point p in crossingPoints)
            {
                image.SetPixel(p.X, p.Y, Color.Black);
            }

            image.GetDrawableImage().Save(@"D:\Developments\Git\TheArtOfData\Test images\parsedData.png");
        }

        public Point[] GetRectangle(int x, int y)
        {
            List<Point> points = new List<Point>();
            Point[] v0 = GetBresenhamLine(GetCorner(x, y), GetCorner(x, y + 1));
            Point[] v1 = GetBresenhamLine(GetCorner(x + 1, y), GetCorner(x + 1, y + 1));

            for (int i = 0; i < (v0.Length <= v1.Length ? v0.Length : v1.Length); i++)
            {
                points.AddRange(GetBresenhamLine(v0[i], v1[i]));
            }

            return points.Distinct().ToArray();
        }

        private void SetCorner(int x, int y, Point p)
        {
            crossingPoints[y * (width + 1) + x] = p;
        }

        public void SetColor(int x, int y, DataColors color)
        {
            colors[y * width + x] = color;
        }

        private Point GetCorner(int x, int y)
        {
            return crossingPoints[y * (width + 1) + x];
        }

        public Image GetData()
        {
            CustomImage image = new CustomImage(width, height);
            int x = 0, y = 0;
            foreach (DataColors c in colors)
            {
                image.SetPixel(x, y, c);
                x++;
                if (x >= width)
                {
                    x = 0;
                    y++;
                }
            }

            //image.GetDrawableImageScaled(20).Save(@"D:\Developments\Git\TheArtOfData\Test images\parsedData.png");

            return image.GetDrawableImage();
        }

        private Point[] GetBresenhamLine(Point start, Point end)
        {
            int x0 = start.X;
            int y0 = start.Y;
            int x1 = end.X;
            int y1 = end.Y;
            List<Point> line = new List<Point>();

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (;;)
            {
                line.Add(new Point(x0, y0));
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
            return line.ToArray();
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
