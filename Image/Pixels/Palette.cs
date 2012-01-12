using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Pixels
{
    /// <summary>Represents a color palette for palette-encoded pixel data or similar means.</summary>
    public class Palette
    {
        #region Properties
        /// <summary>Exposes the palette color list</summary>
        /// <remarks>The data is fully decoded (i.e.: Y'UV4:4:4 as opposed to Y'UV4:2:2</remarks>
        public List<PixelBase> Pixels { get; set; }

        /// <summary>The number of bits per pixel represented in this palette</summary>
        public Int32 BitsPerPixel { get; set; }
        #endregion
    }
}