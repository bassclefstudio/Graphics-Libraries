using SkiaSharp.Extended.Svg;
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
    public class SkiaSvgDocument : ISvgDocument
    {
        /// <summary>
        /// The attached <see cref="SKSvg"/>.
        /// </summary>
        public SKSvg SvgDocument { get; }

        /// <summary>
        /// Creates a new <see cref="SkiaSvgDocument"/> from a Win2D <see cref="CanvasSvgDocument"/>.
        /// </summary>
        /// <param name="svgDocument">The attached <see cref="CanvasSvgDocument"/>.</param>
        public SkiaSvgDocument(SKSvg svgDocument)
        {
            SvgDocument = svgDocument;
        }

        /// <inheritdoc/>
        public string GetXml()
        {
            return SvgDocument.ToString();
        }
    }
}
