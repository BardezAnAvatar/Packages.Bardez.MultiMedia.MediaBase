using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

namespace Bardez.Projects.Multimedia.MediaBase.Data.Pixels
{
    /// <summary>Interface defining common methods for pixel conversion</summary>
    /// <remarks>LibAV will have its own converter, but should not be in MediaBase</remarks>
    public interface IPixelConverter
    {
        /// <summary>Converts data from the current format to another specified format</summary>
        /// <param name="data">Decompressed data to convert</param>
        /// <param name="sourceFormat">Format to convert from</param>
        /// <param name="destinationFormat">Format to convert to</param>
        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        /// <param name="sourceWidth">Indicates the source width of the image data</param>
        /// <param name="sourceHeight">Indicates the source height of the image data</param>
        /// <param name="decodedBitDepth">The bits per pixel once decoded</param>
        /// <returns>New byte data</returns>
        MemoryStream ConvertData(MemoryStream data, PixelFormat sourceFormat, PixelFormat destinationFormat, Int32 horizontalPacking, Int32 verticalPacking, Int32 sourceWidth, Int32 sourceHeight, Int32 decodedBitDepth);

        /// <summary>Flips pixel data scan line by scan line</summary>
        /// <param name="data">Decompressed data to flip</param>
        /// <param name="metadata">Image metadata for the source frame</param>
        /// <returns>The vertically flipped data</returns>
        /// <remarks>Uses the current pixel data's packing, not the destination's</remarks>
        MemoryStream FlipVertical(MemoryStream data, ImageMetadata metadata);

        /// <summary>Adjusts the packing bytes of the data</summary>
        /// <param name="data">Data to pack</param>
        /// <param name="metadata">Image metadata for the source frame</param>
        /// <param name="destPackingHorizontal">target horizontal packing</param>
        /// <param name="destPackingVertical">target vertical packing</param>
        /// <returns>The adjusted packed bytes</returns>
        MemoryStream AdjustForPacking(MemoryStream data, ImageMetadata metadata, Int32 destPackingHorizontal, Int32 destPackingVertical);
    }
}