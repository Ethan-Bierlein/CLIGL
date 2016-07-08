using System;

namespace CLIGL
{
    /// <summary>
    /// This classs represents a CLIGL buffer, which is effectively
    /// a container for three separate arrays. A text array, a foreground
    /// color array, and a background color array.
    /// </summary>
    internal class Buffer
    {
        public string[] TextBuffer { get; set; }
        public ConsoleColor[] ForegroundColorBuffer { get; set; }
        public ConsoleColor[] BackgroundColorBuffer { get; set; }

        /// <summary>
        /// Constructor for the Buffer class.
        /// </summary>
        /// <param name="bufferSize">The size of the buffer.</param>
        public Buffer(int bufferSize)
        {
            this.TextBuffer = new string[bufferSize];
            this.ForegroundColorBuffer = new ConsoleColor[bufferSize];
            this.BackgroundColorBuffer = new ConsoleColor[bufferSize];
        }
    }
}
