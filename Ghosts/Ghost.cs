using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Media;

namespace PacMan
{
    //Enemy ghosts. Супротивники-привиди
    public abstract class Ghost
    {
        //Ghost id. Ідентифікатор привида
        public int ghostNumber;
        //Coordinate; координата
        public PointF location;
        public Point previousCoordinate;
        public Point currentCoordinate;
        //Starting position. Початкова координата
        public PointF home;
        //Direction; напрямок руху
        public int direction;
        public int nextDirection;
        //Speed; швидкість руху
        public float speed;
        //Array of coordinates around: up, left, down, right
        //масив координат навколо: вверху, зліва, знизу, справа
        Point[] directions = new Point[4];
        public PacForm form;
        public System.Drawing.Color color;
        //Current animation phase; поточна фаза анімації
        public Bitmap currentSprite;
        //Transparent sprite; прозорий ігровий персонаж
        public Bitmap emptySprite;
        //Collection of animated sprites; набір зображень, що утворюють анімацію.
        public List<Bitmap> Sprites = new List<Bitmap>();
        //Control animation phase; керування фазою анімації
        public int spriteLoop;
        //Sprites facing a direction; зображення ігрових персонажів
        public int SpriteRight;
        public int SpriteRight2;
        public int SpriteDown;
        public int SpriteDown2;
        public int SpriteLeft;
        public int SpriteLeft2;
        public int SpriteUp;
        public int SpriteUp2;

        Random random = new Random();
        public Timer blinkTimer;
        public Timer showScoreTimer;
        public static Timer scatterTimer;
        public static Timer chaseTimer;
        public static Stopwatch scatterStopwatch;
        public static Stopwatch chaseStopwatch;
        //animation
        int frightenedSpriteNumber;
        //Ghost is travelling home after it's been eaten.
        //Привид подорожує додому пістя того, як його з'їли
        public bool travellingHome;

        //Has the ghost exited the ghost house? Чи залишив привид свій "будинок"
        public bool exited;
        //Who can exit. Хто з привидів може вийти
        public static bool[] canExit;
        //Counter that determines when ghosts exit. Лічильник, що визначає, коли привид вийде з дому
        public int[] ghostItemCounter;

        //movement type. Привид слідує маршруту
        public bool followingWaypoints;
        //ghost should reverse direction. Привид має змінити напрямок руху
        public bool reverseDirection;

        //Bonus points for eating all ghosts. Бонусні бали за винищення всіх привидів
        static bool eatenAll;
        bool showScore;
        public static int travellingGhosts;
        //when travelling home, the score could change if another ghost is eaten
        int travellingGhostsPrivate;
        //Game phases scatter-chase. Фази гри врозсип-погоня
        public static bool scatter;
        public static int scatterCounter;
        public static int chaseCounter;

