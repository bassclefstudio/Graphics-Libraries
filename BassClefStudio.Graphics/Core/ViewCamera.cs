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
    public class ViewCamera
    {
        #region Properties

        /// <summary>
        /// A collection of child <see cref="ITransform"/> transformations, in the order they will be applied.
        /// </summary>
        public IEnumerable<ITransform> Transforms { get; }

        /// <summary>
        /// Represents the scale of static UI elements, calculated from the <see cref="Transforms"/> collection.
        /// </summary>
        public float Scale => Transforms.Aggregate(1f, (s, cam) => s * cam.Scale);

        #endregion
        #region Constants

        /// <summary>
        /// A static identity <see cref="ViewCamera"/> - one whose input and output points are the same.
        /// </summary>
        public static ViewCamera Identity { get; } = new ViewCamera();

        /// <summary>
        /// A static identity <see cref="ViewCamera"/> - one whose input and output points are the same - but with the y-axis flipped (mathematical-style).
        /// </summary>
        public static ViewCamera IdentityFlip { get; } = new ViewCamera(new Scaling(1, true));

        #endregion
        #region Initialization

        /// <summary>
        /// Creates an identity <see cref="ViewCamera"/> - one whose input and output points are the same.
        /// </summary>
        public ViewCamera()
        {
            Transforms = new ITransform[0];
        }

        /// <summary>
        /// Creates a new <see cref="ViewCamera"/>.
        /// </summary>
        /// <param name="transforms">A collection of child <see cref="ITransform"/> transformations, in the order they will be applied.</param>
        public ViewCamera(IEnumerable<ITransform> transforms)
        {
            Transforms = transforms;
        }

        /// <summary>
        /// Creates a new <see cref="ViewCamera"/>.
        /// </summary>
        /// <param name="transforms">An array of child <see cref="ITransform"/> transformations, in the order they will be applied.</param>
        public ViewCamera(params ITransform[] transforms)
        {
            Transforms = transforms;
        }

        /// <summary>
        /// Creates a basic <see cref="ViewCamera"/> with a <see cref="Translation"/> and a <see cref="Scaling"/> element.
        /// </summary>
        /// <param name="scaling">The <see cref="float"/> scaling factor about the origin.</param>
        /// <param name="translation">The 'position' of the <see cref="ViewCamera"/> in the space - equivalent to a negative <see cref="Translation"/>.</param>
        public ViewCamera(float scaling, Vector2 translation)
        {
            Transforms = new ITransform[]
            {
                new Translation(-translation),
                new Scaling(scaling)
            };
        }

        /// <summary>
        /// Creates a new <see cref="ViewCamera"/> to zoom (see <see cref="GetZoomFactor(Vector2, Vector2, ZoomType)"/>) between a given drawing- and view-space.
        /// </summary>
        /// <param name="viewSpace">The size of the view-space, as a <see cref="Vector2"/>.</param>
        /// <param name="drawSpace">The size of the drawing-space, as a <see cref="Vector2"/>.</param>
        /// <param name="zoomType">A <see cref="ZoomType"/> value indicating the behavior of the zooming.</param>
        /// <param name="isCentered">A <see cref="bool"/> value indicating that the origin and co-ordinate axes are math-style, and adds a translation to place the origin in the center of the screen.</param>
        public ViewCamera(Vector2 viewSpace, Vector2 drawSpace, ZoomType zoomType = ZoomType.FitAll, bool isCentered = false)
        {
            if (isCentered)
            {
                Transforms = new ITransform[]
                {
                    new Translation(viewSpace / 2),
                    new Scaling(GetZoomFactor(viewSpace, drawSpace, zoomType), viewSpace / 2, true)
                };
            }
            else
            {
                Transforms = new ITransform[]
                {
                    new Scaling(GetZoomFactor(viewSpace, drawSpace, zoomType))
                };
            }
        }

        #endregion
        #region GetPoints

        /// <inheritdoc/>
        public Vector2 GetViewPoint(Vector2 drawPoint)
        {
            Vector2 currentPt = drawPoint;
            foreach (var t in Transforms)
            {
                currentPt = t.TransformPoint(currentPt);
            }
            return currentPt;
        }

        #endregion
        #region Arithmetic

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
        /// Adds the effects of two <see cref="ViewCamera"/>s together.
        /// </summary>
        /// <param name="a">The first <see cref="ViewCamera"/>.</param>
        /// <param name="b">The second <see cref="ViewCamera"/>.</param>
        /// <returns>A <see cref="ViewCamera"/> whose resulting transform of <see cref="Vector2"/> points is equivalent to applying the <paramref name="a"/> and then <paramref name="b"/> <see cref="ViewCamera"/>s to that same point.</returns>
        public static ViewCamera operator + (ViewCamera a, ViewCamera b)
        {
            return new ViewCamera(a.Transforms.Concat(b.Transforms).ToArray());
        }

        /// <summary>
        /// Determines if two <see cref="ViewCamera"/>s have the same effect.
        /// </summary>
        /// <param name="a">The first <see cref="ViewCamera"/>.</param>
        /// <param name="b">The second <see cref="ViewCamera"/>.</param>
        /// <returns>A <see cref="bool"/> indicating whether the cameras are the same.</returns>
        public static bool operator ==(ViewCamera a, ViewCamera b)
        {
            return a.Transforms.SequenceEqual(b.Transforms);
        }

        /// <summary>
        /// Determines if two <see cref="ViewCamera"/>s have different effects.
        /// </summary>
        /// <param name="a">The first <see cref="ViewCamera"/>.</param>
        /// <param name="b">The second <see cref="ViewCamera"/>.</param>
        /// <returns>A <see cref="bool"/> indicating whether the cameras are not the same (not equal).</returns>
        public static bool operator !=(ViewCamera a, ViewCamera b)
        {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is ViewCamera cam
                && this == cam;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Transforms.GetHashCode();
        }

        #endregion
    }
}
