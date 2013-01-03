using System;

namespace Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums
{
    /// <summary>Indicates the order in which pixel data comes down the line</summary>
    public enum ScanLineOrder
    {
        /// <summary>The bottom scan line comes first</summary>
        BottomUp,

        /// <summary>The top scan line comes first</summary>
        TopDown
    }
}