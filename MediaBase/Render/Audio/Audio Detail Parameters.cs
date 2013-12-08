using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>Represents source parameters for audio being rendered. Used for audio effects.</summary>
    /// <remarks>This is an attempt to merge XAudio2 and OpenAL 3D effects</remarks>
    public class AudioDetailParameters
    {
        /// <summary>Audio emission source</summary>
        public Emitter Emitter { get; set; }

        /// <summary>Audio listening receptor</summary>
        public Listener Listener { get; set; }

        /// <summary>Not sure that this is needed</summary>
        public Boolean MuffleSound { get; set; }


        //TODO: Support 3D effects (reverberation)
        //  I need a reverb setting at the least. Not sure how things will go from there with additional programmed filters/effects
    }
}