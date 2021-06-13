using BassClefStudio.Graphics.Core;
using BassClefStudio.Graphics.Svg;
using BassClefStudio.Graphics.Transforms;
using BassClefStudio.Graphics.Turtle;
using BassClefStudio.NET.Core.Primitives;
using SkiaSharp;
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
    public class SkiaGraphicsProvider : ITurtleGraphicsProvider, ISvgGraphicsProvider
    {
        #region SkiaSharp

        /// <summary>
        /// The <see cref="SKCanvas"/> that this <see cref="SkiaGraphicsProvider"/> uses to execute drawing commands.
        /// </summary>
        public SKCanvas DrawingCanvas { get; }

        /// <summary>
        /// Creates a new <see cref="SkiaGraphicsProvider"/> to handle the given <see cref="DrawingCanvas"/>.
        /// </summary>
        /// <param name="drawingCanvas">The <see cref="SKCanvas"/> that this <see cref="SkiaGraphicsProvider"/> uses to execute drawing commands.</param>
        public SkiaGraphicsProvider(SKCanvas drawingCanvas)
        {
            if (drawingCanvas == null)
            {
                throw new ArgumentNullException("The SKCanvas managed by this ITurtleGraphicsProvider cannot be null.");
            }

            DrawingCanvas = drawingCanvas;
        }

        private List<SKPaint> Paints = new List<SKPaint>();
        private SKPaint GetPaint(Color color) => GetPaint(color, 0, true);
        private SKPaint GetPaint(Color color, float size) => GetPaint(color, size, false);
        private SKPaint GetPaint(Color color, float size, bool isFilled)
        {
            var paint = new SKPaint()
            {
                Color = color.GetColor(),
                Style = isFilled ? SKPaintStyle.Fill : SKPaintStyle.Stroke,
                StrokeCap = PenType.GetCap(),
                StrokeWidth = size,
            };
            Paints.Add(paint);
            return paint;
        }

        #endregion
        #region SharedGraphics

        private ViewCamera camera = ViewCamera.Identity;
        /// <inheritdoc/>
        public ViewCamera Camera { get => camera; set { camera = value; } }

        /// <inheritdoc/>
        public void Clear(Color baseColor)
        {
            DrawingCanvas.Clear(baseColor.GetColor());
        }

        /// <inheritdoc/>
        public async Task FlushAsync()
        {
            DrawingCanvas.Flush();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var paint in Paints)
            {
                paint.Dispose();
            }
            DrawingCanvas.Dispose();
        }

        #endregion
        #region Svg

        /// <inheritdoc/>
        public void DrawSvg(ISvgDocument svgDocument, Vector2 size, Vector2 location) => DrawSvgInternal(svgDocument, Camera.Scale * size, Camera.GetViewPoint(location));
        private void DrawSvgInternal(ISvgDocument svgDocument, Vector2 size, Vector2 location)
        {
            if (svgDocument is SkiaSvgDocument svg)
            {
                //// TODO: Determine if this is correct scaling procedure.
                DrawingCanvas.Scale(size.X, size.Y);
                DrawingCanvas.DrawPicture(svg.SvgDocument.Picture, location.X, location.Y);
                DrawingCanvas.ResetMatrix();
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
            DrawingCanvas.DrawLine(start.GetPoint(), end.GetPoint(), GetPaint((penColor ?? PenColor), (penSize ?? PenSize)));
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
                DrawingCanvas.DrawPoints(SKPointMode.Polygon, points.Select(p => p.GetPoint()).ToArray(), GetPaint((penColor ?? PenColor), (penSize ?? PenSize)));
            }
        }

        /// <inheritdoc/>
        public void DrawEllipse(Vector2 center, Vector2 radii, Color? penColor = null, float? penSize = null) => DrawEllipseInternal(Camera.GetViewPoint(center), Camera.Scale * radii, penColor, Camera.Scale * penSize);
        private void DrawEllipseInternal(Vector2 center, Vector2 radii, Color? penColor = null, float? penSize = null)
        {
            DrawingCanvas.DrawOval(center.X, center.Y, radii.X, radii.Y, GetPaint((penColor ?? PenColor), (penSize ?? PenSize)));
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
                DrawingCanvas.DrawPoints(SKPointMode.Polygon, points.Select(p => p.GetPoint()).ToArray(), GetPaint((penColor ?? PenColor)));
            }
        }

        /// <inheritdoc/>
        public void FillEllipse(Vector2 center, Vector2 radii, Color? penColor = null) => FillEllipseInternal(Camera.GetViewPoint(center), Camera.Scale * radii, penColor);
        public void FillEllipseInternal(Vector2 center, Vector2 radii, Color? penColor = null)
        {
            DrawingCanvas.DrawOval(center.X, center.Y, radii.X, radii.Y, GetPaint((penColor ?? PenColor)));
        }

        #endregion
    }

    internal static class SkiaExtensions
    {
        public static SKColor GetColor(this Color color)
        {
            return new SKColor(color.R, color.G, color.B, color.A);
        }

        public static SKStrokeCap GetCap(this PenType penType)
        {
            if(penType == PenType.Flat)
            {
                return SKStrokeCap.Butt;
            }
            else if (penType == PenType.Round)
            {
                return SKStrokeCap.Round;
            }
            else
            {
                throw new ArgumentException($"Unknown pen type: {penType}.", "penType");
            }
        }

        public static SKPoint GetPoint(this Vector2 vector)
        {
            return new SKPoint(vector.X, vector.Y);
        }
    }
}
