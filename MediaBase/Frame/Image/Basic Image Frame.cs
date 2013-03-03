using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Video;
using Bardez.Projects.Multimedia.MediaBase.Video.Pixels;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Image
{
    /// <summary>Represents a single image instance, be it a bitmap, PNG, JPEG/JFIF or frame of animation from an audio/video movie or animated picture</summary>
    public class BasicImageFrame : IMultimediaImageFrame
    {
        #region Properties
        /// <summary>Exposes the underlying image binary data, possibly interpreted.</summary>
        public PixelData Pixels { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BasicImageFrame() : this(null) { }

        /// <summary>definition constructor</summary>
        /// <param name="data">PixelData structure defining and containing the pixel data</param>
        public BasicImageFrame(PixelData data)
        {
            this.Pixels = data;
        }
        #endregion


        #region IMultimediaImageFrame implementation
        /// <summary>Exposes the data of a multimedia frame</summary>
        public Stream Data
        {
            get { return this.Pixels.NativeBinaryData; }
        }

        /// <summary>The group of metadata describing the representation details of the data stream</summary>
        public ImageMetadata MetadataImage
        {
            get { return this.Pixels.Metadata; }
        }
        
        /// <summary>Retrieves the pixel data in the specified format, in the specified scan line order</summary>
        /// <param name="pixelConverter">Interface used to convert the pixel data if necessary</param>
        /// <param name="format">Expected output format of the data</param>
        /// <param name="order">Expected scan line order of the output data</param>
        /// <param name="horizontalPacking">Horizontal packing of bytes for output</param>
        /// <param name="verticalPacking">Vertical packing of rows for output</param>
        /// <returns>Binary pixel data of the converted data</returns>
        public MemoryStream GetFormattedData(IPixelConverter pixelConverter, PixelFormat format, ScanLineOrder order, Int32 horizontalPacking, Int32 verticalPacking)
        {
            return this.Pixels.GetPixelData(pixelConverter, format, order, horizontalPacking, verticalPacking);
        }
        #endregion


        #region IDisposable
        /// <summary>Disposes of this object</summary>
        public void Dispose()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>Dispose pattern for the underlying stream</summary>
        /// <param name="managedObjectsStillExist">Flag indicating whether the object is deterministically disposing or is finalizing</param>
        protected virtual void Dispose(Boolean managedObjectsStillExist)
        {
            if (managedObjectsStillExist)
            {
                //managed resources
                if (this.Pixels.NativeBinaryData != null)
                {
                    this.Pixels.NativeBinaryData.Dispose();
                    this.Pixels.NativeBinaryData = null;
                }

                this.Pixels.Metadata.DataPalette = null;
                this.Pixels.Metadata = null;
                this.Pixels = null;
            }
        }
        #endregion
    }
}