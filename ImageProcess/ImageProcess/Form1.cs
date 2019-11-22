using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace ImageProcess
{
    public partial class Form1 : Form
    {
        ImageFunction imagefunction = new ImageFunction();
        private List<Label> labels = new List<Label>();
        private List<PictureBox> pictureboxs = new List<PictureBox>();
        private List<TextBox> textboxes = new List<TextBox>();
        private List<TrackBar> trackbars = new List<TrackBar>();
        private int threshold = 128;
        public Form1()
        {
            InitializeComponent();
            labels.Clear();
            pictureboxs.Clear();
            textboxes.Clear();
            trackbars.Clear();

        }
        private void CleanResult()
        {
            foreach(var item in labels)
            {
                item.Dispose();
            }
            foreach(var item in pictureboxs)
            {
                item.Dispose();
            }
            foreach (var item in trackbars)
            {
                item.Dispose();
            }
            foreach (var item in textboxes)
            {
                item.Dispose();
            }
            labels.Clear();
            pictureboxs.Clear();
            textboxes.Clear();
            trackbars.Clear();
        } 
        private void Q1_click(object sender, EventArgs e)
        {
            /*
            Q1_BChannel.SizeMode = PictureBoxSizeMode.StretchImage;
            Q1_GChannel.SizeMode = PictureBoxSizeMode.StretchImage;
            Q1_RChannel.SizeMode = PictureBoxSizeMode.StretchImage;
            Q1_Grayscale.SizeMode = PictureBoxSizeMode.StretchImage;
            Image i = Image.FromFile("ExampleImage//A_RGB.bmp");
            Q1_source.Image = i;
            Bitmap image = new Bitmap(i);
            imagefunction.GetRGBandGraylevelPic("ExampleImage//A_RGB.bmp");
            Q1_BChannel.Image = imagefunction.GetBChannel();
            Q1_GChannel.Image = imagefunction.GetGChannel();
            Q1_RChannel.Image = imagefunction.GetRChannel();
            Q1_Grayscale.Image = imagefunction.GetGrayLevel();
            */
        }
        private void CreatePicbox(int num,string[] label,Boolean changevalue)
        {
            Debug.Print(ResultGroup.Location.ToString());
            int width = ResultGroup.Width;
            int height = ResultGroup.Height;
            int x = 0;
            int y = 0;
            int image_w = 160;
            int image_h = 120;
            int saperate_w = ((width - (image_w * 3)) / 4);
            int saperate_h = ((height - (image_h * 2)) / 8);
            Image img = Image.FromFile("ExampleImage//A_RGB.bmp");
            for (int i = 0; i < num; ++i )
            {
                int j = i / 3;
                Point pt = new Point(x + saperate_w*((i%3)+1) + (i % 3) * image_w, y + saperate_h* (j*2 + 2) + j * image_h);
                PictureBox p = new PictureBox();
                p.Size = new Size(image_w, image_h);
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                p.Location = pt;
                p.Image = img;
                this.ResultGroup.Controls.Add(p);
                if (i < label.Length)
                {
                    Label l = new Label();
                    l.Text = label[i];
                    Point ptt = new Point(pt.X + image_w / 2 - (int)l.CreateGraphics().MeasureString(l.Text, l.Font).Width/ 2, pt.Y+ image_h + (saperate_h - (int)l.CreateGraphics().MeasureString(l.Text, l.Font).Height) /2 );
                    l.Location = ptt;
                    Debug.Print(l.Location.ToString() + " width " + l.CreateGraphics().MeasureString(l.Text,l.Font).ToString());
                    this.ResultGroup.Controls.Add(l);
                }
                else
                {
                    Label l = new Label();
                    l.Text = "Result " + (i+ 1).ToString();
                    Point ptt = new Point(pt.X + image_w / 2 - (int)l.CreateGraphics().MeasureString(l.Text, l.Font).Width / 2, pt.Y + image_h + (saperate_h - (int)l.CreateGraphics().MeasureString(l.Text, l.Font).Height) / 2);

                    l.Location = ptt;
                    Debug.Print(l.Location.ToString() + " width " + l.Width.ToString());
                    this.ResultGroup.Controls.Add(l);
                }
            }
            if (changevalue) {
                Point pt ;
                if ( num > 3)
                    pt = new Point(saperate_w, height - (int)(2.7 * saperate_h));
                else
                    pt = new Point(saperate_w, image_h + 4 * saperate_h);
                TrackBar tb = new TrackBar();
                tb.SetRange(0, 255);
                tb.Location = pt;
                tb.MouseUp += TrackBar_change;
                tb.KeyUp += TrackBar_change;
                tb.Value = threshold;
                tb.Width = ((width - saperate_w * 3) / 5) * 4;
                this.ResultGroup.Controls.Add(tb);
                trackbars.Add(tb);
                TextBox txtb = new TextBox();
                txtb.Text = threshold.ToString();
                txtb.KeyDown += TextKeyDown;
                txtb.Location = new Point(pt.X + tb.Width + saperate_w, pt.Y + (int)(saperate_h*0.3));
                this.ResultGroup.Controls.Add(txtb);
                textboxes.Add(txtb);
            }
        }

        private void TextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) //Key Enter
            {
                int value = int.Parse(textboxes[0].Text);
                if (value <=255 && value >= 0)
                {
                    threshold = value;
                    trackbars[0].Value = value;
                }
            }
            Debug.Print(e.KeyValue.ToString());
        }

        private void TrackBar_change(object sender, EventArgs e)
        {
            if (trackbars[0].Value != threshold) {
                Debug.Print(trackbars[0].Value.ToString());
                threshold = trackbars[0].Value;
                textboxes[0].Text = threshold.ToString();
            }
            else
            {
                Debug.Print("Same");
            }
        }
        private void Filebtn_Click(object sender, EventArgs e)
        {
            string[] a = { "assssssss", "b","c" };
            CreatePicbox(3,a,true);
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.RestoreDirectory = false;
            dialog.AutoUpgradeEnabled = true;
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png , *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png ;  *.bmp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(dialog.FileName);
            }
        }
    }
}
