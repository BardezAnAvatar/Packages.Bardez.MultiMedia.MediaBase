using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Image
{
    /// <summary>Static repository for image manipulation</summary>
    public static class ImageManipulation
    {
        /// <summary>Gets a sub-image of the current image</summary>
        /// <param name="source">Source <see cref="IMultimediaImageFrame" /> containing both the source data and metadata needed</param>
        /// <param name="x">Source X position</param>
        /// <param name="y">Source Y position</param>
        /// <param name="width">Width of sub-image</param>
        /// <param name="height">Height of sub-image</param>
        /// <returns>The requested sub-image</returns>
        /// <remarks>
        ///     I have done some thinking about how a sub-image should interact with the origin point in
        ///     metadata and have decided that it shouldn't; if a subclass needs it, it can do so. But generally
        ///     speaking, the generated pointof origin should be (0,0).
        /// </remarks>
        public static IMultimediaImageFrame GetSubImage(IMultimediaImageFrame source, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            return ImageManipulation.GetSubImage(source.Data, source.MetadataImage, x, y, width, height);
        }

        /// <summary>Gets a sub-image of the current image</summary>
        /// <param name="source">Source <see cref="PixelData" /> containing both the source data and metadata needed</param>
        /// <param name="x">Source X position</param>
        /// <param name="y">Source Y position</param>
        /// <param name="width">Width of sub-image</param>
        /// <param name="height">Height of sub-image</param>
        /// <returns>The requested sub-image</returns>
        /// <remarks>
        ///     I have done some thinking about how a sub-image should interact with the origin point in
        ///     metadata and have decided that it shouldn't; if a subclass needs it, it can do so. But generally
        ///     speaking, the generated pointof origin should be (0,0).
        /// </remarks>
        public static IMultimediaImageFrame GetSubImage(PixelData source, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            return ImageManipulation.GetSubImage(source.NativeBinaryData, source.Metadata, x, y, width, height);
        }

        /// <summary>Gets a sub-image of the current image</summary>
        /// <param name="sourceData">Source data stream</param>
        /// <param name="sourceMetadata">Metadata for the source data stream</param>
        /// <param name="x">Source X position</param>
        /// <param name="y">Source Y position</param>
        /// <param name="width">Width of sub-image</param>
        /// <param name="height">Height of sub-image</param>
        /// <returns>The requested sub-image</returns>
        /// <remarks>
        ///     I have done some thinking about how a sub-image should interact with the origin point in
        ///     metadata and have decided that it shouldn't; if a subclass needs it, it can do so. But generally
        ///     speaking, the generated pointof origin should be (0,0).
        /// </remarks>
        public static IMultimediaImageFrame GetSubImage(Stream sourceData, ImageMetadata sourceMetadata, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            IMultimediaImageFrame image = null;

            //set up ending positions
            Int64 endX = x + width;
            Int64 endY = y + height;

            //validation
            if (x >= sourceMetadata.Width)
                throw new ArgumentOutOfRangeException("x", String.Format("The provided origin x coordinate ({0}) was greater than the width of the source image ({1}).", x, sourceMetadata.Width));
            else if (x < 0)
                throw new ArgumentOutOfRangeException("x", String.Format("The provided origin x coordinate ({0}) was lass than 0.", x));
            else if (y >= sourceMetadata.Height)
                throw new ArgumentOutOfRangeException("y", String.Format("The provided origin y coordinate ({0}) was greater than the height of the source image ({1}).", y, sourceMetadata.Height));
            else if (y < 0)
                throw new ArgumentOutOfRangeException("y", String.Format("The provided origin y coordinate ({0}) was lass than 0.", y));
            else if (width < 0)
                throw new ArgumentOutOfRangeException("width", String.Format("The provided width ({0}) was lass than 0.", width));
            else if (height < 0)
                throw new ArgumentOutOfRangeException("height", String.Format("The provided height ({0}) was lass than 0.", height));
            else if (endX > sourceMetadata.Width)
                throw new ArgumentOutOfRangeException("width", String.Format("The provided width ({0}) and the provided origin x coordinate ({1}) yields a rectangle that exceeds the width of the source image ({2}).", width, x, sourceMetadata.Width));
            else if (endY > sourceMetadata.Height)
                throw new ArgumentOutOfRangeException("height", String.Format("The provided height ({0}) and the provided origin y coordinate ({1}) yields a rectangle that exceeds the height of the source image ({2}).", height, y, sourceMetadata.Height));

            //copy data information
            Int64 startY = y, startX = x;

            //flip the start position if necessary
            if (sourceMetadata.Order == ScanLineOrder.BottomUp)
            {
                startY = sourceMetadata.Height - endY;
                endY = sourceMetadata.Height - y;
            }

            //determine the start byte and bit needed for the copy.
            Int32 startBit = sourceMetadata.BitsPerDataPixel * x;
            Int32 startByte = startBit / 8;
            startBit = startBit % 8;
            Int32 copyBits = (sourceMetadata.BitsPerDataPixel * width);      //size of a copied row in bits
            Int32 endBit = startBit + copyBits;
            Int32 copySize = endBit / 8;    //get the byte width to copy
            Int32 readSize = copySize;
            endBit = endBit % 8;
            if (endBit > 0)                 //check if an extra byte is needed
                readSize++;

            //create a new Memory Stream
            MemoryStream output = new MemoryStream();

            for (Int64 sourceY = startY; sourceY < endY; sourceY++)
            {
                //get source data
                ReusableIO.SeekIfAble(sourceData, (sourceMetadata.RowDataSize * sourceY) + startByte);
                Byte[] row = ReusableIO.BinaryRead(sourceData, copySize);

                //write the data back
                if (startBit == 0 && endBit == 0)       //multiple of 8 bpp
                    output.Write(row, 0, row.Length);
                else    //not 8-bit aligned
                {
                    Int32 mask = (1 << sourceMetadata.BitsPerDataPixel) - 1;
                    for (Int32 index = 0; index < copySize; ++index)
                    {
                        Int32 value = row[index] >> startBit;

                        if (index < (row.Length - 1))
                            value |= ((row[index + 1] >> startBit) << 8);

                        Byte write = (Byte)(value & mask);

                        output.WriteByte(write);
                    }
                }
            }

            //create new image
            PixelData pd = new PixelData(output, sourceMetadata.Order, sourceMetadata.Format, sourceMetadata.DataPalette, height, width, 1, 1, sourceMetadata.BitsPerDataPixel, 0, 0);
            image = new BasicImageFrame(pd);

            return image;
        }
    }
}