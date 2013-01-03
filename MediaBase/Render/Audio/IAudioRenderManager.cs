using System;

using Bardez.Projects.Multimedia.MediaBase.Frame.Audio;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>
    ///     This interface defines an audio stream's render manager, which exports audio chunks
    ///     when its attempt render method is successful
    /// </summary>
    public interface IAudioRenderManager : IAVStreamRenderManager
    {
        /// <summary>Event to raise for rendering audio output.</summary>
        event Action<IMultimediaAudioFrame> Render;
    }
}