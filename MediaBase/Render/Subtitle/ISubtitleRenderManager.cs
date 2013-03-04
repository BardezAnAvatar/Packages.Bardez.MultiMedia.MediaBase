using System;

using Bardez.Projects.Multimedia.MediaBase.Frame;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Subtitle
{
    /// <summary>Defines an interface for a manager for rendering subtitle information</summary>
    public interface ISubtitleRenderManager : IAVStreamRenderManager
    {
        /// <summary>Event to raise for rendering subtitle output.</summary>
        /// <remarks>Can be text or video, so not sure how to handle yet.</remarks>
        event Action<IMultimediaStreamingFrame> Render;
    }
}