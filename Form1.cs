using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace ImageViewer
{
    public partial class Form1 : Form
    { 
        public Form1()
        {
            InitializeComponent();
        }

      

//Menu Bar//

//Save//

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "jpg (*.jpg)|*jpg|bmp (*.bmp)|*bmp|png (*.png)|*png";

            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName.Length > 0)
            {
                pictureBox1.Image.Save(sfd.FileName);
            }

 
        }

//EXIT//
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


//LOAD IMAGE//
        private void actualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpg (*.jpg)|*jpg|bmp (*.bmp)|*bmp|png (*.png)|*png";

            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Length > 0)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = Image.FromFile(ofd.FileName);

                Bitmap img = new Bitmap(pictureBox1.Image);
                defImgHeight = img.Height;
                defImgWidth = img.Width;
            }

        }


        private void streToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpg (*.jpg)|*jpg|bmp (*.bmp)|*bmp|png (*.png)|*png";

            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Length > 0)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = Image.FromFile(ofd.FileName);

                Bitmap img = new Bitmap(pictureBox1.Image);
                defImgHeight = img.Height;
                defImgWidth = img.Width;
            }
        }


//ROTATE//
        private void button1_Click(object sender, EventArgs e)
        {
            RotateRight.Right(this);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RotateLeft.Left(this);
        }


//Grayscale
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Image);

                int width = bmp.Width;
                int height = bmp.Height;

                Color p;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        p = bmp.GetPixel(x, y);


                        int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;


                        int avg = (r + g + b) / 3;


                        bmp.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                    }
                }

                pictureBox1.Image = bmp;
            }
            
        }

//SEPIA//

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Image);

                int width = bmp.Width;
                int height = bmp.Height;

                Color p;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        p = bmp.GetPixel(x, y);


                        int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;


                        int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                        int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                        int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);


                        if (tr > 255)
                        {
                            r = 255;
                        }
                        else
                        {
                            r = tr;
                        }

                        if (tg > 255)
                        {
                            g = 255;
                        }
                        else
                        {
                            g = tg;
                        }

                        if (tb > 255)
                        {
                            b = 255;
                        }
                        else
                        {
                            b = tb;
                        }

                        bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    }
                }
                pictureBox1.Image = bmp;
            }
            
        }


//ZOOM//

        double zoom = 1;
        int defImgWidth;
        int defImgHeight;

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap img = new Bitmap(pictureBox1.Image);
                if (img != null)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    zoom = zoom + 0.2;
                    Bitmap bbb = new Bitmap(img, Convert.ToInt32(pictureBox1.Width * zoom), Convert.ToInt32(pictureBox1.Width * zoom * defImgHeight / defImgWidth));
                    pictureBox1.Image = bbb;
                }
            }
                
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image != null)
            {
                Bitmap img = new Bitmap(pictureBox1.Image);
                if (img != null)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    if (zoom > 0.2)
                    {
                        zoom = zoom - 0.2;
                        Bitmap bbb = new Bitmap(img, Convert.ToInt32(pictureBox1.Width * zoom), Convert.ToInt32(pictureBox1.Width * zoom * defImgHeight / defImgWidth));
                        pictureBox1.Image = bbb;
                    }
                }
            }
                
            
        }


//SLIDESHOW//


        private string[] folderFile = null;
        private int selected = 0;
        private int end = 0;

        private void button6_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] part1 = null, part2 = null, part3 = null;
                part1 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpg");
                part2 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpeg");
                part3 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.bmp");
                folderFile = new string[part1.Length + part2.Length + part3.Length];
                Array.Copy(part1, 0, folderFile, 0, part1.Length);
                Array.Copy(part2, 0, folderFile, part1.Length, part2.Length);
                Array.Copy(part3, 0, folderFile, part1.Length + part2.Length, part3.Length);
                selected = 0;
                end = folderFile.Length;
                showImage(folderFile[selected]);

                if (timer1.Enabled == true)
                {
                    timer1.Enabled = false;
                }
                else
                {
                    timer1.Enabled = true;
                }

            }

        }

        private void showImage(string path)
        {
            Image imgtemp = Image.FromFile(path);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = imgtemp;
        }

        private void nextImage()
        {
            if (selected == folderFile.Length - 1)
            {
                selected = 0;
                showImage(folderFile[selected]);
            }
            else
            {
                selected = selected + 1; showImage(folderFile[selected]);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nextImage();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                timer1.Enabled = true;
            }
        }

        private void loadToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "jpg (*.jpg)|*jpg|bmp (*.bmp)|*bmp|png (*.png)|*png";

            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Length > 0)
            {
                string[] files = ofd.FileNames;
                int x = 20;
                int y = 20;
                int maxHeight = -1;
                foreach (string img in files)
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = Image.FromFile(img);
                    pic.Location = new Point(x, y);
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    x += pic.Width + 10;
                    maxHeight = Math.Max(pic.Height, maxHeight);
                    if (x > this.ClientSize.Width - 100)
                    {
                        x = 20;
                        y += maxHeight + 10;
                    }
                    this.pictureBox1.Controls.Add(pic);
                }
            }
        }
    }

    public static class RotateRight
    {
        public static void Right(Form1 obj)
        {
            if (obj.pictureBox1.Image != null)
            {
                obj.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                obj.pictureBox1.Refresh();
            }
                
        }
    }

    public static class RotateLeft
    {
        public static void Left(Form1 obj)
        {
            if (obj.pictureBox1.Image != null)
            {
                obj.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                obj.pictureBox1.Refresh();
            }
                
        }
    }

}
