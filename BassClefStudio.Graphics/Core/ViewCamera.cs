using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Core
{
    /// <summary>
    /// Represents a means of transforming graphics co-ordinates into view-coordinates while drawing to an <see cref="IGraphicsProvider"/>.
    /// </summary>
    public struct ViewCamera
    {
        /// <summary>
        /// The <see cref="float"/> scale of all items drawn to the canvas.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// A <see cref="Vector2"/> indicating where the origin of the view should be in drawing co-ordinates (i.e. the position of the 'camera').
        /// </summary>
        public Vector2 Translation { get; set; }

        /// <summary>
        /// A <see cref="CoordinateStyle"/> value indicating where the origin and co-ordinate axes should be placed.
        /// </summary>
        public CoordinateStyle OriginType { get; set; }

        /// <summary>
        /// A <see cref="ViewCamera"/> that maps view points directly from drawing points in a <see cref="CoordinateStyle.Center"/> plane.
        /// </summary>
        public static ViewCamera IdentityCenter { get; } = new ViewCamera(1, new Vector2(0, 0), CoordinateStyle.Center);

        /// <summary>
        /// A <see cref="ViewCamera"/> that maps view points directly from drawing points in a <see cref="CoordinateStyle.TopLeft"/> plane.
        /// </summary>
        public static ViewCamera Identity { get; } = new ViewCamera(1, new Vector2(0, 0), CoordinateStyle.TopLeft);

        /// <summary>
        /// Creates a new <see cref="ViewCamera"/> with the specified properties.
        /// </summary>
        /// <param name="scale">The <see cref="float"/> scale of all items drawn to the canvas.</param>
        /// <param name="translation">A <see cref="Vector2"/> indicating where the origin of the view should be in drawing co-ordinates (i.e. the position of the 'camera').</param>
        /// <param name="originType">A <see cref="CoordinateStyle"/> value indicating where the origin and co-ordinate axes should be placed.</param>
        public ViewCamera(float scale, Vector2 translation, CoordinateStyle originType = CoordinateStyle.TopLeft)
        {
            Scale = scale;
            Translation = translation;
            OriginType = originType;
        }

        /// <summary>
        /// Creates a new <see cref="ViewCamera"/> to zoom (see <see cref="GetZoomFactor(Vector2, Vector2, ZoomType)"/>) between a given drawing- and view-space.
        /// </summary>
        /// <param name="viewSpace">The size of the view-space, as a <see cref="Vector2"/>.</param>
        /// <param name="drawSpace">The size of the drawing-space, as a <see cref="Vector2"/>.</param>
        /// <param name="zoomType">A <see cref="ZoomType"/> value indicating the behavior of the zooming.</param>
        /// <param name="originType">A <see cref="CoordinateStyle"/> value indicating where the origin and co-ordinate axes should be placed. This constructor sets the camera's <see cref="Translation"/> to place the origin relative to the <paramref name="viewSpace"/>.</param>
        public ViewCamera(Vector2 viewSpace, Vector2 drawSpace, ZoomType zoomType = ZoomType.FitAll, CoordinateStyle originType = CoordinateStyle.TopLeft)
        {
            Scale = GetZoomFactor(viewSpace, drawSpace, zoomType);
            OriginType = originType;
            Vector2 centerPoint = viewSpace / 2;
            if (OriginType == CoordinateStyle.Center)
            {
                Translation = centerPoint;
            }
            else
            {
                Translation = centerPoint - (drawSpace / 2);
            }
        }

        /// <summary>
        /// Returns the <see cref="float"/> scale-factor required to scale the <paramref name="drawSpace"/> into the <paramref name="viewSpace"/> with the desired zoom behavior.
        /// </summary>
        /// <param name="viewSpace">The size of the view-space, as a <see cref="Vector2"/>.</param>
        /// <param name="drawSpace">The size of the drawing-space, as a <see cref="Vector2"/>.</param>
        /// <param name="zoomType">A <see cref="ZoomType"/> value indicating the behavior of the zooming.</param>
        /// <returns>The <see cref="float"/> scale factor a <see cref="ViewCamera"/> would need to scale between these spaces.</returns>
        public static float GetZoomFactor(Vector2 viewSpace, Vector2 drawSpace, ZoomType zoomType = ZoomType.FitAll)
        {
            float xRatio = (viewSpace.X / drawSpace.X);
            float yRatio = (viewSpace.Y / drawSpace.Y);
            if (zoomType == ZoomType.FitAll && xRatio > yRatio
                || zoomType == ZoomType.FillView && yRatio > xRatio)
            {
                return yRatio;
            }
            else
            {
                return xRatio;
            }
        }

        /// <summary>
        /// Gets an (x,y) scale <see cref="Vector2"/> from this <see cref="ViewCamera"/>'s <see cref="Scale"/> and <see cref="OriginType"/>.
        /// </summary>
        /// <returns>A <see cref="Vector2"/> scale factor.</returns>
        public Vector2 GetScale()
        {
            if(OriginType == CoordinateStyle.TopLeft)
            {
                return new Vector2(Scale);
            }
            else
            {
                return new Vector2(Scale, -Scale);
            }
        }

        /// <summary>
        /// Transforms a <see cref="Vector2"/> in drawing space to a <see cref="Vector2"/> in view space using this <see cref="ViewCamera"/>. The point is first translated, then scaled (a behavior all implementations of the graphics library are expected to follow).
        /// </summary>
        /// <param name="drawPoint">The point in drawing (graphics) space.</param>
        /// <returns>The point relative to the view.</returns>
        public Vector2 TransformPoint(Vector2 drawPoint)
        {
            return (drawPoint - Translation) * GetScale();
        }
    }
}
