using BassClefStudio.NET.Core.Primitives;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.TurtleGraphics.Win2D
{
    /// <summary>
    /// Represents a Win2D implementation of <see cref="ITurtleGraphicsProvider"/> that draws to a <see cref="CanvasDrawingSession"/>.
    /// </summary>
    public class Win2DTurtleGraphicsProvider : ITurtleGraphicsProvider
    {
        /// <inheritdoc/>
        public float Scale { get; private set; } = 1;

        /// <inheritdoc/>
        public Vector2 EffectiveSize { get; private set; }

        /// <inheritdoc/>
        public float PenSize { get; set; }

        /// <inheritdoc/>
        public Color PenColor { get; set; }

        /// <inheritdoc/>
        public PenType PenType { get; set; }

        /// <summary>
        /// The <see cref="CanvasDrawingSession"/> that this <see cref="Win2DTurtleGraphicsProvider"/> uses to execute drawing commands.
        /// </summary>
        public CanvasDrawingSession DrawingSession { get; }

        /// <summary>
        /// Creates a new <see cref="Win2DTurtleGraphicsProvider"/> to handle the given <see cref="CanvasDrawingSession"/>.
        /// </summary>
        /// <param name="drawingSession">The <see cref="CanvasDrawingSession"/> that this <see cref="Win2DTurtleGraphicsProvider"/> uses to execute drawing commands.</param>
        public Win2DTurtleGraphicsProvider(CanvasDrawingSession drawingSession)
        {
            if(drawingSession == null)
            {
                throw new ArgumentNullException("The CanvasDrawingSession managed by this ITurtleGraphicsProvider cannot be null.");
            }

            DrawingSession = drawingSession;
        }

        /// <inheritdoc/>
        public void Clear(Color baseColor)
        {
            DrawingSession.Clear(baseColor.GetColor());
        }

        /// <inheritdoc/>
        public void SetView(Vector2 viewSize, Vector2 desiredSize, ZoomType zoomType = ZoomType.FitAll, CoordinateStyle coordinateStyle = CoordinateStyle.TopLeft)
        {
            float xRatio = (viewSize.X / desiredSize.X);
            float yRatio = (viewSize.Y / desiredSize.Y);
            if(zoomType == ZoomType.FitAll && xRatio > yRatio
                || zoomType == ZoomType.FillView && yRatio > xRatio)
            {
                Scale = yRatio;
            }
            else
            {
                Scale = xRatio;
            }

            EffectiveSize = desiredSize;
            SetTransform(Scale, EffectiveSize * Scale, viewSize / 2, coordinateStyle);
        }

        private void SetTransform(float scale, Vector2 drawSize, Vector2 centerPoint, CoordinateStyle coordinateStyle)
        {
            Matrix3x2 transform = Matrix3x2.Identity;
            if(coordinateStyle == CoordinateStyle.TopLeft)
            {
                transform = Matrix3x2.CreateScale(scale, scale);
                transform.Translation = centerPoint - (drawSize / 2);
            }
            else if(coordinateStyle == CoordinateStyle.Center)
            {
                transform = Matrix3x2.CreateScale(scale, -scale);
                transform.Translation = centerPoint;
            }
            DrawingSession.Transform = transform;
        }

        /// <inheritdoc/>
        public void DrawLine(Vector2 start, Vector2 end, Color? penColor = null, float? penSize = null, PenType? penType = null)
        {
            DrawingSession.DrawLine(start, end, (penColor ?? PenColor).GetColor(), (penSize ?? PenSize));
            if((penType ?? PenType) == PenType.Round)
            {
                FillEllipse(start, new Vector2((penSize ?? PenSize) / 2), penColor);
                FillEllipse(end, new Vector2((penSize ?? PenSize) / 2), penColor);
            }
        }

        /// <inheritdoc/>
        public void DrawPolygon(Vector2[] points, Color? penColor = null, float? penSize = null)
        {
            if (points.Length <= 1)
            {
                throw new ArgumentException("Cannot create a polygon geometry without two or more points.");
            }
            else if(points.Length == 2)
            {
                DrawLine(points[0], points[1], penColor, penSize);
            }
            else
            {
                var geometry = CanvasGeometry.CreatePolygon(DrawingSession, points);
                DrawingSession.DrawGeometry(geometry, (penColor ?? PenColor).GetColor(), (penSize ?? PenSize));
            }
        }

        /// <inheritdoc/>
        public void DrawEllipse(Vector2 center, Vector2 radii, Color? penColor = null, float? penSize = null)
        {
            DrawingSession.DrawEllipse(center, radii.X, radii.Y, (penColor ?? PenColor).GetColor());
        }

        /// <inheritdoc/>
        public void FillPolygon(Vector2[] points, Color? penColor = null)
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
        public void FillEllipse(Vector2 center, Vector2 radii, Color? penColor = null)
        {
            DrawingSession.FillEllipse(center, radii.X, radii.Y, (penColor ?? PenColor).GetColor());
        }

        /// <inheritdoc/>
        public async Task FlushAsync()
        {
            DrawingSession.Flush();
        }
    }

    internal static class ColorExtensions
    {
        public static Windows.UI.Color GetColor(this Color color)
        {
            return Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
