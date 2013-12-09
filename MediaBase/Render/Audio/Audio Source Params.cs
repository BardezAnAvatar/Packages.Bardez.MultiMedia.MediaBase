using System;
using System.Collections.Generic;

using Bardez.Projects.BasicStructures.ThreeDimensional;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>Represents source parameters for audio being rendered. Used for 3D audio.</summary>
    /// <remarks>This is an attempt to merge XAudio2 and OpenAL 3D effects</remarks>
    public class AudioSourceParams
    {
        #region Fields
        /// <summary>Audio emission source</summary>
        public Emitter Emitter { get; set; }

        /// <summary>Audio listening receptor</summary>
        public Listener Listener { get; set; }

        /// <summary>Audio properties for reverberation. If null, no reverb will be used.</summary>
        public ReverbSettings Reverberation { get; set; }
        #endregion
    }
}