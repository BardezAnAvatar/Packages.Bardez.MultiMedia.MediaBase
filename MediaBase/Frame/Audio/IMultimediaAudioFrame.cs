using System;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.Multimedia.MediaBase.Frame;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Audio
{
    /// <summary>Defines an interface for common elements and operation for a logical group of audio samples</summary>
    public interface IMultimediaAudioFrame : IMultimediaStreamingFrame, IWaveFormatEx
    {
    }
}