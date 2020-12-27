using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Core
{
    /// <summary>
    /// An <see cref="EventArgs"/> for <see cref="IGraphicsView.UpdateRequested"/>, containing information about the current state of the view control.
    /// </summary>
    public class UpdateRequestEventArgs : EventArgs
    {
        /// <summary>
        /// The size, in view co-ordinates, of the available view space, or 'null' if no such size is available.
        /// </summary>
        public Vector2? ViewSize { get; }

        /// <summary>
        /// The associated <see cref="IGraphicsProvider"/> that can be used to execute draw commands.
        /// </summary>
        public IGraphicsProvider GraphicsProvider { get; }

        /// <summary>
        /// Creates a new <see cref="UpdateRequestEventArgs"/>.
        /// </summary>
        /// <param name="graphicsProvider">The associated <see cref="IGraphicsProvider"/> that can be used to execute draw commands.</param>
        /// <param name="viewSize">The size, in view co-ordinates, of the available view space.</param>
        public UpdateRequestEventArgs(Vector2? viewSize, IGraphicsProvider graphicsProvider)
        {
            ViewSize = viewSize;
            GraphicsProvider = graphicsProvider;
        }
    }
}
