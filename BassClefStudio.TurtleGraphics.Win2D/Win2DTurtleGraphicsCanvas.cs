using BassClefStudio.NET.Core.Primitives;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.TurtleGraphics.Win2D
{
    /// <summary>
    /// Represents a Win2D implementation of <see cref="ITurtleGraphicsView"/> that draws to a <see cref="CanvasControl"/>.
    /// </summary>
    public class Win2DTurtleGraphicsCanvas : ITurtleGraphicsView
    {
        private bool autoRefresh;
        /// <inheritdoc/>
        public bool IsAutoRefresh 
        {
            get => autoRefresh; 
            set => throw new NotSupportedException("Win2DTurtleGraphicsCanvas does not support switching between different refresh modes. To change whether this ITurtleGraphicsView is auto-refresh, switch the backing Win2D control between CanvasControl and CanvasAnimatedControl."); 
        }

        /// <summary>
        /// Creates a <see cref="Win2DTurtleGraphicsCanvas"/> from a non-animated <see cref="CanvasControl"/>.
        /// </summary>
        /// <param name="canvas">The <see cref="CanvasControl"/> Win2D canvas to draw on.</param>
        public Win2DTurtleGraphicsCanvas(CanvasControl canvas)
        {
            canvas.Draw += CanvasStaticDrawRequested;
            autoRefresh = false;
        }

        /// <summary>
        /// Creates a <see cref="Win2DTurtleGraphicsCanvas"/> from a non-animated <see cref="CanvasAnimatedControl"/>.
        /// </summary>
        /// <param name="canvas">The <see cref="CanvasAnimatedControl"/> Win2D canvas to draw on.</param>
        public Win2DTurtleGraphicsCanvas(CanvasAnimatedControl canvas)
        {
            canvas.Draw += CanvasAnimatedDrawRequested;
            autoRefresh = true;
        }

        private void CanvasAnimatedDrawRequested(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            if (sender is CanvasAnimatedControl canvas)
            {
                UpdateRequested?.Invoke(
                    this,
                    new UpdateRequestEventArgs(
                        new Vector2((float)canvas.ActualWidth, (float)canvas.ActualHeight),
                        new Win2DTurtleGraphicsProvider(args.DrawingSession)));
            }
            else
            {
                UpdateRequested?.Invoke(
                       this,
                       new UpdateRequestEventArgs(
                           null,
                           new Win2DTurtleGraphicsProvider(args.DrawingSession)));
            }
        }

        private void CanvasStaticDrawRequested(CanvasControl sender, CanvasDrawEventArgs args)
        {
            UpdateRequested?.Invoke(
                this, 
                new UpdateRequestEventArgs(
                    new Vector2((float)sender.ActualWidth, (float)sender.ActualHeight),
                    new Win2DTurtleGraphicsProvider(args.DrawingSession)));
        }

        /// <inheritdoc/>
        public event EventHandler<UpdateRequestEventArgs> UpdateRequested;
    }
}
