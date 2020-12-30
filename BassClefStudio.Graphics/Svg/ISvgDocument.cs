using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.Graphics.Svg
{
    /// <summary>
    /// Represents an SVG file loaded into a graphics system.
    /// </summary>
    public interface ISvgDocument
    {
        /// <summary>
        /// Returns the XML content of the SVG file this <see cref="ISvgDocument"/> represents.
        /// </summary>
        /// <returns>A <see cref="string"/> block of XML content.</returns>
        string GetXml();
    }
}
