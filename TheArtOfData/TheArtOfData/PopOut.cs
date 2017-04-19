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

        public static bool InstanceActive
        {
            get { return instance != null; }
        }

        #endregion

        private void PopOut_Resize(object sender, EventArgs e)
        {
            Padding = new Padding(Width / 3, 200, Width / 3, 200);
        }

        private void PopOut_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
        }
    }
}
