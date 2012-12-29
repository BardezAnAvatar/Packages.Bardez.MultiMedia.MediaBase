using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize
{
    /// <summary>Performs a resize transform on the source data</summary>
    /// <param name="data">Array of input data to resample</param>
    /// <param name="actualHeight">Height of the actual image data</param>
    /// <param name="actualWidth">Width of the actual image data</param>
    /// <param name="dataHeight">Height of the input data (i.e.: with padding)</param>
    /// <param name="dataWidth">Width of the input data (i.e.: with padding)</param>
    /// <param name="targetHeight">Output target height</param>
    /// <param name="targetWidth">Output target width</param>
    /// <returns>An Array of integer values for later transforms</returns>
    public delegate Int32[] ResizeDelegateInteger(Int32[] data, Int32 actualHeight, Int32 actualWidth, Int32 dataHeight, Int32 dataWidth, Int32 targetHeight, Int32 targetWidth);

    /// <summary>Performs a resize transform on the source data</summary>
    /// <param name="data">IList of input data to resample</param>
    /// <param name="actualHeight">Height of the actual image data</param>
    /// <param name="actualWidth">Width of the actual image data</param>
    /// <param name="dataHeight">Height of the input data (i.e.: with padding)</param>
    /// <param name="dataWidth">Width of the input data (i.e.: with padding)</param>
    /// <param name="targetHeight">Output target height</param>
    /// <param name="targetWidth">Output target width</param>
    /// <returns>An IList of Double-precision floating-point values for later transforms</returns>
    public delegate IList<Double> ResizeDelegateFloat(IList<Double> data, Int32 actualHeight, Int32 actualWidth, Int32 dataHeight, Int32 dataWidth, Int32 targetHeight, Int32 targetWidth);

    /// <summary>Performs a resize transform on the source data</summary>
    /// <param name="data">IList of input data to resample</param>
    /// <param name="actualHeight">Height of the actual image data</param>
    /// <param name="actualWidth">Width of the actual image data</param>
    /// <param name="dataHeight">Height of the input data (i.e.: with padding)</param>
    /// <param name="dataWidth">Width of the input data (i.e.: with padding)</param>
    /// <param name="targetHeight">Output target height</param>
    /// <param name="targetWidth">Output target width</param>
    /// <returns>An IList of of Double-precision floating-point values for later transforms</returns>
    public delegate IList<Double> ResizeDelegateIntegerToFloat(IList<Int32> data, Int32 actualHeight, Int32 actualWidth, Int32 dataHeight, Int32 dataWidth, Int32 targetHeight, Int32 targetWidth);
}