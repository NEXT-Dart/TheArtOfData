using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheArtOfDecoding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = @"..\..\..\..\Test images";
            if (open.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            ImageParser ip = new ImageParser(open.FileName);
            Image img = ip.Run();

            ImageDataReader idr = new ImageDataReader(img);
            byte[] data = idr.GetData();
            textBox1.Text = Encoding.ASCII.GetString(data);

            pictureBox1.Image = new Bitmap(open.FileName);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = splitContainer1.Panel1.Width / 2 + splitContainer1.Panel2.Width / 2;
        }
    }
}
