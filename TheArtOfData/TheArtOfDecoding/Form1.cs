using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Touchless.Vision.Camera;

namespace TheArtOfDecoding
{
    public partial class Form1 : Form
    {
        private CameraFrameSource _frameSource;
        private static Bitmap _latestFrame;

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

            pictureBox1.Paint += new PaintEventHandler(drawLatestImage);
            Bitmap bitmap = new Bitmap(open.FileName);
            _latestFrame = bitmap;
            //pictureBox1.Image = bitmap;
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }


        private void startCapturing()
        {
            try
            {
                Camera c = CameraService.AvailableCameras.First(); ;
                setFrameSource(new CameraFrameSource(c));
                _frameSource.Camera.CaptureWidth = 480;
                _frameSource.Camera.CaptureHeight = 320;
                _frameSource.Camera.Fps = 60;
                _frameSource.NewFrame += OnImageCaptured;

                pictureBox1.Paint += new PaintEventHandler(drawLatestImage);
                _frameSource.StartFrameCapture();
                captureTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            thrashOldCamera();
        }

        private void thrashOldCamera()
        {
            // Trash the old camera
            if (_frameSource != null)
            {
                captureTimer.Enabled = false;
                _frameSource.NewFrame -= OnImageCaptured;
                _frameSource.Camera.Dispose();
                setFrameSource(null);
                pictureBox1.Paint -= new PaintEventHandler(drawLatestImage);
            }
        }

        private void setFrameSource(CameraFrameSource cameraFrameSource)
        {
            if (_frameSource == cameraFrameSource)
                return;

            _frameSource = cameraFrameSource;
        }

        private void drawLatestImage(object sender, PaintEventArgs e)
        {
            if (_latestFrame != null)
            {
                // Calculate the height of image
                float imageH = (float)pictureBox1.Width / (float)_latestFrame.Width;

                e.Graphics.DrawImage(_latestFrame, new Rectangle(0, 0, pictureBox1.Width, (int)(_latestFrame.Height * imageH)), 0, 0, _latestFrame.Width, _latestFrame.Height, GraphicsUnit.Pixel);
            }
        }

        public void OnImageCaptured(Touchless.Vision.Contracts.IFrameSource frameSource, Touchless.Vision.Contracts.Frame frame, double fps)
        {
            _latestFrame = frame.Image;
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }

        private void btnCameraConfig_Click(object sender, EventArgs e)
        {
            if (_frameSource != null)
                _frameSource.Camera.ShowPropertiesDialog();
        }

        private void btnStartCam_Click(object sender, EventArgs e)
        {
            if (_frameSource != null && _frameSource.Camera == CameraService.AvailableCameras.First())
                return;

            thrashOldCamera();
            startCapturing();
        }

        private void btnSnapPic_Click(object sender, EventArgs e)
        {
            if (_frameSource == null)
                return;

            Image current = (Image)_latestFrame.Clone();
            ImageParser ip = new ImageParser(current);
            Image img = ip.Run();

            ImageDataReader idr = new ImageDataReader(img);
            byte[] data = idr.GetData();
            textBox1.Text = Encoding.ASCII.GetString(data);

            current.Dispose();
        }

        private void captureTimer_Tick(object sender, EventArgs e)
        {
            if (_frameSource == null || _latestFrame == null)
                return;

            new Thread(parseImage).Start();
        }

        private void parseImage()
        {
            Image current = (Image)_latestFrame.Clone();
            ImageParser ip = new ImageParser(current);
            Image img = ip.Run();
            current.Dispose();

            if (img == null)
                return;


            ImageDataReader idr = new ImageDataReader(img);
            byte[] data = idr.GetData();

            if (this != null && !this.IsDisposed)
                this.Invoke((MethodInvoker)delegate ()
                {
                    if (!textBox1.IsDisposed)
                        textBox1.Text = Encoding.ASCII.GetString(data);
                });
            //textBox1.Text = Encoding.ASCII.GetString(data);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (_latestFrame != null)
            {
                Image interlaced;
                if (checkBox1.Checked)
                {
                    // Draw the latest image from the active camera
                    interlaced = InterlaceData.INSTANCE.DrawInterlace(_latestFrame);
                }
                else
                {
                    interlaced = _latestFrame;
                }

                //e.Graphics.DrawImage(interlaced, 0, 0, interlaced.Width, interlaced.Height);

                // Calculate the height of image
                int width = interlaced.Width;
                int height = interlaced.Height;
                if (width > height)
                {
                    width = pictureBox2.Width;
                    height = (int)((float)interlaced.Height * ((float)pictureBox2.Width / (float)interlaced.Width));
                }

                if (height > width || height > pictureBox2.Height)
                {
                    height = pictureBox2.Height;
                    width = (int)((float)interlaced.Width * ((float)pictureBox2.Height / (float)interlaced.Height));
                }


                e.Graphics.DrawImage(interlaced, new Rectangle(0, 0, width, height), 0, 0, interlaced.Width, interlaced.Height, GraphicsUnit.Pixel);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox2.Visible = checkBox1.Checked;
        }
    }
}
