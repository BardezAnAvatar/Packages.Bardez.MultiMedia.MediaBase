using System;
using System.Collections.Generic;

using Bardez.Projects.BasicStructures.ThreeDimensional;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>Represents source parameters for audio being rendered. Used for 3D audio.</summary>
    /// <remarks>This is an attempt to merge XAudio2 and OpenAL 3D effects</remarks>
    public class AudioSourceParams
    {
        public Vector<Single> Orientation { get; set; }
        public Vector<Single> Position { get; set; }
        public Vector<Single> Velocity { get; set; }
        public Boolean MuffleSound { get; set; }
        public UInt32 CountChannels { get; set; }

        /// <summary>Distance of each channel from Position if CountChannels > 1</summary>
        public Single RadiusChannels { get; set; }

        /// <summary>
        ///     Collection of channel position Azimuths indicating where on the radius the channel
        ///     with the corresponding index is located. See: http://en.wikipedia.org/wiki/Azimuth
        /// </summary>
        public IList<Single> ChannelPositionAzimuths { get; set; }
    }
}