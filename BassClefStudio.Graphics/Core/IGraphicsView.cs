using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.Graphics.Core
{
    /// <summary>
    /// Represents a view in an app that can display <see cref="IGraphicsProvider"/> graphics content.
    /// </summary>
    public interface IGraphicsView
    {
        /// <summary>
        /// An event fired whenever the <see cref="IGraphicsView"/> requests a redraw of the UI.
        /// </summary>
        event EventHandler<UpdateRequestEventArgs> UpdateRequested;

        /// <summary>
        /// A <see cref="bool"/> indicating whether this <see cref="IGraphicsView"/> should continually refresh its UI. If set to 'false', <see cref="UpdateRequested"/> is only called when the view changes (e.g. its size changes).
        /// </summary>
        bool IsAutoUpdate { get; set; }

        /// <summary>
        /// Requests that the <see cref="IGraphicsView"/> updates its UI, providing a callback via the <see cref="UpdateRequested"/> event.
        /// </summary>
        void RequestUpdate();
    }
}
