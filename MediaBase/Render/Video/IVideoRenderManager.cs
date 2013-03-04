using System;

using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Video
{
    public interface IVideoRenderManager : IAVStreamRenderManager
    {
        /// <summary>Event to raise for rendering video output.</summary>
        /// <remarks>
        ///     IMultimediaVideoFrame has Streaming metadata,
        ///     which is only needed for the manger to read, not to expose
        /// </remarks>
        event Action<IMultimediaImageFrame> Render;
    }
}