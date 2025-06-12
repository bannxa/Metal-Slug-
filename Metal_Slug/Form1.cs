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
        List<CMultiImageActor> LHeroR = new List<CMultiImageActor>(); //hero right
        List<CMultiImageActor> LHeroL = new List<CMultiImageActor>(); //hero Left
        List<CMultiImageActor> LHeroIL = new List<CMultiImageActor>(); // hero idle left
        List<CMultiImageActor> LHeroIR = new List<CMultiImageActor>(); // hero idle right
        List<CMultiImageActor> LHeroJL = new List<CMultiImageActor>(); // hero jumpLeft
        List<CMultiImageActor> LHeroJR = new List<CMultiImageActor>(); // hero jumpRight
        List<CMultiImageActor> LEnemyR = new List<CMultiImageActor>(); // enemy right
        List<CMultiImageActor> LEnemyL = new List<CMultiImageActor>(); // enemy left

        Timer tt = new Timer();
        public int idleframe = 1;
        public int mapstart = 5;
        public int mapend = 705;
        public int flag = 0;  // for right movement
        public int flag1 = 0; // for left movement
        public bool IsRight = false;
        public bool IsLeft = false;
        public bool LastDirectionIsRight = true;
        public int flag2 = 0; // for right movement enemy
        public float jumpVelocity = 0;
        public float gravity = 2.5f;
        public float jumpStartVelocity = -18f; 
        public bool isJumping = false;
        public bool isFalling = false;
        public bool isJumpingLeft = false;
        public int jumpFrameLeft = 0;
        public int groundY = 245;
        public int cttimer = 0;
        public bool isJumpingRight = false;
        public int jumpFrameRight = 0;

        public Form1()
        {
            InitializeComponent();
            this.Paint += Form1_Paint;
            tt.Interval = 60;
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
            if (e.KeyCode == Keys.D)
            {
                if (isJumpingLeft) return; // Don't allow move while jumping left

                IsRight = true;
                IsLeft = false;
                LastDirectionIsRight = true;

                int heroX = LHeroR[flag].rcDst.X;
                int heroWidth = LHeroR[flag].rcDst.Width;
                int screenScrollLimit = this.ClientSize.Width / 2;
                int maxMapScroll = Lwrld[0].wrld.Width - Lwrld[0].rcSrc.Width;

                int heroRightOnMap = Lwrld[0].rcSrc.X + heroX + heroWidth;

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
                    foreach (var hero in LHeroJL)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroJR)
                        hero.rcDst.X += 12;
                }
                else if (Lwrld[0].rcSrc.X < maxMapScroll)
                {
                    Lwrld[0].rcSrc.X += 12;
                }
                else
                {
                    foreach (var hero in LHeroR)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroL)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroJL)
                        hero.rcDst.X += 12;
                    foreach (var hero in LHeroJR)
                        hero.rcDst.X += 12;
                }
                cttimer = 0;
                flag++;
                if (flag == LHeroR.Count)
                    flag = 0;
            }
            if (e.KeyCode == Keys.A)
            {
                if (isJumpingLeft) return; // Don't allow move while jumping left

                IsRight = false;
                IsLeft = true;
                LastDirectionIsRight = false;

                int heroX = LHeroL[flag1].rcDst.X;
                int heroWidth = LHeroL[flag1].rcDst.Width;
                int screenScrollLimit = this.ClientSize.Width / 2;
                int minMapScroll = 0;

                int heroLeftOnMap = Lwrld[0].rcSrc.X + heroX;

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
                    foreach (var hero in LHeroJL)
                        hero.rcDst.X -= 12;
                    foreach (var hero in LHeroJR)
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
                    foreach (var hero in LHeroJL)
                        hero.rcDst.X -= 12;
                    foreach (var hero in LHeroJR)
                        hero.rcDst.X -= 12;
                }

                flag1++;
                if (flag1 == LHeroL.Count)
                    flag1 = 0;
                cttimer = 0;
            }
            if (e.KeyCode == Keys.Q)
            {
                if (!isJumpingLeft && !isJumping && !isFalling)
                {
                    isJumpingLeft = true;
                    jumpFrameLeft = 0;
                    jumpVelocity = jumpStartVelocity;
                    IsLeft = false;
                    IsRight = false;
                    LastDirectionIsRight = false;
                }   
            }
            if (e.KeyCode == Keys.E)
            {
                if (!isJumpingRight && !isJumping && !isFalling)
                {
                    isJumpingRight = true;
                    jumpFrameRight = 0;
                    jumpVelocity = jumpStartVelocity;
                    IsLeft = false;
                    IsRight = false;
                    LastDirectionIsRight = true;
                }
            }
            drawdubb(this.CreateGraphics());
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            cttimer++;
            if (!IsLeft && !IsRight && !isJumpingLeft && cttimer==5)
            {
                idleframe++;
                if (idleframe == LHeroIL.Count)
                    idleframe = 0;
                cttimer = 0;
            }

            
            int enemyX = 0;
            int screenScrollLimit = Lwrld[0].rcSrc.Width;
            if (enemyX < screenScrollLimit)
            {
                foreach (var enemy in LEnemyR)
                    enemy.rcDst.X += 20;
            }
            flag2++;
            if (flag2 == LEnemyR.Count)
                flag2 = 0;

           
            if (isJumpingLeft)
            {
                
                jumpFrameLeft++;
                if (jumpFrameLeft >= LHeroJL.Count)
                    jumpFrameLeft = 0;

                // Move left and up/down
                foreach (var hero in LHeroJL)
                {
                    hero.rcDst.X -= 10; // Move left
                    hero.rcDst.Y += (int)jumpVelocity; // Move up/down
                   
                }
                jumpVelocity += gravity;

                
                if (LHeroJL[0].rcDst.Y >= groundY)
                {
                    foreach (var hero in LHeroJL)
                        hero.rcDst.Y = groundY;
                    foreach(var hero in LHeroR)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroL)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroJR)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                   




                    isJumpingLeft = false;
                    jumpFrameLeft = 0;
                    idleframe = 0;
                    cttimer = 0;
                }
            }
            if (isJumpingRight)
            {
                jumpFrameRight++;
                if (jumpFrameRight >= LHeroJR.Count)
                    jumpFrameRight = 0;

                // Move right and up/down
                foreach (var hero in LHeroJR)
                {
                    hero.rcDst.X += 10; // Move right
                    hero.rcDst.Y += (int)jumpVelocity; // Move up/down
                }
                jumpVelocity += gravity;

                if (LHeroJR[0].rcDst.Y >= groundY)
                {
                    foreach (var hero in LHeroJR)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroR)
                        hero.rcDst.X = LHeroJR[0].rcDst.X;
                    foreach (var hero in LHeroL)
                        hero.rcDst.X = LHeroJR[0].rcDst.X;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X = LHeroJR[0].rcDst.X;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.X = LHeroJR[0].rcDst.X;
                    foreach (var hero in LHeroJL)
                        hero.rcDst.X = LHeroJR[0].rcDst.X;

                    isJumpingRight = false;
                    jumpFrameRight = 0;
                    idleframe = 0;
                    cttimer = 0;
                }
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
            creatEnemyRight();
            createHeroJumpLeft();
            createHeroJumpRight();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawdubb(e.Graphics);
        }
        private void createworld()
        {
            CAdvImgActor pnn = new CAdvImgActor();
            pnn.wrld = new Bitmap("Assets/maps/Level1.png");
            pnn.rcSrc = new Rectangle(mapstart, 0, mapend, 220);
            pnn.rcDst = new Rectangle(0, 0, this.ClientSize.Width, 400);
            Lwrld.Add(pnn);
        }
        void creatEnemyRight()
        {
            //1
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(0, 0, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);
            //2
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(120, 0, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);
            //3
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(250, 0, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);
            //4
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(380, 0, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);
            //5
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(510, 0, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);

            //6
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(650, 0, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);

            //7
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(780, 0, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);

            //8
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(910, 0, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);

            //9
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(510, 120, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);

            //10
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(650, 120, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);

            //11
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(780, 120, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);

            //12
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(910, 120, 150, 120);
            pnn.rcDst = new Rectangle(0, 150, 114, 70);
            LEnemyR.Add(pnn);
        }

        private void createHeroRight()
        {
            //first half of right
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 80, 110, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 140, 110, 65, 50);
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
        private void createHeroJumpLeft()
        {
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(10, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(80, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(145, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(215, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(280, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(350, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(415, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(475, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(540, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(605, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJL.Add(pnn);
        }
        private void createHeroJumpRight()
        {
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 70, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 140, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 205, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 270, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 340, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 410, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 478, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 540, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 600, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);

             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 665, 405, 60, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroJR.Add(pnn);
        }


        private void drawscene(Graphics g2)
        {
            g2.Clear(Color.White);
            for (int i = 0; i < Lwrld.Count; i++)
            {
                CAdvImgActor ptrav = Lwrld[i];
                g2.DrawImage(ptrav.wrld, ptrav.rcDst, ptrav.rcSrc, GraphicsUnit.Pixel);
            }

            if (isJumpingLeft)
            {
                g2.DrawImage(LHeroJL[jumpFrameLeft].wrld, LHeroJL[jumpFrameLeft].rcDst, LHeroJL[jumpFrameLeft].rcSrc, GraphicsUnit.Pixel);
            }
            else if (isJumpingRight)
            {
                g2.DrawImage(LHeroJR[jumpFrameRight].wrld, LHeroJR[jumpFrameRight].rcDst, LHeroJR[jumpFrameRight].rcSrc, GraphicsUnit.Pixel);
            }
            else if (IsRight)
            {
                g2.DrawImage(LHeroR[flag].wrld, LHeroR[flag].rcDst, LHeroR[flag].rcSrc, GraphicsUnit.Pixel);
            }
            else if (IsLeft)
            {
                g2.DrawImage(LHeroL[flag1].wrld, LHeroL[flag1].rcDst, LHeroL[flag1].rcSrc, GraphicsUnit.Pixel);
            }
            else
            {
                if (LastDirectionIsRight)
                    g2.DrawImage(LHeroIR[idleframe].wrld, LHeroIR[idleframe].rcDst, LHeroIR[idleframe].rcSrc, GraphicsUnit.Pixel);
                else
                    g2.DrawImage(LHeroIL[idleframe].wrld, LHeroIL[idleframe].rcDst, LHeroIL[idleframe].rcSrc, GraphicsUnit.Pixel);
            }

            g2.DrawImage(LEnemyR[flag2].wrld, LEnemyR[flag2].rcDst, LEnemyR[flag2].rcSrc, GraphicsUnit.Pixel);
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
        public int worldx;
    }
}