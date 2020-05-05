using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;


namespace PacMan
{
    //Main class, program entry point. Головний клас, вхідна точка програми
    public partial class PacForm : Form
    {
        //Scale of the game graphics. Масштабування графіки
        public const int SpriteScale = 2;
        //Size of the smallest square of the game board. Найменша частинка ігрового поля
        public const int BoardTileSize = 12;
        //Sprite size. Розмір ігрового персонажу
        public const int SpriteSize = 24;
        //Size of a board square. Розмір квадрата ігрового поля
        public const int boardUnitSize = BoardTileSize * SpriteScale;
        public const int unitCenter = boardUnitSize / 2;
        public const double volume = 0.1;
        public Board board;
        public PacMan pacman;
        public Items items;
        public Player player;
        public Levels levels;
        public Blinky blinky;
        public Pinky pinky;
        public Inky inky;
        public Clyde clyde;
        public Label textLabel;
        public Label scoreLabel;
        public Label highScoreLabel;
        public Label levelLabel;
        System.Windows.Forms.Timer transitionTimer;
        HighScores highScores;
        bool paused;
        //Keys are locked after collision with a ghost.
        //Вимикання клавіатури після зіткнення з противником
        public bool keysLocked;
        //Анімація зміни рівня
        int LevelTransitionAnimation;
        public DateTime LastTime;
        MediaPlayer wmp_game_start;
        MediaPlayer wmp_intermission;
        public MediaPlayer wmp_siren_1;
        public MediaPlayer wmp_siren_2;
        public MediaPlayer wmp_siren_3;
        public MediaPlayer wmp_siren_4;
        public MediaPlayer wmp_siren_5;
        public bool sirenFinishedPlaying;
        public PacForm()
        {
            //Use double buffering to reduce flicker.
            //Буферизація для уникнення мерехтіння
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);
            this.UpdateStyles();

