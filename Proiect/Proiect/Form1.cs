using PointOperations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

// ReSharper disable InconsistentNaming

namespace Proiect
{
    public partial class Form1 : Form
    {
        public double gammaR, gammaG, gammaB;
        private Bitmap img1;
        private long[] myValues;
        public long maxValue;
        public float myYUnit;
        public float myXUnit;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = Enum.GetValues(typeof(ColorFilterType));
            smoothButton.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = @"Image Files (*.bmp, *.jpg, *.png, *.gif)|*.bmp;*.jpg;*.png;*.gif"
            };
            dlg.ShowDialog();
            var img = Image.FromFile(dlg.FileName);
            img1 = new Bitmap(img);
            pictureBox1.Image = img;
            pictureBox1.Refresh();
        }

        private void digitalNegativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var temp = (Bitmap)img1.Clone();
            var negativeBitmap = temp.ImageToDigitalNegative();
            pictureBox2.Image = negativeBitmap;
            pictureBox2.Refresh();

            temp.Dispose();
            negativeBitmap.Dispose();
        }

        private void contrastStretchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var temp = (Bitmap)img1.Clone();
            var contrastStretch = temp.ImageWithContrastStretch();
            pictureBox2.Image = contrastStretch;
            pictureBox2.Refresh();

            temp.Dispose();
            contrastStretch.Dispose();
        }

        private void thresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var temp = (Bitmap)img1.Clone();
            byte thresholdValue;
            var result = byte.TryParse(textBox1.Text, out thresholdValue);
            if (!result)
            {
                MessageBox.Show(@"Please insert a valid threshold value!");
                return;
            }

            var threshHoldBit = temp.ImageTreshold(thresholdValue);

            pictureBox2.Image = threshHoldBit;
            pictureBox2.Refresh();

            threshHoldBit.Dispose();
            temp.Dispose();
        }

        private void colorFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var temp = (Bitmap)img1.Clone();
            var filter = (ColorFilterType)Enum.Parse(typeof(ColorFilterType), comboBox1.Text);
            var filteredImage = temp.ImageColorFilter(filter);

            pictureBox2.Image = filteredImage;
            pictureBox2.Refresh();

            filteredImage.Dispose();
            temp.Dispose();
        }

        private void gammaCorrectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGammaCorrectionDialog();
        }

        private void ShowGammaCorrectionDialog()
        {
            var temp = (Bitmap)img1.Clone();
            var gammaDialog = new GammaCorrection();

            gammaDialog.ShowDialog();

            if (gammaDialog.DialogResult == DialogResult.OK)
            {
                var gammaFiltered = temp.ImageGammaFilter(Program.gammaR, Program.gammaG, Program.gammaB);
                pictureBox2.Image = gammaFiltered;
                pictureBox2.Refresh();

                gammaFiltered.Dispose();
            }
            gammaDialog.Dispose();
            temp.Dispose();
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var temp = (Bitmap)img1.Clone();
            var brightness = trackBar1.Value;
            var newBrightness = temp.ImageBrightness(brightness);

            pictureBox2.Image = newBrightness;
            pictureBox2.Refresh();

            newBrightness.Dispose();
            temp.Dispose();
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var temp = (Bitmap)img1.Clone();
            var grayImage = temp.ImageGrayscale();
            pictureBox2.Image = grayImage;
            pictureBox2.Refresh();

            grayImage.Dispose();
            temp.Dispose();
        }

        private void getHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var temp = (Bitmap)img1.Clone();

            var histogram = GetHistogram(temp);
            DrawHistogram(histogram);

            temp.Dispose();
            smoothButton.Show();
        }

        private long[] GetHistogram(Bitmap image)
        {
            var myHistogram = new long[256];

            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    var c = image.GetPixel(i, j);

                    long temp = 0;
                    temp += c.R;
                    temp += c.G;
                    temp += c.B;

                    temp = (int)temp / 3;
                    myHistogram[temp]++;
                }
            }
            return myHistogram;
        }
        private void DrawHistogram(long[] values)
        {
            myValues = values;
            maxValue = GetMaxim(myValues);

            myYUnit = (float)(pictureBox2.Height - (60)) / maxValue;
            myXUnit = (float)(pictureBox2.Width - (20)) / (myValues.Length - 1);

            pictureBox2.Paint += HistogramaDesenat_Paint;
            pictureBox2.Refresh();
        }
        private static long GetMaxim(IEnumerable<long> vals)
        {
            return vals.Concat(new long[] { 0 }).Max();
        }
        private void HistogramaDesenat_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var myFont = new Font("TimesNewRoman", 12);

            Pen myPen = new Pen(new SolidBrush(Color.Red), myXUnit);
            for (int i = 0; i < myValues.Length; i++)
            {
                g.DrawLine(myPen,
                    new PointF(20 + (i * myXUnit), pictureBox2.Height - 20),
                    new PointF(20 + (i * myXUnit), pictureBox2.Height - 20 - myValues[i] * myYUnit));

                if (myValues[i] == maxValue)
                {
                    SizeF mySize = g.MeasureString(i.ToString(), myFont);

                    g.DrawString(i.ToString(), myFont, new SolidBrush(Color.Red),
                        new PointF(20 + (i * myXUnit) - (mySize.Width / 2), this.Height - myFont.Height),
                        System.Drawing.StringFormat.GenericDefault);
                }
            }

            g.DrawString((myValues.Length - 1).ToString(), myFont,
                new SolidBrush(Color.Red),
                new PointF(20 + (myValues.Length * myXUnit) - g.MeasureString((myValues.Length - 1).ToString(), myFont).Width,
                this.Height - myFont.Height),
                System.Drawing.StringFormat.GenericDefault);
        }

        private void smoothButton_Click(object sender, EventArgs e)
        {
            var temp = (Bitmap)img1.Clone();

            var histogram = GetHistogram(temp);
            histogram = SmoothHistogram(histogram);

            DrawHistogram(histogram);
            temp.Dispose();
            smoothButton.Hide();
        }

        private long[] SmoothHistogram(long[] originalValues)
        {
            var smoothedValues = new long[originalValues.Length];

            var mask = new double[] { 0.25, 0.5, 0.25 };

            for (var bin = 1; bin < originalValues.Length - 1; bin++)
            {
                var smoothedValue = mask.Select((t, i) => originalValues[bin - 1 + i] * t).Sum();
                smoothedValues[bin] = (int)smoothedValue;
            }

            return smoothedValues;
        }


    }


}
