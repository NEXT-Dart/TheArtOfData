using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using System.Windows.Forms;
using System.IO;
using ImageProcessor.Imaging;

namespace TheArtOfDecoding
{
    class ImageParser
    {

        private const int pixelsPerRow = 10;

        public Image image { get; private set; }

        public ImageParser(string filename)
        {
            image = Image.FromFile(filename);
            //Display();
        }

        public Image Run()
        {
            Transform();
            Crop();
            Read();
            return null;
        }

        private void Crop()
        {
            ImageFactory imageFactory = new ImageFactory();
            imageFactory.Load(image);
            imageFactory.EntropyCrop();
            
            imageFactory.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) , "crop.bmp"));
            image = Image.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "crop.bmp"));
            Display();

        }

        private void Transform()
        {

            ImageFactory imageFactory = new ImageFactory();
            imageFactory.Load(image);

            imageFactory.Brightness(15);
            imageFactory.Contrast(75);

            //ResizeLayer resizeLayer = new ResizeLayer(new Size(pixelsPerRow * 5, 0));
            //resizeLayer.ResizeMode = ResizeMode.Max;
            //imageFactory.Resize(resizeLayer);

            //imageFactory.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "resize.bmp"));
            //image = Image.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "resize.bmp"));
            //Display();
        }

        private void Read()
        {

        }

        private void Display()
        {
            Form form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(800, 600);
            form.Text = "Debug image";

            PictureBox pb = new PictureBox();
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Dock = DockStyle.Fill;
            pb.Image = image;

            form.Controls.Add(pb);
            form.ShowDialog();
        }
    }
}
