using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Media;

namespace PacMan
{
    //Player controllable character
    //Персонаж, що керується гравцем
    public class PacMan
    {
        PacForm form;
        //Movement direction 0-up, 1-left, 2-down, 3-right
        //Напрямок руху 0-вгору, 1-вліво, 2-вниз, 3-вправо
        public int direction;
        public int nextDirection;
        //Player input direction. Напрямок руху, який задає гравець
        public int playerInput;
        //Pac-Man coordinate. Координата гравця
        public PointF location;
        public Point previousCoordinate;
        public Point currentCoordinate;
        //Is Pac-Man dead. Прапорець смерті пакмена
        public bool dead;
        //Pac-Man speed. Швидкість пакмена
        public float speed;
        //Current animation sprite. Малюнок поточної фази анімації
        public Bitmap currentSprite;
        //Collection of Pac-Man sprites. Набір анімації пакмена
        public List<Bitmap> pacmanSprites = new List<Bitmap>();
        //Controlls animation loop. Контролює періоди анімації
        public int spriteLoop;
        public int deadSpriteLoop;
        //Moving flag, used to stop animation when Pac-Man is facing a dead end
        //Прапорець руху, який зупиняє анімацію пакмена, коли той у тупику і не рухається
        public bool moving = false;
        //don't center when game starts. Пакмен не центрує координати на початку гри
        bool keyPressed;
        //Number of Pac-Man lives. Кількість спроб гравця
        public int lives;
        //Check for bonus life. Перевірка можливості збільшення кількості спроб
        int bonusCounter;
        //Pac-Man can eat charged pill. Пакмен може з'їсти привидів
        public bool energized;
        //System.Windows.Forms.Timer energizedTimer;
        //Stopwatch energizedStopwatch;
        public int sleepAfterLunch;
        MediaPlayer wmp_death_1;
        MediaPlayer wmp_death_2;
        public PacMan(PacForm frm)
        {
            lives = 4;
            direction = 0;
            nextDirection = 0;
            deadSpriteLoop = 0;
            spriteLoop = 0;
            form = frm;
            dead = false;
            MakeSprites();
            location = PacForm.coordToPixelCenter(new Point(14, 26));
            location.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            speed = 1f;
            currentSprite = pacmanSprites[2];
            bonusCounter = 0;
            InitializeSound();
        }

        public void ResetPosition()
        {
            location = PacForm.coordToPixelCenter(new Point(14, 26));
            location.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            direction = 0;
            nextDirection = 0;
            currentSprite = pacmanSprites[2];
            moving = false;
            dead = false;
            keyPressed = false;
        }
        //Pac-Man's moving logic. Керує логікою руху пакмена

