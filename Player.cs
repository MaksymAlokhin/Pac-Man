using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PacMan
{
    //Keeps the name of the player and the score
    //Зберігає інформацію про гравця та набрані очки
    public class Player : IComparable<Player>
    {
        //Player name and score. Ім'я гравця та очки
        public string Name;
        public int Score;
        public Player()
        {
            Name = "Player";
            Score = 0;
        }
        //Functions to compare/get/set/change score or name
        //Функції для порівняння, повернення, встановлення, зміни імені чи очків
        public int CompareTo(Player other)
        {
            return Score.CompareTo(other.Score);
        }
        public int getScore()
        {
            return Score;
        }
        public void setScore(int scr)
        {
            Score = scr;
        }
        public string getName()
        {
            return Name;
        }
        public void setName(string name)
        {
            Name = name;
        }
        public void adjustScore(int by)
        {
            Score += by;
        }
    }
}
