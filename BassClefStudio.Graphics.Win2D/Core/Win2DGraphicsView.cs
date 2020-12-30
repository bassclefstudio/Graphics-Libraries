using BassClefStudio.Graphics.Core;
using BassClefStudio.Graphics.Svg;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace BassClefStudio.Graphics.Core
{
    /// <summary>
    /// Represents a Win2D implementation of <see cref="IGraphicsView"/> that draws to a <see cref="CanvasControl"/>.
    /// </summary>
    public class Win2DGraphicsView : IGraphicsView, ISvgManager
    {
        #region Win2D

        FrameworkElement canvasElement;

        /// <summary>
        /// Creates a <see cref="Win2DGraphicsView"/> from a non-animated <see cref="CanvasControl"/>.
        /// </summary>
        /// <param name="canvas">The <see cref="CanvasControl"/> Win2D canvas to draw on.</param>
        public Win2DGraphicsView(CanvasControl canvas)
        {
            canvasElement = canvas;
            canvas.Draw += CanvasStaticDrawRequested;
            autoUpdate = false;
        }

        private void CanvasStaticDrawRequested(CanvasControl sender, CanvasDrawEventArgs args)
        {
            UpdateRequested?.Invoke(
                    this,
                    new UpdateRequestEventArgs(
                        sender.Size.ToVector2(),
                        new Win2DGraphicsProvider(args.DrawingSession)));
        }

        /// <summary>
        /// Creates a <see cref="Win2DGraphicsView"/> from a non-animated <see cref="CanvasVirtualControl"/>.
        /// </summary>
        /// <param name="canvas">The <see cref="CanvasVirtualControl"/> Win2D canvas to draw on.</param>
        public Win2DGraphicsView(CanvasVirtualControl canvas)
        {
            canvasElement = canvas;
            canvas.RegionsInvalidated += CanvasVirtualDrawRequested;
            autoUpdate = false;
        }

        private void CanvasVirtualDrawRequested(CanvasVirtualControl sender, CanvasRegionsInvalidatedEventArgs args)
        {
            if (!args.VisibleRegion.IsEmpty)
            {
                UpdateRequested?.Invoke(
                            this,
                            new UpdateRequestEventArgs(
                                sender.Size.ToVector2(),
                                new Win2DGraphicsProvider(sender.CreateDrawingSession(args.VisibleRegion))));
            }
        }

        /// <summary>
        /// Creates a <see cref="Win2DGraphicsView"/> from a non-animated <see cref="CanvasAnimatedControl"/>.
        /// </summary>
        /// <param name="canvas">The <see cref="CanvasAnimatedControl"/> Win2D canvas to draw on.</param>
        public Win2DGraphicsView(CanvasAnimatedControl canvas)
        {
            canvasElement = canvas;
            canvas.Draw += CanvasAnimatedDrawRequested;
            autoUpdate = true;
        }

        private void CanvasAnimatedDrawRequested(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            if (sender is CanvasAnimatedControl canvas)
            {
                UpdateRequested?.Invoke(
                    this,
                    new UpdateRequestEventArgs(
                        canvas.Size.ToVector2(),
                        new Win2DGraphicsProvider(args.DrawingSession)));
            }
            else
            {
                UpdateRequested?.Invoke(
                       this,
                       new UpdateRequestEventArgs(
                           null,
                           new Win2DGraphicsProvider(args.DrawingSession)));
            }
        }

        #endregion
        #region GraphicsView

        private bool autoUpdate;
        /// <inheritdoc/>
        public bool IsAutoUpdate 
        {
            get => autoUpdate; 
            set => throw new NotSupportedException("Win2DTurtleGraphicsCanvas does not support switching between different refresh modes. To change whether this ITurtleGraphicsView is auto-refresh, switch the backing Win2D control between CanvasControl and CanvasAnimatedControl."); 
        }

        /// <inheritdoc/>
        public void RequestUpdate()
        {
            if (canvasElement is CanvasControl control)
            {
                control.Invalidate();
            }
            else if (canvasElement is CanvasAnimatedControl animatedControl)
            {
                animatedControl.Invalidate();
            }
            else if (canvasElement is CanvasVirtualControl virtualControl)
            {
                virtualControl.Invalidate();
            }
            else
            {
                throw new NotImplementedException($"The attached Win2D control {canvasElement?.GetType().Name} does not support requesting an update.");
            }
        }

        /// <inheritdoc/>
        public event EventHandler<UpdateRequestEventArgs> UpdateRequested;

        #endregion
        #region Svg

        public async Task<ISvgDocument> LoadAsync(Stream fileStream)
        {
            CanvasSvgDocument svgDocument = await CanvasSvgDocument.LoadAsync((canvasElement as ICanvasResourceCreator), fileStream.AsRandomAccessStream());
            return new Win2DSvgDocument(svgDocument);
        }

        public ISvgDocument Load(string xml)
        {
            CanvasSvgDocument svgDocument = CanvasSvgDocument.LoadFromXml((canvasElement as ICanvasResourceCreator), xml);
            return new Win2DSvgDocument(svgDocument);
        }

        #endregion
    }
}
