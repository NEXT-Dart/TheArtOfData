using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfDecoding
{
    class ImageParser
    {
        public Image image { get; private set; }
        public ImageParser(string filename)
        {
            if(!filename.EndsWith(".jpg"))
            {
                throw new ArgumentException("Input files can only be .jpg format");
            }
            image = Image.FromFile(filename);
        }

        public Image Run()
        {
            return null;
        }

        private void Crop()
        {

        }

        private void Transform()
        {

        }

        private void Read()
        {

        }
    }
}
