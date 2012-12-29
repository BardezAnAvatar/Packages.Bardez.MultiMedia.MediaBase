using System;

namespace Bardez.Projects.Multimedia.MediaBase.Container
{
    /// <summary>Represents the most basic information a container needs to expose about a data stream</summary>
    public class StreamProcessingInfo
    {
        #region Fields
        /// <summary>Index of this stream within the container</summary>
        public Int32 Index { get; set; }

        /// <summary>Flag indicating whether to process data from the stream this info represents.</summary>
        public Boolean Process { get; set; }

        /// <summary>Exposes the type of the multimedia stream in the container</summary>
        public StreamMediaType MediaType { get; set; }
        #endregion
    }
}