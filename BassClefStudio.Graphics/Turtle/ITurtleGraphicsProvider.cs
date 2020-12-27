using BassClefStudio.Graphics.Core;
using BassClefStudio.NET.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.Graphics.Turtle
{
    /// <summary>
    /// An <see cref="IGraphicsProvider"/> that supports 'turtle' graphics commands, and can be used to draw vector graphics.
    /// </summary>
    public interface ITurtleGraphicsProvider : IGraphicsProvider
    {
        /// <summary>
        /// Clears the drawing area with a specified <see cref="Color"/>.
        /// </summary>
        /// <param name="baseColor">The background color to clear the surface with.</param>
        void Clear(Color baseColor);

        /// <summary>
        /// The <see cref="float"/> size (diameter, in drawing co-ordinates) of the pen used to draw lines and outlines.
        /// </summary>
        float PenSize { get; set; }

        /// <summary>
        /// The current <see cref="Color"/> of the pen used to draw lines and outlines.
        /// </summary>
        Color PenColor { get; set; }

        /// <summary>
        /// The <see cref="PenType"/> value indicating how line ends should behave.
        /// </summary>
        PenType PenType { get; set; }

        /// <summary>
        /// Draws a single line stroke between two points.
        /// </summary>
        /// <param name="start">The <see cref="Vector2"/> start of the line.</param>
        /// <param name="end">The <see cref="Vector2"/> end of the line.</param>
        /// <param name="penColor">Override the <see cref="PenColor"/> of this stroke.</param>
        /// <param name="penSize">Override the <see cref="PenSize"/> of this stroke.</param>
        /// <param name="penType">Override the <see cref="PenType"/> of this stroke.</param>
        void DrawLine(Vector2 start, Vector2 end, Color? penColor = null, float? penSize = null, PenType? penType = null);

        /// <summary>
        /// Draws the outline of a polygon with the pen between two or more points.
        /// </summary>
        /// <param name="points">An array of <see cref="Vector2"/> vertices - must be at least 2 in the collection (exactly 2 vertices calls <see cref="DrawLine(Vector2, Vector2, Color?, float?, PenType?)"/>).</param>
        /// <param name="penColor">Override the <see cref="PenColor"/> of this stroke.</param>
        /// <param name="penSize">Override the <see cref="PenSize"/> of this stroke.</param>
        void DrawPolygon(Vector2[] points, Color? penColor = null, float? penSize = null);

        /// <summary>
        /// Draws the outline of an ellipse with the pen.
        /// </summary>
        /// <param name="center">The <see cref="Vector2"/> center of the ellipse.</param>
        /// <param name="radii">A <see cref="Vector2"/> indicating the radius in the x and y directions.</param>
        /// <param name="penColor">Override the <see cref="PenColor"/> of this stroke.</param>
        /// <param name="penSize">Override the <see cref="PenSize"/> of this stroke.</param>
        void DrawEllipse(Vector2 center, Vector2 radii, Color? penColor = null, float? penSize = null);

        /// <summary>
        /// Fills the inside of a polygon with the pen between two or more points.
        /// </summary>
        /// <param name="points">An array of <see cref="Vector2"/> vertices - must be at least 2 in the collection (exactly 2 vertices calls <see cref="DrawLine(Vector2, Vector2, Color?, float?, PenType?)"/>).</param>
        /// <param name="penColor">Override the <see cref="PenColor"/> of this stroke.</param>
        void FillPolygon(Vector2[] points, Color? penColor = null);

        /// <summary>
        /// Fills the inside of an ellipse with the pen.
        /// </summary>
        /// <param name="center">The <see cref="Vector2"/> center of the ellipse.</param>
        /// <param name="radii">A <see cref="Vector2"/> indicating the radius in the x and y directions.</param>
        /// <param name="penColor">Override the <see cref="PenColor"/> of this stroke.</param>
        void FillEllipse(Vector2 center, Vector2 radii, Color? penColor = null);
    }

    /// <summary>
    /// Provides extension methods for the <see cref="ITurtleGraphicsProvider"/> class.
    /// </summary>
    public static class GraphicsProviderExtensions
    {
        /// <summary>
        /// Draws the outline of a rectangle with the pen given two opposite points.
        /// </summary>
        /// <param name="graphics">The <see cref="ITurtleGraphicsProvider"/> drawing the shapes</param>
        /// <param name="point1">The first corner of the rectangle.</param>
        /// <param name="point2">An opposite corner of the rectangle.</param>
        /// <param name="penColor">Override the <see cref="ITurtleGraphicsProvider.PenColor"/> of this stroke.</param>
        /// <param name="penSize">Override the <see cref="ITurtleGraphicsProvider.PenSize"/> of this stroke.</param>
        public static void DrawRectangle(this ITurtleGraphicsProvider graphics, Vector2 point1, Vector2 point2, Color? penColor = null, float? penSize = null)
        {
            graphics.DrawPolygon(
                new Vector2[]
                {
                    point1,
                    new Vector2(point1.X, point2.Y),
                    point2,
                    new Vector2(point2.X, point1.Y)
                },
                penColor,
                penSize);
        }

        /// <summary>
        /// Draws the outline of a rectangle with the pen given a center and size.
        /// </summary>
        /// <param name="graphics">The <see cref="ITurtleGraphicsProvider"/> drawing the shapes</param>
        /// <param name="center">The center point of the rectangle.</param>
        /// <param name="width">The width (x) of the rectangle.</param>
        /// <param name="height">The height (y) of the rectangle.</param>
        /// <param name="penColor">Override the <see cref="ITurtleGraphicsProvider.PenColor"/> of this stroke.</param>
        /// <param name="penSize">Override the <see cref="ITurtleGraphicsProvider.PenSize"/> of this stroke.</param>
        public static void DrawRectangle(this ITurtleGraphicsProvider graphics, Vector2 center, float width, float height, Color? penColor = null, float? penSize = null)
        {
            Vector2 size = new Vector2(width, height);
            graphics.DrawPolygon(
                new Vector2[]
                {
                    center + size / 2,
                    center + size * new Vector2(-1, 1) / 2,
                    center - size / 2,
                    center + size * new Vector2(1, -1) / 2
                },
                penColor,
                penSize);
        }

        /// <summary>
        /// Draws the outline of a rectangle with the pen given two opposite points.
        /// </summary>
        /// <param name="graphics">The <see cref="ITurtleGraphicsProvider"/> drawing the shapes</param>
        /// <param name="point1">The first corner of the rectangle.</param>
        /// <param name="point2">An opposite corner of the rectangle.</param>
        /// <param name="penColor">Override the <see cref="ITurtleGraphicsProvider.PenColor"/> of this stroke.</param>
        public static void FillRectangle(this ITurtleGraphicsProvider graphics, Vector2 point1, Vector2 point2, Color? penColor = null)
        {
            graphics.FillPolygon(
                new Vector2[]
                {
                    point1,
                    new Vector2(point1.X, point2.Y),
                    point2,
                    new Vector2(point2.X, point1.Y)
                },
                penColor);
        }

        /// <summary>
        /// Draws the outline of a rectangle with the pen given a center and size.
        /// </summary>
        /// <param name="graphics">The <see cref="ITurtleGraphicsProvider"/> drawing the shapes</param>
        /// <param name="center">The center point of the rectangle.</param>
        /// <param name="width">The width (x) of the rectangle.</param>
        /// <param name="height">The height (y) of the rectangle.</param>
        /// <param name="penColor">Override the <see cref="ITurtleGraphicsProvider.PenColor"/> of this stroke.</param>
        public static void FillRectangle(this ITurtleGraphicsProvider graphics, Vector2 center, float width, float height, Color? penColor = null)
        {
            Vector2 size = new Vector2(width, height);
            graphics.FillPolygon(
                new Vector2[]
                {
                    center + size / 2,
                    center + size * new Vector2(-1, 1) / 2,
                    center - size / 2,
                    center + size * new Vector2(1, -1) / 2
                },
                penColor);
        }
    }
}
