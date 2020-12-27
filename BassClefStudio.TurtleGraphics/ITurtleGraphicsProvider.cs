using BassClefStudio.NET.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.TurtleGraphics
{
    /// <summary>
    /// Represents a drawable surface (either in an app view, or in a file) on which drawing commands can be executed.
    /// </summary>
    public interface ITurtleGraphicsProvider
    {
        /// <summary>
        /// The current scale of the drawing surface.
        /// </summary>
        float Scale { get; }

        /// <summary>
        /// The effective size (in drawing co-ordinates) of the drawing area, as a <see cref="Vector2"/>.
        /// </summary>
        Vector2 EffectiveSize { get; }

        /// <summary>
        /// Clears the drawing area with a specified <see cref="Color"/>.
        /// </summary>
        /// <param name="baseColor">The background color to clear the surface with.</param>
        void Clear(Color baseColor);

        /// <summary>
        /// Sets the view of the <see cref="ITurtleGraphicsProvider"/>.
        /// </summary>
        /// <param name="viewSize">The size, in view co-ordinates, of the available area.</param>
        /// <param name="desiredSize">The desired size of the drawing area, which will be scaled as drawing co-ordinates.</param>
        /// <param name="zoomType">A <see cref="ZoomType"/> value indicating how the <see cref="ITurtleGraphicsProvider"/> should set the <see cref="Scale"/> of the drawing area.</param>
        /// <param name="coordinateStyle">Sets the <see cref="CoordinateStyle"/> defining how points in drawing space map to the view space, and where the origin should be located.</param>
        void SetView(Vector2 viewSize, Vector2 desiredSize, ZoomType zoomType = ZoomType.FitAll, CoordinateStyle coordinateStyle = CoordinateStyle.TopLeft);

        /// <summary>
        /// The <see cref="float"/> size (diameter, in drawing co-ordinates) of the pen used to draw lines and outlines.
        /// </summary>
        float PenSize { get; set; }

        /// <summary>
        /// The current <see cref="Color"/> of the pen used to draw lines and outlines.
        /// </summary>
        Color PenColor { get; set; }

        /// <summary>
        /// Draws a single line stroke between two points.
        /// </summary>
        /// <param name="start">The <see cref="Vector2"/> start of the line.</param>
        /// <param name="end">The <see cref="Vector2"/> end of the line.</param>
        /// <param name="penColor">Override the <see cref="PenColor"/> of this stroke.</param>
        /// <param name="penSize">Override the <see cref="PenSize"/> of this stroke.</param>
        void DrawLine(Vector2 start, Vector2 end, Color? penColor = null, float? penSize = null);

        /// <summary>
        /// Draws the outline of a polygon with the pen between two or more points.
        /// </summary>
        /// <param name="points">An array of <see cref="Vector2"/> vertices - must be at least 2 in the collection (exactly 2 vertices calls <see cref="DrawLine(Vector2, Vector2, Color?, float?)"/>).</param>
        /// <param name="penColor">Override the <see cref="PenColor"/> of this stroke.</param>
        /// <param name="penSize">Override the <see cref="PenSize"/> of this stroke.</param>
        void DrawPolygon(Vector2[] points, Color? penColor = null, float? penSize = null);

        /// <summary>
        /// Fills the inside of a polygon with the pen between two or more points.
        /// </summary>
        /// <param name="points">An array of <see cref="Vector2"/> vertices - must be at least 2 in the collection (exactly 2 vertices calls <see cref="DrawLine(Vector2, Vector2, Color?, float?)"/>).</param>
        /// <param name="penColor">Override the <see cref="PenColor"/> of this stroke.</param>
        void FillPolygon(Vector2[] points, Color? penColor = null);

        /// <summary>
        /// Flushes any unregistered changes to the <see cref="ITurtleGraphicsProvider"/> to the app view or file.
        /// </summary>
        Task FlushAsync();
    }
}