        //Ghosts come here after eaten. Точка збору з'їдених привидів
        public Point AboveHome;
        //Ghosts follow waypoints to get into house. Привиди слідують маршруту додому
        public PointF[] waypoints;
        public int waypointsCounter;
        //Target that is beyond the walls. During scatter ghosts target it. Ціль для привидів у режимі врозсип
        public Point scatterTarget;
        //Waypoints inside ghost house to follow when not exited. Привиди слідують маршруту, коли не вийшли з будинку
        public PointF stayAtHomeWayPointUp;
        public PointF stayAtHomeWayPointDown;
        MediaPlayer wmp_eat_ghost;
        MediaPlayer wmp_extend;
        public Ghost(PacForm frm)
        {
            int SpriteEmpty = 478;
            travellingGhosts = 0;
            emptySprite = PacForm.CutSprite(SpriteEmpty, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale);
            form = frm;
            directions[0] = new Point(0, -1);
            directions[1] = new Point(-1, 0);
            directions[2] = new Point(0, 1);
            directions[3] = new Point(1, 0);
            speed = 1F;
            blinkTimer = new Timer();
            blinkTimer.Interval = form.levels.blinkTimerInterval;
            blinkTimer.Tick += new EventHandler(BlinkingAnimation);
            showScoreTimer = new Timer();
            showScoreTimer.Interval = 1000;
            showScoreTimer.Tick += new EventHandler(showEatenScore);

            scatterTimer = new Timer();
            scatterTimer.Tick += new EventHandler(ScatterOffEvent);
            chaseTimer = new Timer();
            chaseTimer.Tick += new EventHandler(ChaseOffEvent);
            scatterStopwatch = new Stopwatch();
            chaseStopwatch = new Stopwatch();

            //Used in blinking animation. Використовується у анімації блимання
            frightenedSpriteNumber = 10;
            AboveHome = new Point(14, 14);
            waypointsCounter = 0;
            spriteLoop = 0;

            //Exit conditions. Умови иходу привидів з домівки
            canExit = new bool[4];
            ghostItemCounter = new int[4];

            InitializeSound();
        }
        //Move sprite
        //Рух ігрового персонажу
        public void Move(float elapsed)
        {
            //chase-scatter debug
            //if (scatter) form.highScoreLabel.Text = "SCATTER: " + scatterStopwatch.Elapsed.TotalSeconds.ToString() + " (" + scatterCounter + ")";
            //else form.highScoreLabel.Text = "CHASE: " + chaseStopwatch.Elapsed.TotalSeconds.ToString() + " (" + chaseCounter + ")";

            if (insideTunnel()) speed = form.levels.speed * form.levels.get_ghostTunnelSpeed() * elapsed;
            else if (form.pacman.energized) speed = form.levels.speed * form.levels.get_frightGhostSpeed() * elapsed;
            else if (form.items.elroy2) speed = form.levels.speed * form.levels.get_elroy2Speed() * elapsed;
            else if (form.items.elroy1) speed = form.levels.speed * form.levels.get_elroy1Speed() * elapsed;
            else speed = form.levels.speed * form.levels.get_ghostSpeed() * elapsed;
            //speed = 3f;

            //check if a ghost can exit

            //if no dots eaten for some time, let the ghost out
            if (form.items.dotStopwatch.Elapsed.TotalMilliseconds > form.levels.get_exitTimerThreshold())
            {
                for (int i = 0; i < canExit.Length; i++)
                {
                    if (!canExit[i])
                    {
                        canExit[i] = true;
                        form.items.dotStopwatch.Restart();
                        break;
                    }
                }
            }
            //after a life has been lost
            if (form.items.alternativeExitCounterAfterPacManDies)
            {
                if (form.items.globalDotCounter >= 7) canExit[1] = true;
                else if (form.items.globalDotCounter >= 17) canExit[2] = true;
                else if (form.items.globalDotCounter >= 32)
                {
                    canExit[3] = true;
                    form.items.alternativeExitCounterAfterPacManDies = false;
                    form.items.globalDotCounter = 0;
                }
            }
            //at the start and after ghost has been eaten
            else
            {
                if (form.items.ghostDotCounter[ghostNumber] >= form.levels.get_ghostDotThreshold(ghostNumber))
                    canExit[ghostNumber] = true;
            }

            if(!canExit[ghostNumber])
            {
                StayAtHome();
            }
            else if (!exited && canExit[ghostNumber])
            {
                ExitHouse();
            }
            else if (followingWaypoints)
            {
                FollowWaypoints();
            }
            else if (travellingHome)
            {
                ChooseDirection();
                NormalMovement();
            }
            else if (form.pacman.energized)
            {
                ChooseRandomDirection();
                NormalMovement();
            }
            else
            {
                ChooseDirection();
                NormalMovement();
            }
        }
        //Choose where to go. Вибір напрямку руху
        void ChooseDirection()
        {
            currentCoordinate = PacForm.pixelToCoord(location);
            if (currentCoordinate != previousCoordinate)
            {
                previousCoordinate = currentCoordinate;
                bool[] whereToGo = LookAround();
                int nDirections = CountDirections(whereToGo);
                if (nDirections > 1)
                {
                    nextDirection = FindShortestPathToTarget(whereToGo, GetTarget());
                }
                else nextDirection = Math.Abs(nDirections);

                if (currentCoordinate == AboveHome && travellingHome)
                {
                    followingWaypoints = true;
                }
            }
        }
        void ChooseRandomDirection()
        {
            currentCoordinate = PacForm.pixelToCoord(location);
            if (currentCoordinate != previousCoordinate)
            {
                previousCoordinate = currentCoordinate;
                bool[] whereToGo = LookAround();
                int nDirections = CountDirections(whereToGo);
                if (nDirections > 1)
                {
                    nextDirection = FindRandomDirection(whereToGo);
                }
                else nextDirection = Math.Abs(nDirections);
            }
        }
        //Usual movement. Звичайний рух
        void NormalMovement()
        {
            switch (direction)
            {
                case 0:
                    PacManCollision();
                    location = new PointF(location.X, location.Y -= speed);
                    if (location.Y % PacForm.boardUnitSize < PacForm.unitCenter)
                    {
                        if (reverseDirection)
                        {
                            reverseDirection = false;
                            ReverseDirection();
                        }
                        else
                        {
                            direction = nextDirection;
                            location.X = PacForm.coordToPixelCenter(PacForm.pixelToCoord(location)).X;
                        }
                    }
                    PacManCollision();
                    break;
                case 1:
                    PacManCollision();
                    location = new PointF(location.X -= speed, location.Y);
                    if (location.X % PacForm.boardUnitSize < PacForm.unitCenter)
                    {
                        if (reverseDirection)
                        {
                            reverseDirection = false;
                            ReverseDirection();
                        }
                        else
                        {
                            direction = nextDirection;
                            location.Y = PacForm.coordToPixelCenter(PacForm.pixelToCoord(location)).Y;
                        }
                    }
                    PacManCollision();
                    break;
                case 2:
                    PacManCollision();
                    location = new PointF(location.X, location.Y += speed);
                    if (location.Y % PacForm.boardUnitSize > PacForm.unitCenter)
                    {
                        if (reverseDirection)
                        {
                            reverseDirection = false;
                            ReverseDirection();
                        }
                        else
                        {
                            direction = nextDirection;
                            location.X = PacForm.coordToPixelCenter(PacForm.pixelToCoord(location)).X;
                        }
                    }
                    PacManCollision();
                    break;
                case 3:
                    PacManCollision();
                    location = new PointF(location.X += speed, location.Y);
                    if (location.X % PacForm.boardUnitSize > PacForm.unitCenter)
                    {
                        if (reverseDirection)
                        {
                            reverseDirection = false;
                            ReverseDirection();
                        }
                        else
                        {
                            direction = nextDirection;
                            location.Y = PacForm.coordToPixelCenter(PacForm.pixelToCoord(location)).Y;
                        }
                    }
                    PacManCollision();
                    break;
            }

            if (location.X < 0)
                location = new PointF(location.X + form.board.boardPBSize.Width, location.Y);
            if (location.X >= form.board.boardPBSize.Width)
                location = new PointF(location.X - form.board.boardPBSize.Width, location.Y);
        }
        public static void ResetChaseScatter()
        {
            scatterCounter = 0;
            scatter = true;
            scatterTimer.Stop();
            scatterStopwatch.Reset();

            chaseCounter = 0;
            chaseTimer.Stop();
            chaseStopwatch.Reset();
        }
        //Get target coordinates
        //Отримання координатів цілі
        public abstract Point GetTarget();
        //Ghosts roam in their house. Привиди залишаються вдома
        public abstract void StayAtHome();
        //Put sprite in starting position
        //Переміщення спрайта у стартову позицію
        public abstract void ResetPosition();
        //Ghosts return home after eaten by Pac-Man
        //Привиди повертаються додому після того, як їх з'їли
        public abstract void FollowWaypoints();
        //Ghosts leave the ghost house at the start of round
        //Привиди залишають свою домівку на початку раунду
        public void ExitHouse()
        {
            location.X = (float)Math.Round(location.X);
            location.Y = (float)Math.Round(location.Y);
            PointF exitPoint = PacForm.coordToPixelCenter(new Point(14, 14));
            exitPoint.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 4;
            if (location.X > exitPoint.X) { location.X -= 1; direction = 1; }
            else if (location.X < exitPoint.X) { location.X += 1; direction = 3; }
            else if (location.X == exitPoint.X && location.Y > exitPoint.Y) { location.Y -= 1; direction = 0; }
            else if (location.X == exitPoint.X && location.Y < exitPoint.Y) { location.Y += 1; direction = 2; }
            if (location == exitPoint)
            {
                exited = true;
                int dir = random.Next(0, 2);
                if (dir > 0) direction = 1;
                else direction = 3;
            }
        }
        int FindRandomDirection(bool[] array)
        {
            List<int> directions = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == true) directions.Add(i);
            }
            int dir = random.Next(0, directions.Count);
            return directions[dir];
        }
        //When frightened, ghosts reverse direction
        //Злякані привиди змінюють напрямок на протилежний
        public void ReverseDirection()
        {
            int oppositeDirection = 0;

            bool[] whereCanIGo = new bool[4];
            for (int dir = 0; dir < 4; dir++)
            {
                if (CanMove(dir)) whereCanIGo[dir] = true;
            }
            whereCanIGo[direction] = false;

            switch (direction)
            {
                case 0:
                    oppositeDirection = 2;
                    break;
                case 1:
                    oppositeDirection = 3;
                    break;
                case 2:
                    oppositeDirection = 0;
                    break;
                case 3:
                    oppositeDirection = 1;
                    break;
            }

            if (whereCanIGo[oppositeDirection] == false)
            {
                for (int dir = 0; dir < whereCanIGo.Length; dir++)
                {
                    if (whereCanIGo[dir] == true && !travellingHome)
                    {
                        nextDirection = dir;
                        break;
                    }
                }
            }
            else if (!travellingHome) nextDirection = oppositeDirection;
        }

        //Find the path to the target that is the shortest 
        //Знаходження найкоротшого шляху до цілі
        private int FindShortestPathToTarget(bool[] array, Point target)
        {
            int[] pathLengths = new int[4];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == true)
                {
                    Point p = PacForm.pixelToCoord(location);
                    p.X += directions[i].X;
                    p.Y += directions[i].Y;
                    int distance = (p.X - target.X) * (p.X - target.X) + (p.Y - target.Y) * (p.Y - target.Y);
                    pathLengths[i] = distance;
                }
            }
            int min = int.MaxValue;
            int minIndex = 0;
            for (int i = 0; i < pathLengths.Length; i++)
            {
                if (array[i] == true && pathLengths[i] < min)
                {
                    min = pathLengths[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }
        //Find distance between two coordinates
        //Знаходження відстані між двома координатами
        public double FindDistance(Point loc, Point dest)
        {
            return Math.Sqrt(Math.Pow(dest.X - loc.X, 2) + Math.Pow(dest.X - loc.X, 2));
        }
        //Find directions where you can move
        //Знаходження можливих напрямків руху
        private bool[] LookAround()
        {
            bool[] result = new bool[4];
            for (int dir = 0; dir < 4; dir++)
            {
                if (CanMove(dir)) result[dir] = true;
            }
            switch (direction)
            {
                case 0: result[2] = false; break;
                case 1: result[3] = false; break;
                case 2: result[0] = false; break;
                case 3: result[1] = false; break;
            }
            return result;
        }
        //Count the number of directions you can move
        //Підрахунок кількості можливих напрямків руху
        private int CountDirections(bool[] array)
        {
            int count = 0;
            int lastSuccess = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == true)
                {
                    count++;
                    lastSuccess = i;
                }

            }
            if (count == 1) return lastSuccess * -1;
            else return count;
        }
        //Test if you can move in a direction
        //Перевірка можливості руху у заданому напрямку
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
        //Check for collisions with Pac-Man
        //Перевірка зіткнень привида і Пакмена
        private void PacManCollision()
        {
            //if (form.pacman.energized && PacForm.pixelToCoord(form.pacman.location) == PacForm.pixelToCoord(location))
            //if (PacForm.pixelToCoord(location).X < 14) AboveHome = LeftAboveHome;
            //else AboveHome = RightAboveHome;
            if ((form.pacman.energized || form.energyBtn.Checked) && !travellingHome && !followingWaypoints
                && PacForm.pixelToCoord(form.pacman.location) == PacForm.pixelToCoord(location))
            {
                wmp_eat_ghost.Stop();
                wmp_eat_ghost.Open(new Uri("Sound/eat_ghost.wav", UriKind.Relative));
                //wmp_eat_ghost.Position = new TimeSpan(0);
                wmp_eat_ghost.Play();
                travellingHome = true;
                if (travellingGhosts == 0) eatenAll = false;
                travellingGhostsPrivate = travellingGhosts;
                travellingGhosts++;
                form.player.adjustScore((int)Math.Pow(2, travellingGhosts) * 100);
                if (travellingGhosts == 4 && !eatenAll)
                {
                    wmp_extend.Stop();
                    wmp_extend.Open(new Uri("Sound/extend.wav", UriKind.Relative));
                    //wmp_extend.Position = new TimeSpan(0);
                    wmp_extend.Play();
                    form.player.adjustScore(12000);
                    eatenAll = true;
                }
                showScore = true;

                //form.LastTime = DateTime.Now;

                showScoreTimer.Start();
            }
            if (!form.pacman.energized && !travellingHome && !followingWaypoints 
                && PacForm.pixelToCoord(form.pacman.location) == PacForm.pixelToCoord(location))
            {
                if (!form.invincBtn.Checked) { currentSprite = emptySprite; form.pacman.Die(); }
            }
        }
        //Is ghost slowed inside a tunnel? Чи уповільнений привид у тунелі?
        bool insideTunnel()
        {
            if (form.items.itemsMap[PacForm.pixelToCoord(location).Y, PacForm.pixelToCoord(location).X] == 5) return true;
            else return false;
        }
        //Show score when eaten. Показ очків за з'їдання привида
        void showEatenScore(object sender, EventArgs e)
        {
            showScoreTimer.Stop();
            showScore = false;
        }
        //While moving, sprites change animation
        //Під час руху, ігрові персонажі змінюють анімацію
        public void ChangeSprite()
        {
            //blink after frightened. Блимання привида
            if (form.items.elapsed > form.levels.get_frightTime()) blinkTimer.Enabled = true;
            else
            {
                blinkTimer.Enabled = false;
                frightenedSpriteNumber = 10;
            }
            //finished blinking
            if (form.items.elapsed > form.levels.get_frightTime() + (form.levels.blinkTimerInterval * form.levels.get_nFlashes() * 2))
            {
                blinkTimer.Enabled = false;
                frightenedSpriteNumber = 10;
            }
            if (!form.pacman.dead)
            {
                //frightened
                if (form.pacman.energized && !travellingHome)
                {
                    currentSprite = Sprites[frightenedSpriteNumber + spriteLoop];
                }
                //eyes
                else if(showScore) currentSprite = Sprites[20 + travellingGhostsPrivate];
                else if (travellingHome)
                {
                    currentSprite = Sprites[12 + direction * 2 + spriteLoop];
                }
                else currentSprite = Sprites[direction * 2 + spriteLoop];

                spriteLoop++;
                if (spriteLoop > 1) spriteLoop = 0;
            }
        }
        //Animation of fright ending. Кінець анімації страху
        void BlinkingAnimation(object sender, EventArgs e)
        {
            if (frightenedSpriteNumber == 10) frightenedSpriteNumber = 8;
            else frightenedSpriteNumber = 10;
        }
        //Scatter-chase periods. Чередування режимів врозсип-переслідування
        void ChaseOffEvent(object sender, EventArgs e)
        {
            form.blinky.reverseDirection = true;
            form.inky.reverseDirection = true;
            form.pinky.reverseDirection = true;
            form.clyde.reverseDirection = true;
            scatter = true;
            chaseTimer.Stop();
            chaseCounter++;
            scatterTimer.Interval = form.levels.get_chaseScatter(scatterCounter * 2);
            scatterTimer.Start();
            scatterStopwatch.Restart();
        }
        void ScatterOffEvent(object sender, EventArgs e)
        {
            form.blinky.reverseDirection = true;
            form.inky.reverseDirection = true;
            form.pinky.reverseDirection = true;
            form.clyde.reverseDirection = true;
            scatter = false; 
            scatterTimer.Stop();
            scatterCounter++;
            if (scatterCounter < 4)
            {
                chaseTimer.Interval = form.levels.get_chaseScatter(chaseCounter * 2 + 1);
                chaseTimer.Start();
                chaseStopwatch.Restart();
            }
            else
            {
                chaseStopwatch.Restart();
            }
        }

        //Cut out sprites from spritesheet and put them in a list
        //Виокремлення спрайтів із загального полотна і поміщення їх у контейнер list
        public void MakeSprites()
        {
            int SpriteWhite = 268; //8
            int SpriteWhite2 = 270; //9
            int SpriteFrightened = 272; //10
            int SpriteFrightened2 = 274; //11

            int SpriteEyesRight = 592; //12
            int SpriteEyesRight2 = 594; //13
            int SpriteEyesDown = 596; //14
            int SpriteEyesDown2 = 598; //15
            int SpriteEyesLeft = 600; //16
            int SpriteEyesLeft2 = 602; //17
            int SpriteEyesUp = 604; //18
            int SpriteEyesUp2 = 606; //19

            int SpriteScore200 = 400; //20
            int SpriteScore400 = 402; //21
            int SpriteScore800 = 404; //22
            int SpriteScore1600 = 406; //23

            Sprites.Add(PacForm.CutSprite(SpriteUp, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteUp2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteLeft, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteLeft2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteDown, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteDown2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteRight, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteRight2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteWhite, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteWhite2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteFrightened, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteFrightened2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteEyesUp, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteEyesUp2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteEyesLeft, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteEyesLeft2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteEyesDown, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteEyesDown2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteEyesRight, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteEyesRight2, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

            Sprites.Add(PacForm.CutSprite(SpriteScore200, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteScore400, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteScore800, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));
            Sprites.Add(PacForm.CutSprite(SpriteScore1600, PacForm.SpriteSize, PacForm.SpriteSize * PacForm.SpriteScale));

        }
        void InitializeSound()
        {
            //sound. Звук
            wmp_eat_ghost = new MediaPlayer();
            //wmp_eat_ghost.Open(new Uri("Sound/eat_ghost.wav", UriKind.Relative));
            wmp_eat_ghost.Volume = PacForm.volume;

            wmp_extend = new MediaPlayer();
            //wmp_extend.Open(new Uri("Sound/extend.wav", UriKind.Relative));
            wmp_extend.Volume = PacForm.volume;

        }
        //Draw sprite on screen
        //Малювання персонажу на екрані
        public void Draw(Graphics gr)
        {
            PointF drawLocation = PacForm.drawCoord(location);
            gr.DrawImage(currentSprite, drawLocation.X, drawLocation.Y);


            //debug text

            //using (Font myFont = new Font("Arial", 14))
            //{
            //    gr.DrawString(eatenAll.ToString(), myFont, System.Drawing.Brushes.Red, new PointF(location.X, location.Y));
            //}



            //debug targeting

            //using (Brush brush = new SolidBrush(color))
            //{
            //    int size = PacForm.BoardTileSize * PacForm.SpriteScale;
            //    gr.FillRectangle(brush, PacForm.coordToPixelCorner(GetTarget()).X, PacForm.coordToPixelCorner(GetTarget()).Y, size, size);
            //}
        }
    }
}
