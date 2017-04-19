using Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfData.Art;

namespace TheArtOfData
{
    class ArtControlLink
    {
        #region "Fields"

        private static object __timestampLock = new object();

        private Control control;
        private Type type;
        private object[] values;
        private UpdateControlTime method;
        private DateTime start;

        public delegate bool UpdateControlTime(Control control, DateTime time);

        #endregion

        #region "Constructors"

        public ArtControlLink(UpdateControlTime method, Control control, Type type, params object[] values)
        {
            this.control = control;
            this.type = type;
            this.values = values;
            this.method = method;
            this.start = DateTime.Now;
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public void Invoke(byte[] data)
        {
            PropertyInfo imageProperty = control.GetType().GetProperty("Image", BindingFlags.Instance | BindingFlags.Public);
            ArtGenerator generator = Activator.CreateInstance(type) as ArtGenerator;
            generator.AddBytes(data);
            Image image = generator.GetImage(values).GetDrawableImageScaled(control.Width, control.Height);


            lock (__timestampLock)
            {
                if (method.Invoke(control, start))
                {
                    imageProperty.SetValue(control, image);
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
