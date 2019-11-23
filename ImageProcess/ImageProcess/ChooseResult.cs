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
        public string result;
        public ChooseResult()
        {
            InitializeComponent();
            this.ControlBox = false;
            radiobuttons.Clear();
            button1.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        public void CreateRadioButton(string[] s)
        {
            int width = groupBox1.Width;
            int height = groupBox1.Height;
            int x = 0;
            int y = 0;
            int saperate_w = 30;
            int saperate_h = 30;
            for (int i = 0; i < s.Length; ++i)
            {
                RadioButton rb = new RadioButton();
                rb.Text = s[i];
                rb.Location = new Point(x + saperate_w , y + saperate_h * (i + 1));
                if (i == 0)
                {
                    rb.Checked = true;
                }
                radiobuttons.Add(rb);
                groupBox1.Controls.Add(rb);

            }
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
        }
    }
}
