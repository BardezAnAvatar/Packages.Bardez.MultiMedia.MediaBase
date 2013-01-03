using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Multimedia.MediaBase.Container;
using Bardez.Projects.Multimedia.MediaBase.Frame.Buffers;
using Bardez.Projects.Multimedia.MediaBase.Render.Audio;
using Bardez.Projects.Multimedia.MediaBase.Render.Subtitle;
using Bardez.Projects.Multimedia.MediaBase.Render.Video;

namespace Bardez.Projects.Multimedia.MediaBase.Management
{
    /// <summary>Implements a basic version of the Multimedia Controller interface.</summary>
    /// <remarks>Expects the timer and the container to be pre-built.</remarks>
    public class BasicMultimediaController : IMultimediaController
    {
        #region Fields
        /// <summary>Collection of audio stream render managers</summary>
        protected List<IAudioRenderManager> audioStreamRenderManagers;

        /// <summary>Collection of video stream render managers</summary>
        protected List<IVideoRenderManager> videoStreamRenderManagers;

        /// <summary>Collection of video stream render managers</summary>
        protected List<ISubtitleRenderManager> subtitleStreamRenderManagers;

        /// <summary>Container for the multimedia file</summary>
        protected IMultimediaContainer container;

        /// <summary>Represents the object containing the various buffers for reading/playback</summary>
        protected IMultimediaFrameStreamBuffers buffers;

        /// <summary>Current presentation TimeSpan for rendering frames</summary>
        protected TimeSpan currentPresentationTime;

        /// <summary>The TimeSpan from which the movie started playing</summary>
        /// <remarks>If video has seeked or started mid-stream (such as resume after a pause)</remarks>
        protected TimeSpan playbackStartTime;

        /// <summary>Reference to the thread that is decoding the container's streams</summary>
        /// <remarks>Kept so that the thread can be terminated</remarks>
        protected Thread containerDecodingThread;

        /// <summary>Timer for movie playback</summary>
        private ITimer timer;

        /// <summary>Locking Object reference for the multimedia timer</summary>
        private Object timerLock;
        #endregion


        #region IMultimediaController properties
        /// <summary>Exposes the length of the multimedia file</summary>
        public virtual TimeSpan Length
        {
            get { return this.container.Length; }
        }

        /// <summary>Collection of renderer managers for audio streams</summary>
        public IList<IAudioRenderManager> RenderManagersAudio
        {
            get { return this.audioStreamRenderManagers; }
        }

        /// <summary>Collection of renderer managers for video streams</summary>
        public IList<IVideoRenderManager> RenderManagersVideo
        {
            get { return this.videoStreamRenderManagers; }
        }

        /// <summary>Collection of renderer managers for subtitle streams</summary>
        public IList<ISubtitleRenderManager> RenderManagersSubtitle
        {
            get { return this.subtitleStreamRenderManagers; }
        }

