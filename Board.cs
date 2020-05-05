using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PacMan
{
    //Describes game world
    //Описує ігровий світ
    public class Board
    {
        //Board size in squares; розмір ігрового поля у квадратах
        public Size size = new Size(28, 36);//224 x 288
        //Board size in pixels; розмір ігрового поля у пікселях
        public Size boardPBSize;
        //Array representing board layout; масив, що містить розміщення об'єктів на полі
        public int[,] board;
        //Background image of board; фоновий рисунок ігрового поля
        public Bitmap map;
        //PictureBox of the board, everything is drawn here
        //Об'єкт типу PictureBox, усі елементи ігрового поля малюються на ньому
        public PictureBox boardPB;
        //Alternative background image of board, used during level transition animation
        //Альтернативна версія фонового рисунку ігрового поля, використовується під час переходу гри на новий рівень
        public Bitmap mapChanged;
        public Board(PacForm form)
        {
            size = new Size(28, 36);//224 x 288
            boardPBSize = new Size(size.Width * PacForm.BoardTileSize * PacForm.SpriteScale,
                size.Height * PacForm.BoardTileSize * PacForm.SpriteScale);
            board = new int[size.Width, size.Height];
            map = new Bitmap(size.Width * PacForm.BoardTileSize * PacForm.SpriteScale,
                size.Height * PacForm.BoardTileSize * PacForm.SpriteScale);
            boardPB = new PictureBox();
            mapChanged = new Bitmap(map.Width, map.Height);

            //Board layout. Numbers represent sprites on the spritesheet. 022 and 023 are empty sprites
            //Розташування об'єктів на ігровому полі. Номера відповідають номерам спрайтів. 022 та 023 є пустими зображеннями
            board = new int[,] {
                { 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023 },
                { 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023 },
                { 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023 },
                { 145, 154, 154, 154, 154, 154, 154, 154, 154, 154, 154, 154, 154, 187, 186, 154, 154, 154, 154, 154, 154, 154, 154, 154, 154, 154, 154, 144 },
                { 147, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 121, 120, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 146 },
                { 147, 022, 119, 158, 158, 118, 022, 119, 158, 158, 158, 118, 022, 121, 120, 022, 119, 158, 158, 158, 118, 022, 119, 158, 158, 118, 022, 146 },
                { 147, 022, 121, 023, 023, 120, 022, 121, 023, 023, 023, 120, 022, 121, 120, 022, 121, 023, 023, 023, 120, 022, 121, 023, 023, 120, 022, 146 },
                { 147, 022, 185, 116, 116, 184, 022, 185, 116, 116, 116, 184, 022, 123, 295, 022, 185, 116, 116, 116, 184, 022, 185, 116, 116, 184, 022, 146 },
                { 147, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 146 },
                { 147, 022, 119, 158, 158, 118, 022, 119, 118, 022, 119, 158, 158, 158, 158, 158, 158, 118, 022, 119, 118, 022, 119, 158, 158, 118, 022, 146 },
                { 147, 022, 185, 116, 116, 184, 022, 121, 120, 022, 185, 116, 116, 179, 178, 116, 116, 184, 022, 121, 120, 022, 185, 116, 116, 184, 022, 146 },
                { 147, 022, 022, 022, 022, 022, 022, 121, 120, 022, 022, 022, 022, 121, 120, 022, 022, 022, 022, 121, 120, 022, 022, 022, 022, 022, 022, 146 },
                { 149, 156, 156, 156, 156, 118, 022, 121, 180, 158, 158, 118, 022, 121, 120, 022, 119, 158, 158, 181, 120, 022, 119, 156, 156, 156, 156, 148 },
                { 023, 023, 023, 023, 023, 147, 022, 121, 178, 116, 116, 184, 022, 185, 184, 022, 185, 116, 116, 179, 120, 022, 146, 023, 023, 023, 023, 023 },
                { 023, 023, 023, 023, 023, 147, 022, 121, 120, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 121, 120, 022, 146, 023, 023, 023, 023, 023 },
                { 023, 023, 023, 023, 023, 147, 022, 121, 120, 022, 125, 156, 177, 023, 023, 176, 156, 124, 022, 121, 120, 022, 146, 023, 023, 023, 023, 023 },
                { 155, 155, 155, 155, 155, 184, 022, 185, 184, 022, 146, 022, 022, 022, 022, 022, 022, 147, 022, 185, 184, 022, 185, 155, 155, 155, 155, 155 },
                { 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 146, 022, 022, 022, 022, 022, 022, 147, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022 },
                { 156, 156, 156, 156, 156, 118, 022, 119, 118, 022, 146, 022, 022, 022, 022, 022, 022, 147, 022, 119, 118, 022, 119, 156, 156, 156, 156, 156 },
                { 023, 023, 023, 023, 023, 147, 022, 121, 120, 022, 127, 155, 155, 155, 155, 155, 155, 126, 022, 121, 120, 022, 146, 023, 023, 023, 023, 023 },
                { 023, 023, 023, 023, 023, 147, 022, 121, 120, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 121, 120, 022, 146, 023, 023, 023, 023, 023 },
                { 023, 023, 023, 023, 023, 147, 022, 121, 120, 022, 119, 158, 158, 158, 158, 158, 158, 118, 022, 121, 120, 022, 146, 023, 023, 023, 023, 023 },
                { 145, 155, 155, 155, 155, 184, 022, 185, 184, 022, 185, 116, 116, 179, 178, 116, 116, 184, 022, 185, 184, 022, 185, 155, 155, 155, 155, 144 },
                { 147, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 121, 120, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 146 },
                { 147, 022, 119, 158, 158, 118, 022, 119, 158, 158, 158, 118, 022, 121, 120, 022, 119, 158, 158, 158, 118, 022, 119, 158, 158, 118, 022, 146 },
                { 147, 022, 185, 116, 179, 120, 022, 185, 116, 116, 116, 184, 022, 185, 184, 022, 185, 116, 116, 116, 184, 022, 121, 178, 116, 184, 022, 146 },
                { 147, 022, 022, 022, 121, 120, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 121, 120, 022, 022, 022, 146 },
                { 151, 158, 118, 022, 121, 120, 022, 119, 118, 022, 119, 158, 158, 158, 158, 158, 158, 118, 022, 119, 118, 022, 121, 120, 022, 119, 158, 150 },
                { 153, 116, 184, 022, 185, 184, 022, 121, 120, 022, 185, 116, 116, 179, 178, 116, 116, 184, 022, 121, 120, 022, 185, 184, 022, 185, 116, 152 },
                { 147, 022, 022, 022, 022, 022, 022, 121, 120, 022, 022, 022, 022, 121, 120, 022, 022, 022, 022, 121, 120, 022, 022, 022, 022, 022, 022, 146 },
                { 147, 022, 119, 158, 158, 158, 158, 181, 180, 158, 158, 118, 022, 121, 120, 022, 119, 158, 158, 181, 180, 158, 158, 158, 158, 118, 022, 146 },
                { 147, 022, 185, 116, 116, 116, 116, 116, 116, 116, 116, 184, 022, 185, 184, 022, 185, 116, 116, 116, 116, 116, 116, 116, 116, 184, 022, 146 },
                { 147, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 022, 146 },
                { 149, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 157, 148 },
                { 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023 },
                { 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023, 023 },};
            //Creating board background image
            //Створення фонового малюнку ігрового поля
            using (Graphics g = Graphics.FromImage(map))
            {
                //Draw walls. Малювання стін
                for (int i = 0; i < size.Width; i++)
                {
                    for (int j = 0; j < size.Height; j++)
                    {
                        Bitmap chunk = PacForm.CutSprite(board[j, i], PacForm.BoardTileSize, PacForm.BoardTileSize * PacForm.SpriteScale);
                        g.DrawImage(chunk, i * PacForm.BoardTileSize * PacForm.SpriteScale, j * PacForm.BoardTileSize * PacForm.SpriteScale);
                    }
                }
                //Draw ghost house door. Малювання дверей до будинку привидів
                using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 181, 255)))
                {
                    g.FillRectangle(brush, 156 * PacForm.SpriteScale, 188 * PacForm.SpriteScale, 24 * PacForm.SpriteScale, 2 * PacForm.SpriteScale);
                }
            }
            boardPB.Name = "Map";
            boardPB.SizeMode = PictureBoxSizeMode.Normal;
            boardPB.Location = new Point(3, 32);
            boardPB.Size = boardPBSize;
            //boardPB.Image = map;
            mapChanged = AdjustColor();
            boardPB.BackgroundImage = map;
            boardPB.BackColor = Color.Black;
            form.Controls.Add(boardPB);
        }
        //Check if you can move to a tile
        //Перевірка можливості руху на певну ділянку
        public bool ValidMove(Point coord)
        {
            return board[coord.Y, coord.X] == 22;
        }
        //Make background image white for level transition animation
        //Зміна кольору стін лабіринту на білий для анімації переходу на наступний рівень
        public Bitmap AdjustColor()
        {
            Bitmap changedMap = new Bitmap(map.Width, map.Height); //adjustedImage;
            float brightness = 2.0f; // no change in brightness
            float contrast = 0.0f; // twice the contrast
            float gamma = 1.0f; // no change in gamma

            float adjustedBrightness = brightness - 1.0f;
            // create matrix that will brighten and contrast the image
            float[][] ptsArray ={
        new float[] {contrast, 0, 0, 0, 0}, // scale red
        new float[] {0, contrast, 0, 0, 0}, // scale green
        new float[] {0, 0, contrast, 0, 0}, // scale blue
        new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
        new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
            Graphics g = Graphics.FromImage(changedMap);
            g.DrawImage(map, new Rectangle(0, 0, changedMap.Width, changedMap.Height)
                , 0, 0, map.Width, map.Height,
                GraphicsUnit.Pixel, imageAttributes);
            return changedMap;
        }
    }
}
