using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imaging.Helpers
{
    class Rectangle
    {
        #region "Fields"

        private Point topLeft;
        private Point topRight;
        private Point bottomLeft;
        private Point bottomRight;

        #endregion

        #region "Constructors"

        public Rectangle(Point tl, Point tr, Point bl, Point br)
        {
            this.topLeft = tl;
            this.topRight = tr;
            this.bottomLeft = bl;
            this.bottomRight = br;
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public bool IsInRectangle(Point p)
        {
            if (p.X > topLeft.X && p.Y > topLeft.Y)
            {
                if (p.X < topRight.X && p.Y > topRight.Y)
                {
                    if (p.X > bottomLeft.X && p.Y < bottomLeft.Y)
                    {
                        if (p.X > bottomRight.X && p.Y < bottomRight.Y)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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
