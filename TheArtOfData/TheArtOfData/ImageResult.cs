using System;
using System.Collections.Generic;
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

        private Dictionary<Type, Image> image;
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
            image = new Dictionary<Type, Image>();
        }

        #endregion

        #region "Properties"

        public DateTime TimeStamp
        {
            get { return timestamp; }
        }

        public Dictionary<Type, Image> Image
        {
            get { return image; }
        }

        #endregion

        #region "Methods"

        public void Execute()
        {
            idw = new ImageDataWriter();
            idw.AddBytes(data);
            image.Add(typeof(ImageDataWriter), idw.GetImage(colorsPerRow));

            foreach (Type type in outputTypes)
            {
                ArtGenerator gen = Activator.CreateInstance(type) as ArtGenerator;
                gen.AddBytes(data);
                image.Add(type, gen.GetImage());
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
