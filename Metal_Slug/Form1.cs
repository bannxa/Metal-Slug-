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
        List<CMultiImageActor> Llazer = new List<CMultiImageActor>(); // first lazer
        List<CMultiImageActor> LHeroSL = new List<CMultiImageActor>(); // hero shoot left
        List<CImageActor> LBulletL = new List<CImageActor>(); // Left Bullet  
        List<Cactor> lazer = new List<Cactor>(); // actors for lazer fire
        List<CMultiImageActor> Lboom = new List<CMultiImageActor>();
        List<CMultiImageActor> Lenemy2 = new List<CMultiImageActor>();




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
        public int deathenenmy1 = 1500;
        public int slowdownspedd = 0;   // global count in tick to slow down the speed 
        public int Heightlaser;
        public int laserflag = 0;
        public int isShooting = -1;  // -1 means not shooting, 0 means shooting left, 1 means shooting right
        public int shootFrame = 0;
        public bool startshooting = false;
        public int startstairs = 0;
        public int fenemy2 = 0;







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
            isShooting = -1;
            shootFrame = 0;
            startshooting = false; // Reset shooting state
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {




            if (e.KeyCode == Keys.D)
            {
                if (isJumpingLeft || isShooting == 0) return; // Don't allow move while jumping left

                IsRight = true;
                IsLeft = false;
                LastDirectionIsRight = true;

                int heroX = LHeroR[flag].rcDst.X;
                int heroWidth = LHeroR[flag].rcDst.Width;
                int screenScrollLimit = this.ClientSize.Width / 2;
                int maxMapScroll = Lwrld[0].wrld.Width - Lwrld[0].rcSrc.Width;

                int heroRightOnMap = Lwrld[0].rcSrc.X + heroX + heroWidth;

                if (heroRightOnMap >= Lwrld[0].wrld.Width - 400)
                {
                    
                }
                // lader on Lwrld[0].wrld.Width-400    important
                

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
                    foreach (var hero in LHeroSL)
                        hero.rcDst.X += 12;
                   
                }
                else if (Lwrld[0].rcSrc.X < maxMapScroll)
                {
                    Lwrld[0].rcSrc.X += 12;
                    foreach (var enemy in LEnemyR)
                        enemy.rcDst.X -= 16;

                    deathenenmy1 -= 16;

                    foreach (var lazer in Llazer)
                        lazer.rcDst.X -= 16;

                    foreach (var bullet in LBulletL)
                        bullet.x -= 12;

                    if (LHeroL[flag1].rcDst.X > Llazer[0].rcDst.X)
                    {
                        foreach (var enemy2 in Lenemy2)
                            enemy2.rcDst.X -= 16;
                    }

                    //foreach (var lazer in Llazerfire)
                    //    lazer.rcDst.X -= 16;

                    lazer[0].x -= 16; // Adjust the position of the lazer actor

                    foreach (var boom in Lboom)
                    {
                        boom.rcDst.X -= 16;

                    }
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
                if (isJumpingLeft || isShooting == 0) return; // Don't allow move while jumping left

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
                    foreach (var hero in LHeroSL)
                        hero.rcDst.X -= 12;
                }
                else if (Lwrld[0].rcSrc.X > minMapScroll)
                {
                    Lwrld[0].rcSrc.X -= 12;

                    foreach (var enemy in LEnemyR)
                        enemy.rcDst.X += 16;

                    deathenenmy1 += 16;

                    foreach (var lazer in Llazer)
                        lazer.rcDst.X += 16;

                    lazer[0].x += 16;
                    if (LHeroL[flag1].rcDst.X > Llazer[0].rcDst.X)
                    {
                        foreach (var enemy2 in Lenemy2)
                            enemy2.rcDst.X += 16;
                    }

                    foreach (var boom in Lboom)
                    {
                        boom.rcDst.X += 16;

                    }
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
                if (!isJumpingLeft && !isJumping && !isFalling || isShooting == 0)
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
                if (!isJumpingRight && !isJumping && !isFalling || isShooting == 0)
                {
                    isJumpingRight = true;
                    jumpFrameRight = 0;
                    jumpVelocity = jumpStartVelocity;
                    IsLeft = false;
                    IsRight = false;
                    LastDirectionIsRight = true;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                isShooting = 0; // Shooting left
                IsLeft = true;
                IsRight = false;
            }





            drawdubb(this.CreateGraphics());
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            cttimer++;
            //shooting logic 
            if (isShooting == 0 && cttimer % 2 == 0)
            {
                if (shootFrame < LHeroSL.Count - 1)
                {
                    shootFrame++;
                }
                if (shootFrame == LHeroSL.Count - 1)
                {
                    startshooting = true;
                    if(LBulletL.Count > 0) // Limit the number of bullets
                    {
                        if (LBulletL[LBulletL.Count - 1].x <= LHeroSL[shootFrame].rcDst.X - 100 ) 
                        {
                            createLeftBullet(); // Create a new bullet
                        }
                    }
                    else if (LBulletL.Count == 0)
                    {
                        createLeftBullet(); // Create the first bullet
                    }
                    
                    
                }

            }

            if (Lenemy2[0].rcDst.X> Llazer[0].rcDst.X+200)
            {
                if (LHeroL[flag1].rcDst.X > Llazer[0].rcDst.X)
                {
                    if (fenemy2 < 11)
                    {
                        fenemy2++;
                    }
                    else
                    {
                        fenemy2 = 0;
                    }
                    foreach (var enemy2 in Lenemy2)
                        enemy2.rcDst.X -= 5;

                }
            }
           




            for(int i = 0; i < LBulletL.Count; i++)
            {
                if (LBulletL[i].x <= 0)
                {
                    LBulletL.RemoveAt(i);
                    i--;
                }
                else
                {
                    LBulletL[i].x -= 10;
                }
            }
            if (!IsLeft && !IsRight && !isJumpingLeft && cttimer % 5 == 0)
            {
                idleframe++;
                if (idleframe == LHeroIL.Count)
                    idleframe = 0;

            } //idle anim


            int enemyX = 0;
            int screenScrollLimit = Lwrld[0].rcSrc.Width;
            if (enemyX < screenScrollLimit)
            {
                foreach (var enemy in LEnemyR)
                    enemy.rcDst.X += 7;
                
            }
            flag2++;
            if (flag2 == LEnemyR.Count)
                flag2 = 0;

            foreach (var enemy in LEnemyR)
            {
                if (deathenenmy1+2200 <= enemy.rcDst.X + enemy.rcDst.Width+700)
                {
                    enemy.f = 1;
                }
            }

            if(cttimer%30==0 && LEnemyR[flag2].f!=1)
            {
                CMultiImageActor pnn = new CMultiImageActor();
                pnn.wrld = new Bitmap("Assets/enemy/7.png");
                pnn.rcSrc = new Rectangle(0, 0, 49, 52);
                pnn.rcDst = new Rectangle(LEnemyR[flag2].rcDst.X+20, LEnemyR[flag2].rcDst.Y+20, 20, 20);
                Lboom.Add(pnn);
 
            }
            foreach (var boom in Lboom)
            {
                if(boom.rcDst.Y< LHeroR[flag].rcDst.Y+80)
                    boom.rcDst.Y += 7;
                else
                    boom.f = 1;

            }

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
                    foreach (var hero in LHeroR)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroL)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroJR)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    foreach (var hero in LHeroSL)
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
                    foreach (var hero in LHeroSL)
                        hero.rcDst.X = LHeroJR[0].rcDst.X;

                    isJumpingRight = false;
                    jumpFrameRight = 0;
                    idleframe = 0;
                    cttimer = 0;
                }
            }
            slowdownspedd++;

            if (Heightlaser <= groundY && laserflag == 0)
            {
                Heightlaser += 10;
                if (Heightlaser >= groundY)
                {
                    laserflag = 1;
                }
            }
            else if (laserflag == 1)
            {
                Heightlaser -= 10;
                if (Heightlaser <= 10)
                {
                    laserflag = 0;
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
            createlazer();
            createHeroShootingLeft();
            createnemy2();
            

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawdubb(e.Graphics);
        }

        void createnemy2()
        {
            //1
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(0, 0, 140, 100);
            pnn.rcDst = new Rectangle(1500, 280, 140, 100);
            Lenemy2.Add(pnn);

            //2
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(140, 0, 140, 100);
            pnn.rcDst = new Rectangle(300, 280, 140, 100);
            Lenemy2.Add(pnn);

            //3
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(280, 0, 120, 100);
            pnn.rcDst = new Rectangle(300, 280, 120, 100);
            Lenemy2.Add(pnn);


            //4
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(400, 0, 120, 100);
            pnn.rcDst = new Rectangle(300, 280, 120, 100);
            Lenemy2.Add(pnn);


            //5
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(510, 0, 120, 100);
            pnn.rcDst = new Rectangle(300, 280, 120, 100);
            Lenemy2.Add(pnn);


            //6
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(630, 0, 120, 100);
            pnn.rcDst = new Rectangle(300, 280, 120, 100);
            Lenemy2.Add(pnn);

            //7
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(750, 0, 150, 100);
            pnn.rcDst = new Rectangle(300, 280, 150, 100);
            Lenemy2.Add(pnn);


            //8
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(900, 0, 150, 100);
            pnn.rcDst = new Rectangle(300, 280, 150, 100);
            Lenemy2.Add(pnn);




            //9
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(0, 100, 140, 58);
            pnn.rcDst = new Rectangle(300, 310, 140, 58);
            Lenemy2.Add(pnn);


            //10
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(140, 100, 120, 58);
            pnn.rcDst = new Rectangle(300, 310, 120, 58);
            Lenemy2.Add(pnn);

            //11
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(260, 80, 120, 70);
            pnn.rcDst = new Rectangle(300, 280, 120, 70);
            Lenemy2.Add(pnn);


            //12
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(380, 80, 120, 70);
            pnn.rcDst = new Rectangle(300, 280, 120, 70);
            Lenemy2.Add(pnn);
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
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);
            //2
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(120, 0, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);
            //3
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(250, 0, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);
            //4
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(380, 0, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);
            //5
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(510, 0, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);

            //6
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(650, 0, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);

            //7
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(780, 0, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);

            //8
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(910, 0, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);

            //9
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(510, 120, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);

            //10
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(650, 120, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);

            //11
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(780, 120, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);

            //12
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/2.png");
            pnn.rcSrc = new Rectangle(910, 120, 150, 120);
            pnn.rcDst = new Rectangle(480, 150, 114, 70);
            LEnemyR.Add(pnn);


            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/7.png");
            pnn.rcSrc = new Rectangle(0, 0, 49, 52);
            pnn.rcDst = new Rectangle(LEnemyR[flag2].rcDst.X, LEnemyR[flag2].rcDst.Y + 20, 20, 20);
            Lboom.Add(pnn);
        }

        void createlazer()
        {

            //9
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/4.png");
            pnn.rcSrc = new Rectangle(400, 0, 30, 80);
            pnn.rcDst = new Rectangle(460, 10, 30, 80);
            Llazer.Add(pnn);

            Cactor pnn2 = new Cactor();
            pnn2.x = 474;
            pnn2.y = pnn.wrld.Height;
            lazer.Add(pnn2);

            Heightlaser = 10;




            ////fire
            //pnn = new CMultiImageActor();
            //pnn.wrld = new Bitmap("Assets/enemy/5.png");
            //pnn.rcSrc = new Rectangle(0, 0, 90, 200);
            //pnn.rcDst = new Rectangle(430, 10, 40, 80);
            //Llazerfire.Add(pnn);


            // pnn = new CMultiImageActor();
            //pnn.wrld = new Bitmap("Assets/enemy/5.png");
            //pnn.rcSrc = new Rectangle(90, 0, 80, 200);
            //pnn.rcDst = new Rectangle(430, 10, 40, 80);
            //Llazerfire.Add(pnn);


            // pnn = new CMultiImageActor();
            //pnn.wrld = new Bitmap("Assets/enemy/5.png");
            //pnn.rcSrc = new Rectangle(160, 0, 70, 200);
            //pnn.rcDst = new Rectangle(430, 10, 40, 80);
            //Llazerfire.Add(pnn);


            // pnn = new CMultiImageActor();
            //pnn.wrld = new Bitmap("Assets/enemy/5.png");
            //pnn.rcSrc = new Rectangle(230, 0, 70, 240);
            //pnn.rcDst = new Rectangle(430, 10, 40, 80);
            //Llazerfire.Add(pnn);

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
        private void createHeroShootingLeft()
        {
            //first half of left shooting

            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(10, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(80, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(145, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(215, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(280, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(340, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(405, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(465, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(535, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(605, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSL.Add(pnn);

            //second half of left shooting

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(10, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(80, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(145, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(215, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(280, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(350, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(415, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(475, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(540, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/1.png");
            pnn.rcSrc = new Rectangle(605, 710, 65, 50);
            pnn.rcDst = new Rectangle(100, 245, 150, 120);
            LHeroSL.Add(pnn);
        }
        private void createLeftBullet()
        {
            CImageActor pnn = new CImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/b.png");
            pnn.wrld.MakeTransparent(Color.White);
            pnn.x = LHeroSL[LHeroSL.Count-1].rcDst.X - 40;
            pnn.y = LHeroSL[LHeroSL.Count - 1].rcDst.Y + 40;
            LBulletL.Add(pnn);

        }


        private void drawscene(Graphics g2)
        {
            g2.Clear(Color.White);
            for (int i = 0; i < Lwrld.Count; i++)
            {
                CAdvImgActor ptrav = Lwrld[i];
                g2.DrawImage(ptrav.wrld, ptrav.rcDst, ptrav.rcSrc, GraphicsUnit.Pixel);
            }

            // Only draw the hero if not shooting
            if (isShooting == 0)
            {
                g2.DrawImage(LHeroSL[shootFrame].wrld, LHeroSL[shootFrame].rcDst, LHeroSL[shootFrame].rcSrc, GraphicsUnit.Pixel); // shooting left
            }
            else if (isJumpingLeft)
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

            if (LEnemyR[flag2].f == 0)
                g2.DrawImage(LEnemyR[flag2].wrld, LEnemyR[flag2].rcDst, LEnemyR[flag2].rcSrc, GraphicsUnit.Pixel);

            g2.DrawImage(Llazer[0].wrld, Llazer[0].rcDst, Llazer[0].rcSrc, GraphicsUnit.Pixel); //lazer
            g2.FillRectangle(Brushes.DarkRed, lazer[0].x, lazer[0].y, 5, Heightlaser); //red laser
            for (int i=0;i<LBulletL.Count;i++)
            {
                CImageActor bullet = LBulletL[i];
                g2.DrawImage(bullet.wrld, bullet.x, bullet.y, 70, 60); //draw bullet

            }
            for (int i = 0; i < Lboom.Count; i++)
            {
                CMultiImageActor boom = Lboom[i];
                if(boom.f!=1)
                    g2.DrawImage(boom.wrld, boom.rcDst, boom.rcSrc, GraphicsUnit.Pixel);
            }

            
                g2.DrawImage(Lenemy2[fenemy2].wrld, Lenemy2[fenemy2].rcDst, Lenemy2[fenemy2].rcSrc, GraphicsUnit.Pixel);

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
        public int f = 0;       //if the enemy die or not

    }
    public class Cactor
    {
        public int x, y;
    }
    public class CImageActor
    {
        public Bitmap wrld;
        public int x, y;

    }
}