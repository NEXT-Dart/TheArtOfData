﻿using Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfData.Art;

namespace TheArtOfData
{
    public partial class Form1 : Form
    {
        #region "Singleton"

        private static Form1 instance;

        public static Form1 INSTANCE
        {
            get
            {
                return instance;
            }
        }

        #endregion

        private DateTime currentImageTimestamp;
        private object __timestampLock;
        private Thread _thread;
        private Dictionary<Type, Control> controls;
        private Dictionary<Control, DateTime> updateTime;
        private List<ArtControlLink> artGenerators;

        private int selectedTab = 0;

        public Form1()
        {
            InitializeComponent();

            instance = this;
            __timestampLock = new object();
            updateTime = new Dictionary<Control, DateTime>();

            //controls = new Dictionary<Type, Control>();
            //controls.Add(typeof(ImageDataWriter), pictureBox1);
            //controls.Add(typeof(MondriaanArtGenerator), pictureBox2);
            //controls.Add(typeof(MosaicArtGenerator), pictureBox3);
        }

        public void RedrawImage(int colorsPerRow, byte[] data)
        {
            //ImageResult ir = new ImageResult(data, colorsPerRow, typeof(MondriaanArtGenerator), typeof(MosaicArtGenerator));
            //if (_thread != null && _thread.IsAlive)
            //    _thread.Abort();
            //_thread = new Thread(ExecuteThread);
            //_thread.Start(ir);
        }

        private void RedrawImages()
        {
            Thread thread = new Thread(ExecuteImageThead);
            thread.Start();
        }

        private void ExecuteImageThead()
        {
            artGenerators = new List<ArtControlLink>();
            
            if (selectedTab == 0)
                artGenerators.Add(new ArtControlLink(UpdateTimeOnControl, pictureBox1, typeof(NormalArtGenerator), (int)numericUpDown1.Value));
            if (selectedTab == 1)
                artGenerators.Add(new ArtControlLink(UpdateTimeOnControl, pictureBox2, typeof(MondriaanArtGenerator)));
            if (selectedTab == 2)
                artGenerators.Add(new ArtControlLink(UpdateTimeOnControl, pictureBox3, typeof(MosaicArtGenerator)));
            if (selectedTab == 3)
                artGenerators.Add(new ArtControlLink(UpdateTimeOnControl, pictureBox4, typeof(OctagonArtGenerator), (int)numericUpDown1.Value, pictureBox4.Width, pictureBox4.Height));
            if (PopOut.InstanceActive)
                artGenerators.Add(new ArtControlLink(UpdateTimeOnControl, PopOut.INSTANCE.pictureBox1, typeof(NormalArtGenerator), (int)numericUpDown1.Value));

            foreach (ArtControlLink generator in artGenerators)
            {
                generator.Invoke(Encoding.ASCII.GetBytes(textBox1.Text));
            }
        }

        private bool UpdateTimeOnControl(Control control, DateTime time)
        {
            if (updateTime.ContainsKey(control))
            {
                DateTime oldTime;
                updateTime.TryGetValue(control, out oldTime);
                if (time > oldTime)
                {
                    updateTime[control] = time;
                    return true;
                }
            }
            else
            {
                updateTime.Add(control, time);
                return true;
            }

            return false;
        }

        private void ExecuteThread(object data)
        {
            ImageResult ir = (data as ImageResult);
            ir.Execute();

            lock (__timestampLock)
            {
                if (ir.TimeStamp > currentImageTimestamp)
                {
                    foreach (KeyValuePair<Type, Control> kvp in controls)
                    {
                        PropertyInfo pi = kvp.Value.GetType().GetProperty("Image");
                        CustomImage img;
                        ir.Image.TryGetValue(kvp.Key, out img);
                        img.Optimize();
                        pi.SetValue(kvp.Value, img.GetDrawableImageScaled(kvp.Value.Width, kvp.Value.Height));
                    }

                    PropertyInfo popOutPic = PopOut.INSTANCE.pictureBox1.GetType().GetProperty("Image");
                    CustomImage pop;
                    ir.Image.TryGetValue(typeof(ImageDataWriter), out pop);
                    pop.Optimize();
                    popOutPic.SetValue(PopOut.INSTANCE.pictureBox1, pop.GetDrawableImageScaled(PopOut.INSTANCE.pictureBox1.Width, PopOut.INSTANCE.pictureBox1.Height));

                    currentImageTimestamp = ir.TimeStamp;
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                textBox1.SelectAll();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //RedrawImage((int)numericUpDown1.Value, Encoding.ASCII.GetBytes(textBox1.Text));
            RedrawImages();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PopOut.INSTANCE.Show();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //RedrawImage((int)numericUpDown1.Value, Encoding.ASCII.GetBytes(textBox1.Text));
            RedrawImages();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl1.SelectedIndex;
            RedrawImages();
        }
    }
}
