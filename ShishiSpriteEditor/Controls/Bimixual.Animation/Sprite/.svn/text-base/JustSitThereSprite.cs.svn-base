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
    /// Creates an objec that doesn't move but changes only via alterations.
    /// </summary>
    public class JustSitThereSprite : Sprite
    {
        /// <summary>
        /// Create a non-moving object
        /// </summary>
        /// <param name="p_location">Point where to draw the object</param>
        public JustSitThereSprite(Point p_location)
            : base()
        {
            location = p_location;
        }

        /// <summary>
        /// Create a non-moving object that will only exist for the time specified
        /// </summary>
        /// <param name="p_location">Point where to draw the object</param>
        /// <param name="p_timeSpan">double value indicating number of seconds to exist</param>
		public JustSitThereSprite(Point p_location, double p_timeSpan)
            : base(p_timeSpan)
        {
           location = p_location;
        }

        /// <summary>
        /// Tell the position to the base clase
        /// </summary>
        /// <returns>Point value indicating where to draw object</returns>
        protected override Point GetNewPosition()
        {
            return location;
        }
    }
}
