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

            pictureBox1.Image = new Bitmap(open.FileName);
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
                // Draw the latest image from the active camera
                e.Graphics.DrawImage(_latestFrame, 0, 0, _latestFrame.Width, _latestFrame.Height);
            }
        }

        public void OnImageCaptured(Touchless.Vision.Contracts.IFrameSource frameSource, Touchless.Vision.Contracts.Frame frame, double fps)
        {
            _latestFrame = frame.Image;
            pictureBox1.Invalidate();
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
            if (_frameSource == null)
                return;

            new Thread(parseImage).Start();
        }

        private void parseImage()
        {
            Image current = (Image)_latestFrame.Clone();
            ImageParser ip = new ImageParser(current);
            Image img = ip.Run();
            current.Dispose();


            ImageDataReader idr = new ImageDataReader(img);
            byte[] data = idr.GetData();


            this.Invoke((MethodInvoker)delegate ()
            {
                textBox1.Text = Encoding.ASCII.GetString(data);
            });
            //textBox1.Text = Encoding.ASCII.GetString(data);
        }
    }
}
