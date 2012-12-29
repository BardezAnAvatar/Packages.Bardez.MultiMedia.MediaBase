using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>Represents a point on a distance curve and its value</summary>
    public class DistanceCurvePoint
    {
        #region Fields
        /// <summary>Normalized distance at which the setting takes effect</summary>
        /// <value>Between 0.0 and 1.0</value>
        public Single Distance { get; set; }

        /// <summary>Control setting value that takes effect</summary>
        public Single Setting { get; set; }
        #endregion
    }
}