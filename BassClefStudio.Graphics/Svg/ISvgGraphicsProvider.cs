using BassClefStudio.Graphics.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Svg
{
    /// <summary>
    /// An <see cref="IGraphicsProvider"/> that supports loading graphics from SVG files.
    /// </summary>
    public interface ISvgGraphicsProvider : IGraphicsProvider
    {
        /// <summary>
        /// Draws an SVG vector image onto the associated <see cref="IGraphicsView"/> or file.
        /// </summary>
        /// <param name="svgDocument">The SVG document as a loaded <see cref="ISvgDocument"/> that can be read by the underlying graphics system.</param>
        /// <param name="size">The <see cref="Vector2"/> size, in drawing-space, of the SVG viewport.</param>
        /// <param name="location">The <see cref="Vector2"/> location (center-based) of the SVG, in drawing-space.</param>
        void DrawSvg(ISvgDocument svgDocument, Vector2 size, Vector2 location);
    }
}
