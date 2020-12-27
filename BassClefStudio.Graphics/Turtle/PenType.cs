using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.Graphics.Turtle
{
    /// <summary>
    /// An enum indicating the type of end the <see cref="ITurtleGraphicsProvider"/> pen should have while drawing lines.
    /// </summary>
    public enum PenType
    {
        /// <summary>
        /// The pen should end with a flat edge at the start and end-points.
        /// </summary>
        Flat = 0,

        /// <summary>
        /// The ends of the line should be rounded, meaning they may extend beyond the actual start and end-points.
        /// </summary>
        Round = 1
    }
}
