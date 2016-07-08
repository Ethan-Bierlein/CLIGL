using System;

namespace CLIGL
{
    /// <summary>
    /// This class is the "main" static class which manages the global state of
    /// CLIGL at all times.
    /// </summary>
    public static class MainCLIGL
    {
        private static Buffer[] _Buffers = new Buffer[65536];
        private static int _BoundBufferIndex = 0;

        /// <summary>
        /// Set the currently bound buffer index to the passed buffer 
        /// index, which means all next buffer operations will be done
        /// on the currently bound buffer.
        /// </summary>
        /// <param name="bufferIndex"></param>
        public static void BindBuffer(int bufferIndex)
        {
            if(bufferIndex >= 0 && bufferIndex < 65536)
            {
                _BoundBufferIndex = bufferIndex;
            }
            {
                throw new Exception(string.Format("The provided buffer index '{0}' is outside the range of 0-65535", bufferIndex));
            }
        }
    }
}