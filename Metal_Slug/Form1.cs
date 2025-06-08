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
        List<CMultiImageActor> LHeroR = new List<CMultiImageActor>();
        List<CMultiImageActor> LHeroL = new List<CMultiImageActor>();
        Timer tt = new Timer();
        public int idleframe = 1;
        public int mapstart = 5;
        public int mapend = 705;
        public int flag = 0;
        public bool IsRight = true;
        public bool IsLeft = false;



        public Form1()
        {

            InitializeComponent();
            this.Paint += Form1_Paint;
            tt.Interval = 200;
            tt.Start();
            //tt.Tick += Tt_Tick;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            
            
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            IsRight = false;
            IsLeft = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                IsRight = true;
                IsLeft = false;

                int heroX = LHeroR[flag].rcDst.X;
                int heroWidth = LHeroR[flag].rcDst.Width;
                int screenScrollLimit = this.ClientSize.Width / 2;
                int maxMapScroll = Lwrld[0].wrld.Width - Lwrld[0].rcSrc.Width;

                // Calculate the hero's right edge in world coordinates
                int heroRightOnMap = Lwrld[0].rcSrc.X + heroX + heroWidth;

                // Prevent moving past the map's right edge
                if (heroRightOnMap >= Lwrld[0].wrld.Width)
                    return;

                if (heroX < screenScrollLimit)
                {
                    foreach (var hero in LHeroR)
                        hero.rcDst.X += 12;
                }
                else if (Lwrld[0].rcSrc.X < maxMapScroll)
                {
                    Lwrld[0].rcSrc.X += 12;
                }
                else
                {
                    // If map can't scroll, move hero to the right, but not past the map's edge
                    foreach (var hero in LHeroR)
                        hero.rcDst.X += 12;
                }

                flag++;
                if (flag == LHeroR.Count)
                    flag = 0;
            }
            if (e.KeyCode == Keys.Left)
            {
                IsRight = false;
                IsLeft = true;

                int heroX = LHeroR[flag].rcDst.X;
                int minMapScroll = 0;
                int screenScrollLimit = this.ClientSize.Width / 2 -100 ;

                // Prevent moving past the map's left edge
                if (Lwrld[0].rcSrc.X == 0 && heroX <= 0)
                    return;

                if (Lwrld[0].rcSrc.X > minMapScroll && heroX > screenScrollLimit)
                {
                    Lwrld[0].rcSrc.X -= 12;
                }
                else if (heroX > 0)
                {
                    foreach (var hero in LHeroR)
                        hero.rcDst.X -= 12;
                }

                flag--;
                if (flag < 0)
                    flag = LHeroR.Count - 1;
            }
            drawdubb(this.CreateGraphics());
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            
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
            pnn.rcSrc = new Rectangle(mapstart, 0,mapend, 220);
            pnn.rcDst = new Rectangle(0, 0, this.ClientSize.Width, 400);
            Lwrld.Add(pnn);
        }
        private void createHero()
        {
            //first half of right
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width-80, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width-140, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 260, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 320, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 390, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroR.Add(pnn);


            // 2nd half of Right

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 80, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 140, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 260, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 320, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 390, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroR.Add(pnn);



            //start of left half
        }

        private void drawscene(Graphics g2)
        {

            g2.Clear(Color.White);
            for (int i = 0; i < Lwrld.Count; i++)
            {
                CAdvImgActor ptrav = Lwrld[i];
                g2.DrawImage(ptrav.wrld, ptrav.rcDst, ptrav.rcSrc, GraphicsUnit.Pixel);
            }


            if(IsRight)
                g2.DrawImage(LHeroR[flag].wrld, LHeroR[flag].rcDst, LHeroR[flag].rcSrc, GraphicsUnit.Pixel);
            if(IsLeft)
                g2.DrawImage(LHeroL[flag].wrld, LHeroL[flag].rcDst, LHeroL[flag].rcSrc, GraphicsUnit.Pixel);



            //for(int i=0;i< Lhero.Count;i++)
            //{
            //    CMultiImageActor ptrav = Lhero[i];
            //    g2.DrawImage(ptrav.imgs[idleframe], ptrav.x, ptrav.y);


            //}
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

        public Bitmap wrld;
        public Rectangle rcDst, rcSrc;
        public int iframe;

    }
}
