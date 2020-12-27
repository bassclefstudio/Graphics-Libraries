using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.Graphics.Core
{
    /// <summary>
    /// Represents a drawable surface (either in an app <see cref="IGraphicsView"/>, or in a file) on which graphics commands can be executed.
    /// </summary>
    public interface IGraphicsProvider
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
        /// Sets the view of the <see cref="IGraphicsProvider"/>.
        /// </summary>
        /// <param name="viewSize">The size, in view co-ordinates, of the available area.</param>
        /// <param name="desiredSize">The desired size of the drawing area, which will be scaled as drawing co-ordinates.</param>
        /// <param name="zoomType">A <see cref="ZoomType"/> value indicating how the <see cref="IGraphicsProvider"/> should set the <see cref="Scale"/> of the drawing area.</param>
        /// <param name="coordinateStyle">Sets the <see cref="CoordinateStyle"/> defining how points in drawing space map to the view space, and where the origin should be located.</param>
        void SetView(Vector2 viewSize, Vector2 desiredSize, ZoomType zoomType = ZoomType.FitAll, CoordinateStyle coordinateStyle = CoordinateStyle.TopLeft);

        /// <summary>
        /// Flushes any unregistered changes to the <see cref="IGraphicsProvider"/> to the app view or file.
        /// </summary>
        Task FlushAsync();
    }
}
