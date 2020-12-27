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
        /// Returns the SVG's XML, as a <see cref="string"/>.
        /// </summary>
        string ToXml();
    }

    /// <summary>
    /// Represents the most basic <see cref="ISvgDocument"/> - a <see cref="string"/> of SVG text that can be loaded into a graphics system on draw.
    /// </summary>
    public class BaseSvgDocument : ISvgDocument
    {
        /// <summary>
        /// The XML text of this SVG file.
        /// </summary>
        public string Xml { get; }

        /// <summary>
        /// Creates a new <see cref="BaseSvgDocument"/>.
        /// </summary>
        /// <param name="xml">The XML text of this SVG file.</param>
        public BaseSvgDocument(string xml)
        {
            Xml = xml;
        }
        
        /// <inheritdoc/>
        public string ToXml()
        {
            return Xml;
        }
    }
}
