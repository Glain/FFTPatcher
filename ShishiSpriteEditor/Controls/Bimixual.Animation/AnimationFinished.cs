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
    /// Event arguments passed when an animation object has fulfilled its destiny
    /// </summary>
    public class AnimationFinishedEventArgs : EventArgs
    {
        Point endPoint;
        Object info;

        /// <summary>
        /// Creates event arguments for when an animation object has fulfilled its destiny
        /// </summary>
        /// <param name="p_dest">Point location of the objects final destination</param>
        /// <param name="p_info">Object containing data passed in at the objects creation</param>
        public AnimationFinishedEventArgs(Point p_dest, Object p_info)
        {
            endPoint = p_dest;
            info = p_info;
        }

        /// <summary>
        /// Point of where the animation object has arrived
        /// </summary>
        public Point dest
        {
            get { return endPoint; }
            set { endPoint = value; }
        }

        /// <summary>
        /// Object containing info passed to the animation object when it began its journey
        /// </summary>
        public Object Info
        {
            get { return info; }
            set { info = value; }
        }
    }
}
