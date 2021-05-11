using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.Graphics.Transforms
{
    /// <summary>
    /// An enum indicating how the drawing co-ordinate plane aligns with the output view.
    /// </summary>
    public enum CoordinateStyle
    {
        /// <summary>
        /// (0,0) is the top-left corner, and positive y-values are downwards. Matches normal computer graphics.
        /// </summary>
        TopLeft = 0,

        /// <summary>
        /// (0,0) is the 'center' of the co-ordinate grid, and the co-ordinate values follow a 2D axis (positive y is up, x is right). Matches most mathematics and some computer graphics.
        /// </summary>
        Center = 1
    }
}
