using System;

using Bardez.Projects.BasicStructures.ThreeDimensional;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>
    ///     Represents the location in virtual space that receives audio.
    ///     This would, for example, be an observer for a jet plane's sonic boom
    /// </summary>
    public class Listener
    {
        #region Fields
        /// <summary>The "Up" or "Top" component to orientation</summary>
        public Vector<Single> OrientationUp { get; set; }

        /// <summary>The forward-facing component to orientation</summary>
        public Vector<Single> OrientationForward { get; set; }

        /// <summary>Position within a 3-dimensional space</summary>
        public Vector<Single> Position { get; set; }

        /// <summary>Rate of movement within a 3-dimensional space</summary>
        public Vector<Single> Velocity { get; set; }

        /// <summary>Represents the receiving cone of a sound.</summary>
        /// <remarks>OpenAL does not seem to support this. Null is omnidirectional sound receipt. I will not want this for Infinity +1. But it's there for XAudio2.</remarks>
        public AudioCone Cone { get; set; }
        #endregion
    }
}