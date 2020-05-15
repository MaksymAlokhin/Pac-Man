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
    //Keeps record of items on the game board
    //Зберігає інформацію про предмети, з якими гравець може взаємодіяти
    public class Items
    {
        //Array of item locations. Список розташування предметів
        public int[,] itemsMap;
        //Timer for some items to blink. Таймер блимання деяких предметів на полі
        public Timer blinkTimer;
        public Timer fruitTimer;
        public Timer hideScoreTimer;
        bool powerPelletVisible;
        //Container for item sprites. Контейнер для зображень предметів
        public List<Bitmap> Sprites;
        //Keeps the count of all dot items on the board
        //Містить кількість предметів на ігровому полі
        public int dots;
        PacForm form;
        //Power pill timer. Таймер, коли персонаж може з'їсти привида
        private DateTime EnergizedStateStartedTime;
        public float elapsed;
        Random random;
        //fruit can be shown. Можна малювати фрукт
        bool fruitTime;
        //fruits can spawn only twice. Фрукти можуть з'явитися лише двічі
        bool firstItemAppeared;
        bool secondItemAppeared;
        bool showScore;
        //Coordinates for showing bonus fruit and score for consuming it. Координати показу рахунку за з'їдені фрукти
        PointF bonusFruitLocation;
        PointF score1coord;
        PointF score2coord;
        PointF score3coord;
        PointF score4coord;
        double chaseScatterInterruptedAt;
        //Ghosts move faster when few dots left
        public bool elroy1;
        public bool elroy2;

        //Determines when ghosts leave the house. Визначає, коли привиди покидають дім
        public bool alternativeExitCounterAfterPacManDies;
        public int globalDotCounter;
        public int[] ghostDotCounter;
        public Stopwatch dotStopwatch;

        MediaPlayer wmp_eat_fruit;
        MediaPlayer wmp_power_pellet;
        MediaPlayer wmp_munch_3;

        //Don't play until previous has finished. Не грати новий звук, поки грає попередній
        bool playedEatingSound;
        public Items(PacForm frm)
        {
            //0 wall
            //1 free
            //2 dot
            //3 superdot
            //4 bonus fruit
            //5 slow terrain
            random = new Random();
            form = frm;
            //itemsMap = new int[form.board.size.Width, form.board.size.Height];
            itemsMap = new int[,] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0 },
                { 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0 },
                { 0, 3, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 3, 0 },
                { 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0 },
                { 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0 },
                { 0, 2, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 2, 0 },
                { 0, 2, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 2, 0 },
                { 0, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 5, 5, 5, 5, 5, 5, 2, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 2, 5, 5, 5, 5, 5, 5 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
                { 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0 },
                { 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0 },
                { 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0 },
                { 0, 3, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2, 3, 0 },
                { 0, 0, 0, 2, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 2, 0, 0, 0 },
                { 0, 0, 0, 2, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 2, 0, 0, 0 },
                { 0, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0 },
                { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
                { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
                { 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },};
            powerPelletVisible = true;
            blinkTimer = new Timer();
            blinkTimer.Interval = 150;
            blinkTimer.Enabled = true;
            blinkTimer.Tick += new EventHandler(tmr_Blink);
            fruitTimer = new Timer();
            //NextDouble() * (max - min) + min
            fruitTimer.Interval = random.Next(9000, 10001);
            fruitTimer.Tick += new EventHandler(HideFruit);
            hideScoreTimer = new Timer();
            hideScoreTimer.Tick += new EventHandler(HideScore);
            hideScoreTimer.Interval = 3000;
            Sprites = new List<Bitmap>();
            bonusFruitLocation = PacForm.coordToPixelCorner(new Point(13, 19));
            bonusFruitLocation.Y += PacForm.unitCenter;
            score1coord = PacForm.coordToPixelCorner(new Point(12, 20));
            score2coord = PacForm.coordToPixelCorner(new Point(13, 20));
            score3coord = PacForm.coordToPixelCorner(new Point(14, 20));
            score4coord = PacForm.coordToPixelCorner(new Point(15, 20));

            ghostDotCounter = new int[4];
            dotStopwatch = new Stopwatch();

            InitializeSound();

            MakeSprites();
        }

        //Item blink animation timer. Таймер блимання предметів
        private void tmr_Blink(object sender, EventArgs e)
        {
            if (powerPelletVisible) powerPelletVisible = false;
            else powerPelletVisible = true;
        }
        //Pac-Man eats food. Пакмен поглинає "їжу"
        public void Eat(Point coord)
        {
            DateTime now = DateTime.Now;
            elapsed = (float)(now - EnergizedStateStartedTime).TotalMilliseconds;
            if (elapsed > form.levels.get_frightTime() + 
                (form.levels.blinkTimerInterval * form.levels.get_nFlashes() * 2) && form.pacman.energized)
            {
                form.pacman.energized = false;
                //at the end of frightened period, resume the usual chase-scatter timers
                if (Ghost.scatter)
                {
                    Ghost.scatterTimer.Interval = form.levels.get_chaseScatter(Ghost.scatterCounter * 2) - (int)chaseScatterInterruptedAt;
                    Ghost.scatterTimer.Start();
                    Ghost.scatterStopwatch.Start();
                }
                else if(Ghost.chaseCounter < 3)
                {
                    Ghost.chaseTimer.Interval = form.levels.get_chaseScatter(Ghost.chaseCounter * 2 + 1) - (int)chaseScatterInterruptedAt;
                    Ghost.chaseTimer.Start();
                    Ghost.chaseStopwatch.Start();
                }
            }

            if (itemsMap[coord.Y, coord.X] > 1 && itemsMap[coord.Y, coord.X] < 5)
            {
                //regular dot
                if (itemsMap[coord.Y, coord.X] == 2)
                {
                    if(playedEatingSound)
                    {
                        wmp_munch_3.Stop();
                        wmp_munch_3.Open(new Uri("Sounds/munch_3.wav", UriKind.Relative));
                        //wmp_munch_3.Position = new TimeSpan(0);
                        wmp_munch_3.Play();
                        playedEatingSound = false;
                    }
                    //eatingSoundTimer.Start();

                    dotStopwatch.Start();
                    form.player.adjustScore(10);
                    itemsMap[coord.Y, coord.X] = 1;
                    form.pacman.sleepAfterLunch++;

                    //Dot counters for ghosts exit

                    //Scenario after life has been lost
                    if (alternativeExitCounterAfterPacManDies)
                    {
                        globalDotCounter++;
                    }
                    //Scenario at the start of the game and after ghosts are eaten
                    else
                    {
                        for (int i = 0; i < Ghost.canExit.Length; i++)
                        {
                            if (!Ghost.canExit[i])
                            {
                                ghostDotCounter[i]++;
                                break;
                            }
                        }
                    }
                }

                //super dot
                if (itemsMap[coord.Y, coord.X] == 3)
                {
                    wmp_power_pellet.Stop();
                    wmp_power_pellet.Open(new Uri("Sounds/power_pellet.wav", UriKind.Relative));
                    //wmp_power_pellet.Position = new TimeSpan(0);
                    wmp_power_pellet.Play();
                    EnergizedStateStartedTime = DateTime.Now;
                    form.player.adjustScore(50);
                    if (!form.pacman.energized)
                    {
                        form.blinky.reverseDirection = true;
                        form.inky.reverseDirection = true;
                        form.pinky.reverseDirection = true;
                        form.clyde.reverseDirection = true;

                        //pause chase-scatter timer for the time being frightened
                        if (Ghost.scatter)
                        {
                            Ghost.scatterTimer.Stop();
                            Ghost.scatterStopwatch.Stop();
                            chaseScatterInterruptedAt = Ghost.scatterStopwatch.Elapsed.TotalMilliseconds;
                        }
                        else if (Ghost.chaseCounter < 3)
                        {
                            Ghost.chaseTimer.Stop();
                            Ghost.chaseStopwatch.Stop();
                            chaseScatterInterruptedAt = Ghost.chaseStopwatch.Elapsed.TotalMilliseconds;
                        }
                    }
                    itemsMap[coord.Y, coord.X] = 1;
                    form.pacman.sleepAfterLunch += 3;
                    form.pacman.energized = true;
                }
            }
            if(fruitTime && itemsMap[coord.Y, coord.X] == 4)
            {
                wmp_eat_fruit.Stop();
                wmp_eat_fruit.Open(new Uri("Sounds/eat_fruit.wav", UriKind.Relative));
                //wmp_eat_fruit.Position = new TimeSpan(0);
                wmp_eat_fruit.Play();
                showScore = true;
                hideScoreTimer.Start();
                fruitTimer.Stop();
                fruitTime = false;
                form.player.adjustScore(form.levels.get_bonusPoints());
            }
            if (dots == 0)//0-240
            {
                form.StartLevelTransition();
            }
            if (dots == 70 && !firstItemAppeared)
            {
                fruitTime = true;
                firstItemAppeared = true;
                fruitTimer.Start();
            }
            if (dots == 170 && !secondItemAppeared)
            {
                fruitTime = true;
                secondItemAppeared = true;
                fruitTimer.Start();
            }
            if (dots <= form.levels.get_elroy1DotsLeft()) elroy1 = true;
            if (dots <= form.levels.get_elroy2DotsLeft()) elroy2 = true;
        }
        private void EatingSoundRewind(object sender, EventArgs e)
        {
            playedEatingSound = true;
        }
        public int get_ghostDotCounter(Object obj)
        {
            if (obj.GetType() == typeof(Blinky)) return ghostDotCounter[0];
            else if (obj.GetType() == typeof(Pinky)) return ghostDotCounter[1];
            else if (obj.GetType() == typeof(Inky)) return ghostDotCounter[2];
            else return ghostDotCounter[3];
        }
        public int get_globalDotCounter(Object obj)
        {
            return globalDotCounter;
        }
        //Fruit Timer
        void HideFruit(object sender, EventArgs e)
        {
            fruitTimer.Stop();
            fruitTime = false;
        }
        //HideScore Timer
        void HideScore(object sender, EventArgs e)
        {
            hideScoreTimer.Stop();
            showScore = false;
        }
        void MakeSprites()
        {
            int dotSprite = 16; //0
            int powerPelletSprite = 20; //1
            int cherriesSprite = 320; //2
            int strawberrySprite = 322; //3
            int peachSprite = 324; //4
            int appleSprite = 328; //5
            int grapesSprite = 330; //6
            int galaxianSprite = 332; //7
            int bellSprite = 326; //8
            int keySprite = 334; //9

            int score_100_1_Sprite = 129; //10
            int score_300_1_Sprite = 130; //11
            int score_500_1_Sprite = 131; //12
            int score_700_1_Sprite = 132; //13
            int score_00_Sprite = 133; //14
            int score_1000_1_Sprite = 134; //15
            int score_2000_1_1Sprite = 135; //16
            int score_2000_2_Sprite = 136; //17
            int score_3000_1_Sprite = 137; //18
            int score_3000_2_Sprite = 138; //19
            int score_5000_1_Sprite = 139; //20
            int score_5000_2_Sprite = 140; //21
            int score_5000_3_Sprite = 141; //22
            int score_5000_4_Sprite = 142; //23

            Sprites.Add(PacForm.CutSprite(dotSprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(powerPelletSprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(cherriesSprite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(strawberrySprite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(peachSprite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(appleSprite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(grapesSprite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(galaxianSprite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(bellSprite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(keySprite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(score_100_1_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_300_1_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_500_1_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_700_1_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_00_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_1000_1_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_2000_1_1Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_2000_2_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_3000_1_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_3000_2_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_5000_1_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_5000_2_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_5000_3_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(score_5000_4_Sprite, PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale));
        }
        void InitializeSound()
        {
            //Sound. Звук
            wmp_eat_fruit = new MediaPlayer();
            //wmp_eat_fruit.Open(new Uri("Sounds/eat_fruit.wav", UriKind.Relative));
            wmp_eat_fruit.Volume = PacForm.volume;

            wmp_power_pellet = new MediaPlayer();
            //wmp_power_pellet.Open(new Uri("Sounds/power_pellet.wav", UriKind.Relative));
            wmp_power_pellet.Volume = PacForm.volume;

            wmp_munch_3 = new MediaPlayer();
            wmp_munch_3.MediaEnded += EatingSoundRewind;
            //wmp_munch_3.Open(new Uri("Sounds/munch_3.wav", UriKind.Relative));
            wmp_munch_3.Volume = PacForm.volume;

            playedEatingSound = true;

        }
        //Display items on the board. Малювання предметів на ігровому полі
        public void Draw(Graphics gr)
        {
            dots = 0;
            using (Bitmap result = new Bitmap(form.board.boardPBSize.Width, form.board.boardPBSize.Height))
            {
                using (Graphics tempGr = Graphics.FromImage(result))
                {
                    for (int i = 0; i < form.board.size.Height; i++)
                    {
                        for (int j = 0; j < form.board.size.Width; j++)
                        {
                            if (itemsMap[i, j] == 2)
                            {
                                tempGr.DrawImage(Sprites[0], PacForm.coordToPixelCorner(new Point(j, i)));
                                dots++;
                            }
                            if (powerPelletVisible)
                            {
                                if (itemsMap[i, j] == 3)
                                {
                                    tempGr.DrawImage(Sprites[1], PacForm.coordToPixelCorner(new Point(j, i)));
                                }
                            }
                        }
                    }
                    //Draw bonus fruit. Малювання фрукту, який можна з'їсти
                    if (fruitTime)
                    {
                        tempGr.DrawImage(Sprites[form.levels.get_bonusSymbol()], bonusFruitLocation);
                    }
                    
                    
                    //Draw level fruits. Малювання фруктів на позначення рівня
                    int startingLevelToDraw = 0;
                    if (form.levels.get_level() > 5) startingLevelToDraw = form.levels.get_level() - 5;
                    int numberOfFruitsToDraw = 0;
                    if (form.levels.get_level() < 5) numberOfFruitsToDraw = form.levels.get_level() + 1;
                    else numberOfFruitsToDraw = 6;
                    for (int pos = 24, lvl = startingLevelToDraw; pos >= 26 - numberOfFruitsToDraw * 2; pos -= 2, lvl++)
                    {
                        PointF fruitLocation = PacForm.coordToPixelCorner(new Point(pos, 34));
                        tempGr.DrawImage(form.items.Sprites[form.levels.get_bonusSymbol(lvl)], fruitLocation.X, fruitLocation.Y);
                    }

                    //Draw score after eating fruit. Малювання балів за з'їдені фрукти
                    if(showScore)
                    {
                        switch(form.levels.get_level())
                        {
                            case 0:
                                tempGr.DrawImage(Sprites[10], score2coord);
                                tempGr.DrawImage(Sprites[14], score3coord);
                                break;
                            case 1:
                                tempGr.DrawImage(Sprites[11], score2coord);
                                tempGr.DrawImage(Sprites[14], score3coord);
                                break;
                            case 2:
                            case 3:
                                tempGr.DrawImage(Sprites[12], score2coord);
                                tempGr.DrawImage(Sprites[14], score3coord);
                                break;
                            case 4:
                            case 5:
                                tempGr.DrawImage(Sprites[13], score2coord);
                                tempGr.DrawImage(Sprites[14], score3coord);
                                break;
                            case 6:
                            case 7:
                                tempGr.DrawImage(Sprites[15], score2coord);
                                tempGr.DrawImage(Sprites[14], score3coord);
                                break;
                            case 8:
                            case 9:
                                tempGr.DrawImage(Sprites[16], score1coord);
                                tempGr.DrawImage(Sprites[17], score2coord);
                                tempGr.DrawImage(Sprites[22], score3coord);
                                tempGr.DrawImage(Sprites[23], score4coord);
                                break;
                            case 10:
                            case 11:
                                tempGr.DrawImage(Sprites[18], score1coord);
                                tempGr.DrawImage(Sprites[19], score2coord);
                                tempGr.DrawImage(Sprites[22], score3coord);
                                tempGr.DrawImage(Sprites[23], score4coord);
                                break;
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                            case 16:
                            case 17:
                            case 18:
                            case 19:
                            case 20:
                                tempGr.DrawImage(Sprites[20], score1coord);
                                tempGr.DrawImage(Sprites[21], score2coord);
                                tempGr.DrawImage(Sprites[22], score3coord);
                                tempGr.DrawImage(Sprites[23], score4coord);
                                break;
                        }
                    }
                    //Final drawing of everything. Перенесення намальованого на екран
                    gr.DrawImage(result, PointF.Empty);
                }
            }
        }
    }
}