        public void Move(float elapsed)
        {
            if (sleepAfterLunch == 0)
            {
                GetLife();
                if (energized) speed = form.levels.speed * form.levels.get_frightPacmanSpeed() * elapsed;
                else speed = form.levels.speed * form.levels.get_pacmanSpeed() * elapsed;
                //speed = 3f;
                form.items.Eat(PacForm.pixelToCoord(location));
                if (CanMove(nextDirection))
                {
                    direction = nextDirection;
                }
                //else nextDirection = direction;

                if (CanMove(direction))
                {
                    moving = true;
                    switch (direction)
                    {
                        case 0:
                            location.Y -= speed;
                            centerX();
                            break;
                        case 1:
                            location.X -= speed;
                            centerY();
                            break;
                        case 2:
                            location.Y += speed;
                            centerX();
                            break;
                        case 3:
                            location.X += speed;
                            centerY();
                            break;
                    }
                }
                else
                {
                    moving = false;
                    //don't center when game starts
                    if(keyPressed)
                    {
                        centerX();
                        centerY();
                    }
                }

                if (location.X < 0)
                    location = new PointF(location.X + form.board.boardPBSize.Width, location.Y);
                if (location.X >= form.board.boardPBSize.Width)
                    location = new PointF(location.X - form.board.boardPBSize.Width, location.Y);
            }
            else sleepAfterLunch--;
        }
        void centerX()
        {
            if (location.X % PacForm.boardUnitSize < PacForm.unitCenter)
            {
                if (location.X + speed % PacForm.boardUnitSize < PacForm.unitCenter)
                {
                    location.X += speed;
                }
                else
                {
                    location.X = (float)Math.Round(location.X);
                    location.X++;
                }
            }
            if (location.X % PacForm.boardUnitSize > PacForm.unitCenter)
                if (location.X - speed % PacForm.boardUnitSize > PacForm.unitCenter)
                {
                    location.X -= speed;
                }
                else
                {
                    location.X = (float)Math.Round(location.X);
                    location.X--;
                }
        }
        void centerY()
        {
            if (location.Y % PacForm.boardUnitSize < PacForm.unitCenter)
            {
                if (location.Y + speed % PacForm.boardUnitSize < PacForm.unitCenter)
                {
                    location.Y += speed;
                }
                else
                {
                    location.Y = (float)Math.Round(location.Y);
                    location.Y++;
                }
            }
            if (location.Y % PacForm.boardUnitSize > PacForm.unitCenter)
                if (location.Y - speed % PacForm.boardUnitSize > PacForm.unitCenter)
                {
                    location.Y -= speed;
                }
                else
                {
                    location.Y = (float)Math.Round(location.Y);
                    location.Y--;
                }
        }
        //Determines if movement is possible. Визначає, чи можливий рух
        public bool CanMove(int dir)
        {
            Point nextCoord = PacForm.pixelToCoord(location);
            switch (dir)
            {
                case 0:
                    if (form.board.ValidMove(new Point(nextCoord.X, nextCoord.Y - 1)))
                        return true;
                    break;
                case 1:
                    if ((nextCoord.X - 1) < 0)
                        nextCoord = new Point(nextCoord.X + form.board.size.Width, nextCoord.Y);
                    if (form.board.ValidMove(new Point(nextCoord.X - 1, nextCoord.Y)))
                        return true;
                    break;
                case 2:
                    if (form.board.ValidMove(new Point(nextCoord.X, nextCoord.Y + 1)))
                        return true;
                    break;
                case 3:
                    if ((nextCoord.X + 1) >= form.board.size.Width)
                        nextCoord = new Point(nextCoord.X - form.board.size.Width, nextCoord.Y);
                    if (form.board.ValidMove(new Point(nextCoord.X + 1, nextCoord.Y)))
                        return true;
                    break;
            }
            return false;
        }
        //Receives player keyboard input. Отримує команди від клавіатури гравця
        public void PlayerInput(int nextDir)
        {
            keyPressed = true;
            nextDirection = nextDir;
        }
        void GetLife()
        {
            if (form.player.Score >= bonusCounter + form.levels.newLifeScore)
            {
                form.levels.GetLife();
                bonusCounter += form.levels.newLifeScore;
            }
        }
        //Animation. Анімація
        public void ChangeSprite()
        {
            if (dead)
            {
                form.tmr_animation.Interval = 270;
                currentSprite = pacmanSprites[16 + deadSpriteLoop];
                deadSpriteLoop++;
                if (deadSpriteLoop > 11)
                {
                    form.tmr_animation.Stop();
                    form.tmr_animation.Interval = 33;
                    deadSpriteLoop = 0;
                    lives--;
                    if (lives == 0)
                    {
                        form.GameOver();
                    }
                    else form.Restart();
                }
            }
            else if (moving)
            {
                currentSprite = pacmanSprites[direction * 4 + spriteLoop];
                spriteLoop++;
                if (spriteLoop > 3) spriteLoop = 0;
            }
        }
        //Happens after player collides with an enemy
        //Відбувається після зіткнення гравця з противником
        public void Die()
        {
            form.items.alternativeExitCounterAfterPacManDies = true;
            form.tmr_movement.Stop();

            form.wmp_siren_1.Stop();
            form.wmp_siren_2.Stop();
            form.wmp_siren_3.Stop();
            form.wmp_siren_4.Stop();
            form.wmp_siren_5.Stop();

            wmp_death_1.Stop();
            wmp_death_1.Open(new Uri("Sounds/death_1.wav", UriKind.Relative));
            //wmp_death_1.Position = new TimeSpan(0);
            wmp_death_1.Play();

            dead = true;
            moving = false;
            form.keysLocked = true;
        }
        public void PlayDeathSound(object sender, EventArgs e)
        {
            wmp_death_2.Stop();
            wmp_death_2.Open(new Uri("Sounds/death_2.wav", UriKind.Relative));
            //wmp_death_2.Position = new TimeSpan(0);
            wmp_death_2.Play();
        }
        //Cutting sprites from spritesheet. Вирізання спрайтів з полотна
        private void MakeSprites()
        {
            int SpriteLeft = 192;
            int SpriteLeft2 = 196;
            int SpriteUp = 194;
            int SpriteUp2 = 198;
            int SpriteRight = 200;
            int SpriteRight2 = 204;
            int SpriteDown = 202;
            int SpriteDown2 = 206;
            int SpriteFull = 448;
            int SpriteDead = 456;

            pacmanSprites.Add(PacForm.CutSprite(SpriteUp, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteUp2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteFull, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteUp2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            pacmanSprites.Add(PacForm.CutSprite(SpriteLeft, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteLeft2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteFull, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteLeft2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            pacmanSprites.Add(PacForm.CutSprite(SpriteDown, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteDown2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteFull, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteDown2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            pacmanSprites.Add(PacForm.CutSprite(SpriteRight, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteRight2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteFull, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            pacmanSprites.Add(PacForm.CutSprite(SpriteRight2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            for (int i = 0; i < 24; i += 2)
            {
                pacmanSprites.Add(PacForm.CutSprite(SpriteDead + i, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            }
        }
        void InitializeSound()
        {
            wmp_death_1 = new MediaPlayer();
            //wmp_death_1.Open(new Uri("Sounds/death_1.wav", UriKind.Relative));
            wmp_death_1.Volume = PacForm.volume;
            wmp_death_1.MediaEnded += PlayDeathSound;
            wmp_death_2 = new MediaPlayer();
            //wmp_death_2.Open(new Uri("Sounds/death_2.wav", UriKind.Relative));
            wmp_death_2.Volume = PacForm.volume;
        }
        //Drawing Pac-Man. Малювання пакмена
        public void Draw(Graphics gr)
        {
            PointF drawLocation = PacForm.drawCoord(location);
            gr.DrawImage(currentSprite, drawLocation.X, drawLocation.Y);
            //Draw Lives. Малювання кількості спроб
            for (int i = 2; i < lives * 2 + 2; i += 2)
            {
                PointF livesLocation = PacForm.coordToPixelCorner(new Point(i, 34));
                gr.DrawImage(pacmanSprites[4], livesLocation.X, livesLocation.Y);
            }


            //coordinates debug

            //using (Font myFont = new Font("Arial", 14))
            //{
            //    gr.DrawString(location.X.ToString() + "x" + location.Y.ToString(), myFont, Brushes.Red, new PointF(location.X, location.Y));
            //}

            //coords as squares debug

            //using (Font myFont = new Font("Arial", 14))
            //{
            //    Point coord = PacForm.pixelToCoord(location);
            //    gr.DrawString(coord.X.ToString() + "x" + coord.Y.ToString(), myFont, Brushes.Red, new PointF(location.X, location.Y));
            //}

            //chase-scatter debug

            //using (Font myFont = new Font("Arial", 14))
            //{
            //    if (Ghost.scatter) gr.DrawString("SCATTER", myFont, Brushes.Red, new PointF(location.X, location.Y));
            //    else gr.DrawString("CHASE", myFont, Brushes.Red, new PointF(location.X, location.Y));
            //}

            //using (Brush brush = new SolidBrush(Color.Green))
            //{
            //    int size = PacForm.BoardTileSize * PacForm.SpriteScale;
            //    gr.FillRectangle(brush, location.X, location.Y, size, size);
            //}

            //using (Font myFont = new Font("Arial", 14))
            //{
            //    gr.DrawString(String.Format("\n{0:F4}",speed), myFont, Brushes.Red, new PointF(location.X, location.Y));
            //    gr.DrawString(location.X.ToString() + "x" + location.Y.ToString(), myFont, Brushes.Red, new PointF(location.X, location.Y));
            //}



            //using (Pen pen = new Pen(Color.Red, 2))
            //{
            //    gr.DrawRectangle(pen, 0, 0, size, size);
            //}
        }
    }
}
