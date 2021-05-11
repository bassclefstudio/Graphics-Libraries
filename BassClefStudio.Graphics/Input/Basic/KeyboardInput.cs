using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BassClefStudio.Graphics.Input.Basic
{
    /// <summary>
    /// An <see cref="IInput"/> detected from the keyboard (virtual or physical), containing information about the key that was pressed.
    /// </summary>
    public struct KeyboardInput : IBinaryInput
    {
        /// <inheritdoc/>
        public bool IsPressed { get; set; }

        /// <summary>
        /// A <see cref="KeyType"/> enum containing information about the key that was pressed or released.
        /// </summary>
        public KeyType Key { get; set; }

        /// <summary>
        /// Creates a new <see cref="KeyboardInput"/>.
        /// </summary>
        /// <param name="isPressed">A <see cref="bool"/> indicating whether the <see cref="IBinaryInput"/> represents a press ('true') or release ('false') input detection.</param>
        /// <param name="key">A <see cref="KeyType"/> enum containing information about the key that was pressed or released.</param>
        public KeyboardInput(bool isPressed, KeyType key)
        {
            IsPressed = isPressed;
            Key = key;
        }
    }

    /// <summary>
    /// An enum containing every available key that a <see cref="KeyboardInput"/> input could represent.
    /// </summary>
    public enum KeyType
    {
        /// <summary>
        /// The 'Enter' or 'Return' key.
        /// </summary>
        Enter = 13,
        /// <summary>
        /// The spacebar.
        /// </summary>
        Space = 32,
        /// <summary>
        /// The left arrow key.
        /// </summary>
        Left = 37,
        /// <summary>
        /// The up arrow key.
        /// </summary>
        Up = 38,
        /// <summary>
        /// The right arrow key.
        /// </summary>
        Right = 39,
        /// <summary>
        /// The down arrow key.
        /// </summary>
        Down = 40,
        /// <summary>
        /// The 'A' key.
        /// </summary>
        A = 65,
        /// <summary>
        /// The 'B' key.
        /// </summary>
        B = 66,
        /// <summary>
        /// The 'C' key.
        /// </summary>
        C = 67,
        /// <summary>
        /// The 'D' key.
        /// </summary>
        D = 68,
        /// <summary>
        /// The 'E' key.
        /// </summary>
        E = 69,
        /// <summary>
        /// The 'F' key.
        /// </summary>
        F = 70,
        /// <summary>
        /// The 'G' key.
        /// </summary>
        G = 71,
        /// <summary>
        /// The 'H' key.
        /// </summary>
        H = 72,
        /// <summary>
        /// The 'I' key.
        /// </summary>
        I = 73,
        /// <summary>
        /// The 'J' key.
        /// </summary>
        J = 74,
        /// <summary>
        /// The 'K' key.
        /// </summary>
        K = 75,
        /// <summary>
        /// The 'L' key.
        /// </summary>
        L = 76,
        /// <summary>
        /// The 'M' key.
        /// </summary>
        M = 77,
        /// <summary>
        /// The 'N' key.
        /// </summary>
        N = 78,
        /// <summary>
        /// The 'O' key.
        /// </summary>
        O = 79,
        /// <summary>
        /// The 'P' key.
        /// </summary>
        P = 80,
        /// <summary>
        /// The 'Q' key.
        /// </summary>
        Q = 81,
        /// <summary>
        /// The 'R' key.
        /// </summary>
        R = 82,
        /// <summary>
        /// The 'S' key.
        /// </summary>
        S = 83,
        /// <summary>
        /// The 'T' key.
        /// </summary>
        T = 84,
        /// <summary>
        /// The 'U' key.
        /// </summary>
        U = 85,
        /// <summary>
        /// The 'V' key.
        /// </summary>
        V = 86,
        /// <summary>
        /// The 'W' key.
        /// </summary>
        W = 87,
        /// <summary>
        /// The 'X' key.
        /// </summary>
        X = 88,
        /// <summary>
        /// The 'Y' key.
        /// </summary>
        Y = 89,
        /// <summary>
        /// The 'Z' key.
        /// </summary>
        Z = 90,
        /// <summary>
        /// The '0' number key.
        /// </summary>
        Num0 = 96,
        /// <summary>
        /// The '1' number key.
        /// </summary>
        Num1 = 97,
        /// <summary>
        /// The '2' number key.
        /// </summary>
        Num2 = 98,
        /// <summary>
        /// The '3' number key.
        /// </summary>
        Num3 = 99,
        /// <summary>
        /// The '4' number key.
        /// </summary>
        Num4 = 100,
        /// <summary>
        /// The '5' number key.
        /// </summary>
        Num5 = 101,
        /// <summary>
        /// The '6' number key.
        /// </summary>
        Num6 = 102,
        /// <summary>
        /// The '7' number key.
        /// </summary>
        Num7 = 103,
        /// <summary>
        /// The '8' number key.
        /// </summary>
        Num8 = 104,
        /// <summary>
        /// The '9' number key.
        /// </summary>
        Num9 = 105
    }
}
