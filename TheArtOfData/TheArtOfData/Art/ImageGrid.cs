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

        private int size;
        private Random rand;
        private List<LineData> lines;
        private List<Point> crossings;
        private List<Point> knownPoints;

        #endregion

        #region "Constructors"

        public ImageGrid(int size)
        {
            this.size = size;
            lines = new List<LineData>();
            crossings = new List<Point>();
            knownPoints = new List<Point>();
            rand = new Random();
        }

        #endregion

        #region "Properties"

        public int Size
        {
            get { return size; }
        }

        public List<Point> Crossings
        {
            get { return crossings.ToList(); }
        }

        #endregion

        #region "Methods"

        public void AddLine(LineData line)
        {
            lines.Add(line);
            knownPoints.AddRange(line.ExtraArea);

            crossings = knownPoints.GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key).ToList();
        }

        public CustomImage CreateImage()
        {
            CustomImage img = new CustomImage(size, size);

            foreach (LineData line in lines)
            {
                foreach (Point point in line.Line)
                {
                    img.SetPixel(point.X, point.Y, Color.Gold);
                }
                foreach (KeyValuePair<Point, int> point in line.DataPoints)
                {
                    Color c = Color.Gold;
                    if (point.Value == 0)
                        c = Color.Maroon;
                    else if (point.Value == 1)
                        c = Color.Blue;
                    img.SetPixel(point.Key.X, point.Key.Y, c);
                }
            }

            return img;
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
