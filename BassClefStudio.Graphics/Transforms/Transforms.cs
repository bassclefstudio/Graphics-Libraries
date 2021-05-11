using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Transforms
{
    /// <summary>
    /// A transform to graphical elements represents a way to transform <see cref="Vector2"/> co-ordinates from one space to another, as well as a scaling factor for static UI elements or shapes.
    /// </summary>
    public interface ITransform
    {
        /// <summary>
        /// Transforms a <see cref="Vector2"/> in drawing-space to a <see cref="Vector2"/> in view-space using this <see cref="ViewCamera"/>. The point is first translated, then scaled (a behavior all implementations of the graphics library are expected to follow).
        /// </summary>
        /// <param name="drawPoint">The point in drawing (graphics) space.</param>
        /// <returns>The point relative to the view.</returns>
        Vector2 TransformPoint(Vector2 drawPoint);

        /// <summary>
        /// Transforms a <see cref="Vector2"/> found in view-space to a <see cref="Vector2"/> representing the place in drawing-space this represents using this <see cref="ViewCamera"/>. This is the inverse of <see cref="TransformPoint(Vector2)"/>.
        /// </summary>
        /// <param name="viewPoint">The point in view-space.</param>
        /// <returns>The point relative to the drawing-space.</returns>
        Vector2 RetreivePoint(Vector2 viewPoint);

        /// <summary>
        /// Gets a <see cref="float"/> representing the ratio of the scale in drawing-space to view-space (for UI elements, stroke widths, etc.).
        /// </summary>
        float Scale { get; }
    }

    /// <summary>
    /// Represents a linear movement by the specified amount.
    /// </summary>
    public struct Translation : ITransform
    {
        /// <inheritdoc/>
        public float Scale { get; }

        /// <summary>
        /// A <see cref="Vector2"/> indicating the amount of translation.
        /// </summary>
        public Vector2 Amount { get; set; }

        /// <summary>
        /// Creates a new <see cref="Translation"/>.
        /// </summary>
        /// <param name="amount">A <see cref="Vector2"/> indicating the amount of translation.</param>
        public Translation(Vector2 amount)
        {
            Scale = 1;
            Amount = amount;
        }

        /// <inheritdoc/>
        public Vector2 TransformPoint(Vector2 drawPoint)
        {
            return drawPoint + Amount;
        }

        /// <inheritdoc/>
        public Vector2 RetreivePoint(Vector2 viewPoint)
        {
            return viewPoint - Amount;
        }
    }

    /// <summary>
    /// Represents a linear movement by the specified amount.
    /// </summary>
    public struct Scaling : ITransform
    {
        /// <inheritdoc/>
        public float Scale { get; set; }

        /// <summary>
        /// A <see cref="bool"/> indicating whether the <see cref="Scale"/> should be flipped along the vertical axis.
        /// </summary>
        public bool FlipVertical { get; set; }

        /// <summary>
        /// The center about where the <see cref="Scaling"/> is applied.
        /// </summary>
        public Vector2 Center { get; set; }

        /// <summary>
        /// The constant <see cref="Vector2"/> used to flip the y-axis of a given <see cref="Vector2"/> (by multiplication).
        /// </summary>
        public static Vector2 FlipConstant { get; } = new Vector2(1, -1);

        /// <summary>
        /// Creates a new <see cref="Translation"/> about (0,0).
        /// </summary>
        /// <param name="amount">The amount of scaling to apply to points and elements.</param>
        /// <param name="flipVertical">A <see cref="bool"/> indicating whether the <see cref="Scale"/> should be flipped along the vertical axis.</param>
        public Scaling(float amount, bool flipVertical = false)
        {
            Scale = amount;
            Center = Vector2.Zero;
            FlipVertical = flipVertical;
        }

        /// <summary>
        /// Creates a new <see cref="Translation"/>.
        /// </summary>
        /// <param name="amount">The amount of scaling to apply to points and elements.</param>
        /// <param name="center">The center about where the <see cref="Scaling"/> is applied.</param>
        /// <param name="flipVertical">A <see cref="bool"/> indicating whether the <see cref="Scale"/> should be flipped along the vertical axis.</param>
        public Scaling(float amount, Vector2 center, bool flipVertical = false)
        {
            Scale = amount;
            Center = center;
            FlipVertical = flipVertical;
        }

        /// <inheritdoc/>
        public Vector2 TransformPoint(Vector2 drawPoint)
        {
            if (FlipVertical)
            {
                return ((drawPoint - Center) * (new Vector2(Scale) * FlipConstant)) + Center;
            }
            else
            {
                return ((drawPoint - Center) * new Vector2(Scale)) + Center;
            }
        }

        /// <inheritdoc/>
        public Vector2 RetreivePoint(Vector2 viewPoint)
        {
            if (FlipVertical)
            {
                return ((viewPoint - Center) / (new Vector2(Scale) * FlipConstant)) + Center;
            }
            else
            {
                return ((viewPoint - Center) / new Vector2(Scale)) + Center;
            }
        }
    }
}
