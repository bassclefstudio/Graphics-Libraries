using BassClefStudio.Graphics.Input.Basic;
using BassClefStudio.Graphics.Transforms;
using BassClefStudio.NET.Core.Streams;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Input
{
    /// <summary>
    /// Represents a service that can detect user input inside of a graphics space.
    /// </summary>
    public interface IInputWatcher
    {
        /// <summary>
        /// The <see cref="ViewCamera"/> currently used by this <see cref="IInputWatcher"/> to shift between input- and view-space. Note that this <see cref="ViewCamera"/> will be used in reverse to convert view inputs into returned co-ordinates.
        /// </summary>
        ViewCamera Camera { get; set; }

        /// <summary>
        /// Gets whether this <see cref="IInputWatcher"/> is currently listening for inputs.
        /// </summary>
        bool InputEnabled { get; }

        /// <summary>
        /// Starts listening for inputs that this <see cref="IInputWatcher"/> can detect, emitting values onto the <see cref="InputStream"/> when they are.
        /// </summary>
        void StartInput();

        /// <summary>
        /// Stops listening for inputs.
        /// </summary>
        void StopInput();

        /// <summary>
        /// An <see cref="IStream{T}"/> that outputs any <see cref="IInput"/>s this <see cref="IInputWatcher"/> detects.
        /// </summary>
        IStream<IInput> InputStream { get; }
    }

    /// <summary>
    /// Contains extension methods for the <see cref="IInputWatcher"/> interface regarding checking for specific input types.
    /// </summary>
    public static class InputWatcherExtensions
    {
        /// <summary>
        /// Creates an <see cref="IStream{T}"/> that relays all <see cref="KeyboardInput"/> inputs of a specified <see cref="KeyType"/>.
        /// </summary>
        /// <param name="watcher">The <see cref="IInputWatcher"/> detecting user input.</param>
        /// <param name="key">The <see cref="KeyType"/> key this <see cref="IStream{T}"/> is listening for.</param>
        public static IStream<KeyboardInput> GetKeyStream(this IInputWatcher watcher, KeyType key)
        {
            return watcher.InputStream
                .Where(i => i is KeyboardInput)
                .Cast<IInput, KeyboardInput>()
                .Where(k => k.Key == key);
        }

        /// <summary>
        /// Creates an <see cref="IStream{T}"/> that detects any <see cref="PointerInput"/>s of the given <see cref="PointerBehaviour"/>.
        /// </summary>
        /// <param name="watcher">The <see cref="IInputWatcher"/> detecting user input.</param>
        /// <param name="behaviour">The <see cref="PointerBehaviour"/> to listen for.</param>
        public static IStream<PointerInput> GetClickStream(this IInputWatcher watcher, PointerBehaviour behaviour)
        {
            return watcher.InputStream
                .Where(i => i is PointerInput)
                .Cast<IInput, PointerInput>()
                .Where(p => p.Behaviour == behaviour);
        }

        /// <summary>
        /// Creates an <see cref="IStream{T}"/> that emits the current <see cref="Vector2"/> position of the <see cref="PointerInput"/> pointer as it changes.
        /// </summary>
        /// <param name="watcher">The <see cref="IInputWatcher"/> detecting user input.</param>
        public static IStream<Vector2> GetPointerPosition(this IInputWatcher watcher)
        {
            return watcher.InputStream
                .Where(i => i is PointerInput)
                .Cast<IInput, PointerInput>()
                .Select(p => p.Point)
                .UniqueEq();
        }
    }
}
