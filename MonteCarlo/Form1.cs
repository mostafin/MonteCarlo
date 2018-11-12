using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonteCarlo
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        List<Color> colors = new List<Color>();
        BackgroundWorker worker = new BackgroundWorker();
        Automat2D automat2D;

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Height = 4 *Convert.ToInt32(textBox2.Text);
            pictureBox1.Width = 4 *Convert.ToInt32(textBox2.Text);

            //int size1 = (1 * Convert.ToInt32(textBox2.Text) / 4);
            //int size2 = (1 * Convert.ToInt32(textBox2.Text) / 4);


            int size1 = Convert.ToInt32(textBox2.Text);
            int size2 = Convert.ToInt32(textBox2.Text);

            this.Height = Convert.ToInt32(1.2 * pictureBox1.Height);
            this.Width = Convert.ToInt32(1.8 * pictureBox1.Width);


            bitmap = new Bitmap(size1, size2);

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            Random rnd = new Random(DateTime.Now.Millisecond);
            for(int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
            {
                colors.Add(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            }
            automat2D = new Automat2D(size1, size2, "nieperiodyczne", pictureBox1, bitmap,Convert.ToInt32(textBox1.Text) , colors , comboBox1.Text);

            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            automat2D.Iterate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            automat2D.stop();
        }
    }

}
