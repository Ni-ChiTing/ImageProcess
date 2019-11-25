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
    public partial class ChooseResult : Form
    {
        private List<RadioButton> radiobuttons = new List<RadioButton>();
        private List<TextBox> textboxes = new List<TextBox>();
        public string result;
        public string angle;
        public string ws;
        public string hs;
        private int X;
        private int Y;
        public ChooseResult()
        {
            InitializeComponent();
            X = 0;
            Y = 0;
            this.ControlBox = false;
            radiobuttons.Clear();
            textboxes.Clear();
            button1.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        public void CreatTextLable()
        {
            int saperate_w = 30;
            int saperate_h = 30;
            Label t = new Label();
            Y = Y + saperate_h;
            t.Location = new Point(X +saperate_w, Y);
            t.Text = "Use Q5 Combined result to Do";
            t.Width = groupBox1.Width - saperate_w * 2;
            groupBox1.Controls.Add(t);

        }
        public void CreateRadioButton(string[] s)
        {
            int width = groupBox1.Width;
            int height = groupBox1.Height;
            int saperate_w = 30;
            int saperate_h = 30;
            Y = Y + saperate_h;
            for (int i = 0; i < s.Length; ++i)
            {
                RadioButton rb = new RadioButton();
                rb.Text = s[i];
                rb.Location = new Point(X + saperate_w , Y);
                if (i == 0)
                {
                    rb.Checked = true;
                }
                radiobuttons.Add(rb);
                groupBox1.Controls.Add(rb);
                Y = Y + saperate_h;

            }

        }
        public void CreateTextBoxAndLabel()
        {
            int width = groupBox1.Width;
            int height = groupBox1.Height;
            int saperate_w = 30;
            int saperate_h = 30;
            Y = Y + saperate_h;
            int itemwidth = (width - 3*saperate_w)/3;
            Label t = new Label();
            t.Location = new Point(X + saperate_w, Y);
            t.Text = "Rotate Angle";
            t.Width = itemwidth;
            TextBox tb = new TextBox();
            tb.Location = new Point(X + 2*saperate_w + itemwidth, Y);
            tb.Width = itemwidth;
            tb.Text = "45";
            tb.KeyPress +=  HandleKeyPress;
            textboxes.Add(tb);
            groupBox1.Controls.Add(t);
            groupBox1.Controls.Add(tb);
            Y = Y + saperate_h;
            t = new Label();
            t.Location = new Point(X + saperate_w, Y);
            t.Text = "Scale width";
            t.Width = itemwidth;
            tb = new TextBox();
            tb.Location = new Point(X + 2 * saperate_w + itemwidth, Y);
            tb.Width = itemwidth;
            tb.Text = "1";
            tb.KeyPress += HandleKeyPress2;
            textboxes.Add(tb);
            groupBox1.Controls.Add(t);
            groupBox1.Controls.Add(tb);
            Y = Y + saperate_h;
            t = new Label();
            t.Location = new Point(X + saperate_w, Y);
            t.Text = "Scale height";
            t.Width = itemwidth;
            tb = new TextBox();
            tb.Location = new Point(X + 2 * saperate_w + itemwidth, Y);
            tb.Width = itemwidth;
            tb.Text = "1";
            tb.KeyPress += HandleKeyPress2;
            textboxes.Add(tb);
            groupBox1.Controls.Add(t);
            groupBox1.Controls.Add(tb);

        }
        private void HandleKeyPress2(object sender, KeyPressEventArgs e)

        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar == '.') )
                e.Handled = true;
        }
        private void HandleKeyPress(object sender, KeyPressEventArgs e)

        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control c in groupBox1.Controls)
            { 
                if (c is RadioButton)
                {
                    RadioButton rb = c as RadioButton;
                    if (rb.Checked)
                    {
                        result = rb.Text;
                    }
                }

            }
            if (textboxes.Count>0)
            {
                angle = textboxes[0].Text;
                ws = textboxes[1].Text;
                hs = textboxes[2].Text;
            }
                
        }
    }
}
