using Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfData.Art;

namespace TheArtOfData
{
    class ImageResult
    {
        #region "Fields"

        private Dictionary<Type, CustomImage> image;
        private int colorsPerRow;
        private byte[] data;
        private ImageDataWriter idw;
        private DateTime timestamp;
        private Type[] outputTypes;

        #endregion

        #region "Constructors"

        public ImageResult(byte[] data, int colorsPerRow, params Type[] outputImages)
        {
            this.data = data;
            this.colorsPerRow = colorsPerRow;
            this.timestamp = DateTime.Now;
            this.outputTypes = outputImages;
            image = new Dictionary<Type, CustomImage>();
        }

        #endregion

        #region "Properties"

        public DateTime TimeStamp
        {
            get { return timestamp; }
        }

        public Dictionary<Type, CustomImage> Image
        {
            get { return image; }
        }

        #endregion

        #region "Methods"

        public void Execute()
        {
            //idw = new ImageDataWriter();
            //idw.AddBytes(data);
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //image.Add(typeof(ImageDataWriter), idw.GetImage(colorsPerRow));
            //sw.Stop();
            //Debug.WriteLine("Normal: " + sw.ElapsedMilliseconds + " ms");

            //foreach (Type type in outputTypes)
            //{
            //    sw = new Stopwatch();
            //    sw.Start();
            //    ArtGenerator gen = Activator.CreateInstance(type) as ArtGenerator;
            //    gen.AddBytes(data);
            //    image.Add(type, gen.GetImage());
            //    sw.Stop();
            //    Debug.WriteLine(type.Name + ": " + sw.ElapsedMilliseconds + " ms");
            //}
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
