using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video
{
    /// <summary>Interface to getting a specific image from a format that logically stores multiple frames</summary>
    public interface IImageSet
    {
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <param name="index">Index of the frame to get</param>
        /// <returns>A frame containing the pixel data</returns>
        Frame GetFrame(Int32 index);
    }
}