using System;
using System.Windows.Forms;

namespace Proiect
{
    public partial class GammaCorrection : Form
    {
        public GammaCorrection()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox3.Text != null)
            {
                var result = double.TryParse(textBox1.Text, out Program.gammaR);
                if (!result)
                {
                    MessageBox.Show(@"Invalid Red filter.");
                    return;
                }
                result = double.TryParse(textBox2.Text, out Program.gammaG);
                if (!result)
                {
                    MessageBox.Show(@"Invalid Green filter.");
                    return;
                }
                result = double.TryParse(textBox3.Text, out Program.gammaB);
                if (!result)
                {
                    MessageBox.Show(@"Invalid Blue filter.");
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(@"Invalid filters. They must be doubles.");
            }
        }
    }
}
