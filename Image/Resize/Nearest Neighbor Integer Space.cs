using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Resize
{
    /// <summary>Performs implementations of bilinear interpolation in Integer-native space.</summary>
    public static class NearestNeighborIntegerSpace
    {
        /// <summary>Performs a nearest neighbor transform on the source data</summary>
        /// <param name="data">Array of input data to resample</param>
        /// <param name="actualHeight">Height of the actual image data</param>
        /// <param name="actualWidth">Width of the actual image data</param>
        /// <param name="dataHeight">Height of the input data (i.e.: with padding)</param>
        /// <param name="dataWidth">Width of the input data (i.e.: with padding)</param>
        /// <param name="targetHeight">Output target height</param>
        /// <param name="targetWidth">Output target width</param>
        /// <returns>An Array of integer values for later transforms</returns>
        public static Int32[] NearestNeighborResampleInteger(Int32[] data, Int32 actualHeight, Int32 actualWidth, Int32 dataHeight, Int32 dataWidth, Int32 targetHeight, Int32 targetWidth)
        {
            Int32[] destinationData = new Int32[targetWidth * targetHeight];

            for (Int32 v = 0; v < (actualHeight * targetHeight); v += targetHeight)
            {
                Int32 yDst = (v / targetHeight) * targetWidth;
                Int32 ySrc = (v / actualHeight) * dataWidth;

                for (Int32 h = 0; h < (actualWidth * targetWidth); h += targetWidth)
                {
                    Int32 xDst = (h / targetWidth);
                    Int32 xDest = h / actualWidth;
                    destinationData[yDst + xDst] = data[ySrc + xDest];
                }
            }

            return destinationData;
        }
    }
}