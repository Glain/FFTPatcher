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
    /// Rotates the sprite so that it is normal to its path
    /// </summary>
    public class RotateToNormal : Rotate
    {
		GeometricsInterface geometricObject; // object from which to get the normal angle
		
        /// <summary>
        /// Create an alteration that rotates sprite to its path's normal
        /// </summary>
        /// <param name="p_gi">GeometricsInterface</param> object from which to get the normal's angle
        public RotateToNormal(GeometricsInterface p_gi) : base(0)
        {
			geometricObject = p_gi;
        }

        /// <summary>
        /// Apply the appropriate rotation amount for this frame
        /// </summary>
        /// <param name="p_g">Graphics object</param>
        /// <param name="p_point">Point as the location of object</param>
        /// <param name="p_bitmap">Bitmap reference of thing to draw</param>
        public override void ApplyAlteration(Graphics p_g, Point p_point, ref Bitmap p_bitmap)
        {
            base.rotation = geometricObject.NormalAngle - 90.0d; // our standing rule is that the top of the bitmap points to the angle indicated, therefore need to rotate 90
            base.ApplyAlteration(p_g, p_point, ref p_bitmap);
		}
    }
}
