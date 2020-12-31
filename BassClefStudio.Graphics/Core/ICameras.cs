using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Core
{
    /// <summary>
    /// Represents a means of transforming graphics co-ordinates into view-coordinates while drawing to an <see cref="IGraphicsProvider"/>.
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// Transforms a <see cref="Vector2"/> in drawing-space to a <see cref="Vector2"/> in view-space using this <see cref="ViewCamera"/>. The point is first translated, then scaled (a behavior all implementations of the graphics library are expected to follow).
        /// </summary>
        /// <param name="drawPoint">The point in drawing (graphics) space.</param>
        /// <returns>The point relative to the view.</returns>
        Vector2 TransformPoint(Vector2 drawPoint);

        /// <summary>
        /// Gets a <see cref="float"/> representing the ratio of the scale in drawing-space to view-space.
        /// </summary>
        float Scale { get; }
    }

    /// <summary>
    /// Represents the most basic <see cref="ICamera"/> - one which translates and scales a given <see cref="Vector2"/>.
    /// </summary>
    public class ViewCamera : ICamera
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
        /// A <see cref="bool"/> value indicating that the origin and co-ordinate axes are math-style (y-axis pointing upwards), rather than computer-style (top-left, with y-axis downwards).
        /// </summary>
        public bool FlipVertical { get; set; }

        /// <summary>
        /// A <see cref="ViewCamera"/> that maps view points directly from drawing points in a math-style plane (see <see cref="FlipVertical"/>).
        /// </summary>
        public static ViewCamera IdentityCenter { get; } = new ViewCamera(1, new Vector2(0, 0), true);

        /// <summary>
        /// A <see cref="ViewCamera"/> that maps view points directly from drawing points.
        /// </summary>
        public static ViewCamera Identity { get; } = new ViewCamera(1, new Vector2(0, 0));

        /// <summary>
        /// Represents the center constant <see cref="Vector2"/> - when multiplied to an existing <see cref="Vector2"/>, it applies the <see cref="FlipVertical"/> transform.
        /// </summary>
        public static Vector2 CenterConstant { get; } = new Vector2(1, -1);

        /// <summary>
        /// Creates a new <see cref="ViewCamera"/> with the specified properties.
        /// </summary>
        /// <param name="scale">The <see cref="float"/> scale of all items drawn to the canvas.</param>
        /// <param name="translation">A <see cref="Vector2"/> indicating where the origin of the view should be in drawing co-ordinates (i.e. the position of the 'camera').</param>
        /// <param name="flipVertical">A <see cref="bool"/> value indicating that the origin and co-ordinate axes are math-style (y-axis pointing upwards), rather than computer-style (top-left, with y-axis downwards).</param>
        public ViewCamera(float scale, Vector2 translation, bool flipVertical = false)
        {
            Scale = scale;
            Translation = translation;
            FlipVertical = flipVertical;
        }

        /// <summary>
        /// Creates a new <see cref="ViewCamera"/> to zoom (see <see cref="GetZoomFactor(Vector2, Vector2, ZoomType)"/>) between a given drawing- and view-space.
        /// </summary>
        /// <param name="viewSpace">The size of the view-space, as a <see cref="Vector2"/>.</param>
        /// <param name="drawSpace">The size of the drawing-space, as a <see cref="Vector2"/>.</param>
        /// <param name="zoomType">A <see cref="ZoomType"/> value indicating the behavior of the zooming.</param>
        /// <param name="flipVertical">A <see cref="bool"/> value indicating that the origin and co-ordinate axes are math-style (y-axis pointing upwards), rather than computer-style (top-left, with y-axis downwards).</param>
        public ViewCamera(Vector2 viewSpace, Vector2 drawSpace, ZoomType zoomType = ZoomType.FitAll, bool flipVertical = false)
        {
            Scale = GetZoomFactor(viewSpace, drawSpace, zoomType);
            FlipVertical = flipVertical;
            Vector2 centerPoint = viewSpace / 2;
            if (FlipVertical)
            {
                Translation = -centerPoint;
            }
            else
            {
                Translation = (drawSpace / 2) - centerPoint;
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
            if (FlipVertical)
            {
                return new Vector2(Scale) * CenterConstant;
            }
            else
            {
                return new Vector2(Scale);
            }
        }

        /// <inheritdoc/>
        public Vector2 TransformPoint(Vector2 drawPoint)
        {
            return (drawPoint - Translation) * GetScale();
        }
    }
    
    /// <summary>
    /// Represents an <see cref="ICamera"/> whose transformation is the sum of a collection of other <see cref="ICamera"/> transformations.
    /// </summary>
    public class ComplexCamera : ICamera
    {
        /// <summary>
        /// A collection of child <see cref="ICamera"/> transformations, in the order they will be applied.
        /// </summary>
        public IEnumerable<ICamera> ChildCams { get; }

        /// <inheritdoc/>
        public float Scale => ChildCams.Aggregate(1f, (s, cam) => s * cam.Scale);

        /// <summary>
        /// Creates a new <see cref="ComplexCamera"/>.
        /// </summary>
        /// <param name="childCams">A collection of child <see cref="ICamera"/> transformations, in the order they will be applied.</param>
        public ComplexCamera(IEnumerable<ICamera> childCams)
        {
            ChildCams = childCams;
        }

        /// <summary>
        /// Creates a new <see cref="ComplexCamera"/>.
        /// </summary>
        /// <param name="childCams">An array of child <see cref="ICamera"/> transformations, in the order they will be applied.</param>
        public ComplexCamera(params ICamera[] childCams)
        {
            ChildCams = childCams;
        }

        /// <inheritdoc/>
        public Vector2 TransformPoint(Vector2 drawPoint)
        {
            Vector2 currentPt = drawPoint;
            foreach (var cam in ChildCams)
            {
                currentPt = cam.TransformPoint(currentPt);
            }
            return currentPt;
        }
    }
}
