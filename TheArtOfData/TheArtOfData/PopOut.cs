using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheArtOfData
{
    public partial class PopOut : Form
    {
        public PopOut()
        {
            InitializeComponent();
        }

        #region "Singleton"

        private static PopOut instance;

        public static PopOut INSTANCE
        {
            get
            {
                if (instance == null)
                    instance = new PopOut();
                return instance;
            }
        }

        #endregion

        private void PopOut_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                TopMost = true;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Normal;
                Screen screen = Screen.FromControl(this);
                Size = screen.Bounds.Size;
                Location = new Point(0, 0);
            }
        }

        private void PopOut_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
        }
    }
}
