using BassClefStudio.NET.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.Graphics.Core
{
    /// <summary>
    /// Represents a drawable surface (either on an app <see cref="IGraphicsView"/>, or in a file) on which graphics commands can be executed.
    /// </summary>
    public interface IGraphicsProvider
    {
        /// <summary>
        /// The <see cref="ViewCamera"/> currently used by this <see cref="IGraphicsProvider"/> to shift between drawing- and view-space.
        /// </summary>
        ViewCamera Camera { get; set; }

        /// <summary>
        /// Clears the drawing area with a specified <see cref="Color"/>.
        /// </summary>
        /// <param name="baseColor">The background color to clear the surface with.</param>
        void Clear(Color baseColor);

        /// <summary>
        /// Flushes any unregistered changes to the <see cref="IGraphicsProvider"/> to the app view or file.
        /// </summary>
        Task FlushAsync();
    }
}
