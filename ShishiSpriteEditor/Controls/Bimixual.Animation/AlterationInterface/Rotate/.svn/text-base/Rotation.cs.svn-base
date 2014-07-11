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
	/// Rotate the sprite a fixed amount that never changes through the life of the sprite.
	/// Notice it is "rotation" which is a noun. Singular. Which means it is rotated once.
	/// As opposed to "rotate" which is a verb which indicates that it is ongoing. If you
	/// want your sprite to rotate x degrees every frame then use the Rotate alteration instead.
	/// </summary>
    public class Rotation : Rotate
    {
        protected double rotationAmount; 

        /// <summary>
        /// Rotates the sprite the same amount everytime.
        /// Use it to do things like flip and image if you want to use it upside-down for the whole
        /// animation. Use this Rotation alteration to turn it. ie: it doesn't spin.
        /// </summary>
        /// <param name="p_rotation">double as how much to turn it during the animation</param>
        public Rotation(double p_rotation) : base(0)
        {
            rotationAmount = p_rotation;
        }

        /// <summary>
        /// Apply the rotation to each draw
        /// </summary>
        /// <param name="p_g">Graphics object</param>
        /// <param name="p_point">Point as the location of object</param>
        /// <param name="p_bitmap">Bitmap reference of thing to draw</param>
        public override void ApplyAlteration(Graphics p_g, Point p_point, ref Bitmap p_bitmap)
        {
            base.rotation = rotationAmount; // replace the base classes rotation value with our own and apply it to base alteration method
            base.ApplyAlteration(p_g, p_point, ref p_bitmap);
        }

    }
}
