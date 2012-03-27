using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video
{
    /// <summary>Interface for a custom timer</summary>
    public interface ITimer : IDisposable
    {
        /// <summary>Frequency period of the timer interval</summary>
        UInt32 Interval { get; set; }

        /// <summary>Represents when the timer was last started (stop sets to <see cref="TimeSpan.MinValue"/>)</summary>
        TimeSpan StartTime { get; set; }

        /// <summary>Public-facing timer event</summary>
        event Action<TimeSpan> Elapsed;

        /// <summary>Starts the timer</summary>
        void Start();

        /// <summary>Stops the timer</summary>
        void Stop();

        /// <summary>Resets the time for when the timer goes off</summary>
        void ResetStartTime();
    }
}