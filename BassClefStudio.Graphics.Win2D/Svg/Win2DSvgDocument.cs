using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.Graphics.Svg
{
    /// <summary>
    /// An <see cref="ISvgDocument"/> wrapper around the <see cref="CanvasSvgDocument"/> Win2D class.
    /// </summary>
    public class Win2DSvgDocument : ISvgDocument
    {
        /// <summary>
        /// The attached <see cref="CanvasSvgDocument"/> used for rendering this SVG in the Win2D platform.
        /// </summary>
        public CanvasSvgDocument SvgCanvas { get; }

        /// <summary>
        /// Creates a new <see cref="Win2DSvgDocument"/> from the given XML data.
        /// </summary>
        /// <param name="canvasResourceCreator">An <see cref="ICanvasResourceCreator"/> which provides the ability to load documents into the Win2D framework (e.g. a <see cref="CanvasControl"/>).</param>
        /// <param name="xml">The <see cref="string"/> XML for the SVG file to load into memory.</param>
        public Win2DSvgDocument(ICanvasResourceCreator canvasResourceCreator, string xml)
        {
            SvgCanvas = CanvasSvgDocument.LoadFromXml(canvasResourceCreator, xml);
        }

        /// <inheritdoc/>
        public string ToXml()
        {
            return SvgCanvas?.GetXml();
        }
    }
}
