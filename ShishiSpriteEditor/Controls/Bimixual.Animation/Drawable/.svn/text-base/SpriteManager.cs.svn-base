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
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Bimixual.Animation
{
    /// <summary>
    /// Displays animation objects
    /// </summary>
    public class SpriteManager : IDrawable
    {
        ///// <summary>
        ///// Frame per second is static but can be set (or reset) by an instance constructor. Crazy but true. Why not, eh?
        ///// Chances are games will use just one AM. But if more than one, be careful.
        ///// The purpose for it being static is so all animation objects have quick easy reference to frame rate.
        ///// The purpose for it to be easily set is so that it is easy to set. So don't fuck around.
        ///// </summary>
        static private int globalFps;
        static public int FPS
        {
            get
            {
                return globalFps;
            }
        }

        /// <summary>
        /// LinkedList, quick to traverse and easy to add and remove shit (oneday I should expand this
        /// and actually take advantage of this ability)
        /// </summary>
        LinkedList<Sprite> animationObjects = new LinkedList<Sprite>();

        /// <summary>
        /// If any animation objects are "going", then this is true.
        /// </summary>
        private bool running; public bool Running { get { return running; } }
        private bool selfTiming; public bool SelfTiming { get { return selfTiming; } }

        FpsTimer fps;

        /// <summary>
        /// Creates an animation manager
        /// </summary>
        /// <param name="p_fps">FpsTimer object onwhich to sync the animations</param>
        public SpriteManager(FpsTimer p_fps)
        {
            fps = p_fps;
            SpriteManager.globalFps = fps.FPS;
            fps.Start();
            selfTiming = false;
        }

        /// <summary>
        /// <summary>
        /// Creates an animation manager.
        /// Will set a default frame rate to 36fps
        /// </summary>
        /// </summary>
        public SpriteManager() : this(new FpsTimer(36))
        {
            selfTiming = true;
        }

        /// <summary>
        /// Add the given animation object to the display list
        /// </summary>
        /// <param name="p_ao">An Sprite to be displayed by manager</param>
        /// <returns>true if successful else fales</returns>
        public bool AddObject(Sprite p_ao)
        {
            if (p_ao.Valid())
            {
                animationObjects.AddLast(p_ao);
                running = true; // if any animation objects are present then animations are running
                return true;
            }

            return false;
        }

        /// <summary>
        /// Stop all animations and clear the list of them (animation objects will need to be re-added)
        /// </summary>
        public void Stop()
        {
            animationObjects.Clear();
            running = false;
        }

        /// <summary>
        /// Draw the next frame - fulfills IDrawable contract
        /// </summary>
        /// <param name="p_g">Graphics object</param>
        public void Draw(Graphics p_g)
        {
            RunNext(p_g);
        }

        /// <summary>
        /// Tells this AnimationManager to draw all current objects on the given
        /// Graphics object
        /// </summary>
        /// <param name="p_g">the Graphics object onwhich to draw</param>
        public void RunNext(Graphics p_g)
        {
            if (selfTiming)
            {
                if (!fps.TimeElapsed()) return;
                fps.Reset();
                fps.Start();
            }

            int removeIndex = 0; // we need to remember how many completed objects to delete

            // A list of AnimationObjects that have completed on this run
            Sprite[] aoToRemove = new Sprite[animationObjects.Count];

            foreach (Sprite animationObject in animationObjects)
            {   // Run each Sprite in turn
                if (animationObject.RunNext(p_g) == 1) // a return of 1 indicates completion
                {   // Remember to remove this object since it is done
                    aoToRemove[removeIndex++] = (Sprite)(animationObject);
                }
            }

            for (int i = 0; i < removeIndex; i++)
            {   // remove all completed Animation objects from list and
                // call the completion event for each (before removal)
                aoToRemove[i].CallCompletionEvent();
                animationObjects.Remove(aoToRemove[i]);
            }
            if (animationObjects.Count == 0) running = false;
        }
    }
}
