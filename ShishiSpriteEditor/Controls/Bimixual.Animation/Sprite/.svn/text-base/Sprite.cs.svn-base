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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Bimixual.Animation
{
    /// <summary>
    /// Abstract class from which to create animation objects.
    /// This class handles many very operations and object tracking.
    /// </summary>
    public abstract class Sprite
    {
        // redefined in derived class. This returns the location of where to draw the next frame
        protected abstract Point GetNewPosition();
        protected Point location;   // current position of object
        protected bool completed;   // indicates which cur objs ani is complete
        protected Bitmap bitmap;    // the bitmap that we are animating
		protected long tickSpan; // how long to run animation before ending 0=infinity
		protected long startTicks; // when time is being used, this tracks what time we started

        // the event to raise after animation completes
        public event EventHandler<AnimationFinishedEventArgs> eventOnCompletion;

        // list of alterations to apply to the bitmap for each frame
        private List<IAlteration> alterations = new List<IAlteration>(5);

        // info to pass to the event handler after animation completion
        protected Object completionInfo;

        /// <summary>
        /// Point location of object's current position
        /// </summary>
        public Point Location
        {
            get { return location; }
        }

        /// <summary>
        /// Object of info to pass out as an event argument when animation for the object completes
        /// </summary>
        public Object CompletionInfo
        {
            get { return completionInfo; }
            set { completionInfo = value; }
        }

        /// <summary>
        /// The bitmap that this object be
        /// </summary>
        public Bitmap Bitmap
        {
            set { bitmap = value; }
        }


        /// <summary>
        /// Create an animation object that can be displayed by the AnimationManager
        /// </summary>
        /// <param name="p_ci">An object passed to an event handler after animation completes</param>
        public Sprite(Object p_ci) : this()
        {
            completionInfo = p_ci;
        }

        /// <summary>
        /// Create an animation object that can be displayed by the AnimationManager
        /// </summary>
        /// <param name="p_ci">An object passed to an event handler after animation completes</param>
        /// <param name="p_timeSpan">double value for the length of time to run the animation</param>
		public Sprite(Object p_ci, double p_timeSpan): this(p_ci)
		{
			SetTickSpanBySeconds(p_timeSpan);
		}
		
        /// <summary>
        /// Create a default animation object that will display indefinitely
        /// </summary>
        public Sprite()
        {
			tickSpan = 0;
			startTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Create an animation object that will die after a certain time
        /// </summary>
        /// <param name="p_timeSpan">double value for how long (in seconds) to show the object</param>
		public Sprite(double p_timeSpan) : this()
		{
			SetTickSpanBySeconds(p_timeSpan);
		}
		
        /// <summary>
        /// Add a bitmap alteration to be applied to the object before each drawing.
        /// You can add as many as you like and they will be applied to the object in
        /// the same order they are added.
        /// </summary>
        /// <param name="p_aao"></param>
        public void AddAlteration(IAlteration p_aao)
        {
            alterations.Add(p_aao);
        }

        /// <summary>
        /// Raise the completion event if it is defined.
        /// </summary>
        public void CallCompletionEvent()
        {
            EventHandler<AnimationFinishedEventArgs> temp = eventOnCompletion;
            if (temp != null)
                temp(this, new AnimationFinishedEventArgs(location, completionInfo));
        }

        /// <summary>
        /// Draw the next frame at the right location
        /// </summary>
        /// <param name="p_g">Graphic object onwhich to draw object</param>
        /// <returns>
        /// 0 = success and showed the new frame
        /// 1 = complete
        /// 2 = time interval not yet passed
        /// -1 = failed/error
        /// </returns>
        public virtual int RunNext(Graphics p_g)
        {
            location = GetNewPosition();
            DrawFilteredImage(p_g, location);

            if (tickSpan > 0 && (DateTime.Now.Ticks - startTicks) > tickSpan)
				completed = true;
			
            if (completed) return 1;

            return 0;
        }

        /// <summary>
        /// Draw object applying all alterations in order
        /// </summary>
        /// <param name="p_g">Graphic object onwhich to draw object</param>
        /// <param name="p_point">Location where to draw object</param>
        public virtual void DrawFilteredImage(Graphics p_g, Point p_point)
        {
            if (alterations.Count > 0)
            {
                Matrix originalMatrix = p_g.Transform;
                foreach (IAlteration ai in alterations)
                {
                    ai.ApplyAlteration(p_g, p_point, ref bitmap);
                }
                if (bitmap != null) p_g.DrawImage(bitmap, p_point);
                p_g.Transform = originalMatrix;
            }
            else
            {
                if (bitmap != null) p_g.DrawImage(bitmap, p_point);
            }

        }

        /// <summary>
        /// Determines how many ticks are in a given number of seconds
        /// </summary>
        /// <param name="p_timeSpan">double value of how many seconds to calculate with</param>
		protected void SetTickSpanBySeconds(double p_timeSpan)
		{
			tickSpan = (long)(p_timeSpan * 10000000);
		}

		/// <summary>
		/// Stop the sprite. Kills it. Calls completion event.
		/// </summary>
		public virtual void Stop()
		{
			completed = true;
		}
		
        /// <summary>
        /// Is object valid?
        /// </summary>
        /// <returns>true by default</returns>
        public virtual bool Valid()
        {
            return true;
        }
    }
}
