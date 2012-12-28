using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels
{
    /// <summary>Represents a color palette for palette-encoded pixel data or similar means.</summary>
    public class Palette
    {
        #region Fields
        /// <summary>Represents the palette color list</summary>
        /// <remarks>The data is fully decoded (i.e.: Y'UV4:4:4 as opposed to Y'UV4:2:2</remarks>
        protected IList<PixelBase> pixels;
        #endregion


        #region Properties
        /// <summary>Exposes the palette color list</summary>
        /// <remarks>The data is fully decoded (i.e.: Y'UV4:4:4 as opposed to Y'UV4:2:2</remarks>
        public IList<PixelBase> Pixels
        {
            get { return this.pixels; }
            set
            {
                this.pixels = value;
                this.GeneratePixelData();   //generate the appropriate Byte arrays
            }
        }

        /// <summary>Exposes the binary Byte Array values for the Pixels</summary>
        /// <remarks>Stored as a cache to avoid redundnant binary generation</remarks>
        public IList<Byte[]> PixelData { get; set; }

        /// <summary>The number of bits per pixel represented in this palette</summary>
        public Int32 BitsPerPixel { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Palette() { }

        /// <summary>Definition constructor</summary>
        public Palette(Int32 bitsPerPixel, IList<PixelBase> pixelValues)
        {
            this.BitsPerPixel = bitsPerPixel;
            this.Pixels = pixelValues;
        }
        #endregion


        #region Helpers
        /// <summary>Generates the Byte Arrays for</summary>
        public virtual void GeneratePixelData()
        {
            //Generate Binary data
            List<Byte[]> pixelBytes = new List<Byte[]>();
            foreach (PixelBase pixel in this.Pixels)
                pixelBytes.Add(pixel == null ? null : pixel.GetBytes());

            this.PixelData = pixelBytes;
        }
        #endregion
    }
}