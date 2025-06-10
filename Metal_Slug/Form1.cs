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
        List<CMultiImageActor> LHeroIL = new List<CMultiImageActor>();
        List<CMultiImageActor> LHeroIR = new List<CMultiImageActor>();
        Timer tt = new Timer();
        public int idleframe = 1;
        public int mapstart = 5;
        public int mapend = 705;
        public int flag = 0;  // for right movement
        public int flag1 = 0; // for left movement
        public bool IsRight = false;
        public bool IsLeft = false;
        public bool LastDirectionIsRight = true;



        public Form1()
        {

            InitializeComponent();
            this.Paint += Form1_Paint;
            tt.Interval = 300;
            tt.Start();
            tt.Tick += Tt_Tick;
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
                LastDirectionIsRight = true;

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
                    foreach (var hero in LHeroL)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroIR)
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
                    foreach (var hero in LHeroL)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroIR)
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
                LastDirectionIsRight = false;

                int heroX = LHeroL[flag1].rcDst.X;
                int heroWidth = LHeroL[flag1].rcDst.Width;
                int screenScrollLimit = this.ClientSize.Width / 2;
                int minMapScroll = 0;

                // Calculate the hero's left edge in world coordinates
                int heroLeftOnMap = Lwrld[0].rcSrc.X + heroX;

                // Prevent moving past the map's left edge
                if (heroLeftOnMap <= 0)
                    return;

                if (heroX > screenScrollLimit)
                {
                    foreach (var hero in LHeroL)
                        hero.rcDst.X -= 12;
                    foreach (var hero in LHeroR)
                        hero.rcDst.X -= 12;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X -= 12;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.X -= 12;
                }
                else if (Lwrld[0].rcSrc.X > minMapScroll)
                {
                    Lwrld[0].rcSrc.X -= 12;
                }
                else
                {
                    foreach (var hero in LHeroL)
                        hero.rcDst.X -= 12;
                    foreach (var hero in LHeroR)
                        hero.rcDst.X -= 12;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X -= 12;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.X -= 12;
                }

                flag1++;
                if (flag1 == LHeroL.Count)
                    flag1 = 0;
            }
            drawdubb(this.CreateGraphics());
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            if(!IsLeft||!IsRight)
            {
                idleframe++;
                if (idleframe == LHeroIL.Count)
                    idleframe = 0;
            }
            
            drawdubb(this.CreateGraphics());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Location = new Point(0, this.ClientSize.Height / 2);
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            createworld();
            createHeroRight();
            createHeroLeft();
            createHeroIdleLeft();
            createHeroIdleRight();

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
        private void createHeroRight()
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


        }
        private void createHeroLeft()
        {
            //start of left half
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(10, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(75, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(135, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(195, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(260, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(325, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroL.Add(pnn);

            //second half left

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(10, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(75, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(135, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(195, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(260, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(325, 160, 65, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroL.Add(pnn);

        }
        private void createHeroIdleLeft()
        {
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(10, 30, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroIL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(75, 30, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroIL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(135, 30, 60, 50);
            pnn.rcDst = new Rectangle(100, 248, 150, 120);
            LHeroIL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(195, 30, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroIL.Add(pnn);
        }
        private void createHeroIdleRight()
        {
            //first half of right Idle
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 70, 30, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroIR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 130, 30, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroIR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 190, 30, 60, 50);
            pnn.rcDst = new Rectangle(100, 248, 150, 120);
            LHeroIR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 258, 30, 60, 50);
            pnn.rcDst = new Rectangle(100, 248, 150, 120);
            LHeroIR.Add(pnn);
        }

        private void drawscene(Graphics g2)
        {

            g2.Clear(Color.White);
            for (int i = 0; i < Lwrld.Count; i++)
            {
                CAdvImgActor ptrav = Lwrld[i];
                g2.DrawImage(ptrav.wrld, ptrav.rcDst, ptrav.rcSrc, GraphicsUnit.Pixel);
            }

            if (IsRight)
                g2.DrawImage(LHeroR[flag].wrld, LHeroR[flag].rcDst, LHeroR[flag].rcSrc, GraphicsUnit.Pixel);
            else if (IsLeft)
                g2.DrawImage(LHeroL[flag1].wrld, LHeroL[flag1].rcDst, LHeroL[flag1].rcSrc, GraphicsUnit.Pixel);
            else
            {
                if(LastDirectionIsRight)
                    g2.DrawImage(LHeroIR[idleframe].wrld, LHeroIR[idleframe].rcDst, LHeroIR[idleframe].rcSrc, GraphicsUnit.Pixel);
                else
                    g2.DrawImage(LHeroIL[idleframe].wrld, LHeroIL[idleframe].rcDst, LHeroIL[idleframe].rcSrc, GraphicsUnit.Pixel);
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

        public Bitmap wrld;
        public Rectangle rcDst, rcSrc;
        public int iframe;

    }
}
