using System;

namespace CLIGL
{
    /// <summary>
    /// This struct is intended to be a compact representation of a "pixel"
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
        /// Constructor for the RenderingPixel struct.
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
    }
}
