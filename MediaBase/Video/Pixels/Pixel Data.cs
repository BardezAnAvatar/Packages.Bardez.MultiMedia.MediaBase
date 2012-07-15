using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.MultiMedia.LibAV;
using Bardez.Projects.ReusableCode;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels
{
    /// <summary>Represents a single-source for an image's pixel data. This class will record the input type, and output pixel data as requested, converting as necessary</summary>
    public class PixelData
    {
        #region Helper Objects
        /// <summary>Exposes grouped metadata</summary>
        public class MetaData
        {
            #region Properties
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
            #endregion


            #region Construction
            /// <summary>Default constructor</summary>
            public MetaData() { }

            /// <summary>Definition constructor</summary>
            /// <param name="height">Height of the data</param>
            /// <param name="width">Width of the data</param>
            /// <param name="rowPacking">Row packing of the data</param>
            /// <param name="heightPacking">Height packing of the data</param>
            /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream</param>
            public MetaData(Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel)
            {
                this.Height = height;
                this.Width = width;
                this.HorizontalPacking = rowPacking;
                this.VerticalPacking = heightPacking;
                this.BitsPerDataPixel = bitsPerStreamPixel;
            }
            #endregion


            #region Cloning
            /// <summary>Performs a deep copy of the object and returns another, separate instace of it.</summary>
            public MetaData Clone()
            {
                return new MetaData(this.Height, this.Width, this.HorizontalPacking, this.VerticalPacking, this.BitsPerDataPixel);
            }
            #endregion
        }
        #endregion


        #region Fields
        /// <summary>Exposes the group of metadata</summary>
        public MetaData Metadata { get; set; }

        /// <summary>Indicates the data format of the pixels</summary>
        public PixelFormat Format { get; set; }

        /// <summary>Indicates the order of scan lines in the pixel data</summary>
        public ScanLineOrder Order { get; set; }

        /// <summary>Represents the palette of pixel data.</summary>
        /// <value>null if the pixel data is raw for its format</value>
        public Palette DataPalette { get; set; }

        /// <summary>Represents the binary array of pixel data read from source.</summary>
        /// <value>The data must be either palette-indexed or fully decoded to a multiple of 8 bits. Y'UV 4:2:2 would not be acceptable, not Y'V12, but Y'UV 4:4:4 would be.</value>
        private MemoryStream nativeBinaryData;
        #endregion


        #region Properties
        /// <summary>Gets the packed size, in bytes, of a logical row of pixels based off of the bits per pixel</summary>
        protected Int32 RowDataSize
        {
            get { return this.PackedRowByteWidth(this.ExpandedBitsPerPixel, this.Metadata.HorizontalPacking); }
        }

        /// <summary>The byte width of a single row in this Pixel Data</summary>
        protected Int32 RowByteWidth
        {
            get
            {
                Int32 rowSize = this.ExpandedBitsPerPixel * this.Metadata.Width;     //bits per row for data
                rowSize = (rowSize / 8) + ((rowSize % 8) > 0 ? 1 : 0);  //bytes per row
                return rowSize;
            }
        }

        /// <summary>Accesses the decompressed/raw output Bits per pixel</summary>
        protected Int32 ExpandedBitsPerPixel
        {
            get { return this.DataPalette == null ? this.Metadata.BitsPerDataPixel : this.DataPalette.BitsPerPixel; }
        }

        /// <summary>Gets the count of pixel rows in the binary data, packed</summary>
        protected Int32 RowCount
        {
            get { return this.PackedRowCount(this.Metadata.VerticalPacking); }
        }

        /// <summary>Represents the binary array of pixel data read from source.</summary>
        /// <value>The data must be either palette-indexed or fully decoded to a multiple of 8 bits. Y'UV 4:2:2 would not be acceptable, not Y'V12, but Y'UV 4:4:4 would be.</value>
        public MemoryStream NativeBinaryData
        {
            get { return this.nativeBinaryData; }
            set
            {
                if (this.nativeBinaryData != null)
                    this.nativeBinaryData.Dispose();

                this.nativeBinaryData = value;
            }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor, assumes no palette</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="order">Scan line order of the data</param>
        /// <param name="format">Pixel format of the data</param>
        /// <param name="palette">Data palette of the data</param>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing of the data</param>
        /// <param name="heightPacking">Height packing of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream. If paletted, use index size.</param>
        public PixelData(Byte[] binary, ScanLineOrder order, PixelFormat format, Palette palette, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel)
        {
            this.Format = format;
            this.Order = order;
            this.NativeBinaryData = new MemoryStream(binary);
            this.DataPalette = palette;
            this.Metadata = new MetaData(height, width, rowPacking, heightPacking, bitsPerStreamPixel);
        }

        /// <summary>Definition constructor, assumes no palette</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="order">Scan line order of the data</param>
        /// <param name="format">Pixel format of the data</param>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing of the data</param>
        /// <param name="heightPacking">Height packing of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream</param>
        public PixelData(Byte[] binary, ScanLineOrder order, PixelFormat format, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel)
            : this(binary, order, format, null, height, width, rowPacking, heightPacking, bitsPerStreamPixel) { }

        /// <summary>Definition constructor, assumes no palette, RGBA and bottom up order</summary>
        /// <param name="binary">Byte data of the pixel data</param>
        /// <param name="order">Scan line order of the data</param>
        /// <param name="format">Pixel format of the data</param>
        /// <param name="palette">Data palette of the data</param>
        /// <param name="height">Height of the data</param>
        /// <param name="width">Width of the data</param>
        /// <param name="rowPacking">Row packing of the data</param>
        /// <param name="heightPacking">Height packing of the data</param>
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream</param>
        public PixelData(Byte[] binary, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel)
            : this(binary, ScanLineOrder.BottomUp, PixelFormat.RGBA_R8G8B8A8, null, height, width, rowPacking, heightPacking, bitsPerStreamPixel) { }
        #endregion


        #region Methods
        /// <summary>Gets the size, in bytes, of a logical row of pixels based off of the bits per pixel</summary>
        /// <param name="bitsPerPixel">The bits per pixel of the row (compressed or decompressed)</param>
        /// <returns>The byte width queried</returns>
        protected Int32 UnpackedRowByteWidth(Int32 bitsPerPixel)
        {
            Int32 rowSize = bitsPerPixel * this.Metadata.Width;     //bits per row for data
            rowSize = (rowSize / 8) + ((rowSize % 8) > 0 ? 1 : 0);  //bytes per row
            return rowSize;
        }

        /// <summary>Gets the size, in bytes, of a logical row of pixels based off of the bits per pixel, packing them to a byte aligment size specfied</summary>
        /// <param name="bitsPerPixel">The bits per pixel of the row (compressed or decompressed)</param>
        /// <param name="packing">The packing of bytes to a single row</param>
        /// <returns>The byte width queried</returns>
        protected Int32 PackedRowByteWidth(Int32 bitsPerPixel, Int32 packing)
        {
            Int32 rowSize = this.UnpackedRowByteWidth(bitsPerPixel);

            if (packing > 0)
                rowSize += (rowSize % packing);                     //packed bytes per row

            return rowSize;
        }

        /// <summary>Gets the count of pixel rows in the binary data, packed</summary>
        /// <param name="packing">The packing of rows</param>
        /// <returns>The byte width queried</returns>
        protected Int32 PackedRowCount(Int32 packing)
        {
            Int32 packed = this.Metadata.Height;        //count of rows;

            if (packing > 0)
                packed += this.Metadata.Height % packing;
            
            return packed;
        }

        /// <summary>Decodes the palette data to a decoded binary data Byte array</summary>
        /// <returns>The palette-decoded pixel data, aligned to the current row order</returns>
        protected MemoryStream DecodePaletteData()
        {
            MemoryStream bitmapData = null;

            if (this.DataPalette != null)
            {
                Int32 rowSize = this.RowDataSize;

                bitmapData = new MemoryStream(rowSize * this.RowCount);

                //now, interpret the pixel data
                for (Int32 row = 0; row < this.Metadata.Height; ++row)
                {
                    Byte[] interim = this.DecodePaletteDataRow(row);  //read a row of pixels
                    bitmapData.WriteAtOffset(row * rowSize, interim);
                }
            }

            return bitmapData;
        }

        /// <summary>Decodes a row of palette data to a decoded binary data Byte array</summary>
        /// <param name="row">Row number to read</param>
        /// <returns>The palette-decoded pixel data row</returns>
        /// <remarks>The decoded data will share the same packing as the encoded; if a row is packed to 4 bytes, the decoded data is also. Same with vertical resolution.</remarks>
        protected Byte[] DecodePaletteDataRow(Int32 row)
        {
            //get starting position
            Byte[] decompressed = new Byte[this.PackedRowByteWidth(this.DataPalette.BitsPerPixel, this.Metadata.HorizontalPacking)];
            Int32 location = row * this.PackedRowByteWidth(this.Metadata.BitsPerDataPixel, this.Metadata.HorizontalPacking);

            //decompress data (yay!)
            //read the packed data, 1 value at a time, and write the palette entry to the decompressed stream.
            //the only valid palettes I know are 8 bit or less, being 1, 2, 4, 8. I'm going to only implement those, for now.

            if (this.Metadata.BitsPerDataPixel == 8)    //optimized method
                this.DecodePaletteDataRow8Bit(decompressed, location);
            else
                this.DecodePaletteDataRow2nBit(decompressed, location);

            return decompressed;
        }


        #region DecodePalette methods
        /// <summary>Decodes a row of a generic 2^n-bit palette data to a decoded binary data Byte array</summary>
        /// <param name="decompressed">Decompressed Byte array to write to</param>
        /// <param name="location">Location in source stream to start from</param>
        protected void DecodePaletteDataRow2nBit(Byte[] decompressed, Int32 location)
        {
            //Metadata fetch
            Int32 width = this.Metadata.Width;
            Int32 bpp = this.Metadata.BitsPerDataPixel;

            Int32 xByte = 0, xBits = 0;

            //stored references to speed up code
            IList<Byte[]> pixBinData = this.DataPalette.PixelData;
            MemoryStream nativeData = this.NativeBinaryData;

            for (Int32 x = 0; x < width; ++x)
            {
                Byte value = nativeData.ReadByteAtOffset(location + xByte);   //get the current byte, assign it to a shiftable variable
                xBits += bpp;

                //wrap as needed
                if (xBits > 7)
                    ++xByte;

                if (xBits > 8)
                    xBits -= 8;

                //Bit shifting.
                Int32 xBitsMinusData = xBits - bpp;
                Byte mask = (Byte)((0xFF >> (8 - xBits)) << xBitsMinusData);
                Byte index = (Byte)((value &= mask) >> xBitsMinusData);

                //now we need the pixel.
                Byte[] data = pixBinData[index];
                Int32 len = data.Length;

                Int32 position = len * x;

                //Array.Copy is slow here...
                for (Int32 i = 0; i < len; ++i)
                    decompressed[position + i] = data[i];
            }
        }

        /// <summary>Decodes a row of 8-bit palette data to a decoded binary data Byte array</summary>
        /// <param name="decompressed">Decompressed Byte array to write to</param>
        /// <param name="location">Location in source stream to start from</param>
        protected virtual void DecodePaletteDataRow8Bit(Byte[] decompressed, Int32 sourceLocation)
        {
            //stored references to speed up code
            IList<Byte[]> pixBinData = this.DataPalette.PixelData;
            MemoryStream nativeData = this.NativeBinaryData;
            Int32 destPosition = 0;
            Int32 dataLen = this.DataPalette.BitsPerPixel / 8;  //might be a hack
            Int32 end = sourceLocation + this.Metadata.Width;

            for (; sourceLocation < end; ++sourceLocation)
            {
                Byte value = nativeData.ReadByteAtOffset(sourceLocation);   //get the current byte, assign it to a shiftable variable

                //now we need the pixel.
                Byte[] data = pixBinData[value];

                //Array.Copy is slow here...
                for (Int32 i = 0; i < dataLen; ++i)
                    decompressed[destPosition++] = data[i];
            }
        }
        #endregion


        /// <summary>Flips pixel data scan line by scan line</summary>
        /// <param name="data">Decompressed data to flip</param>
        /// <returns>The vertically flipped data</returns>
        /// <remarks>Uses the current pixel data's packing, not the destination's</remarks>
        protected MemoryStream FlipVertical(MemoryStream data)
        {
            Int32 rowLength = this.PackedRowByteWidth(this.ExpandedBitsPerPixel, this.Metadata.HorizontalPacking);
            MemoryStream destination = new MemoryStream(Convert.ToInt32(data.Length));

            //loop through half of the number of rows in the data
            Int32 rows = this.RowCount;

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
        protected MemoryStream AdjustForPacking(MemoryStream data, Int32 sourcePackingHorizontal, Int32 sourcePackingVertical, Int32 destPackingHorizontal, Int32 destPackingVertical, ScanLineOrder scanLineOrder)
        {
            //current and end widths
            Int32 destRowSize = this.PackedRowByteWidth(this.ExpandedBitsPerPixel, destPackingHorizontal);
            Int32 destRowCount = this.PackedRowCount(destPackingVertical);
            Int32 currentRowSize = this.PackedRowByteWidth(this.ExpandedBitsPerPixel, sourcePackingHorizontal);
            Int32 currentRowCount = this.PackedRowCount(sourcePackingVertical);

            //width of actualy bytes to read for a given row
            Int32 byteWidth = ((this.Metadata.BitsPerDataPixel / 8) * this.Metadata.Width);

            Byte[] bitmapData = new Byte[destRowSize * destRowCount];

            //get the number of rows to copy
            Int32 rowCopyCount = currentRowCount < destRowCount ? currentRowCount : destRowCount;
            Int32 start = 0;

            //determine the start and end position for the copy loop
            switch (scanLineOrder)
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

        /// <summary>Converts data from the current format to another specified format</summary>
        /// <param name="data">Decompressed data to convert</param>
        /// <param name="formatStart">Format to convert from</param>
        /// <param name="formatEnd">Format to convert to</param>
        /// <returns>New byte data</returns>
        protected MemoryStream ConvertData(MemoryStream inputData, PixelFormat formatStart, PixelFormat formatEnd)
        {
            MemoryStream result = null;

            //translate pixel formats
            LibAVPixelFormat sourceFormat = formatStart.ToLibAVPixelFormat();
            LibAVPixelFormat destFormat = formatEnd.ToLibAVPixelFormat();

            using (SWScale scale = new SWScale(
                        LibAVPictureDetail.Build(this.Metadata.Width, this.Metadata.Height, sourceFormat),
                        LibAVPictureDetail.Build(this.Metadata.Width, this.Metadata.Height, destFormat),
                        ResizeOptions.Lanczos))
            {
                MemoryStream outStream = null;
                scale.Transform(inputData, out outStream);
                result = outStream;
            }

            return result;
        }

        /// <summary>Retrieves the pixel data in the specified format, in the specified scan line order</summary>
        /// <param name="format">Expected output format of the data</param>
        /// <param name="order">Expected scan line order of the output data</param>
        /// <param name="horizontalPacking">Horizontal packing of bytes for output</param>
        /// <param name="verticalPacking">Vertical packing of rows for output</param>
        /// <returns>Binary pixel dataof the converted data</returns>
        public MemoryStream GetPixelData(PixelFormat format, ScanLineOrder order, Int32 horizontalPacking, Int32 verticalPacking)
        {
            MemoryStream dataStream = this.NativeBinaryData;

            //decode data from palette
            if (this.DataPalette != null)
                dataStream = this.DecodePaletteData();

            //flip if necessary
            if (this.Order != order)
                dataStream = this.FlipVertical(dataStream);    //vertically swap each pixel row

            //strip packing
            if (this.Metadata.HorizontalPacking != 0 || this.Metadata.VerticalPacking != 0)
                dataStream = this.AdjustForPacking(dataStream, this.Metadata.HorizontalPacking, this.Metadata.VerticalPacking, 0, 0, order);     // it should know the existing packing for this instance, specify the new packing

            //convert as necessary
            if (format != this.Format)
                dataStream = this.ConvertData(dataStream, this.Format, format);

            //widen data to the destination horizontal and vertical packing specified
            if (horizontalPacking != 0 || verticalPacking != 0)
                dataStream = this.AdjustForPacking(dataStream, 0, 0, horizontalPacking, verticalPacking, order);     // it should know the existing packing for this instance, specify the new packing

            //in a return state
            return dataStream;
        }
        #endregion


        #region Conversion methods
        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        protected Byte[] ConvertRgb24ToRgba(Byte[] data, Int32 horizontalPacking, Int32 verticalPacking)
        {
            Int32 dataRowWidth = this.PackedRowByteWidth(this.ExpandedBitsPerPixel, horizontalPacking);
            Int32 dataRowCount = this.PackedRowCount(verticalPacking);
            Int32 newRowWidth = this.PackedRowByteWidth(RgbQuad.BitsPerPixel, horizontalPacking);

            Byte[] rgba = new Byte[newRowWidth * dataRowCount];

            //constant speedup
            Int32 srcWidth = (this.Metadata.Width * 3);

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
                    Int32 srcOffset= srcDataOffset + srcPixelX;

                    rgba[destOffset] = data[srcOffset];             //blue
                    rgba[destOffset + 1] = data[srcOffset + 1];     //green
                    rgba[destOffset + 2] = data[srcOffset + 2];     //red
                    rgba[destOffset + 3] = 255;                     //alpha

                    //increment
                    srcPixelX += RgbTriplet.BytesPerPixel;
                    destPixelX += RgbQuad.BytesPerPixel;
                }
            }

            return rgba;
        }

        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        protected Byte[] ConvertRgb16_555_ToRgba(Byte[] data, Int32 horizontalPacking, Int32 verticalPacking)
        {
            Int32 dataRowWidth = this.PackedRowByteWidth(this.ExpandedBitsPerPixel, horizontalPacking);
            Int32 dataRowCount = this.PackedRowCount(verticalPacking);
            Int32 newRowWidth = this.PackedRowByteWidth(RgbQuad.BitsPerPixel, horizontalPacking);

            Byte[] rgba = new Byte[newRowWidth * dataRowCount];

            //constant speedup
            Int32 srcWidth = (this.Metadata.Width * 2);

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
                    Int32 srcOffset= srcDataOffset + srcPixelX;

                    UInt16 pixel = ReusableIO.ReadUInt16FromArray(data, srcOffset);

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

            return rgba;
        }

        protected Byte[] ConvertJfifYCbCrToRgba(Byte[] data, Int32 horizontalPacking, Int32 verticalPacking)
        {
            Int32 dataRowWidth = this.PackedRowByteWidth(this.ExpandedBitsPerPixel, horizontalPacking);
            Int32 dataRowCount = this.PackedRowCount(verticalPacking);
            Int32 newRowWidth = this.PackedRowByteWidth(RgbQuad.BitsPerPixel, horizontalPacking);

            Byte[] rgba = new Byte[newRowWidth * dataRowCount];
            //loop rows
            for (Int32 row = 0; row < dataRowCount; ++row)
            {
                Int32 srcDataOffset = (row * dataRowWidth);
                Int32 destDataOffset = (row * newRowWidth);
                Int32 srcPixelX = 0, destPixelX = 0;

                while (srcPixelX < (this.Metadata.Width * 3))
                {
                    Double Y = data[srcDataOffset + srcPixelX];
                    Double Cb = data[srcDataOffset + srcPixelX + 1];
                    Double Cr = data[srcDataOffset + srcPixelX + 2];
                    Double red      = Y + ((Cr - 128) * 1.402);
                    Double green    = Y - ((Cb - 128) * 0.34414) - ((Cr - 128) * 0.71414);
                    Double blue     = Y + ((Cb - 128) * 1.772);
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

            return rgba;
        }
        #endregion


        #region Cloning
        /// <summary>Performs a deep copy of the object and returns another, separate instance of it.</summary>
        public PixelData Clone()
        {
            Byte[] data = this.NativeBinaryData.ToArray();
            return new PixelData(data, this.Order, this.Format, this.DataPalette, this.Metadata.Height, this.Metadata.Width, this.Metadata.HorizontalPacking, this.Metadata.VerticalPacking, this.Metadata.BitsPerDataPixel);
        }
        #endregion


        #region Stream closing
        /// <summary>Assigns the Stream reference, closing the existing stream if it is not the existing </summary>
        /// <param name="oldData">Old stream's data</param>
        /// <param name="newData">New stream to assign</param>
        protected void AssignStreamAndConditionallyCloseSource(ref Stream oldData, Stream newData)
        {
            //original reference is neither null nor is it native data
            if (oldData != this.NativeBinaryData && oldData != null)
                oldData.Dispose();

            oldData = newData;
        }
        #endregion
    }
}