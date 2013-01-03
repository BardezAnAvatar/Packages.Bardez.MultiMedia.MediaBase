using System;
using System.Collections.Generic;

using Bardez.Projects.Multimedia.MediaBase.Container;
using Bardez.Projects.Multimedia.MediaBase.Frame;
using Bardez.Projects.Multimedia.MediaBase.Frame.Audio;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Buffers
{
    /// <summary>This interface defines a buffer manager for multiple multimedia frame buffers</summary>
    public interface IMultimediaFrameStreamBuffers : IDisposable
    {
        #region Properties
        /// <summary>Exposes a flag indicating whether or not the buffers are full</summary>
        Boolean BuffersFull { get; }

		/// <summary>Dictionary of video stream processing records, the key being the stream number</summary>
        Dictionary<Int32, IMultimediaFrameBuffer<IMultimediaImageFrame>> StreamsVideo { get; }

		/// <summary>Dictionary of audio stream processing records, the key being the stream number</summary>
        Dictionary<Int32, IMultimediaFrameBuffer<IMultimediaAudioFrame>> StreamsAudio { get; }

        /// <summary>Dictionary of subtitle stream processing records, the key being the stream number</summary>
        Dictionary<Int32, IMultimediaFrameBuffer<IMultimediaStreamingFrame>> StreamsSubtitle { get; }
        #endregion
    }
}