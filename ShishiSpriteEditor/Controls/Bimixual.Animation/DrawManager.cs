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
using System.Windows.Forms;
using System.Collections.Generic;

namespace Bimixual.Animation
{
    /// <summary>
    /// Draw all things a screen may need based including backgrounds, animations and foregrounds.
    /// </summary>
	public class DrawManager
	{
        private FormBuffer formBuffer;  // draw on this double buffer
		private LinkedList<IDrawable> drawables; // list of things to draw
        private FpsTimer fpsTimer;   // rate at which to draw
        private bool selfTiming; public bool SelfTiming { get { return selfTiming; } }

        /// <summary>
        /// int value of frames per second to display
        /// </summary>
		public int FPS
        {
            get
            {
                return fpsTimer.FPS;
            }
        }

		//// <value>
		/// Returns the current FpsTimer object
		/// </value>
		public FpsTimer FpsTimer
		{
			get
			{
				return fpsTimer;
			}
		}
		
        /// <summary>
        /// Create DrawManager for a given form
        /// </summary>
        /// <param name="p_form">Form to draw on</param>
        /// <param name="p_fps">FpsTimer object</param>
		public DrawManager(Control p_form, FpsTimer p_fps)
        {
            fpsTimer = p_fps;
            formBuffer = new FormBuffer(p_form);
            drawables = new LinkedList<IDrawable>();
            fpsTimer.Start();
			selfTiming = false;
		}

        /// <summary>
        /// Creates an draw manager.
        /// Will set a default frame rate to 36fps
        /// </summary>
        public DrawManager(Form p_form) : this(p_form, new FpsTimer(36))
        {
            selfTiming = true;
        }

        /// <summary>
        /// Adds a drawable item to the list of things the card game will draw.
        /// </summary>
        /// <param name="p">A thing that will be drawn</param>
        public void AddDrawable(IDrawable p_)
        {
            drawables.AddLast(p_);
        }

        /// <summary>
        /// Remove drawable item from list
        /// </summary>
        /// <param name="p_">Item to remove</param>
        public void RemoveDrawable(IDrawable p_)
        {
            drawables.Remove(p_);
        }

        /// <summary>
        /// Call everytime we want a game redraw. Should be called at the highest frames per second that
        /// the game uses.
        /// </summary>
        public void DrawGame()
        {
            if (selfTiming)
            {
                if (!fpsTimer.TimeElapsed()) return;
                fpsTimer.Reset();
                fpsTimer.Start();
            }

            foreach (IDrawable cgd in drawables)
                cgd.Draw(formBuffer.Graphics);
            formBuffer.Render();
        }
	}
}
