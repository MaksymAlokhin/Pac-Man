using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Levels
    {
        int level;
        public float speed;
        float[] pacmanSpeed;
        float[] frightPacmanSpeed;
        float[] ghostSpeed;
        float[] ghostTunnelSpeed;
        float[] frightGhostSpeed;
        int[] frightTime;
        int[] nFlashes;
        int[] bonusSymbol;
        int[] bonusPoints;
        int[] elroy1DotsLeft;
        int[] elroy2DotsLeft;
        int[,] ghostDotThreshold;
        int[] globalDotThreshold;
        int[] exitTimerThreshold;
        float[] elroy1Speed;
        float[] elroy2Speed;
        int[,] chaseScatter;
        PacForm form;
        int bonuslives;
        public int newLifeScore;
        public int blinkTimerInterval;
        public Levels(PacForm frm)
        {

            form = frm;
            level = 0;
            bonuslives = 1;
            newLifeScore = 10000;
            blinkTimerInterval = 300;
            speed = PacForm.boardUnitSize * 10F / 1000F;
            frightTime = new int[] { 6000, 5000, 4000, 3000, 2000, 5000, 2000, 2000, 1000, 5000, 2000, 1000, 1000, 3000, 1000, 1000, 0, 1000, 0, 0, 0 };
            nFlashes = new int[] { 5, 5, 5, 5, 5, 5, 5, 5, 3, 5, 5, 3, 3, 5, 3, 3, 0, 3, 0, 0, 0 };
            pacmanSpeed = new float[] { 0.8F, 0.9F, 0.9F, 0.9F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 0.9F };
            frightPacmanSpeed = new float[] { 0.9F, 0.95F, 0.95F, 0.95F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F };
            ghostSpeed = new float[] { 0.75F, 0.85F, 0.85F, 0.85F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F, 0.95F };
            ghostTunnelSpeed = new float[] { 0.40F, 0.45F, 0.45F, 0.45F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F, 0.50F };
            frightGhostSpeed = new float[] { 0.5F, 0.55F, 0.55F, 0.55F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F, 0.6F };
            bonusSymbol = new int[] { 2, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
            bonusPoints = new int[] { 100, 300, 500, 500, 700, 700, 1000, 1000, 2000, 2000, 3000, 3000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000 };
            chaseScatter = new int[,]
            {
                { 0007000, 0020000, 0007000, 0020000, 0005000, 0020000, 0005000, 9999999 },//0
                { 0007000, 0020000, 0007000, 0020000, 0005000, 1033000, 0000016, 9999999 },//1
                { 0007000, 0020000, 0007000, 0020000, 0005000, 1033000, 0000016, 9999999 },//2
                { 0007000, 0020000, 0007000, 0020000, 0005000, 1033000, 0000016, 9999999 },//3
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//4
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//5
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//6
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//7
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//8
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//9
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//10
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//11
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//12
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//13
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//14
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//15
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//16
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//17
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//18
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 },//19
                { 0005000, 0020000, 0005000, 0020000, 0005000, 1037000, 0000016, 9999999 } //20
            };
            elroy1DotsLeft = new int[] { 20, 30, 40, 40, 40, 50, 50, 50, 60, 60, 60, 80, 80, 80, 100, 100, 100, 100, 120, 120, 120 };
            elroy2DotsLeft = new int[] { 10, 15, 20, 20, 20, 25, 25, 25, 30, 30, 30, 40, 40, 40, 50, 50, 50, 50, 60, 60, 60 };
            elroy1Speed = new float[] { 0.8F, 0.9F, 0.9F, 0.9F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F };
            elroy2Speed = new float[] { 0.85F, 0.95F, 0.95F, 0.95F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F, 1.05F };
            ghostDotThreshold = new int[,] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 60, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};
            globalDotThreshold = new int[] { 0, 7, 17, 32 };
            exitTimerThreshold = new int[] { 4000, 4000, 4000, 4000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000 };
        }
        public void levelUp()
        {
            if (level < 20)
            { 
                level++;
                form.UpdateLevel();
            }
        }
        public int get_frightTime()
        {
            return frightTime[level];
        }
        public int get_nFlashes()
        {
            return nFlashes[level];
        }
        public int get_elroy1DotsLeft()
        {
            return elroy1DotsLeft[level];
        }
        public int get_elroy2DotsLeft()
        {
            return elroy2DotsLeft[level];
        }
        public float get_pacmanSpeed()
        {
            return pacmanSpeed[level];
        }
        public float get_elroy1Speed()
        {
            return elroy1Speed[level];
        }
        public float get_elroy2Speed()
        {
            return elroy2Speed[level];
        }
        public float get_frightPacmanSpeed()
        {
            return frightPacmanSpeed[level];
        }
        public float get_ghostSpeed()
        {
            return ghostSpeed[level];
        }
        public float get_ghostTunnelSpeed()
        {
            return ghostTunnelSpeed[level];
        }
        public float get_frightGhostSpeed()
        {
            return frightGhostSpeed[level];
        }
        public int get_bonusSymbol(int lvl)
        {
            return bonusSymbol[lvl];
        }
        public int get_bonusSymbol()
        {
            return bonusSymbol[level];
        }
        public int get_bonusPoints()
        {
            return bonusPoints[level];
        }
        public int get_level()
        {
            return level;
        }
        public void GetLife()
        {
            if (bonuslives > 0)
            {
                bonuslives--;
                form.pacman.lives++;
            }
        }
        public int get_chaseScatter(int phase)
        {
            return chaseScatter[level, phase];
        }
        public int get_ghostDotThreshold(int ghost)
        {
            return ghostDotThreshold[ghost, level];
        }
        public int get_globalDotThreshold(int ghost)
        {
            return globalDotThreshold[ghost];
        }
        public int get_exitTimerThreshold()
        {
            return exitTimerThreshold[level];
        }
    }
}
