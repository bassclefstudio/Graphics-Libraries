using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Input.Basic
{
    /// <summary>
    /// An <see cref="IInput"/> detected from the primary input device, such as a single finger touch, mouse, or stylus.
    /// </summary>
    public class PointerInput : ISpatialInput
    {
        /// <inheritdoc/>
        public Vector2 Point { get; }

        /// <summary>
        /// A <see cref="PointerBehaviour"/> value indicating the types of behaviour associated with this <see cref="PointerInput"/>.
        /// </summary>
        public PointerBehaviour Behaviour { get; }

        /// <summary>
        /// Creates a new <see cref="PointerInput"/>.
        /// </summary>
        /// <param name="point">The point in input- or graphics- space that the <see cref="PointerInput"/> was detected.</param>
        /// <param name="behaviour">A <see cref="PointerBehaviour"/> value indicating the types of behaviour associated with this <see cref="PointerInput"/>.</param>
        public PointerInput(Vector2 point, PointerBehaviour behaviour)
        {
            Point = point;
            Behaviour = behaviour;
        }
    }

    /// <summary>
    /// An enum representing the types of behaviour that can be associated with a <see cref="PointerInput"/> input.
    /// </summary>
    public enum PointerBehaviour
    {
        /// <summary>
        /// The pointer is hovering above a specific location.
        /// </summary>
        Hover = 0,

        /// <summary>
        /// The main button or tap action has been performed.
        /// </summary>
        Press = 1,

        /// <summary>
        /// The secondary (right-click) button or action has been performed.
        /// </summary>
        PressSecondary = 2,

        /// <summary>
        /// A <see cref="PointerBehaviour.Press"/> or <see cref="PointerBehaviour.PressSecondary"/> behaviour has been completed.
        /// </summary>
        Release = 3
    }
}
