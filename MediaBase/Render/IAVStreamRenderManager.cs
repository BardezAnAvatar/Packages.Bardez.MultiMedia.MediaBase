using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Multimedia
{
    /// <summary>This interface defines methods and events common to LibAV stream renderers</summary>
    public interface IStreamRenderManager
    {
        /// <summary>
        ///     Attempts to render audio if the buffer is processed and the timespan is within the
        ///     rendering threshold
        /// </summary>
        /// <param name="time">Time code to render by</param>
        void AttemptRender(TimeSpan time);
    }
}