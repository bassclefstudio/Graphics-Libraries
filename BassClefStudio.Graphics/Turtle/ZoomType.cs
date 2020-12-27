using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.Graphics.Turtle
{
    /// <summary>
    /// An enum representing the behavior that should be taken when scaling a screen or control's drawing area.
    /// </summary>
    public enum ZoomType
    {
        /// <summary>
        /// Fit the entire drawable area in the view or control. This may cause letterboxing.
        /// </summary>
        FitAll = 0,

        /// <summary>
        /// Fill the entire view or control with the drawable surface. Some drawing may be done outside of the visible area.
        /// </summary>
        FillView = 1
    }
}
