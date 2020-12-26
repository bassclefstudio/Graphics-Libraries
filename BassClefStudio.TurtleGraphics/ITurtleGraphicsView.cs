﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.TurtleGraphics
{
    /// <summary>
    /// Represents an <see cref="ITurtleGraphicsProvider"/> that is providing UI or graphics content to a view.
    /// </summary>
    public interface ITurtleGraphicsView : ITurtleGraphicsProvider
    {
        /// <summary>
        /// An event fired whenever the <see cref="ITurtleGraphicsView"/> requests a redraw of the UI.
        /// </summary>
        event EventHandler<UpdateRequestEventArgs> UpdateRequested;

        /// <summary>
        /// A <see cref="bool"/> indicating whether this <see cref="ITurtleGraphicsView"/> should continually refresh its UI. If set to 'false', <see cref="UpdateRequested"/> is only called when the view changes (e.g. its size changes).
        /// </summary>
        bool IsAutoRefresh { get; set; }
    }
}
