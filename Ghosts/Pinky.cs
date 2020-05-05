using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    //Enemy ghosts. Супротивники-привиди
    public class Pinky : Ghost
    {
        public Pinky(PacForm frm) : base(frm)
        {
            ghostNumber = 1;
            color = Color.Pink;
            SpriteRight = 512;
            SpriteRight2 = 514;
            SpriteDown = 516;
            SpriteDown2 = 518;
            SpriteLeft = 520;
            SpriteLeft2 = 522;
            SpriteUp = 524;
            SpriteUp2 = 526;
            home = PacForm.coordToPixelCenter(new Point(14, 17));
            scatterTarget = new Point(2, 0);
            stayAtHomeWayPointUp = PacForm.coordToPixelCorner(new Point(14, 17));
            stayAtHomeWayPointDown = PacForm.coordToPixelCorner(new Point(14, 18));
            MakeWaypoints();
            MakeSprites();
            ResetPosition();
        }
        //Put sprite in starting position
        //Переміщення спрайта у стартову позицію
        public override void ResetPosition()
        {
            exited = false;
            canExit[1] = false;
            travellingHome = false;
            followingWaypoints = false;
            direction = 2;
            location = home;
            location.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            spriteLoop = 0;
            currentSprite = Sprites[direction * 2];
        }
        //Get target coordinates
        //Отримання координатів цілі
        public override Point GetTarget()
        {
            if (travellingHome) return AboveHome;
            else if (scatter) return scatterTarget;
            else
            {
                Point myTarget = PacForm.pixelToCoord(form.pacman.location);
                switch (form.pacman.direction)
                {
                    case 0:
                        myTarget.Y -= 4;
                        break;
                    case 1:
                        myTarget.X -= 4;
                        break;
                    case 2:
                        myTarget.Y += 4;
                        break;
                    case 3:
                        myTarget.X += 4;
                        break;
                }
                return myTarget;
            }
        }
        void MakeWaypoints()
        {
            PointF way_01 = PacForm.coordToPixelCenter(new Point(14, 14));
            way_01.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            PointF way_02 = PacForm.coordToPixelCenter(new Point(14, 17));
            way_02.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            waypoints = new PointF[]
            {
                way_01, way_02
            };
        }
        public override void FollowWaypoints()
        {
            location.X = (float)Math.Round(location.X);
            location.Y = (float)Math.Round(location.Y);
            if (location.X > waypoints[waypointsCounter].X) location.X -= 1;
            else if (location.X < waypoints[waypointsCounter].X) location.X += 1;
            else if (location.X == waypoints[waypointsCounter].X && location.Y > waypoints[waypointsCounter].Y) location.Y -= 1;
            else if (location.X == waypoints[waypointsCounter].X && location.Y < waypoints[waypointsCounter].Y) location.Y += 1;
            if (location == waypoints[waypointsCounter]) waypointsCounter++;
            if (waypointsCounter == waypoints.Length)
            {
                followingWaypoints = false;
                travellingHome = false;
                waypointsCounter = 0;
                exited = false;
                travellingGhosts--;
            }
        }
        public override void StayAtHome()
        {
            if (direction == 0)
            {
                if (location.Y > stayAtHomeWayPointUp.Y)
                {
                    location.Y--;
                }
                else direction = 2;
            }
            else
            {
                if (location.Y < stayAtHomeWayPointDown.Y)
                {
                    location.Y++;
                }
                else direction = 0;
            }
        }
    }
}