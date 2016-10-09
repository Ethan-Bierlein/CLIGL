using System;

namespace CLIGL
{
    /// <summary>
    /// This class is intended to be a compact representation of a "pixel"
    /// in CLIGL, which can be used instead of passing around individual characters
    /// and colors.
    /// </summary>
    public struct RenderingPixel
    {
        public static RenderingPixel EmptyPixel = new RenderingPixel(' ', ConsoleColor.Black, ConsoleColor.Black);
        public static RenderingPixel FullPixel = new RenderingPixel(' ', ConsoleColor.White, ConsoleColor.White);

        public char Character { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Constructor for the RenderingPixel class.
        /// </summary>
        /// <param name="character">The character that represents the pixel.</param>
        /// <param name="foregroundColor">The foreground color of the pixel.</param>
        /// <param name="backgroundColor">The background color of the pixel.</param>
        public RenderingPixel(char character, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            this.Character = character;
            this.ForegroundColor = foregroundColor;
            this.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Check to see if two rendering pixels are equal.
        /// </summary>
        /// <param name="a">The first rendering pixel.</param>
        /// <param name="b">The second rendering pixel.</param>
        /// <returns>Whether or not the objects are equal.</returns>
        public static bool operator==(RenderingPixel a, RenderingPixel b)
        {
            if(
                a.Character == b.Character &&
                a.ForegroundColor == b.ForegroundColor &&
                a.BackgroundColor == b.BackgroundColor
            )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check to see if two rendering pixels are not equal.
        /// </summary>
        /// <param name="a">The first rendering pixel.</param>
        /// <param name="b">The second rendering pixel.</param>
        /// <returns>Whether or not the objects are not equal.</returns>
        public static bool operator!=(RenderingPixel a, RenderingPixel b)
        {
            if(
                a.Character == b.Character && 
                a.ForegroundColor == b.ForegroundColor &&
                a.BackgroundColor == b.BackgroundColor
            )
            {
                return false;
            }
            return true;
        }
    }
}
