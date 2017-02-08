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

        private List<Point> crossings;
        private Point start;
        private Point end;
        private bool horizontal;

        #endregion

        #region "Constructors"

        public LineData(Point start, Point end)
        {
            this.start = start;
            this.end = end;
            horizontal = end.Y - start.Y == 0;
            crossings = new List<Point>();
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

        public Point[] Area
        {
            get
            {
                List<Point> area = new List<Point>();
                for (int i = start.X; i < end.X + 1; i++)
                {
                    for (int j = start.Y; j < end.Y + 1; j++)
                    {
                        if (!crossings.Contains(new Point(i, j)))
                            area.Add(new Point(i, j));
                    }
                }
                return area.ToArray();
            }
        }

        #endregion

        #region "Methods"

        public void AddCrossing(Point crossing)
        {
            crossings.Add(crossing);
        }

        public void SetBitsOnLine(ImageGrid ig, string bits)
        {
            Point[] area = Area;
            int index = 0;
            Random rand = new Random(0);
            foreach (char c in bits)
            {
                index += rand.Next(1, area.Length / (bits.Length));
                ig.SetPoint(area[index].X, area[index].Y, c - 48);
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
