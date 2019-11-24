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
using System.Windows.Forms.DataVisualization.Charting;
namespace ImageProcess
{

    public partial class Form1 : Form
    {
        ImageFunction imagefunction = new ImageFunction();
        private List<Label> labels = new List<Label>();
        private List<PictureBox> pictureboxs = new List<PictureBox>();
        private List<TextBox> textboxes = new List<TextBox>();
        private List<TrackBar> trackbars = new List<TrackBar>();
        private int opt = 0;
        //private int threshold = 128;
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
            ResultGroup.Controls.Clear();
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
        private void CreatePicbox(int num,string[] label,Bitmap[] img,bool changevalue,bool historgram,int [] value1,int [] value2)
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
            //Image img = Image.FromFile("ExampleImage//A_RGB.bmp");
            for (int i = 0; i < num; ++i )
            {
                int j = i / 3;
                Point pt = new Point(x + saperate_w*((i%3)+1) + (i % 3) * image_w, y + saperate_h* (j*2 + 2) + j * image_h);
                PictureBox p = new PictureBox();
                p.Size = new Size(image_w, image_h);
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                p.Location = pt;
                p.Image = img[i];
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
                tb.Value = imagefunction.GetThreshold();
                tb.Width = ((width - saperate_w * 3) / 5) * 4;
                this.ResultGroup.Controls.Add(tb);
                trackbars.Add(tb);
                TextBox txtb = new TextBox();
                txtb.Text = imagefunction.GetThreshold().ToString();
                txtb.KeyDown += TextKeyDown;
                txtb.Location = new Point(pt.X + tb.Width + saperate_w, pt.Y + (int)(saperate_h*0.3));
                this.ResultGroup.Controls.Add(txtb);
                textboxes.Add(txtb);
            }
            if (historgram)
            {
                int[] X_label = Enumerable.Range(0, 256).ToArray(); //0~255
                
                Chart chart1 = new Chart();
                Chart chart2 = new Chart();
                chart1.ChartAreas.Add("ChartArea");
                chart1.Series.Add("Series");
                Title title = new Title();
                title.Text = "Historgram of gray level";
                chart1.Titles.Add(title);
                chart1.ChartAreas["ChartArea"].BackColor = Color.Transparent;
                chart1.BackColor = Color.Transparent;
                chart1.Size = new Size(274, 183);
                chart1.Location = new Point(9, 189); //289 189
                chart1.Series["Series"].Points.DataBindXY(X_label, value1);
                chart1.Series["Series"].Color = Color.Green;
                this.ResultGroup.Controls.Add(chart1);
                chart2.ChartAreas.Add("ChartArea");
                chart2.Series.Add("Series");
                title = new Title();
                title.Text = "Historgram of gray level";
                chart2.Titles.Add(title);
                chart2.ChartAreas["ChartArea"].BackColor = Color.Transparent;
                chart2.BackColor = Color.Transparent;
                chart2.Size = new Size(274, 183);
                chart2.Location = new Point(289, 189); //289 189
                chart2.Series["Series"].Points.DataBindXY(X_label, value2);
                chart2.Series["Series"].Color = Color.DarkSlateBlue;
                this.ResultGroup.Controls.Add(chart2);
            }
        }

