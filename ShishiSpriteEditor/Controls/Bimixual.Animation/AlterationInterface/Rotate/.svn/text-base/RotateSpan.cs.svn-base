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
    /// Rotate an object a given number of degrees over the span of its movement/
    /// This works by extending rotation, but intercepting its calculated rotation amounts
    /// at each frame.
    /// </summary>
    public class RotateSpan : Rotate
    {
        protected double spanAngle;
        private double steps;  // number of steps to take - yes, it can be fractional
        private int step;   // current step - but we only walk in whole steps (frames)

        /// <summary>
        /// Construct a rotation span object
        /// </summary>
        /// <param name="p_steps">double as number of steps to be taken between its start and end positions</param>
        /// <param name="p_spanAngle">double value of degrees to rotate over the course of its movement</param>
        public RotateSpan(double p_steps, double p_spanAngle) : base(0)
        {
            spanAngle = p_spanAngle;
            steps = p_steps;
            step = 0;
            base.angle = (double)((double)(spanAngle / steps));
        }

        /// <summary>
        /// Apply the appropriate rotation amount for this frame
        /// </summary>
        /// <param name="p_g">Graphics object</param>
        /// <param name="p_point">Point as the location of object</param>
        /// <param name="p_bitmap">Bitmap reference of thing to draw</param>
        public override void ApplyAlteration(Graphics p_g, Point p_point, ref Bitmap p_bitmap)
        {
            base.rotation = step * base.angle; // replace the base classes rotation value with our own and apply it to base alteration method
            base.ApplyAlteration(p_g, p_point, ref p_bitmap);
            step++;
        }

    }
}
