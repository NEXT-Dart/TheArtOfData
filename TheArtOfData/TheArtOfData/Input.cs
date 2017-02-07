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
    public partial class Input : Form
    {
        public Input()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Form1.INSTANCE.RedrawImage((int)numericUpDown1.Value, Encoding.ASCII.GetBytes(textBox1.Text));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Form1.INSTANCE.RedrawImage((int)numericUpDown1.Value, Encoding.ASCII.GetBytes(textBox1.Text));
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                textBox1.SelectAll();
        }
    }
}
