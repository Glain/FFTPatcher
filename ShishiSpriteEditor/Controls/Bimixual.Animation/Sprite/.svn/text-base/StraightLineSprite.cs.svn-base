//Copyright 2009 Derek Duban
//This file is part of the Bimixual Animation Library.
//
//Bimixual Animation is free software: you can redistribute it and/or modify
//it under the terms of the GNU Lesser General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//Bimixual Animation is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public License
//along with Bimixual Animation Library.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Drawing;

namespace Bimixual.Animation
{
    /// <summary>
    /// An Sprite that moves in a straight line from one point to another
    /// </summary>
    public class StraightLineSprite : Sprite
    {
        Point startPoint, endPoint;
        float xi, yi;
        float steps, x, y;
        int k;
        int speed; // pixels per second

        /// <summary>
        /// Defined in constructor. Useful in alterations to know how much to do
        /// something given the total number of steps
        /// </summary>
        public float Steps
        {
            get
            {
                return steps;
            }
        }

        /// <summary>
        /// Create an animation object that travels in a straight from one point to another
        /// </summary>
        /// <param name="p_speed">int value of pixels per second to move</param>
        /// <param name="p_start">Point to start from</param>
        /// <param name="p_end">Point to end up at</param>
        public StraightLineSprite(int p_speed, Point p_start, Point p_end)
            : base()
        {
            speed = p_speed;

            SetMovement(p_start, p_end);
        }

        /// <summary>
        /// We can calculate how much to move at each step at construction time rather than for each frame
        /// </summary>
        /// <param name="p_start">Point to start from</param>
        /// <param name="p_end">Point to end up at</param>
        private void SetMovement(Point p_start, Point p_end)
        {
            startPoint = p_start;
            endPoint = p_end;

            if (speed == 0)
            {

                xi = 0;
                yi = 0;
                steps = -1;
                return;
            }

            float xDistance = startPoint.X - endPoint.X;
            float yDistance = startPoint.Y - endPoint.Y;

            float duration;
            if (Math.Abs(xDistance) > Math.Abs(yDistance))
                duration = xDistance / this.speed;
            else
                duration = yDistance / this.speed;

            steps = Math.Abs(duration * SpriteManager.FPS);

            xi = Math.Abs(xDistance / steps);
            yi = Math.Abs(yDistance / steps);

            if (startPoint.X > endPoint.X) xi *= -1;
            if (startPoint.Y > endPoint.Y) yi *= -1;

            x = startPoint.X - xi;  // subtract xi so the first frame is at the starting position since GetNewPostion incr without a check
            y = startPoint.Y - yi;  // since doing a check would take time for each cycle.

            k = 1;
        }

        /// <summary>
        /// Returns the next position of the object
        /// </summary>
        /// <returns>Point of where to draw next</returns>
        protected override Point GetNewPosition()
        {
            if (steps == -1) return startPoint;

            x += xi;
            y += yi;

            if (k >= steps)
            {
                completed = true;//even though completed is set to true, this frame still draws. therefore catch the stop condition one frame back from the last
                return endPoint;
            }

            k++;
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}
