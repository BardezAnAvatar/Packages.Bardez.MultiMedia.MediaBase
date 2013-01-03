using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.MultiMedia.MediaBase.Video.Pixels;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.MultiMedia.MediaBase.Video.Pixels
{
    /// <summary>Basic pixel dat converter, not especially efficient. No external dependencies.</summary>
    public class BasicPixelConverter : IPixelConverter
    {
        #region IPixelConverter implementation
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
        public MemoryStream ConvertData(MemoryStream data, PixelFormat sourceFormat, PixelFormat destinationFormat, Int32 horizontalPacking, Int32 verticalPacking, Int32 sourceWidth, Int32 sourceHeight, Int32 decodedBitDepth)
        {
            MemoryStream converted = null;

            //convert data
            if (sourceFormat == PixelFormat.RGB_B8G8R8 && destinationFormat == PixelFormat.RGBA_B8G8R8A8)           //RGB BGR 24 -> RGBA BGRA 32
                converted = this.ConvertRgb24ToRgba(data, horizontalPacking, verticalPacking, sourceWidth, sourceHeight, decodedBitDepth);
            else if (sourceFormat == PixelFormat.YCbCrJpeg && destinationFormat == PixelFormat.RGBA_B8G8R8A8)       //YCbCr (JFIF)-> RGBA BGRA 32
                converted = this.ConvertJfifYCbCrToRgba(data, horizontalPacking, verticalPacking, sourceWidth, sourceHeight, decodedBitDepth);
            else if (sourceFormat == PixelFormat.RGB_B5G5R5X1 && destinationFormat == PixelFormat.RGBA_B8G8R8A8)    //RGB BGR 16 -> RGBA BGRA 32
                converted = this.ConvertRgb16_555_ToRgba(data, horizontalPacking, verticalPacking, sourceWidth, sourceHeight, decodedBitDepth);
            else
                throw new NotSupportedException("Other conversions not supported at this time.");

            return converted;
        }
        #endregion


        #region Conversion methods
        /// <summary>Converts RGB24 data to RGB32 data</summary>
        /// <param name="data">Decompressed data to convert</param>
        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        /// <param name="sourceWidth">Indicates the source width of the image data</param>
        /// <param name="sourceHeight">Indicates the source height of the image data</param>
        /// <returns>The converted data</returns>
        protected MemoryStream ConvertRgb24ToRgba(MemoryStream data, Int32 horizontalPacking, Int32 verticalPacking, Int32 sourceWidth, Int32 sourceHeight, Int32 decodedBitDepth)
        {
            Int32 dataRowWidth = PixelCalculations.PackedRowByteWidth(decodedBitDepth, horizontalPacking, sourceWidth);
            Int32 dataRowCount = PixelCalculations.PackedRowCount(verticalPacking, sourceHeight);
            Int32 newRowWidth = PixelCalculations.PackedRowByteWidth(RgbQuad.BitsPerPixel, horizontalPacking, sourceWidth);

            Byte[] rgba = new Byte[newRowWidth * dataRowCount];

            //constant speedup
            Int32 srcWidth = (sourceWidth * 3);

            //loop rows
            for (Int32 row = 0; row < dataRowCount; ++row)
            {
                Int32 srcDataOffset = (row * dataRowWidth);
                Int32 destDataOffset = (row * newRowWidth);
                Int32 srcPixelX = 0, destPixelX = 0;

                while (srcPixelX < srcWidth)
                {
                    //save a bit of time on additions?
                    Int32 destOffset = destDataOffset + destPixelX;
                    Int32 srcOffset = srcDataOffset + srcPixelX;

                    ReusableIO.SeekIfAble(data, srcOffset);

                    rgba[destOffset] = ReusableIO.BinaryReadByte(data);         //blue
                    rgba[destOffset + 1] = ReusableIO.BinaryReadByte(data);     //green
                    rgba[destOffset + 2] = ReusableIO.BinaryReadByte(data);     //red
                    rgba[destOffset + 3] = 255;                                 //alpha

                    //increment
                    srcPixelX += RgbTriplet.BytesPerPixel;
                    destPixelX += RgbQuad.BytesPerPixel;
                }
            }

            return new MemoryStream(rgba);
        }

        /// <summary>Converts RGB16 (red5, blue5, green5) data to RGB32 data</summary>
        /// <param name="data">Decompressed data to convert</param>
        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        /// <param name="sourceWidth">Indicates the source width of the image data</param>
        /// <param name="sourceHeight">Indicates the source height of the image data</param>
        /// <returns>The converted data</returns>
        protected MemoryStream ConvertRgb16_555_ToRgba(MemoryStream data, Int32 horizontalPacking, Int32 verticalPacking, Int32 sourceWidth, Int32 sourceHeight, Int32 decodedBitDepth)
        {
            Int32 dataRowWidth = PixelCalculations.PackedRowByteWidth(decodedBitDepth, horizontalPacking, sourceWidth);
            Int32 dataRowCount = PixelCalculations.PackedRowCount(verticalPacking, sourceHeight);
            Int32 newRowWidth = PixelCalculations.PackedRowByteWidth(RgbQuad.BitsPerPixel, horizontalPacking, sourceWidth);

            Byte[] rgba = new Byte[newRowWidth * dataRowCount];

            //constant speedup
            Int32 srcWidth = (sourceWidth * 2);

            //loop rows
            for (Int32 row = 0; row < dataRowCount; ++row)
            {
                Int32 srcDataOffset = (row * dataRowWidth);
                Int32 destDataOffset = (row * newRowWidth);
                Int32 srcPixelX = 0, destPixelX = 0;

                while (srcPixelX < srcWidth)
                {
                    //save a bit of time on additions?
                    Int32 destOffset = destDataOffset + destPixelX;
                    Int32 srcOffset = srcDataOffset + srcPixelX;

                    ReusableIO.SeekIfAble(data, srcOffset);
                    UInt16 pixel = ReusableIO.ReadUInt16FromStream(data);

                    /* 
                     * expecting Xrrrrrgggggbbbbb; the typical approach, I'm told, is to take
                     * the 3 most significant and use them for the missing least significant
                    */
                    Byte first = (Byte)(pixel & 0x001F);
                    Byte second = (Byte)((pixel & 0x03E0) >> 5);
                    Byte third = (Byte)((pixel & 0x7C00) >> 10);
                    Byte mask = 0x1C;   //top three bits of 5 bit nibble
                    first = (Byte)((first << 3) | ((first & mask) >> 2));
                    second = (Byte)((second << 3) | ((second & mask) >> 2));
                    third = (Byte)((third << 3) | ((third & mask) >> 2));

                    rgba[destOffset] = first;            //blue
                    rgba[destOffset + 1] = second;        //green
                    rgba[destOffset + 2] = third;      //red
                    rgba[destOffset + 3] = 255;                         //alpha

                    //increment
                    srcPixelX += 2;
                    destPixelX += RgbQuad.BytesPerPixel;
                }
            }

            return new MemoryStream(rgba);
        }

        /// <summary>Converts JFIF YCbCr data to RGB32 data</summary>
        /// <param name="data">Decompressed data to convert</param>
        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        /// <param name="sourceWidth">Indicates the source width of the image data</param>
        /// <param name="sourceHeight">Indicates the source height of the image data</param>
        /// <returns>The converted data</returns>
        protected MemoryStream ConvertJfifYCbCrToRgba(MemoryStream data, Int32 horizontalPacking, Int32 verticalPacking, Int32 sourceWidth, Int32 sourceHeight, Int32 decodedBitDepth)
        {
            Int32 dataRowWidth = PixelCalculations.PackedRowByteWidth(decodedBitDepth, horizontalPacking, sourceWidth);
            Int32 dataRowCount = PixelCalculations.PackedRowCount(verticalPacking, sourceHeight);
            Int32 newRowWidth = PixelCalculations.PackedRowByteWidth(RgbQuad.BitsPerPixel, horizontalPacking, sourceWidth);

            Byte[] rgba = new Byte[newRowWidth * dataRowCount];
            //loop rows
            for (Int32 row = 0; row < dataRowCount; ++row)
            {
                Int32 srcDataOffset = (row * dataRowWidth);
                Int32 destDataOffset = (row * newRowWidth);
                Int32 srcPixelX = 0, destPixelX = 0;

                while (srcPixelX < (sourceWidth * 3))
                {
                    //seek
                    ReusableIO.SeekIfAble(data, srcDataOffset + srcPixelX);
                    Double Y = ReusableIO.BinaryReadByte(data);
                    Double Cb = ReusableIO.BinaryReadByte(data);
                    Double Cr = ReusableIO.BinaryReadByte(data);

                    Double red = Y + ((Cr - 128) * 1.402);
                    Double green = Y - ((Cb - 128) * 0.34414) - ((Cr - 128) * 0.71414);
                    Double blue = Y + ((Cb - 128) * 1.772);

                    Byte redByte = red <= 0 ? (Byte)0 : red >= 255 ? (Byte)255 : Convert.ToByte(red);
                    Byte greenByte = green <= 0 ? (Byte)0 : green >= 255 ? (Byte)255 : Convert.ToByte(green);
                    Byte blueByte = blue <= 0 ? (Byte)0 : blue >= 255 ? (Byte)255 : Convert.ToByte(blue);

                    rgba[destDataOffset + destPixelX] = blueByte;       //blue
                    rgba[destDataOffset + destPixelX + 1] = greenByte;  //green
                    rgba[destDataOffset + destPixelX + 2] = redByte;    //red
                    rgba[destDataOffset + destPixelX + 3] = 255;        //alpha

                    //increment
                    srcPixelX += RgbTriplet.BytesPerPixel;
                    destPixelX += RgbQuad.BytesPerPixel;
                }
            }

            return new MemoryStream(rgba);
        }
        #endregion
    }
}