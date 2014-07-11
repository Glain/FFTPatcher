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
    /// The Translation alteration is like a movement but is very useful for treating the focal point
    /// of an animation object to be a location other than 0,0
    /// </summary>
    public class Translation : IAlteration
    {
        Size size;

        /// <summary>
        /// Create a Translation alteration
        /// </summary>
        /// <param name="p_size">Size of the translation</param>
        public Translation(Size p_size)
        {
            size = p_size;
        }

        /// <summary>
        /// Draw the object
        /// </summary>
        /// <param name="p_g">Graphic object</param>
        /// <param name="p_point">Location where to draw object</param>
        /// <param name="p_bitmap">Reference to bitmap to draw on</param>
        public virtual void ApplyAlteration(Graphics p_g, Point p_point, ref Bitmap p_bitmap)
        {
            p_g.TranslateTransform((float)(size.Width), (float)(size.Height));
        }

    }
}
