using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData.Art
{
    class ImageGrid
    {
        #region "Fields"

        private int[,] colors;
        private int size;
        private Random rand;

        #endregion

        #region "Constructors"

        public ImageGrid(int size, Random rand)
        {
            colors = new int[size, size];
            this.size = size;
            this.rand = rand;
        }

        #endregion

        #region "Properties"



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

            for (int i = start; i < length; i++)
            {
                colors[i, row] = 1;
            }
        }

        public void SetColumn(int col)
        {
            int length = rand.Next(size / 3, size);
            if (rand.Next(0, 5) == 0)
            {
                length = size;
            }

            int start = length == size ? 0 : (size / 2 - length / 2 - rand.Next(0, (size - length) / 2));

            for (int i = start; i < length; i++)
            {
                colors[col, i] = 1;
            }
        }

        public bool SetPoint(int x, int y, int val)
        {
            if (colors[x, y] == 1)
                colors[x, y] = val;
            else
                return false;
            return true;
        }

        public Image CreateBitmap(int s)
        {
            Bitmap bitmap = new Bitmap(s, s);

            int size = s / this.size;

            for (int x = 0; x < this.size; x++)
            {
                for (int y = 0; y < this.size; y++)
                {
                    Color c = Color.White;

                    if (colors[x, y] == 0)
                        continue;
                    else if (colors[x, y] == 1)
                        c = Color.Gold;
                    else if (colors[x, y] == 2)
                        c = Color.Maroon;
                    else if (colors[x, y] == 3)
                        c = Color.Blue;

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            bitmap.SetPixel(x * size + i, y * size + j, c);
                        }
                    }

                }
            }

            return bitmap;
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
