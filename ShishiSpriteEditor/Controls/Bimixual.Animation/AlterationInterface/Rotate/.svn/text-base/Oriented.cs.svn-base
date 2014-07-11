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
    /// Use this when you want bit map to point towards its destination, not just
    /// spinning. Types of orientation are supported:
    /// 1. point towards destination
    /// 2. point towards a third location
    /// </summary>
    public class Oriented : Rotate
    {
        protected Point target;

        /// <summary>
        /// Object is oriented such that the top of the object is pointing towards the
        /// given point no matter where the object is on the screen.
        /// </summary>
        /// <param name="target">Point to location that bitmap should point to</param>
        /// 
        public Oriented(Point p_target)
            : base(0)
        {
            target = p_target;
        }

        /// <summary>
        /// assumes bitmap is pointing up (90degrees)
        /// then rotates bitmap to point to the target
        /// </summary>
		/// <param name="p_g">
		/// A <see cref="Graphics"/> on which the sprite is being drawn
		/// </param>
		/// <param name="p_point">
		/// A <see cref="Point"/> indicating where the sprite will be drawn
		/// </param>
		/// <param name="p_bitmap">
		/// A <see cref="Bitmap"/> of the sprite
		/// </param>
        public override void ApplyAlteration(Graphics p_g, Point p_point, ref Bitmap p_bitmap)
        {
            float dx, dy;
            int offset = -90;
            
            //dx = (p_point.X - target.X);
            //dy = (p_point.Y - target.Y);

            dx = (target.X - p_point.X);
            dy = (target.Y - p_point.Y);

			if (dx < 0) offset = 90;

            double ratio;
            if (dx == 0) ratio = dy;
            else  ratio = dy / dx;

            rotation = -(float)Math.Atan((float)ratio) * (float)(180 / Math.PI) + offset;
            base.ApplyAlteration(p_g, p_point, ref p_bitmap);
        }
    }
}
