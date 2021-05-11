using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Input
{
    /// <summary>
    /// Represents a single, abstracted input detected by an <see cref="IInputWatcher"/>, of any kind.
    /// </summary>
    public interface IInput
    {
    }

    /// <summary>
    /// An <see cref="IInput"/> with a spatial component associated with it by the <see cref="IInputWatcher"/> that detected it.
    /// </summary>
    public interface ISpatialInput : IInput
    {
        /// <summary>
        /// The point in input- or graphics- space that the <see cref="IInputWatcher"/> associated with this input (for example, if a mouse click was processed, this would include the location where the mouse was at the time).
        /// </summary>
        Vector2 Point { get; }
    }

    /// <summary>
    /// Represents any button or other binary input that can be enabled and disabled (pressed and released).
    /// </summary>
    public interface IBinaryInput : IInput
    {
        /// <summary>
        /// A <see cref="bool"/> indicating whether the <see cref="IBinaryInput"/> represents a press ('true') or release ('false') input detection.
        /// </summary>
        bool IsPressed { get; }
    }
}
