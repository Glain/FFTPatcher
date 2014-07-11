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
using System.Diagnostics;

namespace Bimixual.Animation
{
    /// <summary>
    /// Implements a stopwatch that can indicate when are new frame should be drawn.
    /// Set the fps with the constructor and call TimeElapsed() to test if a frame time has elapsed or
    /// call Next() which will reset the timer if TimeElapsed() and return true;
    /// </summary>
    public class FpsTimer
    {
        private const int defaultFPS = 36;
        private Stopwatch stopwatch; // our watch

        /// <summary>
        /// based on fps, this is how many ticks must pass per frame
        /// </summary>
        private int waitCount; public int WaitCount { get { return waitCount; } }

        /// <summary>
        /// frames per second. Can be set in midrun if you want
        /// </summary>
        private int fps; public int FPS { get { return fps; } set { fps = value; } }

        /// <summary>
        /// Set to true if timer is active
        /// </summary>
        private bool running; public bool Running { get { return running; } }

        /// <summary>
        /// Constructor a fps timer
        /// </summary>
        /// <param name="p_fps">int of frames per second</param>
        public FpsTimer(int p_fps)
        {
            fps = p_fps;

            Init();
        }

        /// <summary>
        /// Setup the wait mechanism between frames (ticks per frame)
        /// </summary>
        private void Init()
        {
            try
            {
                waitCount = (int)(Stopwatch.Frequency / fps);
            }
            catch (DivideByZeroException)
            {
                fps = defaultFPS;
				waitCount = (int)(Stopwatch.Frequency / fps);
            }

            stopwatch = new Stopwatch();
            stopwatch.Reset();
        }

        /// <summary>
        /// Check if the time for the frame has elapsed
        /// </summary>
        /// <returns>true if the frame time has elapsed, else false</returns>
        public bool TimeElapsed()
        {
            if (stopwatch.ElapsedTicks >= waitCount) // && C1061 removed state condition
                return true;

            return false;
        }

        /// <summary>
        /// Returns true and resets and starts the time of frame time has elapsed
        /// </summary>
        /// <returns>True if frame time has passed, else false</returns>
        public bool Next()
        {
            if (!TimeElapsed()) return false;

            stopwatch.Reset();
            stopwatch.Start();

            return true;
        }

		/// <summary>
		/// Reset the current timer and don't restart it.
		/// </summary>
        public void Reset()
        {
            stopwatch.Reset();
            running = false;
        }

		/// <summary>
		/// Start the timer
		/// </summary>
        public void Start()
        {
            stopwatch.Start();
            running = true;
        }

		/// <summary>
		/// Stop the timer
		/// </summary>
        public void Stop()
        {
            stopwatch.Stop();
            running = false;
        }
    }
}
