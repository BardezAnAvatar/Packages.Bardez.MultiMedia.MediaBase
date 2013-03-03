using Bardez.Projects.Multimedia.MediaBase.Video;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Image
{
    /// <summary>Interface for images that can return a frame for processing</summary>
    public interface IImage
    {
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        IMultimediaImageFrame GetFrame();
    }
}