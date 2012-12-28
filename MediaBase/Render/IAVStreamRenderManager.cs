using System;

namespace Bardez.Projects.MultiMedia.MediaBase.Render
{
    /// <summary>This interface defines methods and events common to multimedia stream renderers</summary>
    /// <remarks>Classes implementing this interface will have frame buffers to render from</remarks>
    public interface IAVStreamRenderManager
    {
        /// <summary>
        ///     Attempts to render audio if the buffer is processed and the timespan is within the
        ///     rendering threshold
        /// </summary>
        /// <param name="time">Time code to render by</param>
        void AttemptRender(TimeSpan time);
    }
}