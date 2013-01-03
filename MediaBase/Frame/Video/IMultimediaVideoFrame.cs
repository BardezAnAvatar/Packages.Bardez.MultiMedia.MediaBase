using System;

using Bardez.Projects.Multimedia.MediaBase.Frame;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Video
{
    /// <summary>Defines an interface common for multimedia video frame implementations</summary>
    public interface IMultimediaVideoFrame : IMultimediaImageFrame, IMultimediaStreamingFrame
    {
    }
}