using System;

using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video
{
    /// <summary>Represents a single image instance, be it a bitmap, PNG, JPEG/JFIF or frame of animation from an audio/video movie or animated picture</summary>
    public class Frame
    {
        #region Properties
        /// <summary>Indicates the horizontal origin of the frame.</summary>
        public UInt64 OriginX { get; set; }

        /// <summary>Indicates the vertical origin of the frame.</summary>
        public UInt64 OriginY { get; set; }

        /// <summary>Exposes the underlying image binary data, possibly interpreted.</summary>
        public PixelData Pixels { get; set; }
        #endregion
    }
}