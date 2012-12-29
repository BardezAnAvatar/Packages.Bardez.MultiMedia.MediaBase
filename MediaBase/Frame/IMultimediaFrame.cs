using System;
using System.IO;

namespace Bardez.Projects.Multimedia.MediaBase.Frame
{
    /// <summary>Defines an basic interface for common elements of audio, video and metadata frames of data samples</summary>
    /// <remarks>
    ///     Common elements for all frames are:
    ///         data:           a Byte array, or perhaps a stream
    ///         metadata:       something indicating parameters of the data described such as:
    ///             data format:    something indicating the type of data represented
    ///             audio details/height, width, sample bit depth, packing, rendering origin, etc.
    ///         metadata has no real base that I can think of other than data format, which may not make the best
    ///             enumeration to join together (such as LibAV). It is, however, the only common element.
    ///     For multimedia playback, a start time and a lifetime are relevant. However, for still images, it is not.
    ///         An animated GIF would have a lifetime, whereas a JPEG just wouldn't.
    /// </remarks>
    public interface IMultimediaFrame : IDisposable
    {
        #region Properties
        /// <summary>Exposes the data of a multimedia frame</summary>
        Stream Data { get; }
        #endregion
    }
}