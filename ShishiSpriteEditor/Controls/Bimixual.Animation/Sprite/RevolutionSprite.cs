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
using System.Drawing.Drawing2D;

namespace Bimixual.Animation
{

    /// <summary>
    /// Makes the object move around in a circle.
    /// </summary>
    public class RevolutionSprite : Sprite
    {
        Point center;   // the center around which to revolve
        int radius;     // the radius of the circle
        int step;       // increments with each step taken
        double degreesPerStep;
		//double timePerStep;
		Point y0xLocation; // reference location at y=0, x=radius from which we calc rotations
        int maxSteps;   // max number of steps per revolution
		double stopAt;	// degrees along circle at which to stop the sprite
		double originalAngle; // The angle of the location relative to the center

        /// <summary>
        /// Setup a revolving object based on how long it takes to revolve one revolution
        /// </summary>
        /// <param name="p_location">Point where object starts moving from</param>
        /// <param name="p_center">Point around which object revolves</param>
        /// <param name="p_revolutionTime">double value of time it takes to do one revolution in seconds</param>
        public RevolutionSprite(Point p_location, Point p_center, double p_revolutionTime)
        {
            SetRadius(p_location, p_center);
			SetOriginalAngleCoors(p_location, p_center);
            SetDegreesByTime(p_revolutionTime);
			stopAt = -1.0d;
        }

        /// <summary>
        /// Setup a revolving object based on how long it takes to revolve one revolution and
        /// how long to keep the object going in total.
        /// </summary>
        /// <param name="p_location">Point where object starts moving from</param>
        /// <param name="p_center">Point around which object revolves</param>
        /// <param name="p_revolutionTime">double value of time it takes to do one revolution in seconds</param>
        /// <param name="p_timeSpan">double value for how long to animate</param>
		public RevolutionSprite(Point p_location, Point p_center, double p_revolutionTime, double p_timeSpan)
			: base(p_timeSpan)
		{
            SetRadius(p_location, p_center);
			SetOriginalAngleCoors(p_location, p_center);
            SetDegreesByTime(p_revolutionTime);
			stopAt = -1.0d;
        }

        /// <summary>
        /// Setup a revolving object based on how long many pixels to move per second
        /// </summary>
        /// <param name="p_location">Point where object starts moving from</param>
        /// <param name="p_center">Point around which object revolves</param>
        /// <param name="p_revolutionSpeed">int value for how many pixels to move per second</param>
        public RevolutionSprite(Point p_location, Point p_center, int p_revolutionSpeed)
        {
            SetRadius(p_location, p_center);
			SetOriginalAngleCoors(p_location, p_center);
            SetDegreesBySpeed(p_revolutionSpeed);
			stopAt = -1.0d;
        }

        /// <summary>
        /// Setup a revolving object based on how long many pixels to move per second and
        /// how long to keep the object going in total.
        /// </summary>
        /// <param name="p_location">Point where object starts moving from</param>
        /// <param name="p_center">Point around which object revolves</param>
        /// <param name="p_revolutionSpeed">int value for how many pixels to move per second</param>
        /// <param name="p_timeSpan">double value for how long to animate</param>
        public RevolutionSprite(Point p_location, Point p_center, int p_revolutionSpeed, double p_timeSpan)
			: base(p_timeSpan)
		{
            SetRadius(p_location, p_center);
			SetOriginalAngleCoors(p_location, p_center);
            SetDegreesBySpeed(p_revolutionSpeed);
			stopAt = -1.0d;
        }
		
        /// <summary>
        /// Based on revolution time, calculate how many degrees to move per step and how many steps are required per revolution.
        /// </summary>
        /// <param name="p_revolutionTime">double value of time it takes to do one revolution in seconds</param>
        private void SetDegreesByTime(double p_revolutionTime)
        {
            maxSteps = (int)Math.Abs((int)Math.Round(p_revolutionTime * (double)SpriteManager.FPS));
            degreesPerStep = (double)(360d / (double)(p_revolutionTime * (double)SpriteManager.FPS));
			//degreesPerStep = degreesPerStep * -1;
			//timePerStep = (3.0d / maxSteps);
			step = (int)Math.Round(originalAngle / (degreesPerStep));
			stopAt = -1.0d;
        }

