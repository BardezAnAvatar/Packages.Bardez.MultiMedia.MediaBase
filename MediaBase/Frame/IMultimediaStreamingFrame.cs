using System;

namespace Bardez.Projects.Multimedia.MediaBase.Frame
{
    /// <summary>
    ///     Interface defining common elements and operations of a frame that is part of a series
    ///     to be rendered, such as audio, video, and subtitles
    /// </summary>
    public interface IMultimediaStreamingFrame : IMultimediaFrame
    {
        #region Properties
        /// <summary>Metadata for streaming</summary>
        StreamingMetadata StreamingMetadata { get; }
        #endregion
    }
}