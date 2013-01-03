using System;

namespace Bardez.Projects.Multimedia.MediaBase.Data.Pixels
{
    /// <summary>Represents a base type for pixel data</summary>
    public abstract class PixelBase
    {
        /// <summary>Gets the underlying bytes of the pixel</summary>
        /// <returns>A byte array of the underlying bytes</returns>
        public abstract Byte[] GetBytes();
    }
}