            InitializeComponent();
            levels = new Levels(this);
            tmr_movement.Tick += new EventHandler(MovementTick);
            highScores = new HighScores();
            textLabel = new Label();
            scoreLabel = new Label();
            highScoreLabel = new Label();
            levelLabel = new Label();
            transitionTimer = new System.Windows.Forms.Timer();
            transitionTimer.Interval = 300;
            transitionTimer.Tick += new EventHandler(LevelTransitionBlinking);
            blinky = new Blinky(this);
            pinky = new Pinky(this);
            inky = new Inky(this);
            clyde = new Clyde(this);
            InitializeTextLabels();
            KeyDown += new KeyEventHandler(KeyWatch);
            keysLocked = true;
            InitializeSound();
        }
        //Start a new game. Початок нової гри
        public void NewGame()
        {
            pacman = new PacMan(this);
            items = new Items(this);
            player = new Player();
            levels = new Levels(this);

            board = new Board(this);
            board.boardPB.Visible = false;
            board.boardPB.Paint += new PaintEventHandler(boardPB_Paint);
            board.boardPB.BringToFront();

            tmr_movement.Stop();
            tmr_animation.Stop();
            board.boardPB.Visible = false;
            scoreLabel.Visible = false;
            highScoreLabel.Visible = false;
            textLabel.Visible = false;
            levelLabel.Visible = false;

            blinky.ResetPosition();
            pinky.ResetPosition();
            inky.ResetPosition();
            clyde.ResetPosition();
            Ghost.ResetChaseScatter();
            Ghost.travellingGhosts = 0;

            ShowScoreLabel();
            ShowHighScoreLabel();
            ShowLevelLabel();
            board.boardPB.Visible = true; //.Hide() / .Show()

            ShowReadyText();
            tmr_refresh.Start();

            wmp_game_start.Stop();
            wmp_game_start.Open(new Uri("Sound/game_start.wav", UriKind.Relative));
            //wmp_game_start.Position = new TimeSpan(0);
            //wmp_game_start.Position = new TimeSpan(0, 0, 0, 3, 935);
            wmp_game_start.Play();

        }
        //Next level transition. Перехід на наступний рівень
        public void StartLevelTransition()
        {
            tmr_movement.Stop();
            tmr_animation.Stop();

            wmp_siren_5.Stop();
            sirenFinishedPlaying = true;
            wmp_intermission.Stop();
            wmp_intermission.Open(new Uri("Sound/intermission.wav", UriKind.Relative));
            //wmp_intermission.Position = new TimeSpan(0);
            wmp_intermission.Play();

            LevelTransitionAnimation = 0;
            transitionTimer.Enabled = true;    
        }
        void LevelTransitionBlinking(object sender, EventArgs e)
        {
            if (LevelTransitionAnimation % 2 == 0) board.boardPB.BackgroundImage = board.map;
            else board.boardPB.BackgroundImage = board.mapChanged;
            LevelTransitionAnimation++;
        }
        void NextLevel(object sender, EventArgs e)
        {
            wmp_intermission.Stop();
            transitionTimer.Enabled = false;
            board.boardPB.BackgroundImage = board.map;
            items = new Items(this);
            levels.levelUp();
            UpdateLevel();
            Restart();
        }
        //Restart after losing life. Перезапуск гри після втрати спроби гравцем
        public void Restart()
        {
            tmr_movement.Stop();
            pacman.ResetPosition();
            blinky.ResetPosition();
            pinky.ResetPosition();
            inky.ResetPosition();
            clyde.ResetPosition();
            Ghost.ResetChaseScatter();
            Ghost.travellingGhosts = 0;

            ShowReadyText();

            wmp_game_start.Stop();
            wmp_game_start.Open(new Uri("Sound/game_start.wav", UriKind.Relative));
            //wmp_game_start.Position = new TimeSpan(0);
            //wmp_game_start.Position = new TimeSpan(0, 0, 0, 3, 935);
            wmp_game_start.Play();
            //textTimer.Start();
        }
        //textTimer. Executed after timer at the beginning of a round
        //Процедура початку гри, виконується по таймеру на початку раунду
        public void Start(object sender, EventArgs e)
        {
            wmp_game_start.Stop();
            LastTime = DateTime.Now;
            keysLocked = false;
            textLabel.Visible = false;
            //textTimer.Stop();
            tmr_movement.Start();
            tmr_animation.Start();
            tmr_animation.Start();
            Ghost.scatterTimer.Interval = levels.get_chaseScatter(Ghost.scatterCounter * 2);
            Ghost.scatterTimer.Start();
            Ghost.scatterStopwatch.Start();
            sirenFinishedPlaying = true;
        }
        //Game over. Кінець гри
        public void GameOver()
        {
            ShowGameOverText();
            CheckScore();
            tmr_movement.Stop();
            tmr_animation.Stop();
            items.blinkTimer.Enabled = false;
        }
        //After the game finishes, check if the player got a high score
        //Перевірка в кінці гри, чи попав гравець у таблицю рекордів
        void CheckScore()
        {
            HighScoresList hs = new HighScoresList(highScores);
            foreach (Player p in highScores.playersList)
            {
                if (player.Score > p.Score)
                {
                    EnterName dialog = new EnterName();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string name = dialog.NameTextBox.Text;
                        Player plr = new Player();
                        plr.Name = name;
                        plr.Score = player.Score;

                        highScores.playersList.Remove(highScores.playersList.Min());
                        highScores.playersList.Add(plr);
                        highScores.playersList.Sort();
                        highScores.playersList.Reverse();
                        highScores.SerializeToXML();

                        hs.FillListView(highScores.playersList);
                        hs.ShowDialog();
                    }
                    break;
                }
            }
        }

        //Informational messages. Інформаційні повідомлення
        public void ShowReadyText()
        {
            textLabel.ForeColor = System.Drawing.Color.Yellow;
            textLabel.Location = new Point(281, 510);
            textLabel.Text = "READY!";
            textLabel.Visible = true;
            textLabel.BringToFront();
        }
        public void ShowGameOverText()
        {
            textLabel.ForeColor = System.Drawing.Color.Red;
            textLabel.Location = new Point(248, 510);
            textLabel.Text = "GAME OVER!";
            textLabel.Visible = true;
            textLabel.BringToFront();
        }
        private void ShowScoreLabel()
        {
            scoreLabel.Text = "Score: " + player.Score.ToString();
            scoreLabel.Visible = true;
            scoreLabel.BringToFront();
        }
        private void ShowLevelLabel()
        {
            levelLabel.Text = "Level: " + (levels.get_level() + 1).ToString();
            levelLabel.Visible = true;
            levelLabel.BringToFront();
        }
        private void ShowHighScoreLabel()
        {
            highScoreLabel.Text = "High Score: " + highScores.GetHighScore();
            highScoreLabel.Visible = true;
            highScoreLabel.BringToFront();
        }
        //Make text labels to show information about game process
        //Створення написів для демонстрації інформації про стан гри
        void InitializeTextLabels()
        {
            textLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            textLabel.Name = "textLabel";
            textLabel.BackColor = System.Drawing.Color.Black;
            textLabel.ForeColor = System.Drawing.Color.Yellow;
            textLabel.AutoSize = true;
            textLabel.Visible = false;
            Controls.Add(textLabel);

            highScoreLabel.Font = new Font("Arial", 16);
            highScoreLabel.Location = new Point(3, 65);
            highScoreLabel.Name = "HighScoreLabel";
            highScoreLabel.Text = "High Score: " + highScores.GetHighScore();
            highScoreLabel.ForeColor = System.Drawing.Color.White;
            highScoreLabel.BackColor = System.Drawing.Color.Black;
            highScoreLabel.AutoSize = true;
            highScoreLabel.Visible = false;
            Controls.Add(highScoreLabel);

            scoreLabel.Font = new Font("Arial", 16);
            scoreLabel.Location = new Point(250, 65);
            scoreLabel.Name = "ScoreLabel";
            scoreLabel.Text = "Score: 0";
            scoreLabel.ForeColor = System.Drawing.Color.White;
            scoreLabel.BackColor = System.Drawing.Color.Black;
            scoreLabel.AutoSize = true;
            scoreLabel.Visible = false;
            Controls.Add(scoreLabel);

            levelLabel.Font = new Font("Arial", 16);
            levelLabel.Location = new Point(500, 65);
            levelLabel.Name = "LevelLabel";
            levelLabel.Text = "Level: " + (levels.get_level() + 1).ToString();
            levelLabel.ForeColor = System.Drawing.Color.White;
            levelLabel.BackColor = System.Drawing.Color.Black;
            levelLabel.AutoSize = true;
            levelLabel.Visible = false;
            Controls.Add(levelLabel);
        }
        void InitializeSound()
        {
            //Sound. Звук
            wmp_game_start = new MediaPlayer();
            wmp_game_start.MediaEnded += Start;
            //wmp_game_start.Open(new Uri("Sound/game_start.wav", UriKind.Relative));
            wmp_game_start.Volume = volume;

            wmp_intermission = new MediaPlayer();
            wmp_intermission.MediaEnded += NextLevel;
            //wmp_intermission.Open(new Uri("Sound/intermission.wav", UriKind.Relative));
            wmp_intermission.Volume = volume;

            wmp_siren_1 = new MediaPlayer();
            wmp_siren_1.MediaEnded += SirenRewind;
            //wmp_siren_1.Open(new Uri("Sound/siren_1.wav", UriKind.Relative));
            wmp_siren_1.Volume = volume;

            wmp_siren_2 = new MediaPlayer();
            wmp_siren_2.MediaEnded += SirenRewind;
            //wmp_siren_2.Open(new Uri("Sound/siren_2.wav", UriKind.Relative));
            wmp_siren_2.Volume = volume;

            wmp_siren_3 = new MediaPlayer();
            wmp_siren_3.MediaEnded += SirenRewind;
            //wmp_siren_3.Open(new Uri("Sound/siren_3.wav", UriKind.Relative));
            wmp_siren_3.Volume = volume;

            wmp_siren_4 = new MediaPlayer();
            wmp_siren_4.MediaEnded += SirenRewind;
            //wmp_siren_4.Open(new Uri("Sound/siren_4.wav", UriKind.Relative));
            wmp_siren_4.Volume = volume;

            wmp_siren_5 = new MediaPlayer();
            wmp_siren_5.MediaEnded += SirenRewind;
            //wmp_siren_5.Open(new Uri("Sound/siren_5.wav", UriKind.Relative));
            wmp_siren_5.Volume = volume;

        }
        public void UpdateScore()
        {
            scoreLabel.Text = "Score: " + player.Score.ToString();
        }
        public void UpdateLevel()
        {
            levelLabel.Text = "Level: " + (levels.get_level() + 1).ToString();
        }
        //Loop siren sound, plays after previous has finished playing. Програвання сирени по закінченні попередньої
        void SirenRewind (object sender, EventArgs e)
        {
            sirenFinishedPlaying = true;
        }
        //Player keyboard input. Чекання вводу команд з клавіатури
        public void KeyWatch(object sender, KeyEventArgs e)
        {
            if (!keysLocked)
            {
                Keys k = e.KeyCode;
                switch (k)
                {
                    case Keys.Up: pacman.PlayerInput(0); break;
                    //case Keys.Up: pacman.PlayerInput(0); levels.levelUp(); break;
                    case Keys.Left: pacman.PlayerInput(1); break;
                    case Keys.Down: pacman.PlayerInput(2); break;
                    //case Keys.Down: pacman.PlayerInput(2); player.adjustScore(1000); break;
                    case Keys.Right: pacman.PlayerInput(3); break;
                }
            }
        }
        //Sprite movement timer. Таймер руху ігрових персонажів
        //Timer for animation. Таймер анімації
        public void AnimationTick(object sender, EventArgs e)
        {
            UpdateScore();
            pacman.ChangeSprite();
            blinky.ChangeSprite();
            pinky.ChangeSprite();
            inky.ChangeSprite();
            clyde.ChangeSprite();

            //Play siren sound on a loop. Безперервне програвання сирени
            if(sirenFinishedPlaying)
            {
                //rewind. Перемотка на початок запису
                wmp_siren_1.Stop();
                wmp_siren_2.Stop();
                wmp_siren_3.Stop();
                wmp_siren_4.Stop();
                wmp_siren_5.Stop();

                wmp_siren_1.Open(new Uri("Sound/siren_1.wav", UriKind.Relative));
                wmp_siren_2.Open(new Uri("Sound/siren_2.wav", UriKind.Relative));
                wmp_siren_3.Open(new Uri("Sound/siren_3.wav", UriKind.Relative));
                wmp_siren_4.Open(new Uri("Sound/siren_4.wav", UriKind.Relative));
                wmp_siren_5.Open(new Uri("Sound/siren_5.wav", UriKind.Relative));

                //wmp_siren_1.Position = new TimeSpan(0);
                //wmp_siren_2.Position = new TimeSpan(0);
                //wmp_siren_3.Position = new TimeSpan(0);
                //wmp_siren_4.Position = new TimeSpan(0);
                //wmp_siren_5.Position = new TimeSpan(0);
                if (items.dots <= 15) wmp_siren_5.Play();
                else if (items.dots <= 30) wmp_siren_4.Play();
                else if (items.dots <= 60) wmp_siren_3.Play();
                else if (items.dots <= 120) wmp_siren_2.Play();
                else wmp_siren_1.Play();
                sirenFinishedPlaying = false;
            }

        }
        public void MovementTick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            float elapsed = (float)(now - LastTime).TotalMilliseconds;

            pacman.Move(elapsed);
            blinky.Move(elapsed);
            pinky.Move(elapsed);
            inky.Move(elapsed);
            clyde.Move(elapsed);

            LastTime = now;
        }
        //Timer to redraw screen. Таймер оновлення екрану
        public void RefreshScreenTick(object sender, EventArgs e)
        {
            board.boardPB.Invalidate(false);
        }
        //Drawing event. Малювання об'єктів
        private void boardPB_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.InterpolationMode = InterpolationMode.High;
            items.Draw(e.Graphics);
            pacman.Draw(e.Graphics);
            blinky.Draw(e.Graphics);
            pinky.Draw(e.Graphics);
            inky.Draw(e.Graphics);
            clyde.Draw(e.Graphics);
        }
        //Coordinate conversion functions. Функції перетворення координат
        public static PointF coordToPixelCorner(Point coord)
        {
            float x, y;
            x = coord.X * BoardTileSize * SpriteScale;
            y = coord.Y * BoardTileSize * SpriteScale;
            return new PointF(x, y);
        }
        public static PointF coordToPixelCenter(Point coord)
        {
            float x, y;
            x = coord.X * BoardTileSize * SpriteScale + (BoardTileSize * SpriteScale) / 2;
            y = coord.Y * BoardTileSize * SpriteScale + (BoardTileSize * SpriteScale) / 2;
            return new PointF(x, y);
        }
        public static Point pixelToCoord(PointF coord)
        {
            int x, y;
            x = (int)(coord.X / BoardTileSize / SpriteScale);
            y = (int)(coord.Y / BoardTileSize / SpriteScale);
            return new Point(x, y);
        }
        public static PointF drawCoord(PointF coord)
        {
            float x, y;
            x = coord.X - SpriteSize * SpriteScale / 2;
            y = coord.Y - SpriteSize * SpriteScale / 2;
            return new PointF(x, y);
        }
        //Cut sprite from a spritesheet. Вирізання малюнку з полотна малюнків
        public static Bitmap CutSprite(int spriteNumber, int sourceSize, int destSize)
        {
            int x, y;
            x = spriteNumber % 32 * 12;
            y = spriteNumber / 32 * 12;

            PointF corner = new PointF(x, y);
            Bitmap result = new Bitmap(destSize, destSize);
            using (Graphics gr = Graphics.FromImage(result))
            {
                PointF[] dest_points =
                {
                    new PointF(0, 0),
                    new PointF(destSize, 0),
                    new PointF(0, destSize),
                };
                RectangleF src_rect = new RectangleF(corner.X, corner.Y, sourceSize - 1, sourceSize - 1);
                //Draws the specified portion of the specified 
                //Image at the specified location and with the specified size.
                //GraphicsUnit.Pixel specifies the units of measure used by the srcRect parameter
                gr.DrawImage(Properties.Resources.Spritemap,
                    dest_points, src_rect, GraphicsUnit.Pixel);
            }
            return result;
        }
        //Menu buttons. Кнопки меню
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void highScoresToolStripMenu_Click(object sender, EventArgs e)
        {
            HighScoresList hs = new HighScoresList(highScores);
            hs.ShowDialog();
        }
        private void pauseToolStripMenu_Click(object sender, EventArgs e)
        {
            if (paused)
            {
                tmr_animation.Start();
                tmr_movement.Start();
                paused = false;
            }
            else
            {
                tmr_animation.Stop();
                tmr_movement.Stop();
                paused = true;
            }
        }
    }
}