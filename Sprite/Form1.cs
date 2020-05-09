﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sprite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Path = null;
        }

        public Bitmap loadimage(String Path)
        {
            if (Path == null) 
            {
                throw new ImageNotSelected();
            }

            Bitmap image = new Bitmap(Path, true);

            if (image.Width != 16 || image.Height != 16)
            {
                throw new FileNotSupported();
            }            
            
            return image;
        }

        public List<pixel> loadpixels()
        {
            Bitmap image = loadimage(Properties.Settings.Default.Path);
            
            List<pixel> pixellist = new List<pixel>();
            List<pixel> c1 = new List<pixel>();
            List<pixel> c2 = new List<pixel>();
            List<pixel> c3 = new List<pixel>();
            List<pixel> c4 = new List<pixel>();

            //LOAD PIXEL LIST
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {                   
                    pixellist.Add(new pixel(i, j, image.GetPixel(i, j)));
                }
            }

            //SPLIT PIXEL LIST INTO C's
            foreach (pixel p in pixellist)
            {
                //C1
                if (p.getX() < 9 && p.getY() < 9)
                {
                    c1.Add(p);
                }
                //C2
                if (p.getX() > 8 && p.getY() < 9)
                {
                    c2.Add(p);
                }
                //C3
                if (p.getX() < 9 && p.getY() > 8)
                {
                    c3.Add(p);
                }
                //C4
                if (p.getX() > 8 && p.getY() > 8)
                {
                    c4.Add(p);
                }
            }

            //CLEAN PIXEL LIST
            pixellist.Clear();

            //LOAD C1
            foreach(pixel p in c1)
            {
                pixellist.Add(p);
            }
            //LOAD C2
            foreach (pixel p in c2)
            {
                pixellist.Add(p);
            }
            //LOAD C3
            foreach (pixel p in c3)
            {
                pixellist.Add(p);
            }
            //LOAD C4
            foreach (pixel p in c4)
            {
                pixellist.Add(p);
            }


            List<Color> Pallete = new List<Color>();
            foreach (pixel p in c1)
            {
                if (Pallete.Contains(p.getColor()))
                {
                    p.setPalleteindex(Pallete.IndexOf(p.getColor()));
                }
                else
                {
                    Pallete.Add(p.getColor());
                    p.setPalleteindex(Pallete.IndexOf(p.getColor()));
                }
            }
            printlist(c1);
            
            return pixellist;
        }

        //PRINT LIST<PIXEL>
        public void printlist(List<pixel> l)
        {
            String s ="";
            foreach(pixel p in l){
                s += p.getPalleteindex().ToString() + ",";
            }
            textBox1.Text = s;
        }
        public void printPallete(List<Color> Pallete)
        {
            String s = "";
            foreach (Color c in Pallete)
            {
                s += "SPRITE_PALETTE["+ Pallete.IndexOf(c).ToString()+ "] = RGB15("+(c.R*31/255).ToString()+","+(c.G * 31 / 255).ToString()+","+(c.B * 31 / 255).ToString()+");@";
            }
            textBox2.Text = s.Replace("@", System.Environment.NewLine);
        }


        public void readimage()
        {
           
            try
            {
                List<pixel> pixellist =  loadpixels();                
                List<Color> Pallete = new List<Color>();
                
                
                printPallete(Pallete);

            }
            catch(FileNotSupported ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(ImageNotSelected ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        //BUTTONS
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Path = openFileDialog.FileName.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            readimage();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
