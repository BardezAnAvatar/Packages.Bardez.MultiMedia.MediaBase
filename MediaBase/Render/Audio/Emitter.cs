using System;
using System.Collections.Generic;

using Bardez.Projects.BasicStructures.ThreeDimensional;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>
    ///     Represents the emission of sound(s) in virtual space.
    ///     This would, for example, be a jet plane emitting the sonic boom heard by an observer
    /// </summary>
    /// <remarks>
    ///     1) As much as I dislike following Microsoft conventions over others, I love the 'Emitter' name.
    ///     2) I think it would be best for an engine to populate all possible values it might ever care about,
    ///     and let the renderer consider or ignore its fields as it sees fit. As such, I will include all the
    ///     XAudio2 extensions, and let OpenAL add/ignore fields as it sees fit.
    /// </remarks>
    public class Emitter : Perspective
    {
        #region Fields
        /// <summary>Radius of the most major effect</summary>
        public Single RadiusInner { get; set; }

        /// <summary>Angle of the most inner radius</summary>
        public Single RadiusAngleInner { get; set; }

        /// <summary>Number of emitters for this sound</summary>
        public UInt32 ChannelCount { get; set; }

        /// <summary>Distance of each channel from Position if CountChannels > 1</summary>
        public Single RadiusChannel { get; set; }

        /// <summary>
        ///     Collection of channel position Azimuths indicating where on the radius the channel
        ///     with the corresponding index is located. See: http://en.wikipedia.org/wiki/Azimuth
        /// </summary>
        public IList<Single> ChannelPositionAzimuths { get; set; }

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

        /// <summary>LPF reverb-path coefficient distance curve. Used for LPF reverb path calculations.</summary>
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

        /// <summary>Doppler shift scaler that is used to exaggerate Doppler shift effect.</summary>
        public Single ScalerDoppler { get; set; }
        #endregion
    }
}