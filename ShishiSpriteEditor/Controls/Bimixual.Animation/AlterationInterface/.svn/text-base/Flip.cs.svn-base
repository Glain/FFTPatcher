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
    /// Flips the image vertically, horizontally or both based on a matrix transformation.
    /// The flip is based upon the object's x,y position so you may (probably) want to translate
    /// the image after the flip so that it is flipped along its midline.
    /// </summary>
    public class Flip : IAlteration
    {
 		public enum FlipType { Horizontal, Vertical, Both };
		FlipType flipType;

		/// <summary>
		/// Construct a Flip alteration.
		/// </summary>
		/// <param name="p_flipType">
		/// A <see cref="FlipType"/> indicating how to flip the image.
		/// </param>
        public Flip(FlipType p_flipType)
        {
            flipType = p_flipType;
        }

        /// <summary>
        /// Apply the flip
        /// </summary>
        /// <param name="p_g">Graphic object</param>
        /// <param name="p_point">Location where to draw object</param>
        /// <param name="p_bitmap">Reference to bitmap to draw on</param>
        public void ApplyAlteration(Graphics p_g, Point p_point, ref Bitmap p_bitmap)
        {
            p_g.TranslateTransform((float)(p_point.X), (float)(p_point.Y));

			Matrix m = new Matrix(1,0,0,1,0,0);
			
			switch(flipType)
			{
			case Animation.Flip.FlipType.Horizontal:
				m = new Matrix(-1,0,0,1,0,0);
				break;
			case Animation.Flip.FlipType.Vertical:
				m = new Matrix(1,0,0,-1,0,0);
				break;
			case Animation.Flip.FlipType.Both:
				m = new Matrix(-1,0,0,-1,0,0);
				break;
			default:
				m = new Matrix(1,0,0,1,0,0);
				break;
			}
			
            // Rotate desired degrees
            p_g.MultiplyTransform(m);

            // Move the origin back to where it belongs while it is rotated
            p_g.TranslateTransform((float)(p_point.X) * -1, (float)(p_point.Y) * -1);
        }
    }
}
