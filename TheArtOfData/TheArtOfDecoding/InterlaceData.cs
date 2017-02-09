using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfDecoding
{
    class InterlaceData
    {
        #region "Fields"

        public Point StartPosition;
        public int PixelsPerRow;
        public int TotalImageWidth;
        public int PixelWidth;
        public List<DataColors> Colors;

        #endregion

        #region "Constructors"

        public InterlaceData()
        {
            StartPosition = new Point(0, 0);
            PixelWidth = 50;
            PixelsPerRow = 10;
            Colors = new List<DataColors>();
        }

        #endregion

        #region "Singleton"

        private static InterlaceData instance;

        public static InterlaceData INSTANCE
        {
            get
            {
                if (instance == null)
                    instance = new InterlaceData();
                return instance;
            }
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public Image DrawInterlace(Image source)
        {
            Graphics g = Graphics.FromImage(source);

            // Calculate the pixel width
            const int scaleDeforming = 10;
            PixelWidth = TotalImageWidth / PixelsPerRow;

            int x = 0;
            int y = 0;
            for (int i = 0; i < Colors.Count; i++)
            {
                if (Colors[i] == -1)
                    continue;

                Brush brush = new SolidBrush(Colors[i]);
                Pen pen = new Pen(Color.White, 3);
                //g.FillRectangle(brush, StartPosition.X + scaleDeforming / 2 + x * PixelWidth, StartPosition.Y + scaleDeforming / 2 + y * PixelWidth, PixelWidth - scaleDeforming, PixelWidth - scaleDeforming);
                g.DrawRectangle(pen, StartPosition.X + scaleDeforming / 2 + x * PixelWidth, StartPosition.Y + scaleDeforming / 2 + y * PixelWidth, PixelWidth - scaleDeforming, PixelWidth - scaleDeforming);

                x++;
                if (x == PixelsPerRow)
                {
                    x = 0;
                    y++;
                }
            }

            return source;
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
