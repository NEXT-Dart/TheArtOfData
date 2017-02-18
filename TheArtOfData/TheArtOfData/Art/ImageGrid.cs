using Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheArtOfData.Art
{
    class ImageGrid
    {
        #region "Fields"

        private int[] colors;
        private int size;
        private Random rand;
        private List<LineData> lines;

        #endregion

        #region "Constructors"

        public ImageGrid(int size, Random rand)
        {
            colors = new int[size * size];
            this.size = size;
            this.rand = rand;
            lines = new List<LineData>();
        }

        #endregion

        #region "Properties"

        public LineData[] Lines
        {
            get
            {
                return lines.OrderByDescending(x => x.IsHorizontal).ThenBy(h => h.IsHorizontal ? h.Start.Y : h.Start.X).ToArray();
            }
        }

        #endregion

        #region "Methods"

        public void SetRow(int row)
        {
            int length = rand.Next(size / 3, size);
            if (rand.Next(0, 5) == 0)
            {
                length = size;
            }

            int start = length == size ? 0 : (size / 2 - length / 2 - rand.Next(0, (size - length) / 2));

            for (int i = start; i < start + length; i++)
            {
                colors[i * size + row] = 1;
            }

            lines.Add(new LineData(new Point(start, row), new Point(start + length, row)));
        }

        public void SetColumn(int col)
        {
            int length = rand.Next(size / 3, size);
            if (rand.Next(0, 5) == 0)
            {
                length = size;
            }

            int start = length == size ? 0 : (size / 2 - length / 2 - rand.Next(0, (size - length) / 2));

            for (int i = start; i < start + length; i++)
            {
                colors[col * size + i] = 1;
            }

            lines.Add(new LineData(new Point(col, start), new Point(col, start + length)));
        }

        public void SetPoint(int x, int y, int val)
        {
            colors[x * size + y] = val + 2;
        }

        public CustomImage CreateBitmap()
        {
            CustomImage img = new CustomImage(size, size);

            //int size = s / this.size;

            for (int x = 0; x < this.size; x++)
            {
                for (int y = 0; y < this.size; y++)
                {
                    Color c = Color.Green;

                    if (colors[x * this.size + y] == 0)
                        continue;
                    else if (colors[x * this.size + y] == 1)
                        c = Color.Gold;
                    else if (colors[x * this.size + y] == 2)
                        c = Color.Maroon;
                    else if (colors[x * this.size + y] == 3)
                        c = Color.Blue;

                    img.SetPixel(x, y, c);
                }
            }

            return img;
        }

        public void CheckCollisions()
        {
            // Get all yellow points
            List<Point> yellows = new List<Point>();
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i] == 1)
                {
                    yellows.Add(new Point(i / size, i % size));
                }
            }

            // Check all the surrounding pixels
            foreach (Point yellow in yellows)
            {
                Point[] surrounding = yellows.Where(x => x.X >= yellow.X - 1 && x.X <= yellow.X + 1 && x.Y >= yellow.Y - 1 && x.Y <= yellow.Y + 1).ToArray();
                surrounding = surrounding.Where(x => x != yellow).ToArray();

                if (surrounding.Length >= 3)
                {
                    LineData[] lines = this.lines.Where(x => x.Area.Contains(yellow)).ToArray();
                    foreach (LineData line in lines)
                    {
                        line.AddCrossing(yellow);
                        //colors[yellow.X * size + yellow.Y] += 1;
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



        #endregion
    }
}