		/// <summary>
        /// Based on pixels per second, setup step size and speed
        /// </summary>
        /// <param name="p_revolutionSpeed">int value of pixels per second</param>
        private void SetDegreesBySpeed(int p_revolutionSpeed)
        {
            int stepsPerSec = p_revolutionSpeed;
            double cir = CalculateCircumference(radius);
            double stepsPerFrame = stepsPerSec / (double)SpriteManager.FPS;

            degreesPerStep = (double)((360d/(cir/stepsPerFrame)));

			step = (int)Math.Round(originalAngle / (degreesPerStep));
			maxSteps = (int)Math.Abs((int)Math.Abs(360/degreesPerStep));
        }

		private void SetOriginalAngleCoors(Point p_location, Point p_center)
		{
			originalAngle = Math.Atan2(p_location.Y - p_center.Y, p_location.X - p_center.X);
			originalAngle /= (Math.PI/180);
			originalAngle *= -1;
			y0xLocation = new Point(center.X + radius, center.Y);
		}
		
		/// <summary>
        /// Determine the radius of the circle to be traversed
        /// </summary>
        /// <param name="p_location">Point value along perimeter of circle</param>
        /// <param name="p_center">Point value of center of circle</param>
        /// <returns>double value of radius</returns>
        private double SetRadius(Point p_location, Point p_center)
        {
            center = p_center;
            location = p_location;

            radius = (int)Math.Round(
                Math.Sqrt(
                    (p_center.X - p_location.X) * (p_center.X - p_location.X) +
                    (p_center.Y - p_location.Y) * (p_center.Y - p_location.Y)
                    )
                );

            step = 1;

            return radius;
        }

        /// <summary>
        /// Returns circumference of circle
        /// </summary>
        /// <param name="radius">double value of radius</param>
        /// <returns>double value that is circumference of circle</returns>
        private double CalculateCircumference(double radius)
        {
            return radius * 2 * Math.PI;
        }

        /// <summary>
        /// Returns the next position for the object
        /// </summary>
        /// <returns>Point value of where next to draw the object</returns>
        protected override Point GetNewPosition()
        {
            float degreesThisStep = (float)(degreesPerStep * step);

//Console.WriteLine("degreePerStep="+degreesPerStep+",step="+step+",degreesThisStep="+degreesThisStep+",maxsteps="+maxSteps);
			

			// subtraction is due to our coorindates system have the origin in the
			// top left corner instead of bottom left. This flips it.
			degreesThisStep = 360 - degreesThisStep;

            double absT = Math.Abs(degreesThisStep);
            double absP = Math.Abs(degreesPerStep);
			if (stopAt >= 0)
			{
                if (absT > (stopAt - absP) &&
                    absT < (stopAt + absP))
					completed = true;
			}
			
            double radians = degreesThisStep * Math.PI / 180;

            Point origin = new Point((y0xLocation.X) - center.X, y0xLocation.Y - center.Y);

            double nx = origin.X * Math.Cos(radians) - origin.Y * Math.Sin(radians);
            double ny = origin.X * Math.Sin(radians) + origin.Y * Math.Cos(radians);

            Point position = new Point((int)(Math.Round(nx)) + center.X, (int)(Math.Round(ny)) + center.Y);

            step = step == maxSteps ? step = 0: step+1;

            return position;
        }

		/// <summary>
		/// Stop the sprite at the given position around the circle.
		/// </summary>
		/// <param name="p_degrees">
		/// A <see cref="System.Double"/> represent at what degree around the circle to stop the sprite
		/// </param>
		public void StopAt(double p_degrees)
		{
            // remember, our coor system is upside down
			stopAt = 360 - p_degrees;
		}
    }
}
