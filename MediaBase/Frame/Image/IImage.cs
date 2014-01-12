using System;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Image
{
    /// <summary>Interface for images that can return a frame for processing</summary>
    public interface IImage
    {
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        IMultimediaImageFrame GetFrame();

        /// <summary>Gets a sub-image of the current image</summary>
        /// <param name="x">Source X position</param>
        /// <param name="y">Source Y position</param>
        /// <param name="width">Width of sub-image</param>
        /// <param name="height">Height of sub-image</param>
        /// <returns>The requested sub-image</returns>
        /// <remarks>
        ///     I have done some thinking about how a sub-image should interact with the origin point in
        ///     metadata and have decided that it shouldn't; if a subclass needs it, it can do so. But generally
        ///     speaking, the generated point of origin should be (0,0).
        /// </remarks>
        IImage GetSubImage(Int32 x, Int32 y, Int32 width, Int32 height);
    }
}