using System;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Pixels.Enums;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Pixels
{
    /// <summary>Represents a single-source for an image's pixel data. This class will record the input type, and output pixel data as requested, converting as necessary</summary>
    public class PixelData
    {
        #region Helper Objects
        /// <summary>Represents a method that will convert between two PixlFormat types, specified by the method name</summary>
        /// <returns>A Byte array of pixel data</returns>
        private delegate Byte[] ConversionMethod();

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
        }
        #endregion

        #region Properties
        /// <summary>Indicates the data format of the pixels</summary>
        public PixelFormat Format { get; set; }

        /// <summary>Indicates the order of scan lines in the pixel data</summary>
        public ScanLineOrder Order { get; set; }

        /// <summary>Represents the palette of pixel data.</summary>
        /// <value>null if the pixel data is raw for its format</value>
        public Palette DataPalette { get; set; }

        /// <summary>Represents the binary array of pixel data read from source.</summary>
        /// <value>The data must be either palette-indexed or fully decoded to a multiple of 8 bits. Y'UV 4:2:2 would not be acceptable, not Y'V12, but Y'UV 4:4:4 would be.</value>
        public Byte[] NativeBinaryData { get; set; }

        /// <summary>Exposes the group of metadata</summary>
        public MetaData Metadata { get; set; }

        /// <summary>Gets the size, in bytes, of a logical row of pixels based off of the bits per pixel</summary>
        protected Int32 RowDataSize
        {
            get { return this.PackedRowByteWidth(this.ExpandedBitsPerPixel, this.Metadata.HorizontalPacking); }
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
        /// <param name="bitsPerStreamPixel">The number of bits per pixel in the binary data stream</param>
        public PixelData(Byte[] binary, ScanLineOrder order, PixelFormat format, Palette palette, Int32 height, Int32 width, Int32 rowPacking, Int32 heightPacking, Int32 bitsPerStreamPixel)
        {
            this.Format = format;
            this.Order = order;
            this.NativeBinaryData = binary;
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
        /// <param name="packing">The packing of bytes to a single row</param>
        /// <returns>The byte width queried</returns>
        protected Int32 PackedRowByteWidth(Int32 bitsPerPixel, Int32 packing)
        {
            Int32 rowSize = bitsPerPixel * this.Metadata.Width;     //bits per row for data
            rowSize = (rowSize / 8) + ((rowSize % 8) > 0 ? 1 : 0);  //bytes per row
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
        protected Byte[] DecodePaletteData()
        {
            Byte[] bitmapData = null;

            if (this.DataPalette != null)
            {
                Int32 rowSize = this.RowDataSize;

                bitmapData = new Byte[rowSize * this.RowCount];

                //now, interpret the pixel data
                for (Int32 row = 0; row < this.Metadata.Height; ++row)
                {
                    Byte[] interim = this.DecodePaletteDataRow(row);  //read a row of pixels
                    Array.Copy(interim, 0, bitmapData, (row * rowSize), interim.Length);
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
            Int32 location = decompressed.Length * row;

            //decompress data (yay!)
            //read the packed data, 1 value at a time, and write the palette entry to the decompressed stream.
            //the only valid palettes I know are 8 bit or less, being 1, 2, 4, 8. I'm going to only implement those, for now.

            Int32 xByte = 0, xBits = 0;
            for (Int32 x = 0; x < this.Metadata.Width; ++x)
            {
                Int32 value = this.NativeBinaryData[location + xByte];   //get the current byte, assign it to a shiftable variable
                xBits += this.Metadata.BitsPerDataPixel;

                //wrap as needed
                if (xBits > 7)
                {
                    ++xByte;
                    xBits -= 8;
                }

                //Bit shifting.
                Int32 right = (31 - xBits);
                Int32 mask = (-0x7FFFFFFF >> right) << (xBits - this.Metadata.BitsPerDataPixel);
                Int32 index = value |= mask;

                //now we need the pixel.
                Byte[] data = this.DataPalette.Pixels[index].GetBytes();

                Int32 position = data.Length * x;

                Array.Copy(data, 0, decompressed, position, data.Length);
            }

            return decompressed;
        }

        /// <summary>Flips pixel data scan line by scan line</summary>
        /// <param name="data">Decompressed data to flip</param>
        /// <returns>The vertically flipped data</returns>
        /// <remarks>Uses the current pixel data's packing, not the destination's</remarks>
        protected void FlipVertical(Byte[] data)
        {
            Byte[] tempRow = new Byte[this.PackedRowByteWidth(this.ExpandedBitsPerPixel, this.Metadata.HorizontalPacking)];

            //loop through half of the number of rows in the data
            Int32 rows = this.RowCount;

            for (Int32 row = 0; row < (rows / 2); ++row) // 5/2 = 2 is what I am looking for. Don't flip the middle row; that's just silly.
            {
                Int32 offset = tempRow.Length * row;
                Array.Copy(data, offset, tempRow, 0, tempRow.Length);           //copy current row
                Int32 destOffset = ((rows - 1) - row) * tempRow.Length;         //destination in data
                Array.Copy(data, destOffset, data, offset, tempRow.Length);     //copy opposite row to current row
                Array.Copy(tempRow, 0, data, destOffset, tempRow.Length);       //write copied 'current' row
            }
        }

        /// <summary>Adjusts the packing bytes of the data</summary>
        /// <param name="data">Data to pack</param>
        /// <param name="horizontalPacking">target horizontal packing</param>
        /// <param name="verticalPacking">target vertical packing</param>
        /// <param name="scanLineOrder">target scanline order</param>
        /// <returns>The adjusted packed bytes</returns>
        protected Byte[] AdjustForPacking(Byte[] data, Int32 horizontalPacking, Int32 verticalPacking, ScanLineOrder scanLineOrder)
        {
            //current and end widths
            Int32 destRowSize = this.PackedRowByteWidth(this.ExpandedBitsPerPixel, horizontalPacking);
            Int32 destRowCount = this.PackedRowCount(verticalPacking);
            Int32 currentRowSize = this.RowDataSize;
            Int32 currentRowCount = this.RowCount;

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

                Array.Copy(data, sourceOffset, bitmapData, destOffset, byteWidth);
            }

            return bitmapData;
        }

        /// <summary>Converts data from the current format to another specified format</summary>
        /// <param name="data">Decompressed data to convert</param>
        /// <param name="format">Format to convert to</param>
        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        /// <returns>New byte data</returns>
        protected Byte[] ConvertData(Byte[] data, PixelFormat format, Int32 horizontalPacking, Int32 verticalPacking)
        {
            Byte[] converted = null;

            //convert data
            if (this.Format == PixelFormat.RGB_B8G8R8 && format == PixelFormat.RGBA_B8G8R8A8)       //RGB BGR 24 -> RGBA BGRA 32
                converted = ConvertRgbToRgba(data, horizontalPacking, verticalPacking);
            else if (this.Format == PixelFormat.YCbCrJpeg && format == PixelFormat.RGBA_B8G8R8A8)   //YCbCr (JFIF)-> RGBA BGRA 32
                    converted = ConvertJfifYCbCrToRgba(data, horizontalPacking, verticalPacking);

            return converted;
        }

        /// <summary>Retrieves the pixel data in the specified format, in the specified scan line order</summary>
        /// <param name="format">Expected output format of the data</param>
        /// <param name="order">Expected scan line order of the output data</param>
        /// <param name="horizontalPacking">Horizontal packing of bytes for output</param>
        /// <param name="verticalPacking">Vertical packing of rows for output</param>
        /// <returns>Binary pixel dataof the converted data</returns>
        public Byte[] GetPixelData(PixelFormat format, ScanLineOrder order, Int32 horizontalPacking, Int32 verticalPacking)
        {
            Byte[] data = this.NativeBinaryData;

            //decode data from palette
            if (this.DataPalette != null)
                data = this.DecodePaletteData();

            //flip if necessary
            if (this.Order != order)
                this.FlipVertical(data);    //vertically swap each pixel row

            //widen data to the horizontal and vertical packing specified
            if (this.Metadata.HorizontalPacking != horizontalPacking || this.Metadata.VerticalPacking != verticalPacking)
                data = this.AdjustForPacking(data, horizontalPacking, verticalPacking, order);     // it should know the existing packing for this instance, specify the new packing

            //convert as necessary
            if (format != this.Format)
                data = this.ConvertData(data, format, horizontalPacking, verticalPacking);

            //in a return state
            return data;
        }
        #endregion

        #region Conversion methods
        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        protected Byte[] ConvertRgbToRgba(Byte[] data, Int32 horizontalPacking, Int32 verticalPacking)
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
                    rgba[destDataOffset + destPixelX] = data[srcDataOffset + srcPixelX];            //blue
                    rgba[destDataOffset + destPixelX + 1] = data[srcDataOffset + srcPixelX + 1];    //green
                    rgba[destDataOffset + destPixelX + 2] = data[srcDataOffset + srcPixelX + 2];    //red
                    rgba[destDataOffset + destPixelX + 3] = 255;                                    //alpha

                    //increment
                    srcPixelX += RgbTriplet.BytesPerPixel;
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
    }
}