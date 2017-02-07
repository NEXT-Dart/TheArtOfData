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
        private Dictionary<Type, Control> controls;

        public Form1()
        {
            InitializeComponent();

            instance = this;
            __timestampLock = new object();

            //new Input().Show();

            controls = new Dictionary<Type, Control>();
            controls.Add(typeof(ImageDataWriter), pictureBox1);
            controls.Add(typeof(MondriaanArtGenerator), pictureBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImageDataWriter idw = new ImageDataWriter();
            idw.AddString("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui");

            Image image = idw.GetImage(Convert.ToInt32(comboBox1.SelectedItem.ToString()));
            pictureBox1.Image = image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;
            if (printDialog.ShowDialog() == DialogResult.Cancel)
                return;
            pd.PrinterSettings = printDialog.PrinterSettings;

            pd.Print();
        }

        public void RedrawImage(int colorsPerRow, byte[] data)
        {
            ImageResult ir = new ImageResult(data, colorsPerRow, typeof(MondriaanArtGenerator));
            ThreadPool.QueueUserWorkItem(ExecuteThread, ir);
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
                        Image img;
                        ir.Image.TryGetValue(kvp.Key, out img);
                        pi.SetValue(kvp.Value, img);
                    }

                    currentImageTimestamp = ir.TimeStamp;
                }
            }
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            ImageDataWriter idw = new ImageDataWriter();
            idw.AddString("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui");
            //idw.AddBytes(File.ReadAllBytes(@"D:\Users\Bas\Desktop\.text"));
            //idw.GetImage(200).Save(@"D:\Users\Bas\Desktop\next200.png", ImageFormat.Png);

            Image img = idw.GetImage(Convert.ToInt32(comboBox1.SelectedItem.ToString()));
            Point loc = new Point(50, 50);
            e.Graphics.DrawImage(img, loc);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                textBox1.SelectAll();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            RedrawImage((int)numericUpDown1.Value, Encoding.ASCII.GetBytes(textBox1.Text));
        }
    }
}
