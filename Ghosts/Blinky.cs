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
    public class Blinky : Ghost
    {
        public Blinky(PacForm frm) : base(frm)
        {
            ghostNumber = 0;
            color = Color.Red;
            SpriteRight = 384;
            SpriteRight2 = 386;
            SpriteDown = 388;
            SpriteDown2 = 390;
            SpriteLeft = 392;
            SpriteLeft2 = 394;
            SpriteUp = 396;
            SpriteUp2 = 398;
            home = PacForm.coordToPixelCenter(new Point(14, 14));
            scatterTarget = new Point(25, 0);
            MakeWaypoints();
            MakeSprites();
            ResetPosition();
        }
        //Put sprite in starting position
        //Переміщення спрайта у стартову позицію
        public override void ResetPosition()
        {
            exited = true;
            canExit[0] = true;
            travellingHome = false;
            followingWaypoints = false;
            direction = 1;
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
            else return PacForm.pixelToCoord(form.pacman.location);
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
        public override void StayAtHome() { }
    }
}