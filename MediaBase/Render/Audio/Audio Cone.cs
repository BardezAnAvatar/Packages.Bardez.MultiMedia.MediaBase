using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Render.Audio
{
    /// <summary>Represents a cone of audio. Values are in Radians.</summary>
    public class AudioCone
    {
        #region Fields
        /// <summary>Inner angle of the audio cone.</summary>
        /// <value>Can be anywhere between 0.0F and 2 * pi</value>
        public Single InnerAngle { get; set; }

        /// <summary>Outer angle of the audio cone.</summary>
        /// <value>Can be anywhere between 0.0F and 2 * pi</value>
        public Single OuterAngle { get; set; }

        /// <summary>Represents the volume adjustment (or "Gain") of the audio inside of the inner cone</summary>
        /// <value>This may not be safe past 2.0, depending on the renderer</value>
        /// <remarks>"Gain" in OpenAL?</remarks>
        public Single InnerVolume { get; set; }

        /// <summary>Represents the volume adjustment (or "Gain") of the audio outside of the outer cone</summary>
        /// <value>This may not be safe past 1.0, depending on the renderer</value>
        /// <remarks>"ConeOuterGain" in OpenAL</remarks>
        public Single OuterVolume { get; set; }

        //TODO: Low-pass filters (dunno what this actually means) (for muffling)

        //TODO: reverbs (dunno what their effect would be)
        #endregion
    }
}