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
        List<CMultiImageActor> LHeroSR = new List<CMultiImageActor>(); // hero shoot left
        List<CImageActor> LBulletL = new List<CImageActor>(); // Left Bullet  
        List<CImageActor> LBulletR = new List<CImageActor>(); // Right Bullet  
        List<CImageActor> LPlatform = new List<CImageActor>();   
        List<CImageActor> LLadder = new List<CImageActor>();   
        List<Cactor> lazer = new List<Cactor>(); // actors for lazer fire
        List<CMultiImageActor> Lboom = new List<CMultiImageActor>();
        List<CMultiImageActor> Lenemy2 = new List<CMultiImageActor>();
        List<CMultiImageActor> Lenemy3 = new List<CMultiImageActor>();




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
        public bool flagelevator = false;
        public bool key = false; // for elevator
        public int flagplatform = 0; // for platform
        public int platformStartX; // The starting X position of the platform
        public int platformEndX;   // The ending X position of the platform
        public int maxMapScroll;
        public int IsOnladder =-1;
        public bool isFallingFromLadder = false;
        public int fenemy3 = 0; 






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

            if(!isFalling)
            {
                if (!flagelevator)
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


                        int heroRightOnMap = Lwrld[0].rcSrc.X + heroX + heroWidth;

                        if (heroRightOnMap >= Lwrld[0].wrld.Width - 250)                // lader on Lwrld[0].wrld.Width-400    important
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
                            foreach (var hero in LHeroSL)
                                hero.rcDst.X += 12;
                            foreach (var hero in LHeroSR)
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
                                bullet.x -= 16;
                            LPlatform[0].x -= 16;
                            LLadder[0].x -= 16;
                            foreach (var enemy3 in Lenemy3)
                                enemy3.rcDst.X -= 16;



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
                            foreach (var hero in LHeroSL)
                                hero.rcDst.X += 12;
                            foreach (var hero in LHeroSR)
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
                            foreach (var hero in LHeroSR)
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
                            LPlatform[0].x += 16;
                            LLadder[0].x += 16;
                            foreach (var enemy3 in Lenemy3)
                            enemy3.rcDst.X += 16;

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
                            foreach (var hero in LHeroSL)
                                hero.rcDst.X -= 12;
                            foreach (var hero in LHeroSR)
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
                    if (e.KeyCode == Keys.Right)
                    {
                        isShooting = 1; // Shooting right
                        IsLeft = false;
                        IsRight = true;
                    }
                    if (e.KeyCode == Keys.W)
                    {
                        if (LHeroIL[idleframe].rcDst.X >= LLadder[0].x && LHeroIL[idleframe].rcDst.X <= LLadder[0].x + LLadder[0].wrld.Width && LHeroIL[idleframe].rcDst.Y >= 0)
                        {
                            IsOnladder = 1; // Indicate that the hero is on the ladder
                            foreach (var hero in LHeroL)
                                hero.rcDst.Y -= 12;
                            foreach (var hero in LHeroR)
                                hero.rcDst.Y -= 12;
                            foreach (var hero in LHeroIL)
                                hero.rcDst.Y -= 12;
                            foreach (var hero in LHeroIR)
                                hero.rcDst.Y -= 12;
                            foreach (var hero in LHeroJL)
                                hero.rcDst.Y -= 12;
                            foreach (var hero in LHeroJR)
                                hero.rcDst.Y -= 12;
                            foreach (var hero in LHeroSL)
                                hero.rcDst.Y -= 12;
                            foreach (var hero in LHeroSR)
                                hero.rcDst.Y -= 12;
                        }
                        else
                        {
                            IsOnladder = -1; // Reset the ladder state if not on the ladder
                            if (LHeroIL[idleframe].rcDst.Y < groundY && !isJumping && !isJumpingLeft && !isJumpingRight)
                            {
                                isFallingFromLadder = true;
                                jumpVelocity = 0;
                            }
                        }
                    }
                    if (e.KeyCode == Keys.S)
                    {
                        if (LHeroIL[idleframe].rcDst.X >= LLadder[0].x && LHeroIL[idleframe].rcDst.X <= LLadder[0].x + LLadder[0].wrld.Width && LHeroIL[idleframe].rcDst.Y <= groundY + 5)
                        {
                            IsOnladder = 1; // Indicate that the hero is on the ladder
                            foreach (var hero in LHeroL)
                                hero.rcDst.Y += 12;
                            foreach (var hero in LHeroR)
                                hero.rcDst.Y += 12;
                            foreach (var hero in LHeroIL)
                                hero.rcDst.Y += 12;
                            foreach (var hero in LHeroIR)
                                hero.rcDst.Y += 12;
                            foreach (var hero in LHeroJL)
                                hero.rcDst.Y += 12;
                            foreach (var hero in LHeroJR)
                                hero.rcDst.Y += 12;
                            foreach (var hero in LHeroSL)
                                hero.rcDst.Y += 12;
                            foreach (var hero in LHeroSR)
                                hero.rcDst.Y += 12;
                        }
                        else
                        {
                            IsOnladder = -1; // Reset the ladder state if not on the ladder
                            if (LHeroIL[idleframe].rcDst.Y < groundY && !isJumping && !isJumpingLeft && !isJumpingRight)
                            {
                                isFallingFromLadder = true;
                                jumpVelocity = 0;
                            }
                        }


                    }
                }
            }
           
            
            drawdubb(this.CreateGraphics());
        }
            


            
        

        private void Tt_Tick(object sender, EventArgs e)
        {
            if (Lenemy3[0].rcDst.X > Llazer[0].rcDst.X + 1200)
            {
                if (LHeroL[flag1].rcDst.X + 500 > Llazer[0].rcDst.X)
                {
                    if (fenemy3 < 5)
                    {
                        fenemy3++;
                    }
                    else
                    {
                        fenemy3 = 0;
                    }
                    foreach (var enemy3 in Lenemy3)
                        enemy3.rcDst.X -= 5;

                }
            }
            checkbulletOnenemy2();
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

            } //left shoot
            if (isShooting == 1 && cttimer % 2 == 0) //right shoot
            {
                if (shootFrame < LHeroSR.Count - 1)
                {
                    shootFrame++;
                }
                if (shootFrame == LHeroSR.Count - 1)
                {
                    startshooting = true;
                    if (LBulletR.Count > 0) 
                    {
                        if (LBulletR[LBulletR.Count - 1].x >= LHeroSR[shootFrame].rcDst.X + 200)
                        {
                            createRightBullet(); // Create a new bullet
                        }
                    }
                    else if (LBulletR.Count == 0)
                    {
                        createRightBullet(); // Create the first bullet
                    }


                }

            } //right shoot

            if (Lenemy2[0].rcDst.X> Llazer[0].rcDst.X)
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



            if (LHeroIL[idleframe].rcDst.X >= LLadder[0].x && LHeroIL[idleframe].rcDst.X <= LLadder[0].x + LLadder[0].wrld.Width && LHeroIL[idleframe].rcDst.Y <= groundY + 5)
            {
              
            }
            else
            {
                IsOnladder = -1;
                if (LHeroIL[idleframe].rcDst.Y < groundY && !isJumping && !isJumpingLeft && !isJumpingRight)
                {
                    isFallingFromLadder = true;
                }
            }

            for (int i = 0; i < LBulletL.Count; i++)
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
            for (int i = 0; i < LBulletR.Count; i++)
            {
                if (LBulletR[i].x >= this.ClientSize.Width)
                {
                    LBulletR.RemoveAt(i);
                    i--;
                }
                else
                {
                    LBulletR[i].x += 10;
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

            if(cttimer%10==0 && LEnemyR[flag2].f!=1)
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
                    foreach (var hero in LHeroSR)
                        hero.rcDst.X = LHeroJL[0].rcDst.X;
                    Lwrld[0].rcSrc.X -=12 ; // Adjust world position to match hero position





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
            if (isFallingFromLadder)
            {
                
                foreach (var hero in LHeroIL)
                    hero.rcDst.Y += (int)jumpVelocity;
                foreach (var hero in LHeroIR)
                    hero.rcDst.Y += (int)jumpVelocity;
                foreach (var hero in LHeroJL)
                    hero.rcDst.Y += (int)jumpVelocity;
                foreach (var hero in LHeroJR)
                    hero.rcDst.Y += (int)jumpVelocity;
                foreach (var hero in LHeroSL)
                    hero.rcDst.Y += (int)jumpVelocity;
                foreach (var hero in LHeroSR)
                    hero.rcDst.Y += (int)jumpVelocity;


                jumpVelocity += gravity;

                // Stop falling when reaching the ground
                if (LHeroIL[0].rcDst.Y >= groundY)
                {
                    foreach (var hero in LHeroIL)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.Y = groundY;
                    isFallingFromLadder = false;
                    jumpVelocity = 0;


                    foreach (var hero in LHeroJR)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroR)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroL)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroJL)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroSL)
                        hero.rcDst.Y = groundY;
                    foreach (var hero in LHeroSR)
                        hero.rcDst.Y = groundY;
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

            if (LHeroIR[idleframe].rcDst.X >= LPlatform[0].x && LHeroIR[idleframe].rcDst.X <= LPlatform[0].x + LPlatform[0].wrld.Width)
            {
                if(flagplatform ==0)
                {
                    flagelevator = true;

                    LPlatform[0].x += 5; // Move the platform to the right

                    foreach (var hero in LHeroR)
                        hero.rcDst.X += 5;
                    foreach (var hero in LHeroL)
                        hero.rcDst.X += 5;
                    foreach (var hero in LHeroIL)
                        hero.rcDst.X += 5;
                    foreach (var hero in LHeroIR)
                        hero.rcDst.X += 5;
                    foreach (var hero in LHeroJL)
                        hero.rcDst.X += 5;
                    foreach (var hero in LHeroJR)
                        hero.rcDst.X += 5;
                    foreach (var hero in LHeroSL)
                        hero.rcDst.X += 5;
                    foreach (var hero in LHeroSR)
                        hero.rcDst.X += 5;
                    if (Lwrld[0].rcSrc.X < Lwrld[0].wrld.Width - Lwrld[0].rcSrc.Width - 10)
                    {
                        Lwrld[0].rcSrc.X += 12;

                    }
                }
      
            }


            drawdubb(this.CreateGraphics());
        }

        void Form1_Load(object sender, EventArgs e)
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
            createHeroShootingRight();
            createPlatform();
            platformStartX = LPlatform[0].x;
            platformEndX = Lwrld[0].wrld.Width - LPlatform[0].wrld.Width - 30;
            maxMapScroll = Lwrld[0].wrld.Width - Lwrld[0].rcSrc.Width;
            createladder();
            createnemy3();



        }
        void Form1_Paint(object sender, PaintEventArgs e)
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
            pnn.rcDst = new Rectangle(1500, 280, 140, 100);
            Lenemy2.Add(pnn);

            //3
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(280, 0, 120, 100);
            pnn.rcDst = new Rectangle(1500, 280, 120, 100);
            Lenemy2.Add(pnn);


            //4
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(400, 0, 120, 100);
            pnn.rcDst = new Rectangle(1500, 280, 120, 100);
            Lenemy2.Add(pnn);


            //5
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(510, 0, 120, 100);
            pnn.rcDst = new Rectangle(1500, 280, 120, 100);
            Lenemy2.Add(pnn);


            //6
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(630, 0, 120, 100);
            pnn.rcDst = new Rectangle(1500, 280, 120, 100);
            Lenemy2.Add(pnn);

            //7
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(750, 0, 150, 100);
            pnn.rcDst = new Rectangle(1500, 280, 150, 100);
            Lenemy2.Add(pnn);


            //8
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(900, 0, 150, 100);
            pnn.rcDst = new Rectangle(1500, 280, 150, 100);
            Lenemy2.Add(pnn);




            //9
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(0, 100, 140, 58);
            pnn.rcDst = new Rectangle(1500, 310, 140, 58);
            Lenemy2.Add(pnn);


            //10
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(140, 100, 120, 58);
            pnn.rcDst = new Rectangle(1500, 310, 120, 58);
            Lenemy2.Add(pnn);

            //11
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(260, 80, 120, 70);
            pnn.rcDst = new Rectangle(1500, 280, 120, 70);
            Lenemy2.Add(pnn);


            //12
             pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/9.png");
            pnn.rcSrc = new Rectangle(380, 80, 120, 70);
            pnn.rcDst = new Rectangle(1500, 280, 120, 70);
            Lenemy2.Add(pnn);
        }
        void createworld()
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
        void createHeroRight()
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
        void createHeroLeft()
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
        void createHeroIdleLeft()
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
        void createHeroIdleRight()
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
        void createHeroJumpLeft()
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
        void createHeroJumpRight()
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
        void createHeroShootingLeft()
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
        void createHeroShootingRight()
        {
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 70, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 140, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 205, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 270, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 340, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 400, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 460, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 530, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 600, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 660, 650, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            //second half of right shooting

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 75, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 140, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 205, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 275, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 345, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 415, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 480, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 540, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 600, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);

            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/2.png");
            pnn.rcSrc = new Rectangle(pnn.wrld.Width - 670, 710, 60, 50);
            pnn.rcDst = new Rectangle(100, 250, 150, 120);
            LHeroSR.Add(pnn);
        }
        void createLeftBullet()
        {
            CImageActor pnn = new CImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/b.png");
            pnn.wrld.MakeTransparent(Color.White);
            pnn.x = LHeroSL[LHeroSL.Count-1].rcDst.X - 40;
            pnn.y = LHeroSL[LHeroSL.Count - 1].rcDst.Y + 40;
            LBulletL.Add(pnn);

        }
        void createRightBullet()
        {
            CImageActor pnn = new CImageActor();
            pnn.wrld = new Bitmap("Assets/Hero/b.png");
            pnn.wrld.MakeTransparent(Color.White);
            pnn.x = LHeroSL[LHeroSR.Count - 1].rcDst.X + LHeroSR[LHeroSR.Count - 1].rcDst.Width;
            pnn.y = LHeroSL[LHeroSR.Count - 1].rcDst.Y + 45;
            LBulletR.Add(pnn);

        }
        void createPlatform()
        {
            CImageActor pnn = new CImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/6.png");
            pnn.wrld.MakeTransparent(Color.White);
            pnn.x = Lwrld[0].wrld.Width+ 260;
            pnn.y = 355;
            LPlatform.Add(pnn);

        }
        void createladder()
        {
            CImageActor pnn = new CImageActor();
            pnn.wrld = new Bitmap("Assets/Maps/L.png");
            pnn.wrld.MakeTransparent(Color.White);
            pnn.x = 2000;
            pnn.y = 0;
            LLadder.Add(pnn);

        }
        void checkbulletOnenemy2()
        {
            for (int i = LBulletR.Count - 1; i >= 0; i--)
            {
                CImageActor bullet = LBulletR[i];
                // Check all frames of enemy 2 (assuming enemy 2 is all frames in Lenemy2)
                for (int e = 0; e < Lenemy2.Count; e++)
                {
                    if (Lenemy2[e].f == 0) // Only check if alive
                    {
                        Rectangle enemyRect = Lenemy2[e].rcDst;
                        Rectangle bulletRect = new Rectangle(bullet.x, bullet.y, bullet.wrld.Width, bullet.wrld.Height);
                        if (enemyRect.IntersectsWith(bulletRect))
                        {
                            // Mark all frames as dead
                            for (int j = 0; j < Lenemy2.Count; j++)
                                Lenemy2[j].f = 1;
                            // Remove the bullet
                            LBulletR.RemoveAt(i);
                            return; // Only kill once per bullet
                        }
                    }
                }
            }
        }
        private void drawdubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawscene(g2);
            g.DrawImage(off, 0, 0);
        }
        private void drawscene(Graphics g2)
        {
            g2.Clear(Color.White);
            for (int i = 0; i < Lwrld.Count; i++)
            {
                CAdvImgActor ptrav = Lwrld[i];
                g2.DrawImage(ptrav.wrld, ptrav.rcDst, ptrav.rcSrc, GraphicsUnit.Pixel);
            }
            g2.DrawImage(LLadder[0].wrld, LLadder[0].x, LLadder[0].y, 250, 400); //draw ladder
            // Only draw the hero if not shooting
            if (isShooting == 0)
            {
                g2.DrawImage(LHeroSL[shootFrame].wrld, LHeroSL[shootFrame].rcDst, LHeroSL[shootFrame].rcSrc, GraphicsUnit.Pixel); // shooting left
            }
            else if (isShooting == 1)
            {
                g2.DrawImage(LHeroSR[shootFrame].wrld, LHeroSR[shootFrame].rcDst, LHeroSR[shootFrame].rcSrc, GraphicsUnit.Pixel); // shooting right
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
            for (int i = 0; i < LBulletR.Count; i++)
            {
                CImageActor bullet = LBulletR[i];
                g2.DrawImage(bullet.wrld, bullet.x, bullet.y, 70, 60); //draw bullet

            }
            for (int i = 0; i < Lboom.Count; i++)
            {
                CMultiImageActor boom = Lboom[i];
                if(boom.f!=1)
                    g2.DrawImage(boom.wrld, boom.rcDst, boom.rcSrc, GraphicsUnit.Pixel);
            }

            g2.DrawImage(LPlatform[0].wrld, LPlatform[0].x, LPlatform[0].y, 150, 50); //draw platform
            if (Lenemy2.Count > 0 && Lenemy2[0].f == 0)
                g2.DrawImage(Lenemy2[fenemy2].wrld, Lenemy2[fenemy2].rcDst, Lenemy2[fenemy2].rcSrc, GraphicsUnit.Pixel);
            g2.DrawImage(Lenemy3[fenemy3].wrld, Lenemy3[fenemy3].rcDst, Lenemy3[fenemy3].rcSrc, GraphicsUnit.Pixel);


        }
        void createnemy3()
        {
            //1
            CMultiImageActor pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/12.png");
            pnn.rcSrc = new Rectangle(0, 0, 120, 90);
            pnn.rcDst = new Rectangle(3000, 270, 120, 90);
            Lenemy3.Add(pnn);

            //2
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/12.png");
            pnn.rcSrc = new Rectangle(110, 0, 120, 90);
            pnn.rcDst = new Rectangle(3000, 270, 120, 90);
            Lenemy3.Add(pnn);


            //3
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/12.png");
            pnn.rcSrc = new Rectangle(230, 0, 120, 90);
            pnn.rcDst = new Rectangle(3000, 270, 120, 90);
            Lenemy3.Add(pnn);


            //4
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/12.png");
            pnn.rcSrc = new Rectangle(345, 0, 120, 90);
            pnn.rcDst = new Rectangle(3000, 270, 120, 90);
            Lenemy3.Add(pnn);



            //5
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/12.png");
            pnn.rcSrc = new Rectangle(460, 0, 120, 90);
            pnn.rcDst = new Rectangle(3000, 270, 120, 90);
            Lenemy3.Add(pnn);


            //6
            pnn = new CMultiImageActor();
            pnn.wrld = new Bitmap("Assets/enemy/12.png");
            pnn.rcSrc = new Rectangle(575, 0, 120, 90);
            pnn.rcDst = new Rectangle(3000, 270, 120, 90);
            Lenemy3.Add(pnn);

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