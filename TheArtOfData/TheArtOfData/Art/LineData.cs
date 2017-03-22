using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheArtOfData.Art
{
    class LineData
    {
        #region "Fields"

        private Dictionary<Point, int> dataPoints;
        private Point start;
        private Point end;
        private bool horizontal;

        private Random rand;
        private int minsize;
        private int maxsize;
        private int row;
        private int col;

        #endregion

        #region "Constructors"

        public LineData(int seed, int minsize, int maxsize, int row = -1, int col = -1)
        {
            rand = new Random(seed);
            this.minsize = minsize;
            this.maxsize = maxsize;
            this.row = row;
            this.col = col;
            GenerateLine();
            dataPoints = new Dictionary<Point, int>();
        }

        #endregion

        #region "Properties"

        public Point Start
        {
            get { return start; }
        }

        public bool IsHorizontal
        {
            get { return horizontal; }
        }

        public ImageGrid ImageGrid { get; set; }

        public Point[] Area
        {
            get
            {
                List<Point> area = new List<Point>();
                for (int i = start.X; i < end.X + 1; i++)
                {
                    for (int j = start.Y; j < end.Y + 1; j++)
                    {
                        if (!ImageGrid.Crossings.Contains(new Point(i, j)))
                            area.Add(new Point(i, j));
                    }
                }
                return area.ToArray();
            }
        }

        public Point[] Line
        {
            get
            {
                List<Point> area = new List<Point>();
                for (int i = start.X; i < end.X + 1; i++)
                {
                    for (int j = start.Y; j < end.Y + 1; j++)
                    {
                        area.Add(new Point(i, j));
                    }
                }
                return area.ToArray();
            }
        }

        public Point[] ExtraArea
        {
            get
            {
                List<Point> area = new List<Point>(Area);
                if (IsHorizontal)
                {
                    area.Add(new Point(start.X - 1, start.Y));
                    area.Add(new Point(start.X + 1, start.Y));
                }
                else
                {
                    area.Add(new Point(start.X, start.Y - 1));
                    area.Add(new Point(start.X, start.Y + 1));
                }

                return area.ToArray();
            }
        }

        public Dictionary<Point, int> DataPoints
        {
            get { return dataPoints; }
        }

        #endregion

        #region "Methods"

        private void GenerateLine()
        {
            int length = rand.Next(minsize, maxsize);
            if (row > -1)
            {
                this.start = new Point(rand.Next(0, maxsize - length), row);
                this.end = new Point(start.X + length, row);
            }
            else if (col > -1)
            {
                this.start = new Point(col, rand.Next(0, maxsize - length));
                this.end = new Point(col, start.Y + length);
            }
            horizontal = end.Y - start.Y == 0;
        }

        private void SetData(Point p, int value)
        {
            dataPoints.Add(p, value);
        }

        public void SetBitsOnLine(string bits)
        {
            List<Point> area = new List<Point>(Area);
            while(area.Count < bits.Length)
            {
                GenerateLine();
                area = new List<Point>(Area);
            }
            Random rand = new Random();
            List<Point> points = new List<Point>();
            foreach (char c in bits)
            {
                Point p = area[rand.Next(0, area.Count)];
                area.Remove(p);
                points.Add(p);
            }

            points = points.OrderBy(x => IsHorizontal ? x.X : x.Y).ToList();

            for (int i = 0; i < points.Count; i++)
            {
                SetData(points[i], bits[i] - 48);
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
