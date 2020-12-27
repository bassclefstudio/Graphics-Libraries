using BassClefStudio.Graphics.Core;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// <param name="document">The SVG document, as an <see cref="ISvgDocument"/> that can be read by the underlying graphics system.</param>
        void DrawSvg(ISvgDocument document);
    }
}
