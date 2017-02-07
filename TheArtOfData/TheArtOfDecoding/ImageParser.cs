using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using System.Windows.Forms;

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
            Crop();
            Transform();
            Read();
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

        private void Display()
        {
            Form form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = img.Size;

            PictureBox pb = new PictureBox();
            pb.Dock = DockStyle.Fill;
            pb.Image = img;

            form.Controls.Add(pb);
            form.ShowDialog();
        }
    }
}
