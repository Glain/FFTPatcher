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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Bimixual.Animation
{
    /// <summary>
    /// Represents a screen double buffer matched to the form passed in the constructor.
    /// </summary>
    public class FormBuffer
    {
        protected BufferedGraphicsContext bufferContext;
        protected BufferedGraphics bufferG;
        protected Graphics curGraphicsObject;   // saves where to write - either via Paint, or Draw to form
        protected Control control; // the screen to which we are applying the buffer

        /// <summary>
        /// Returns the graphics object that should be drawn upon to have it double buffered
        /// for the form.
        /// </summary>
        public Graphics Graphics
        {
            get
            {
                return bufferG.Graphics;
            }
        }

        /// <summary>
        /// Create a double buffer for the given form
        /// </summary>
        /// <param name="p_form">the form to which this double buffer will apply</param>
        public FormBuffer(Control control)
        {
            this.control = control;
            SetBuffer(); // setup
            curGraphicsObject = Graphics.FromHwnd(control.Handle); // set the default Graphics object
        }

        /*
         * code taken from MSDN Help
         * */
        protected void SetBuffer()
        {
            // Retrieves the BufferedGraphicsContext for the 
            // current application domain.
            bufferContext = BufferedGraphicsManager.Current;

            // Sets the maximum size for the primary graphics buffer
            // of the buffered graphics context for the application
            // domain.  Any allocation requests for a buffer larger 
            // than this will create a temporary buffered graphics 
            // context to host the graphics buffer.
            bufferContext.MaximumBuffer = new Size(control.Width + 1, control.Height + 1);

            // Allocates a graphics buffer the size of this form
            // using the pixel format of the Graphics created by 
            // the Form.CreateGraphics() method, which returns a 
            // Graphics object that matches the pixel format of the form.
            bufferG = bufferContext.Allocate(control.CreateGraphics(),
                 new Rectangle(0, 0, control.Width, control.Height));

            // Removed because it really doesn't belong here but I don't want to
            // forget to use it later
            //bufferG.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        }

        /// <summary>
        /// Call to draw the current buffer to the screen.
        /// </summary>
        public void Render()
        {
            bufferG.Render(curGraphicsObject);
        }

        /// <summary>
        /// Can temporarily draw to another Graphics object. Can be used to draw to the
        /// Graphics object given in Paint functions.
        /// 
        /// EndPaint must be called after rending to reset graphics to original
        /// form Graphics object
        /// </summary>
        /// <param name="p_g">Graphics object onwhich to render output</param>
        public void BeginPaint(Graphics p_g)
        {
            curGraphicsObject.Dispose();
            curGraphicsObject = p_g;
        }

        /// <summary>
        /// Called after calling BeginPaint and Render in order to reset original
        /// Graphics object begotten from form
        /// </summary>
        public void EndPaint()
        {
            curGraphicsObject = Graphics.FromHwnd(control.Handle);
        }
    }
}