        private void TextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) //Key Enter
            {
                int value = int.Parse(textboxes[0].Text);
                if (value <=255 && value >= 0)
                {
                    imagefunction.SetThreshold(value);
                    trackbars[0].Value = value;
                    CleanResult();
                    if (opt == 4)
                    {
                        //checkBox1.Checked = false;
                        //imagefunction.SetResultPic(checkBox1.Checked);
                        imagefunction.SetPreStepLabel("Source");
                        imagefunction.ThresholdCal();
                    }
                    else if (opt == 6)
                    {
                        imagefunction.RenewTheThresholdAndOverlap();
                    }

                    var list = imagefunction.GetNowStepPicture();
                    Debug.Print(list.Count.ToString());
                    string[] labels = new string[list.Count];
                    Bitmap[] bitmaps = new Bitmap[list.Count];
                    for (int i = 0; i < list.Count; ++i)
                    {
                        labels[i] = list[i].label;
                        bitmaps[i] = list[i].pic;
                    }
                    CreatePicbox(list.Count, labels, bitmaps, true, false, null, null);
                }
                else
                {
                    textboxes[0].Text = imagefunction.GetThreshold().ToString();
                }
            }
            Debug.Print(e.KeyValue.ToString());
        }

        private void TrackBar_change(object sender, EventArgs e)
        {
            if (trackbars[0].Value != imagefunction.GetThreshold()) {
                Debug.Print(trackbars[0].Value.ToString());
                imagefunction.SetThreshold(trackbars[0].Value);
                textboxes[0].Text = imagefunction.GetThreshold().ToString();
                CleanResult();
                if (opt == 4)
                {
                    //checkBox1.Checked = false;
                    //imagefunction.SetResultPic(checkBox1.Checked);
                    imagefunction.SetPreStepLabel("Source");
                    imagefunction.ThresholdCal();
                }else if (opt == 6)
                {
                    imagefunction.RenewTheThresholdAndOverlap();
                }
                    
                var list = imagefunction.GetNowStepPicture();
                Debug.Print(list.Count.ToString());
                string[] labels = new string[list.Count];
                Bitmap[] bitmaps = new Bitmap[list.Count];
                for (int i = 0; i < list.Count; ++i)
                {
                    labels[i] = list[i].label;
                    bitmaps[i] = list[i].pic;
                }
                CreatePicbox(list.Count, labels, bitmaps, true, false, null, null);
            }
            else
            {
                Debug.Print("Same");
            }
        }
        private void Filebtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.RestoreDirectory = false;
            dialog.AutoUpgradeEnabled = true;
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png , *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png ;  *.bmp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Filepath.Text = dialog.FileName;
                imagefunction.SetFileName(dialog.FileName);
            }
        }

        private void OK_click(object sender, EventArgs e)
        {
  
            int func = 0;
            foreach(Control c in groupBox8.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = c as RadioButton;
                    if (rb.Checked)
                    {
                        func = int.Parse(rb.Tag.ToString());
                    }
                }
               
            }
            Debug.Print(func.ToString());
            
            if (checkBox1.Checked)
            {
                var list = imagefunction.GetNowStepPicture();
                if (list.Count > 1)
                {
                    ChooseResult chr = new ChooseResult();
                    string[] s = new string[list.Count - 1];
                    int j = 0;
                    for ( int i = 0; i < list.Count; ++i)
                    {
                        if (list[i].label != "Source")
                        {
                            s[j] = list[i].label;
                            ++j;
                        }
                    }
                    chr.CreateRadioButton(s);
                    chr.ShowDialog(this);
                    if (chr.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        Debug.Print(chr.result);
                        imagefunction.SetPreStepLabel(chr.result);
                    }
                }
            }
            CleanResult();
            switch (func)
            {
                case 1:
                    opt = 1;
                    imagefunction.GetRGBandGraylevelPic();
                    var list = imagefunction.GetNowStepPicture();
                    Debug.Print(list.Count.ToString());
                    string[] labels = new string[list.Count];
                    Bitmap[] bitmaps = new Bitmap[list.Count];
                    for (int i = 0; i < list.Count; ++i)
                    {
                        labels[i] = list[i].label;
                        bitmaps[i] = list[i].pic;
                    }
                    CreatePicbox(list.Count, labels, bitmaps, false,false,null,null);
                    break;
                case 2:
                    opt = 2;
                    imagefunction.SmoothFilter();
                    list = imagefunction.GetNowStepPicture();
                    Debug.Print(list.Count.ToString());
                    labels = new string[list.Count];
                    bitmaps = new Bitmap[list.Count];
                    for (int i = 0; i < list.Count; ++i)
                    {
                        labels[i] = list[i].label;
                        bitmaps[i] = list[i].pic;
                    }
                    CreatePicbox(list.Count, labels, bitmaps, false,false,null,null);
                    break;
                case 3:
                    opt = 3;
                    imagefunction.HistorgramEqualization();
                    list = imagefunction.GetNowStepPicture();
                    Debug.Print(list.Count.ToString());
                    labels = new string[list.Count];
                    bitmaps = new Bitmap[list.Count];
                    for (int i = 0; i < list.Count; ++i)
                    {
                        labels[i] = list[i].label;
                        bitmaps[i] = list[i].pic;
                    }
                    CreatePicbox(list.Count, labels, bitmaps, false, true,list[0].historgramvalue, list[1].historgramvalue);
                    break;
                case 4:
                    opt = 4;
                    imagefunction.ThresholdCal();
                    list = imagefunction.GetNowStepPicture();
                    Debug.Print(list.Count.ToString());
                    labels = new string[list.Count];
                    bitmaps = new Bitmap[list.Count];
                    for (int i = 0; i < list.Count; ++i)
                    {
                        labels[i] = list[i].label;
                        bitmaps[i] = list[i].pic;
                    }
                    CreatePicbox(list.Count, labels, bitmaps, true, false, null, null);
                    break;
                case 5:
                    opt = 5;
                    imagefunction.SobelFilter();
                    list = imagefunction.GetNowStepPicture();
                    Debug.Print(list.Count.ToString());
                    labels = new string[list.Count];
                    bitmaps = new Bitmap[list.Count];
                    for (int i = 0; i < list.Count; ++i)
                    {
                        labels[i] = list[i].label;
                        bitmaps[i] = list[i].pic;
                    }
                    CreatePicbox(list.Count, labels, bitmaps, false, false, null, null);
                    break;
                case 6:
                    opt = 6;
                    imagefunction.OverlapImage();
                    list = imagefunction.GetNowStepPicture();
                    Debug.Print(list.Count.ToString());
                    labels = new string[list.Count];
                    bitmaps = new Bitmap[list.Count];
                    for (int i = 0; i < list.Count; ++i)
                    {
                        labels[i] = list[i].label;
                        bitmaps[i] = list[i].pic;
                    }
                    CreatePicbox(list.Count, labels, bitmaps, true, false, null, null);
                    break;
                case 7:
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (imagefunction.GetNowStep() >= 0 )
            {
                imagefunction.SetResultPic(checkBox1.Checked);
                Debug.Print(checkBox1.Checked.ToString());

            }
            else
            {
                checkBox1.Checked = false;
            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (imagefunction.GetNowStep() >= 0)
            {
                imagefunction.Undo();
                CleanResult();
                var list = imagefunction.GetNowStepPicture();
                Debug.Print(list.Count.ToString());
                string[] labels = new string[list.Count];
                Bitmap[] bitmaps = new Bitmap[list.Count];
                for (int i = 0; i < list.Count; ++i)
                {
                    labels[i] = list[i].label;
                    bitmaps[i] = list[i].pic;
                }
                if (list.Count > 0)
                {
                    if (list[0].UseHitogram)
                    {
                        CreatePicbox(list.Count, labels, bitmaps, list[0].UseTrackBar, list[0].UseHitogram, list[0].historgramvalue,list[1].historgramvalue);
                    }
                    else
                    {
                        CreatePicbox(list.Count, labels, bitmaps, list[0].UseTrackBar, list[0].UseHitogram, null, null);

                    }
                    
                }
                
                if(imagefunction.GetNowStep() == 0)
                {
                    checkBox1.Checked = false;
                }
                    
               
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CleanResult();
            Image i = null;
            i = Image.FromFile(".\\ExampleImage\\B_noisy.bmp");
            Bitmap image = new Bitmap(i);
            Bitmap[] bitmaps = { image, image };
            string[] s = { "Aaaaa", "BBBBBB" };
            CreatePicbox(2,s , bitmaps, false, true, Enumerable.Range(0, 256).ToArray(), Enumerable.Range(0, 256).ToArray());
        }
    }
}
