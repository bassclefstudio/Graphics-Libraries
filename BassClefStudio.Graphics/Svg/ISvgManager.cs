using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.Graphics.Svg
{
    /// <summary>
    /// Represents a service managed by a graphics system for loading SVG graphics.
    /// </summary>
    public interface ISvgManager
    {
        /// <summary>
        /// Creates an <see cref="ISvgDocument"/> asynchronously from a SVG file.
        /// </summary>
        /// <param name="fileStream">A stream containing the SVG content, either from a file or other streamable source.</param>
        /// <returns>An <see cref="ISvgDocument"/> that can be drawn by the graphics system at a later point.</returns>
        Task<ISvgDocument> LoadAsync(Stream fileStream);

        /// <summary>
        /// Creates an <see cref="ISvgDocument"/> from a SVG file.
        /// </summary>
        /// <param name="xml">The XML content of the SVG graphic.</param>
        ISvgDocument Load(string xml);
    }
}
