using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels
{
    /// <summary>Repository class for shared pixel data calculations</summary>
    public static class PixelCalculations
    {
        /// <summary>Gets the size, in bytes, of a logical row of pixels based off of the bits per pixel</summary>
        /// <param name="bitsPerPixel">The bits per pixel of the row (compressed or decompressed)</param>
        /// <param name="width">Indicates the width of the logical image data</param>
        /// <returns>The byte width queried</returns>
        public static Int32 UnpackedRowByteWidth(Int32 bitsPerPixel, Int32 width)
        {
            Int32 rowSize = bitsPerPixel * width;                   //bits per row for data
            rowSize = (rowSize / 8) + ((rowSize % 8) > 0 ? 1 : 0);  //bytes per row
            return rowSize;
        }

        /// <summary>Gets the size, in bytes, of a logical row of pixels based off of the bits per pixel, packing them to a byte aligment size specfied</summary>
        /// <param name="bitsPerPixel">The bits per pixel of the row (compressed or decompressed)</param>
        /// <param name="packing">The packing of bytes to a single row</param>
        /// <param name="width">Indicates the width of the logical image data</param>
        /// <returns>The byte width queried</returns>
        public static Int32 PackedRowByteWidth(Int32 bitsPerPixel, Int32 packing, Int32 width)
        {
            Int32 rowSize = PixelCalculations.UnpackedRowByteWidth(bitsPerPixel, width);

            if (packing > 0)
                rowSize += (rowSize % packing);         //packed bytes per row

            return rowSize;
        }

        /// <summary>Gets the count of pixel rows in the binary data, packed</summary>
        /// <param name="packing">The packing of rows</param>
        /// <param name="height">Indicates the height of the logical image data</param>
        /// <returns>The byte width queried</returns>
        public static Int32 PackedRowCount(Int32 packing, Int32 height)
        {
            Int32 packed = height;        //count of rows;

            if (packing > 0)
                packed += height % packing;

            return packed;
        }
    }
}