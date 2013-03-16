using System;
using System.Collections.Generic;

using Bardez.Projects.BasicStructures.ThreeDimensional;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>Represents source parameters for audio being rendered. Used for 3D audio.</summary>
    /// <remarks>This is an attempt to merge XAudio2 and OpenAL 3D effects</remarks>
    public class AudioSourceParams
    {
        /// <summary>Audio emission source</summary>
        public Emitter Emitter { get; set; }

        /// <summary>Audio listening receptor</summary>
        public Listener Listener { get; set; }

        /// <summary>Not sure that this is needed</summary>
        public Boolean MuffleSound { get; set; }
    }
}