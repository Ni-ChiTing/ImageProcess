using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Q1_click(object sender, EventArgs e)
        {
            
            Q1_BChannel.SizeMode = PictureBoxSizeMode.StretchImage;
            Q1_Gchannel.SizeMode = PictureBoxSizeMode.StretchImage;
            Q1_RChannel.SizeMode = PictureBoxSizeMode.StretchImage;
            Q1_Grayscale.SizeMode = PictureBoxSizeMode.StretchImage;
            Image i = Image.FromFile("ExampleImage//A_RGB.bmp");
            Q1_source.Image = i;
            Bitmap image = new Bitmap(i);
            test.Text = image.Size.ToString();
        }

        private void Q2_click(object sender, EventArgs e)
        {

        }

        private void Q3_click(object sender, EventArgs e)
        {

        }

        private void Q4_click(object sender, EventArgs e)
        {

        }

        private void Q5_click(object sender, EventArgs e)
        {

        }

        private void Q6_click(object sender, EventArgs e)
        {

        }

        private void Q7_click(object sender, EventArgs e)
        {

        }
    }
}
