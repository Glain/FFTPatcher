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
    /// Rotates the Animation object to which it is applied.
    /// Rotate is based on degrees per-second
    /// </summary>
    public class Rotate : IAlteration
    {
        protected double angle; // the delta angle per frame
        protected double rotation; // cummulative delta

        /// <summary>
        /// Rotate object as specified.
        /// </summary>
        /// <param name="p_angle">how much should it rotate per second</param>
        public Rotate(double p_angle)
        {
            angle = p_angle / SpriteManager.FPS;
            rotation = angle;
        }
			
        /// <summary>
        /// Default constructor doesn't rotate anything.
        /// Rotation overrides can change angle/rotation so it changes a
        /// different amount per frame.
        /// </summary>
        public Rotate()
        {
            angle = 0;
            rotation = 0;
        }

        /// <summary>
        /// Apply the rotation
        /// </summary>
        /// <param name="p_g">Graphic object</param>
        /// <param name="p_point">Location where to draw object</param>
        /// <param name="p_bitmap">Reference to bitmap to draw on</param>
        public virtual void ApplyAlteration(Graphics p_g, Point p_point, ref Bitmap p_bitmap)
        {
            p_g.TranslateTransform((float)(p_point.X), (float)(p_point.Y));

            // Rotate desired degrees
            p_g.RotateTransform((float)rotation * -1);

            // Move the origin back to where it belongs while it is rotated
            p_g.TranslateTransform((float)(p_point.X) * -1, (float)(p_point.Y) * -1);

            rotation += angle;
            if (rotation > 360) rotation -= 360;
        }
    }
}
