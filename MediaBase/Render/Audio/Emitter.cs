using System;
using System.Collections.Generic;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>
    ///     Represents the emission of sound(s) in virtual space.
    ///     This would, for example, be a jet plane emitting the sonic boom heard by an observer
    /// </summary>
    /// <remarks>As much as I dislike following Microsoft conventions over others, I love the 'Emitter' name.</remarks>
    public class Emitter
    {
        #region Fields
        /// <summary>Represents the cone in which the sound is emitted. Null indicates an omni directional sound emission.</summary>
        public AudioCone Cone { get; set; }

        /// <summary>Indicates a series of distances at which a sound attenuates in strength</summary>
        /// <value>Null indicates default, which is inverse square</value>
        /// <remarks>Not supported by OpenAL?</remarks>
        public IList<DistanceCurvePoint> DistanceCurveVolume { get; set; }

        /// <summary>Indicates a series of distances at which a Low frequency effect (bass) sound attenuates or gains in strength</summary>
        /// <value>Null indicates default, which is inverse square</value>
        /// <remarks>Not supported by OpenAL?</remarks>
        public IList<DistanceCurvePoint> DistanceCurveLowFrequencyEffectRollOff { get; set; }

        /// <summary>Indicates a series of distances at which a Low frequency effect (bass) sound attenuates or gains in strength</summary>
        /// <value>Null indicates default, which is inverse square</value>
        /// <remarks>Not supported by OpenAL?</remarks>
        public IList<DistanceCurvePoint> DistanceCurveLowPassFilterDirectPathCoefficient { get; set; }
        public IList<DistanceCurvePoint> DistanceCurveLowPassFilterReverb { get; set; }

        /// <summary>Indicates a series of distances at which a Low frequency effect (bass) sound attenuates or gains in strength</summary>
        /// <value>Null indicates default, which is inverse square</value>
        /// <remarks>Not supported by OpenAL?</remarks>
        public IList<DistanceCurvePoint> DistanceCurveReverb { get; set; }

        /// <summary>
        ///     Represents the value (indicating a distance) at which sound effects begin attenuating.
        /// </summary>
        /// <remarks>
        ///     I believe this is what translates world units into meters.
        ///     So, distance of "5.0" feet means that 5 x 0.3048. This scaler would be 0.3048,
        ///     and the distance curve value would be 5.0
        /// </remarks>
        public Single ScalerCurveDistance { get; set; }

        public Single ScalerDoppler { get; set; }
        #endregion
    }
}