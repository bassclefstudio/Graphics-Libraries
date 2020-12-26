using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BassClefStudio.TurtleGraphics
{
    /// <summary>
    /// An <see cref="EventArgs"/> for <see cref="ITurtleGraphicsView.UpdateRequested"/>, containing information about the current state of the view control.
    /// </summary>
    public class UpdateRequestEventArgs : EventArgs
    {
        /// <summary>
        /// The size, in view co-ordinates, of the available view space.
        /// </summary>
        public Vector2 ViewSize { get; }

        /// <summary>
        /// Creates a new <see cref="UpdateRequestEventArgs"/>.
        /// </summary>
        /// <param name="viewSize">The size, in view co-ordinates, of the available view space.</param>
        public UpdateRequestEventArgs(Vector2 viewSize)
        {
            ViewSize = viewSize;
        }
    }
}
