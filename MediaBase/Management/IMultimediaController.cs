using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Container;
using Bardez.Projects.Multimedia.MediaBase.Render.Audio;
using Bardez.Projects.Multimedia.MediaBase.Render.Subtitle;
using Bardez.Projects.Multimedia.MediaBase.Render.Video;

namespace Bardez.Projects.Multimedia.MediaBase.Management
{
    interface IMultimediaController : IDisposable
    {
        #region Properties
        /// <summary>Exposes the length of multimedia within this container</summary>
        TimeSpan Length { get; }

        /// <summary>Collection of renderer managers for audio streams</summary>
        IList<IAudioRenderManager> RenderManagersAudio { get; }

        /// <summary>Collection of renderer managers for video streams</summary>
        IList<IVideoRenderManager> RenderManagersVideo { get; }

        /// <summary>Collection of renderer managers for subtitle streams</summary>
        IList<ISubtitleRenderManager> RenderManagersSubtitle { get; }

        /// <summary>Generates a collection of Stream processing info for whether or not to bother decoding a given stream</summary>
        Dictionary<Int32, StreamProcessingInfo> StreamProcessing { get; }
        #endregion


        #region Methods
        /// <summary>Opens the specified multimedia file</summary>
        /// <param name="path">Path of the file to open</param>
        void OpenMedia(String path);

        /// <summary>Opens the multimedia from a source Stream</summary>
        /// <param name="source">Source stream to read data from</param>
        void OpenMedia(Stream source);

        /// <summary>Commands the controller to start decoding the multimedia</summary>
        void StartDecoding();

        /// <summary>Resets the streams to the beginning</summary>
        /// <remarks>Decoders should clear buffers and start from the beginning of the video</remarks>
        void Reset();

        /// <summary>Seeks the streams to the specified timestamp or a close relative thereof</summary>
        /// <param name="timestamp">Time at which to seek to</param>
        /// <remarks>It is allowable to find the nearest keyframe for X stream and sync, then continue from there.</remarks>
        void Seek(TimeSpan timestamp);

        /// <summary>Starts multimedia playback</summary>
        void StartPlayback();

        /// <summary>Pauses video playback, if playing</summary>
        void PausePlayback();
        #endregion
    }
}