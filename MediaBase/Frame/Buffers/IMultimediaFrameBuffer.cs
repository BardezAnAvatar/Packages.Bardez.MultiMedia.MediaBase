using System;

using Bardez.Projects.Multimedia.MediaBase.Frame;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Buffers
{
    /// <summary>This interface defines a buffer for Mutlimedia frames</summary>
    public interface IMultimediaFrameBuffer<FrameType> where FrameType : IMultimediaFrame
    {
        #region Properties
        /// <summary>Exposes a flag indicating whether or not the buffer is full</summary>
        Boolean BufferFull { get; }

        /// <summary>Exposes the size of the buffer available to be read.</summary>
        Int32 FrameCount { get; }

        /// <summary>Exposes the size of the buffer available to be written.</summary>
        Int32 FrameVacancy { get; }
        #endregion


        #region Stream Access
        /// <summary>Adds (pushes) a frame onto the stream</summary>
        /// <param name="frame">Frame to be pushed onto the stream's queue</param>
        void AddFrame(FrameType frame);

        /// <summary>Adds (pushes) a frame onto the stream</summary>
        /// <returns>null if no frames available</returns>
        FrameType PeekFrame();

        /// <summary>Consumes and removes (pops) the next frame from the stream</summary>
        /// <returns>null if no frames available</returns>
        FrameType ConsumeFrame();
        #endregion
    }
}