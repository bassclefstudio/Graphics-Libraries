using Microsoft.Graphics.Canvas.Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.Graphics.Svg
{
    /// <summary>
    /// Represents a Win2D loaded <see cref="ISvgDocument"/>.
    /// </summary>
    public class Win2DSvgDocument : ISvgDocument
    {
        /// <summary>
        /// The attached <see cref="CanvasSvgDocument"/>.
        /// </summary>
        public CanvasSvgDocument SvgDocument { get; }

        /// <summary>
        /// Creates a new <see cref="Win2DSvgDocument"/> from a Win2D <see cref="CanvasSvgDocument"/>.
        /// </summary>
        /// <param name="svgDocument">The attached <see cref="CanvasSvgDocument"/>.</param>
        public Win2DSvgDocument(CanvasSvgDocument svgDocument)
        {
            SvgDocument = svgDocument;
        }

        /// <inheritdoc/>
        public string GetXml()
        {
            return SvgDocument.GetXml();
        }
    }
}
