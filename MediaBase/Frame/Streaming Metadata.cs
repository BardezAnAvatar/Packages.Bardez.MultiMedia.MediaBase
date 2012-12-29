using System;

using Bardez.Projects.BasicStructures.Math;

namespace Bardez.Projects.Multimedia.MediaBase.Frame
{
    /// <summary>Common base for video, audio, subtitle, etc. streaming metadata</summary>
    public class StreamingMetadata
    {
        #region Fields
        /// <summary>Exposes the rate at which this stream is sampled</summary>
        public Rational SampleRate { get; set; }

        /// <summary>Exposes a <see cref="TimeSpan" /> that indicates when the frame should start to be rendered</summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>Exposes a <see cref="TimeSpan" /> that indicates when the frame should cease to be rendered</summary>
        public TimeSpan EndTime { get; set; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="sampleRate">Rate at which this stream is sampled</param>
        /// <param name="start">Start time of this frame</param>
        /// <param name="end">End time of this frame</param>
        public StreamingMetadata(Rational sampleRate, TimeSpan start, TimeSpan end)
        {
            this.SampleRate = sampleRate;
            this.StartTime = start;
            this.EndTime = end;
        }

        /// <summary>Flexible definition constructor</summary>
        /// <param name="numerator">Numerator for the rate at which this stream is sampled</param>
        /// <param name="denominator">Denominator for the rate at which this stream is sampled</param>
        /// <param name="start">Start time of this frame</param>
        /// <param name="end">End time of this frame</param>
        public StreamingMetadata(Int32 numerator, Int32 denominator, TimeSpan start, TimeSpan end)
            : this(new Rational(numerator, denominator), start, end) { }
        #endregion
    }
}