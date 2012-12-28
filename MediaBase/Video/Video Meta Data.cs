using System;

using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;

namespace Bardez.Projects.MultiMedia.MediaBase.Video
{
    /// <summary>Represents basic metadata for a video frame</summary>
    public class VideoMetaData
    {
        #region Fields
        /// <summary>Indicates the width of the logical image data</summary>
        public Int32 Width { get; set; }

        /// <summary>Indicates the height of the logical image data</summary>
        public Int32 Height { get; set; }

        /// <summary>Indicates the row byte alignment of the logical image data</summary>
        public Int32 HorizontalPacking { get; set; }

        /// <summary>Indicates the height row alignment of the logical image data</summary>
        public Int32 VerticalPacking { get; set; }

        /// <summary>Represents the number of bits per pixel in the data stream (as opposed to those in the palette)</summary>
        public Int32 BitsPerDataPixel { get; set; }

        /// <summary>Indicates the horizontal origin of the frame.</summary>
        public Int64 OriginX { get; set; }

        /// <summary>Indicates the vertical origin of the frame.</summary>
        public Int64 OriginY { get; set; }

        /// <summary>Indicates the data format of the video data</summary>
        public PixelFormat Format { get; set; }

        /// <summary>Indicates the order of scan lines in the pixel data</summary>
        public ScanLineOrder Order { get; set; }

        /// <summary>Represents the palette of pixel data.</summary>
        /// <value>null if the pixel data is raw for its format</value>
        public Palette DataPalette { get; set; }
        #endregion


        #region Properties
        /// <summary>Accesses the decompressed/raw output Bits per pixel</summary>
        public Int32 ExpandedBitsPerPixel
        {
            get { return this.DataPalette == null ? this.BitsPerDataPixel : this.DataPalette.BitsPerPixel; }
        }

        /// <summary>Gets the count of pixel rows in the binary data, packed</summary>
        public Int32 RowCount
        {
            get { return PixelCalculations.PackedRowCount(this.VerticalPacking, this.Height); }
        }

        /// <summary>Gets the packed size, in bytes, of a logical row of pixels based off of the bits per pixel</summary>
        public Int32 RowDataSize
        {
            get { return PixelCalculations.PackedRowByteWidth(this.ExpandedBitsPerPixel, this.HorizontalPacking, this.Width); }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing (bytes per row) of the data</param>
        /// <param name="heightPacking">Height packing (number of rows) of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream</param>
        /// <param name="originX">Indicates the horizontal origin of the frame</param>
        /// <param name="originY">Indicates the vertical origin of the frame</param>
        /// <param name="format">Indicates the data format of the pixels</param>
        /// <param name="order">Order of scan lines in the pixel data</param>
        /// <param name="palette">Palette of pixel data; nullable</param>
        public VideoMetaData(Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel, Int64 originX, Int64 originY, PixelFormat format, ScanLineOrder order, Palette palette = null)
        {
            this.Height = height;
            this.Width = width;
            this.HorizontalPacking = rowPacking;
            this.VerticalPacking = heightPacking;
            this.BitsPerDataPixel = bitsPerStreamPixel;
            this.OriginX = originX;
            this.OriginY = originY;
            this.Format = format;
            this.Order = order;
            this.DataPalette = palette;
        }
        #endregion


        #region Cloning
        /// <summary>Performs a deep copy of the object and returns another, separate instace of it.</summary>
        public VideoMetaData DeepClone()
        {
            return new VideoMetaData(this.Height, this.Width, this.HorizontalPacking, this.VerticalPacking, this.BitsPerDataPixel, this.OriginX, this.OriginY, this.Format, this.Order, this.DataPalette);
        }
        #endregion
    }
}