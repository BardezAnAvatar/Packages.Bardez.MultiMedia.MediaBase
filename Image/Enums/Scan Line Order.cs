using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Enums
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
