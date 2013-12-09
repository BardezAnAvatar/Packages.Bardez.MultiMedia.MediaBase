using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.Multimedia.MediaBase.Data.Pixels
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
        public virtual MemoryStream ConvertData(MemoryStream data, PixelFormat sourceFormat, PixelFormat destinationFormat, Int32 horizontalPacking, Int32 verticalPacking, Int32 sourceWidth, Int32 sourceHeight, Int32 decodedBitDepth)
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

        /// <summary>Flips pixel data scan line by scan line</summary>
        /// <param name="data">Decompressed data to flip</param>
        /// <param name="metadata">Image metadata for the source frame</param>
        /// <returns>The vertically flipped data</returns>
        /// <remarks>Uses the current pixel data's packing, not the destination's</remarks>
        public virtual MemoryStream FlipVertical(MemoryStream data, ImageMetadata metadata)
        {
            Int32 rowLength = PixelCalculations.PackedRowByteWidth(metadata.ExpandedBitsPerPixel, metadata.HorizontalPacking, metadata.Width);
            MemoryStream destination = new MemoryStream(Convert.ToInt32(data.Length));

            //loop through half of the number of rows in the data
            Int32 rows = metadata.RowCount;

            for (Int32 row = 0; row < rows; ++row) // 5/2 = 2 is what I am looking for. Don't flip the middle row; that's just silly.
            {
                Int32 destOffset = ((rows - 1) - row) * rowLength;                  //destination in data
                Int32 sourceOffset = rowLength * row;                               //where to read from
                Byte[] buffer = data.ReadBytesAtOffset(sourceOffset, rowLength);    //read initial data
                destination.WriteAtOffset(destOffset, buffer);                      //copy opposite row to current row
            }

            return destination;
        }
        
        /// <summary>Adjusts the packing bytes of the data</summary>
        /// <param name="data">Data to pack</param>
        /// <param name="sourcePackingHorizontal">source horizontal packing</param>
        /// <param name="sourcePackingVertical">source vertical packing</param>
        /// <param name="destPackingHorizontal">target horizontal packing</param>
        /// <param name="destPackingVertical">target vertical packing</param>
        /// <param name="scanLineOrder">target scanline order</param>
        /// <returns>The adjusted packed bytes</returns>
        public virtual MemoryStream AdjustForPacking(MemoryStream data, ImageMetadata metadata, Int32 destPackingHorizontal, Int32 destPackingVertical)
        {
            //current and end widths
            Int32 destRowSize = PixelCalculations.PackedRowByteWidth(metadata.ExpandedBitsPerPixel, destPackingHorizontal, metadata.Width);
            Int32 destRowCount = PixelCalculations.PackedRowCount(destPackingVertical, metadata.Height);
            Int32 currentRowSize = PixelCalculations.PackedRowByteWidth(metadata.ExpandedBitsPerPixel, metadata.HorizontalPacking, metadata.Width);
            Int32 currentRowCount = PixelCalculations.PackedRowCount(metadata.VerticalPacking, metadata.Height);

            //width of actualy bytes to read for a given row
            Int32 byteWidth = ((metadata.BitsPerDataPixel / 8) * metadata.Width);

            Byte[] bitmapData = new Byte[destRowSize * destRowCount];

            //get the number of rows to copy
            Int32 rowCopyCount = currentRowCount < destRowCount ? currentRowCount : destRowCount;
            Int32 start = 0;

            //determine the start and end position for the copy loop
            switch (metadata.Order)
            {
                case ScanLineOrder.BottomUp:
                    start = destRowCount - rowCopyCount;
                    break;
                case ScanLineOrder.TopDown:
                    start = 0;
                    break;
            }

            //end loop condition
            Int32 end = start + rowCopyCount;

            //copy loop
            for (Int32 row = start; row < end; ++row)
            {
                Int32 sourceOffset = (row * currentRowSize);
                Int32 destOffset = (row * destRowSize);

                if (sourceOffset < 0)
                    sourceOffset = -sourceOffset;

                if (destOffset < 0)
                    destOffset = -destOffset;

                Byte[] source = data.ReadBytesAtOffset(sourceOffset, byteWidth);
                Array.Copy(source, 0, bitmapData, destOffset, byteWidth);
            }

            return new MemoryStream(bitmapData);
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