        /// <summary>Generates a collection of Stream processing info for whether or not to bother decoding a given stream</summary>
        public Dictionary<Int32, StreamProcessingInfo> StreamProcessing
        {
            get { return this.container.StreamInfo; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="timerInstance">Instance of an ITimer interface</param>
        /// <param name="containerInstance">Instance of an IMultimediaContainer interface</param>
        public BasicMultimediaController(ITimer timerInstance, IMultimediaContainer containerInstance)
        {
            this.timerLock = new Object();

            //Assign interfaces
            this.timer = timerInstance;
            this.container = containerInstance;

            //timer execution
            this.timer.Elapsed += new Action<TimeSpan>(this.PresentationTimerExpired);
        }
        #endregion


        #region Destruction
        /// <summary>Disposal method</summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>Dispose pattern implementation</summary>
        /// <param name="deterministicallyDisposing">Flag indicating whether or not Dispose was called or a destructor is calling</param>
        public virtual void Dispose(Boolean deterministicallyDisposing)
        {
            if (deterministicallyDisposing)
            {
                this.audioStreamRenderManagers = null;
                this.videoStreamRenderManagers = null;
                this.subtitleStreamRenderManagers = null;
                this.timerLock = null;
                this.currentPresentationTime = TimeSpan.Zero;
                this.playbackStartTime = TimeSpan.Zero;

                if (this.container != null)
                {
                    this.container.Dispose();
                    this.container = null;
                }

                if (this.buffers != null)
                {
                    this.buffers.Dispose();
                    this.buffers = null;
                }

                if (this.timer != null)
                {
                    this.timer.Dispose();   //should implicitly Stop()
                    this.timer = null;
                }

                if (this.containerDecodingThread != null)
                {
                    this.containerDecodingThread.Abort();
                    this.containerDecodingThread = null;
                }
            }
        }
        #endregion


        #region IMultimediaController methods
        /// <summary>Resets the streams to the beginning</summary>
        /// <remarks>Decoders should clear buffers and start from the beginning of the video</remarks>
        public virtual void Reset()
        {
            this.currentPresentationTime = TimeSpan.Zero;
            this.playbackStartTime = TimeSpan.Zero;
            this.container.Seek(TimeSpan.Zero);
        }

        /// <summary>Seeks the streams to the specified timestamp or a close relative thereof</summary>
        /// <param name="timestamp">Time at which to seek to</param>
        /// <remarks>It is allowable to find the nearest keyframe for X stream and sync, then continue from there.</remarks>
        public virtual void Seek(TimeSpan timestamp)
        {
            this.currentPresentationTime = timestamp;
            this.playbackStartTime = timestamp;
            this.container.Seek(timestamp);
        }

        /// <summary>Starts multimedia playback</summary>
        public virtual void StartPlayback()
        {
            //do not set interval, since you want it going off more frequently than 33 or 66 milliseconds
            //this.timer.Interval = (UInt32)this.FrameRateTimerParameters.MillisecondDelay;

            this.playbackStartTime = this.currentPresentationTime;
            this.timer.Start();
        }

        /// <summary>Pauses video playback, if playing</summary>
        public virtual void PausePlayback()
        {
            this.StopTimer();
        }

        /// <summary>Opens the specified multimedia file</summary>
        /// <param name="path">Path of the file to open</param>
        public virtual void OpenMedia(String path)
        {
            this.container.OpenMedia(path);
        }

        /// <summary>Opens the multimedia from a source Stream</summary>
        /// <param name="source">Source stream to read data from</param>
        public virtual void OpenMedia(Stream source)
        {
            this.container.OpenMedia(source);
        }

        /// <summary>Commands the controller to start decoding the multimedia</summary>
        public virtual void StartDecoding()
        {
            this.containerDecodingThread = new Thread(() => { this.container.DecodeStreamsThread(); });
            this.containerDecodingThread.IsBackground = true;
            this.containerDecodingThread.Name = "IMultimediaContainer decoder";
            this.containerDecodingThread.Start();
        }
        #endregion


        #region Timer code
        /// <summary>Event handler for expiration of the timer</summary>
        protected virtual void PresentationTimerExpired(TimeSpan timeCode)
        {
            lock (this.timerLock)
            {
                TimeSpan effectiveTimeCode = timeCode + this.playbackStartTime;
                if (effectiveTimeCode > this.Length)
                {
                    //stop the timer
                    this.StopTimer();
                }
                else
                {
                    foreach (IAudioRenderManager audio in this.audioStreamRenderManagers)
                        audio.AttemptRender(effectiveTimeCode);

                    foreach (IVideoRenderManager video in this.videoStreamRenderManagers)
                        video.AttemptRender(effectiveTimeCode);

                    foreach (ISubtitleRenderManager video in this.subtitleStreamRenderManagers)
                        video.AttemptRender(effectiveTimeCode);
                }
            }
        }

        /// <summary>Stops the time playback</summary>
        protected virtual void StopTimer()
        {
            lock (this.timerLock)
                if (this.timer != null)
                    this.timer.Stop();
        }

        /// <summary>Starts the time playback</summary>
        protected virtual void StartTimer()
        {
            lock (this.timerLock)
                if (this.timer != null)
                    this.timer.Start();
        }
        #endregion
    }
}