using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metal_Slug
{

    public partial class Form1 : Form
    {

        Bitmap off;
        List<CAdvImgActor> Lwrld = new List<CAdvImgActor>();
        List<CMultiImageActor> Lhero = new List<CMultiImageActor>();
        public int HeroPosX = 200;
        public int HeroPosY = 0;
        Timer tt = new Timer();
        public int idleframe = 1;


        public Form1()
        {
            InitializeComponent();
            this.Paint += Form1_Paint;
            tt.Interval = 200;
            tt.Start();
            tt.Tick += Tt_Tick;
            this.KeyDown += Form1_KeyDown;
            
            
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.D)
            {

            }
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            idleframe = (idleframe + 1) % 4; // Cycles 0,1,2,3
            drawdubb(this.CreateGraphics());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Location = new Point(0, this.ClientSize.Height / 2);
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            createworld();
            createHero();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawdubb(e.Graphics);
        }
        private void createworld()
        {
            CAdvImgActor pnn = new CAdvImgActor();
            pnn.wrld = new Bitmap("Assets/maps/Level1.png");
            pnn.rcSrc = new Rectangle(0, 0,ClientSize.Width, ClientSize.Height);
            pnn.rcDst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Lwrld.Add(pnn);
        }
        private void createHero()
        {
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.x = 0;
            pnn.y = Lwrld[0].wrld.Height-100;
            pnn.imgs = new List<Bitmap>();
            for(int i=1;i<5;i++)
            {
                Bitmap img = new Bitmap("Assets/Hero/" + i + ".png");
                img.MakeTransparent(img.GetPixel(0, 0));
                pnn.imgs.Add(img);
            }
            pnn.x = HeroPosX;
            pnn.y = Lwrld[0].wrld.Height - 400;
            Lhero.Add(pnn);
          



        }
        
        private void drawscene(Graphics g2)
        {
            
            g2.Clear(Color.White);
            for (int i = 0; i < Lwrld.Count; i++)
            {
                CAdvImgActor ptrav = Lwrld[i];
                g2.DrawImage(ptrav.wrld, ptrav.rcDst, ptrav.rcSrc, GraphicsUnit.Pixel);
            }
            for(int i=0;i< Lhero.Count;i++)
            {
                CMultiImageActor ptrav = Lhero[i];
                g2.DrawImage(ptrav.imgs[idleframe], ptrav.x, ptrav.y);
                
                
            }
        }
        private void drawdubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawscene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
    public class CAdvImgActor
    {
        public Bitmap wrld;
        public Rectangle rcDst, rcSrc;
    }
    public class CMultiImageActor
    {
        public int x, y;
        public List<Bitmap> imgs;
        public int iframe;

    }
}
