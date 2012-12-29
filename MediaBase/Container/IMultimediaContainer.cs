using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Frame.Buffers;

namespace Bardez.Projects.Multimedia.MediaBase.Container
{
    /// <summary>Defines common elements, operations for a multimedia container</summary>
    public interface IMultimediaContainer : IDisposable
    {
        #region Properties
        /// <summary>Exposes the length of multimedia within this container</summary>
        TimeSpan Length { get; }

        /// <summary>Collection of Stream processing information that is both informational and controllable</summary>
        /// <remarks>This should be referenced to indicate which streams to add to buffers and which ones to ignore</remarks>
        Dictionary<Int32, StreamProcessingInfo> StreamInfo { get; }
        #endregion


        #region Methods
        /// <summary>Opens the specified multimedia file</summary>
        /// <param name="path">Path of the file to open</param>
        void OpenMedia(String path);

        /// <summary>Opens the multimedia from a source Stream</summary>
        /// <param name="source">Source stream to read data from</param>
        void OpenMedia(Stream source);

        /// <summary>Method to launch in a new thread which decodes the various streams and fills them into buffers</summary>
        void DecodeStreamsThread(IMultimediaFrameStreamBuffers streams);
        #endregion
    }
}