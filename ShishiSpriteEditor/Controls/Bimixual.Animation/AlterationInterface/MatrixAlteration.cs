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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

// useful: http://www.senocular.com/flash/tutorials/transformmatrix/
// http://www.gnome.org/~mathieu/libart/libart-affine-transformation-matrices.html
namespace Bimixual.Animation
{
	/// <summary>
	/// MatrixAlteration applies the given matrix to the Graphics object before the
	/// sprite is drawn.
	/// </summary>
    public class MatrixAlteration : IAlteration
    {
        Matrix matrix;

		/// <summary>
		/// Create a MatrixAlteration to apply to the sprite
		/// </summary>
		/// <param name="p_matrix">
		/// A <see cref="Matrix"/> defining what alteration to apply to the sprite
		/// </param>
        public MatrixAlteration(Matrix p_matrix)
        {
            matrix = p_matrix;
        }

		/// <summary>
		/// Fulfill the IAlteration contract. Applies the matrix alteration
		/// to the Graphics object of the sprite.
		/// Bug: 
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
        public void ApplyAlteration(Graphics p_g, Point p_point, ref Bitmap p_bitmap)
        {
            //p_g.Transform = matrix;
			p_g.MultiplyTransform(matrix);
        }
    }
}
