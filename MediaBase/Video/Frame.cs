using System;

using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video
{
    /// <summary>Represents a single image instance, be it a bitmap, PNG, JPEG/JFIF or frame of animation from an audio/video movie or animated picture</summary>
    public class Frame
    {
        #region Properties
        /// <summary>Indicates the horizontal origin of the frame.</summary>
        public Int64 OriginX { get; set; }

        /// <summary>Indicates the vertical origin of the frame.</summary>
        public Int64 OriginY { get; set; }

        /// <summary>Exposes the underlying image binary data, possibly interpreted.</summary>
        public PixelData Pixels { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Frame() { }

        /// <summary>Partial definition constructor</summary>
        public Frame(PixelData data) : this(data, 0L, 0L) { }

        /// <summary>Definition constructor</summary>
        public Frame(PixelData data, Int64 originX, Int64 originY)
        {
            this.Pixels = data;
            this.OriginX = originX;
            this.OriginY = originY;
        }
        #endregion
    }
}