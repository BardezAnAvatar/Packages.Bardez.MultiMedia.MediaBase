using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Image
{
    /// <summary>Interface defining the common elements and operations for an image frame</summary>
    public interface IMultimediaImageFrame : IMultimediaFrame
    {
        #region Properties
        /// <summary>The group of metadata describing the representation details of the data stream</summary>
        ImageMetadata MetadataImage { get; }
        #endregion


        #region Methods
        /// <summary>Retrieves the pixel data in the specified format, in the specified scan line order</summary>
        /// <param name="pixelConverter">Interface used to convert the pixel data if necessary</param>
        /// <param name="format">Expected output format of the data</param>
        /// <param name="order">Expected scan line order of the output data</param>
        /// <param name="horizontalPacking">Horizontal packing of bytes for output</param>
        /// <param name="verticalPacking">Vertical packing of rows for output</param>
        /// <returns>Binary pixel data of the converted data</returns>
        MemoryStream GetFormattedData(IPixelConverter pixelConverter, PixelFormat format, ScanLineOrder order, Int32 horizontalPacking, Int32 verticalPacking);
        
        /// <summary>Gets a sub-image of the current image</summary>
        /// <param name="x">Source X position</param>
        /// <param name="y">Source Y position</param>
        /// <param name="width">Width of sub-image</param>
        /// <param name="height">Height of sub-image</param>
        /// <returns>The requested sub-image</returns>
        /// <remarks>The generated point of origin should be (0,0).</remarks>
        IMultimediaImageFrame GetSubImage(Int32 x, Int32 y, Int32 width, Int32 height);
        #endregion
    }
}