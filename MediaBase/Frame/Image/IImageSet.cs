using System;

using Bardez.Projects.MultiMedia.MediaBase.Video;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Image
{
    /// <summary>Interface to getting a specific image from a format that logically stores multiple frames</summary>
    public interface IImageSet
    {
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <param name="index">Index of the frame to get</param>
        /// <returns>A frame containing the pixel data</returns>
        IMultimediaImageFrame GetFrame(Int32 index);

        /// <summary>Property exposing the count of frames in the Image Set</summary>
        Int64 FrameCount { get; }
    }
}