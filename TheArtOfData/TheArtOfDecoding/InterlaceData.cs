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
        public Rectangle CropInfo;

        #endregion

        #region "Constructors"

        public InterlaceData()
        {
            StartPosition = new Point(0, 0);
            PixelWidth = 50;
            PixelsPerRow = 10;
            CropInfo = new Rectangle(0, 0, 1, 1);
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
            Bitmap bitmap = new Bitmap(source);
            Bitmap cropped = new Bitmap(CropInfo.Width, CropInfo.Height);
            Graphics g = Graphics.FromImage(cropped);

            g.DrawImage(bitmap, 0, 0, CropInfo, GraphicsUnit.Pixel);

            // Calculate the pixel width
            const int scaleDeforming = 10;
            PixelWidth = TotalImageWidth / PixelsPerRow;

            int x = 0;
            int y = 0;
            lock (Colors)
            {
                for (int i = 0; i < Colors.Count; i++)
                {
                    if (i > Colors.Count)
                        break;

                    if (Colors[i] == -1)
                        continue;

                    Brush brush = new SolidBrush(Colors[i]);
                    Pen pen = new Pen(Color.White, 3);
                    //g.FillRectangle(brush, StartPosition.X + scaleDeforming / 2 + x * PixelWidth, StartPosition.Y + scaleDeforming / 2 + y * PixelWidth, PixelWidth - scaleDeforming, PixelWidth - scaleDeforming);
                    //g.DrawRectangle(pen, StartPosition.X + scaleDeforming / 2 + x * PixelWidth, StartPosition.Y + scaleDeforming / 2 + y * PixelWidth, PixelWidth - scaleDeforming, PixelWidth - scaleDeforming);
                    g.DrawRectangle(pen, scaleDeforming / 2 + x * PixelWidth, scaleDeforming / 2 + y * PixelWidth, PixelWidth - scaleDeforming, PixelWidth - scaleDeforming);

                    x++;
                    if (x == PixelsPerRow)
                    {
                        x = 0;
                        y++;
                    }
                }
            }



            return cropped;
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
