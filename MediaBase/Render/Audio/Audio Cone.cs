using System;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
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

        #region Low-pass filters (dunno what this actually means) (for muffling)
        /// <summary>LPF direct-path or reverb-path coefficient scaler on/within inner cone.</summary>
        public Single InnerLowPassFilterCoefficient { get; set; }

        /// <summary>LPF direct-path or reverb-path coefficient scaler outside inner cone.</summary>
        public Single OuterLowPassFilterCoefficient { get; set; }
        #endregion

        #region Reverbs (dunno what their effect would be)
        /// <summary>Reverb send level scaler on or within inner cone. This must be within 0.0f to 2.0f.</summary>
        public Single InnerReverbScaler;

        /// <summary>Reverb send level scaler on/beyond outer cone. This must be within 0.0f to 2.0f.</summary>
        public Single OuterReverbScaler;
        #endregion
        #endregion
    }
}