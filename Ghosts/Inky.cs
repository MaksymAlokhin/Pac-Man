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
    public class Inky : Ghost
    {
        public Inky(PacForm frm) : base(frm)
        {
            ghostNumber = 2;
            color = Color.Aqua;
            SpriteRight = 528;
            SpriteRight2 = 530;
            SpriteDown = 532;
            SpriteDown2 = 534;
            SpriteLeft = 536;
            SpriteLeft2 = 538;
            SpriteUp = 540;
            SpriteUp2 = 542;
            home = PacForm.coordToPixelCenter(new Point(12, 17));
            scatterTarget = new Point(27, 35);
            stayAtHomeWayPointUp = PacForm.coordToPixelCorner(new Point(12, 17));
            stayAtHomeWayPointDown = PacForm.coordToPixelCorner(new Point(12, 18));
            MakeWaypoints();
            MakeSprites();
            ResetPosition();
        }
        //Put sprite in starting position
        //Переміщення спрайта у стартову позицію
        public override void ResetPosition()
        {
            exited = false;
            canExit[2] = false;
            travellingHome = false;
            followingWaypoints = false;
            direction = 0;
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
                Point myTarget = new Point();
                Point pacmanLoc = PacForm.pixelToCoord(form.pacman.location);
                Point blinkyLoc = PacForm.pixelToCoord(form.blinky.location);
                myTarget.X = pacmanLoc.X + (pacmanLoc.X - blinkyLoc.X);
                myTarget.Y = pacmanLoc.Y + (pacmanLoc.Y - blinkyLoc.Y);
                return myTarget;
            }
        }
        void MakeWaypoints()
        {
            PointF way_01 = PacForm.coordToPixelCenter(new Point(14, 14));
            way_01.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            PointF way_02 = PacForm.coordToPixelCenter(new Point(14, 17));
            way_02.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            PointF way_03 = PacForm.coordToPixelCenter(new Point(12, 17));
            way_03.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            waypoints = new PointF[]
            {
                way_01, way_02, way_03
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