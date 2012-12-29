using System;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.MultiMedia.MediaBase.Video;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Video
{
    /// <summary>Interface for movies that can return a frame sequentially for processing</summary>
    public interface IMovie : IWaveFormatEx
    {
        /// <summary>Fetches the next MediaBase Frame for output</summary>
        /// <returns>A MediaBase Frame for output</returns>
        IMultimediaImageFrame GetNextFrame();

        /// <summary>Resets the video stream to the beginning</summary>
        void ResetVideo();

        /// <summary>Stops video playback, if playing</summary>
        void StopVideoPlayback();

        /// <summary>Creates a playback timer for the movie</summary>
        void StartVideoPlayback();

        /// <summary>Retrieves audio block from the data cache</summary>
        /// <param name="blockNumber">Block number to retrieve</param>
        /// <param name="streamNumber">Stream number to retrieve</param>
        /// <returns>Byte Array of samples</returns>
        Byte[] GetAudioBlock(Int32 blockNumber, Int32 streamNumber);

        /// <summary>Exposes the event for timer elapse</summary>
        event Action<IMultimediaImageFrame> PlayFrame;

        /// <summary>Control method to clear the TimerElapsed event delegates</summary>
        void ClearTimerElapsed();
    }
}