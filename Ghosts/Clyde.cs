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
    public class Clyde : Ghost
    {
        public Clyde(PacForm frm) : base(frm)
        {
            ghostNumber = 3;
            color = Color.Orange;
            SpriteRight = 576;
            SpriteRight2 = 578;
            SpriteDown = 580;
            SpriteDown2 = 582;
            SpriteLeft = 584;
            SpriteLeft2 = 586;
            SpriteUp = 588;
            SpriteUp2 = 590;
            home = PacForm.coordToPixelCenter(new Point(15, 17));
            scatterTarget = new Point(0, 35);
            stayAtHomeWayPointUp = PacForm.coordToPixelCorner(new Point(16, 17));
            stayAtHomeWayPointDown = PacForm.coordToPixelCorner(new Point(16, 18));
            MakeWaypoints();
            MakeSprites();
            ResetPosition();
        }
        //Put sprite in starting position
        //Переміщення спрайта у стартову позицію
        public override void ResetPosition()
        {
            exited = false;
            canExit[3] = false;
            travellingHome = false;
            followingWaypoints = false;
            direction = 0;
            location = home;
            location.X += PacForm.BoardTileSize * PacForm.SpriteScale / 2;
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
                Point pacmanLoc = PacForm.pixelToCoord(form.pacman.location);
                Point myLoc = PacForm.pixelToCoord(location);
                if (FindDistance(myLoc, pacmanLoc) < 9)
                {
                    return scatterTarget;
                }
                else return pacmanLoc;
            }
        }
        void MakeWaypoints()
        {
            PointF way_01 = PacForm.coordToPixelCenter(new Point(14, 14));
            way_01.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            PointF way_02 = PacForm.coordToPixelCenter(new Point(14, 17));
            way_02.X -= PacForm.BoardTileSize * PacForm.SpriteScale / 2;
            PointF way_03 = PacForm.coordToPixelCenter(new Point(16, 17));
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
