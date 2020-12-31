using BassClefStudio.Graphics.Core;
using BassClefStudio.Graphics.Svg;
using BassClefStudio.Graphics.Turtle;
using BassClefStudio.NET.Core.Primitives;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Svg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.Graphics.Core
{
    /// <summary>
    /// Represents a Win2D implementation of <see cref="IGraphicsProvider"/> that draws to a <see cref="CanvasDrawingSession"/>.
    /// </summary>
    public class Win2DGraphicsProvider : ITurtleGraphicsProvider, ISvgGraphicsProvider
    {
        #region Win2D

        /// <summary>
        /// The <see cref="CanvasDrawingSession"/> that this <see cref="Win2DGraphicsProvider"/> uses to execute drawing commands.
        /// </summary>
        public CanvasDrawingSession DrawingSession { get; }

        /// <summary>
        /// Creates a new <see cref="Win2DGraphicsProvider"/> to handle the given <see cref="CanvasDrawingSession"/>.
        /// </summary>
        /// <param name="drawingSession">The <see cref="CanvasDrawingSession"/> that this <see cref="Win2DGraphicsProvider"/> uses to execute drawing commands.</param>
        public Win2DGraphicsProvider(CanvasDrawingSession drawingSession)
        {
            if (drawingSession == null)
            {
                throw new ArgumentNullException("The CanvasDrawingSession managed by this ITurtleGraphicsProvider cannot be null.");
            }

            DrawingSession = drawingSession;
        }

        #endregion
        #region SharedGraphics

        private ViewCamera camera = ViewCamera.Identity;
        /// <inheritdoc/>
        public ViewCamera Camera { get => camera; set { camera = value; } }

        /// <inheritdoc/>
        public void Clear(Color baseColor)
        {
            DrawingSession.Clear(baseColor.GetColor());
        }

        /// <inheritdoc/>
        public async Task FlushAsync()
        {
            DrawingSession.Flush();
        }

        #endregion
        #region Svg

        /// <inheritdoc/>
        public void DrawSvg(ISvgDocument svgDocument, Vector2 size, Vector2 location) => DrawSvgInternal(svgDocument, Camera.Scale * size, Camera.GetViewPoint(location));
        private void DrawSvgInternal(ISvgDocument svgDocument, Vector2 size, Vector2 location)
        {
            if (svgDocument is Win2DSvgDocument svg)
            {
                DrawingSession.DrawSvg(svg.SvgDocument, size.ToSize(), location);
            }
            else
            {
                throw new ArgumentException($"Cannot read SVG document of type {svgDocument?.GetType().Name}");
            }
        }

        #endregion
        #region Turtle

        private float penSize;
        /// <inheritdoc/>
        public float PenSize { get => penSize * Camera.Scale; set => penSize = value; }

        /// <inheritdoc/>
        public Color PenColor { get; set; } = new Color(255, 255, 255);

        /// <inheritdoc/>
        public PenType PenType { get; set; } = PenType.Round;

        /// <inheritdoc/>
        public void DrawLine(Vector2 start, Vector2 end, Color? penColor = null, float? penSize = null, PenType? penType = null) => DrawLineInternal(Camera.GetViewPoint(start), Camera.GetViewPoint(end), penColor, Camera.Scale * penSize, penType);
        private void DrawLineInternal(Vector2 start, Vector2 end, Color? penColor = null, float? penSize = null, PenType? penType = null)
        {
            DrawingSession.DrawLine(start, end, (penColor ?? PenColor).GetColor(), (penSize ?? PenSize));
            if ((penType ?? PenType) == PenType.Round)
            {
                FillEllipseInternal(start, new Vector2((penSize ?? PenSize) / 2), penColor);
                FillEllipseInternal(end, new Vector2((penSize ?? PenSize) / 2), penColor);
            }
        }

        /// <inheritdoc/>
        public void DrawPolygon(Vector2[] points, Color? penColor = null, float? penSize = null) => DrawPolygonInternal(points.Select(p => Camera.GetViewPoint(p)).ToArray(), penColor, Camera.Scale * penSize);
        private void DrawPolygonInternal(Vector2[] points, Color? penColor = null, float? penSize = null)
        {
            if (points.Length <= 1)
            {
                throw new ArgumentException("Cannot create a polygon geometry without two or more points.");
            }
            else if (points.Length == 2)
            {
                DrawLineInternal(points[0], points[1], penColor, penSize);
            }
            else
            {
                var geometry = CanvasGeometry.CreatePolygon(DrawingSession, points);
                DrawingSession.DrawGeometry(geometry, (penColor ?? PenColor).GetColor(), (penSize ?? PenSize));
            }
        }

        /// <inheritdoc/>
        public void DrawEllipse(Vector2 center, Vector2 radii, Color? penColor = null, float? penSize = null) => DrawEllipseInternal(Camera.GetViewPoint(center), Camera.Scale * radii, penColor, Camera.Scale * penSize);
        private void DrawEllipseInternal(Vector2 center, Vector2 radii, Color? penColor = null, float? penSize = null)
        {
            DrawingSession.DrawEllipse(center, radii.X, radii.Y, (penColor ?? PenColor).GetColor(), (penSize ?? PenSize));
        }

        /// <inheritdoc/>
        public void FillPolygon(Vector2[] points, Color? penColor = null) => FillPolygonInternal(points.Select(p => Camera.GetViewPoint(p)).ToArray(), penColor);
        private void FillPolygonInternal(Vector2[] points, Color? penColor = null)
        {
            if (points.Length <= 2)
            {
                throw new ArgumentException("Cannot fill a polygon geometry without three or more points.");
            }
            else
            {
                var geometry = CanvasGeometry.CreatePolygon(DrawingSession, points);
                DrawingSession.FillGeometry(geometry, (penColor ?? PenColor).GetColor());
            }
        }

        /// <inheritdoc/>
        public void FillEllipse(Vector2 center, Vector2 radii, Color? penColor = null) => FillEllipseInternal(Camera.GetViewPoint(center), Camera.Scale * radii, penColor);
        public void FillEllipseInternal(Vector2 center, Vector2 radii, Color? penColor = null)
        {
            DrawingSession.FillEllipse(center, radii.X, radii.Y, (penColor ?? PenColor).GetColor());
        }

        #endregion
    }

    internal static class ColorExtensions
    {
        public static Windows.UI.Color GetColor(this Color color)
        {
            return Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
