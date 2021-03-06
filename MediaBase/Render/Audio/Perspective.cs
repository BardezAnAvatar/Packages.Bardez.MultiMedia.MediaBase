using System;
using System.Collections.Generic;

using Bardez.Projects.BasicStructures.ThreeDimensional;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>Common base class for Listener and Emitter.</summary>
    public class Perspective
    {
        #region Fields
        /// <summary>Forward orientation vector</summary>
        public Vector<Single> OrientationFront { get; set; }

        /// <summary>Vertical orientation vector</summary>
        /// <remarks>I do not see how this isn't covered by a single vector</remarks>
        public Vector<Single> OrientationVertical { get; set; }

        /// <summary>Position of the perspective</summary>
        public Vector<Single> Position { get; set; }

        /// <summary>Velocity of the perspective</summary>
        public Vector<Single> Velocity { get; set; }

        /// <summary>Cone of the major area of effect (emission or receipt)</summary>
        public AudioCone Cone { get; set; }
        #endregion
    }
}