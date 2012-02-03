using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize
{
    /// <summary>Resizes a source image using various techniques</summary>
    public static class BilinearFloatSpace
    {
        /// <summary>Performs a weighted bilinear transform on the source data (more mathematically accurate)</summary>
        /// <param name="data">Array of input data to resample</param>
        /// <param name="actualHeight">Height of the actual image data</param>
        /// <param name="actualWidth">Width of the actual image data</param>
        /// <param name="dataHeight">Height of the input data (i.e.: with padding)</param>
        /// <param name="dataWidth">Width of the input data (i.e.: with padding)</param>
        /// <param name="targetHeight">Output target height</param>
        /// <param name="targetWidth">Output target width</param>
        /// <returns>An Array of integer values for later floating point transforms</returns>
        /// <remarks>Assumes a top-down approach. If a bottom-up is needed, swap first.</remarks>
        public static Int32[] BilinearResampleInteger(Int32[] data, Int32 actualHeight, Int32 actualWidth, Int32 dataHeight, Int32 dataWidth, Int32 targetHeight, Int32 targetWidth)
        {
            Int32[] xTransform = new Int32[targetWidth * actualHeight];
            Int32[] yTransform;
            Double xPixelStride = Convert.ToDouble(actualWidth) / Convert.ToDouble(targetWidth);    //how much each target pixel should stride through source
            Double yPixelStride = Convert.ToDouble(actualHeight) / Convert.ToDouble(targetHeight);  //how much each target pixel should stride through source
            Double xPixelWeight = Convert.ToDouble(targetWidth) / Convert.ToDouble(actualWidth);    //how much each source pixel worth
            Double yPixelWeight = Convert.ToDouble(targetHeight) / Convert.ToDouble(actualHeight);  //how much each source pixel worth

            Int32 sourcePre, sourcePost;                                //output variables
            Double sourcePixelStartPosition, sourcePixelEndPosition;    //output variables

            Int32 destIndex = 0;

            //do not transform if target and actual widths are the same
            if (actualWidth == targetWidth)
            {
                for (Int32 y = 0; y < actualHeight; ++y)
                {
                    Int32 yBase = (y * dataWidth);
                    Array.Copy(data, yBase, xTransform, destIndex, actualWidth);
                    destIndex += actualWidth;
                }
            }
            else
            {
                //go through source pixels, and do a horizontal scale
                for (Int32 y = 0; y < actualHeight; ++y)
                {
                    Int32 baseY = (y * dataWidth);
                    for (Int32 x = 0; x < targetWidth; ++x)
                    {
                        //if the distance between source pixels > 2, then the middle pixels keep their pixel weight. That is, the outer pixels must total the remainder of 1 - sum(middle pixel wights).
                        BilinearFloatSpace.GetBilinearIndeces(x, xPixelWeight, xPixelStride, out sourcePre, out sourcePost, out sourcePixelStartPosition, out sourcePixelEndPosition);  //get target X indecies
                        Double pixelRangeStartSubWeight = 1.0 - (sourcePixelStartPosition % 1.0);
                        Double pixelRangeEndSubWeight = sourcePixelEndPosition % 1.0;
                        Double difference = xPixelStride;

                        //if we end on an even number, don't sample and back up. Also, if I exceed the width, back up.
                        if (sourcePixelEndPosition % 1.0 == 0.0 || sourcePixelEndPosition >= actualWidth)
                        {
                            difference -= pixelRangeStartSubWeight;
                            sourcePost--;
                            pixelRangeEndSubWeight = xPixelStride;
                        }

                        Double sampleValue;
                        if (sourcePre == sourcePost)    //short-circuit the resampling. this will be 100% of the only sample being resampled
                            sampleValue = data[baseY + sourcePre];
                        else    //I should ALWAYS have more than 1 sample returned, now, as the range between sourcePre and sourcePost
                        {
                            //first sample
                            sampleValue = data[baseY + sourcePre] * pixelRangeStartSubWeight;

                            //any intermediate samples
                            for (Int32 i = (sourcePre + 1); i < (sourcePost - 1); ++i)
                                sampleValue += (data[baseY + sourcePre + i]);

                            //final sample
                            sampleValue += data[baseY + sourcePost] * pixelRangeEndSubWeight;

                            //if stride =< 1, there will be only 2 samples, the start and end, skipping the loop.
                            //if stride >1, then it is possible to get three or more. here, the total exceeds a single sample
                            // either way, dividing the difference (usually stride) out will give the the weighted average.
                            sampleValue /= difference;
                        }


                        xTransform[destIndex] = Convert.ToInt32(sampleValue);
                        ++destIndex;
                    }
                }
            }

            destIndex = 0;
            
            //do not transform if target and actual heights are the same
            if (actualHeight == targetHeight)
                yTransform = xTransform;    //no difference in data.
            else
            {
                yTransform = new Int32[targetWidth * targetHeight];

                //go through the scaled temporary transform
                for (Int32 y = 0; y < targetHeight; ++y)
                {
                    //List<Double> row = new List<Double>();
                    Double[] row = new Double[targetWidth];
                    Int32 rowIndex = 0;
                    BilinearFloatSpace.GetBilinearIndeces(y, yPixelWeight, yPixelStride, out sourcePre, out sourcePost, out sourcePixelStartPosition, out sourcePixelEndPosition);  //get target Y indecies
                    Double pixelRangeStartSubWeight = 1.0 - (sourcePixelStartPosition % 1.0);
                    Double pixelRangeEndSubWeight = sourcePixelEndPosition % 1.0;
                    Double difference = yPixelStride;

                    //if we end on an even number, don't sample and back up. Also, if I exceed the height, back up.
                    if (sourcePixelEndPosition % 1.0 == 0.0 || sourcePixelEndPosition >= actualHeight)
                    {
                        difference -= pixelRangeEndSubWeight;
                        sourcePost--;
                        pixelRangeEndSubWeight = yPixelStride;
                    }
                    Int32 baseY = (sourcePre * targetWidth);

                    if (sourcePre == sourcePost)    //short-circuit the resampling. this will be 100% of the only sample being resampled
                        Array.Copy(xTransform, baseY, row, 0, targetWidth);
                    else    //I should ALWAYS have more than 1 sample returned, now, as the range between sourcePre and sourcePost
                    {
                        //first sample row
                        for (Int32 x = 0; x < targetWidth; ++x)
                        {
                            row[rowIndex] = xTransform[baseY + x] * pixelRangeStartSubWeight;
                            ++rowIndex;
                        }

                        //any intermediate sample rows
                        for (Int32 i = (sourcePre + 1); i < (sourcePost - 1); ++i)
                        {
                            baseY = ((sourcePre + 1) * targetWidth);

                            for (Int32 x = 0; x < targetWidth; ++x)
                                row[x] += xTransform[baseY + x];
                        }

                        //last sample row
                        baseY = (sourcePost * targetWidth);
                        for (Int32 x = 0; x < targetWidth; ++x)
                            row[x] += xTransform[baseY + x] * pixelRangeEndSubWeight;

                        //if stride =< 1, there will be only 2 samples, the start and end, skipping the multiple y loop.
                        //if stride >1, then it is possible to get three or more. here, the total exceeds a single sample
                        // either way, dividing the difference (usually stride) out will give the the weighted average.
                        for (rowIndex = 0; rowIndex < row.Length; ++rowIndex)
                            row[rowIndex] /= difference;
                    }

                    //add the row to the output
                    foreach (Double sample in row)
                    {
                        yTransform[destIndex] = Convert.ToInt32(sample);
                        ++destIndex;
                    }
                }
            }

            xTransform = null;

            return yTransform;
        }
        
        /// <summary>Performs a bilinear transform on the source data</summary>
        /// <param name="data">IList of input data to resample</param>
        /// <param name="actualHeight">Height of the actual image data</param>
        /// <param name="actualWidth">Width of the actual image data</param>
        /// <param name="dataHeight">Height of the input data (i.e.: with padding)</param>
        /// <param name="dataWidth">Width of the input data (i.e.: with padding)</param>
        /// <param name="targetHeight">Output target height</param>
        /// <param name="targetWidth">Output target width</param>
        /// <returns>A List of Double-precision floating point values for later floating point transforms</returns>
        /// <remarks>Assumes a top-down approach. If a bottom-up is needed, swap first.</remarks>
        public static List<Double> BilinearResampleFloat(IList<Double> data, Int32 actualHeight, Int32 actualWidth, Int32 dataHeight, Int32 dataWidth, Int32 targetHeight, Int32 targetWidth)
        {
            List<Double> xTransform = new List<Double>(actualHeight * targetWidth), yTransform = new List<Double>(targetWidth * targetHeight);
            Double xPixelStride = Convert.ToDouble(actualWidth) / Convert.ToDouble(targetWidth);    //how much each target pixel should stride through source
            Double yPixelStride = Convert.ToDouble(actualHeight) / Convert.ToDouble(targetHeight);  //how much each target pixel should stride through source
            Double xPixelWeight = Convert.ToDouble(targetWidth) / Convert.ToDouble(actualWidth);    //how much each source pixel worth
            Double yPixelWeight = Convert.ToDouble(targetHeight) / Convert.ToDouble(actualHeight);  //how much each source pixel worth

            Int32 sourcePre, sourcePost;                                //output variables
            Double sourcePixelStartPosition, sourcePixelEndPosition;    //output variables


            //do not transform if target and actual widths are the same
            if (actualWidth == targetWidth)
            {
                for (Int32 y = 0; y < actualHeight; ++y)
                {
                    Int32 yBase = (y * dataWidth);
                    for (Int32 x = 0; x < actualWidth; ++x)
                        xTransform.Add(Convert.ToDouble(data[yBase + x]));
                }
            }
            else
            {
                //go through source pixels, and do a horizontal scale
                for (Int32 y = 0; y < actualHeight; ++y)
                {
                    Int32 baseY = (y * dataWidth);
                    for (Int32 x = 0; x < targetWidth; ++x)
                    {
                        //if the distance between source pixels > 2, then the middle pixels keep their pixel weight. That is, the outer pixels must total the remainder of 1 - sum(middle pixel wights).
                        BilinearFloatSpace.GetBilinearIndeces(x, xPixelWeight, xPixelStride, out sourcePre, out sourcePost, out sourcePixelStartPosition, out sourcePixelEndPosition);  //get target X indecies
                        Double pixelRangeStartSubWeight = 1.0 - (sourcePixelStartPosition % 1.0);
                        Double pixelRangeEndSubWeight = sourcePixelEndPosition % 1.0;
                        Double difference = xPixelStride;

                        //if we end on an even number, don't sample and back up. Also, if I exceed the width, back up.
                        if (sourcePixelEndPosition % 1.0 == 0.0 || sourcePixelEndPosition >= actualWidth)
                        {
                            difference -= pixelRangeEndSubWeight;
                            sourcePost--;
                            pixelRangeEndSubWeight = xPixelStride;
                        }

                        Double sampleValue;
                        if (sourcePre == sourcePost)    //short-circuit the resampling. this will be 100% of the only sample being resampled
                            sampleValue = data[baseY + sourcePre];
                        else    //I should ALWAYS have more than 1 sample returned, now, as the range between sourcePre and sourcePost
                        {
                            //first sample
                            sampleValue = data[baseY + sourcePre] * pixelRangeStartSubWeight;

                            //any intermediate samples
                            for (Int32 i = (sourcePre + 1); i < (sourcePost - 1); ++i)
                                sampleValue += (data[baseY + sourcePre + i]);

                            //final sample
                            sampleValue += data[baseY + sourcePost] * pixelRangeEndSubWeight;

                            //if stride =< 1, there will be only 2 samples, the start and end, skipping the loop.
                            //if stride >1, then it is possible to get three or more. here, the total exceeds a single sample
                            // either way, dividing the difference (usually stride) out will give the the weighted average.
                            sampleValue /= difference;
                        }

                        xTransform.Add(sampleValue);
                    }
                }
            }

            //do not transform if target and actual heights are the same
            if (actualHeight == targetHeight)
                yTransform = xTransform;    //no difference in data.
            else
            {
                //go through the scaled temporary transform
                for (Int32 y = 0; y < targetHeight; ++y)
                {
                    List<Double> row = new List<Double>();
                    BilinearFloatSpace.GetBilinearIndeces(y, yPixelWeight, yPixelStride, out sourcePre, out sourcePost, out sourcePixelStartPosition, out sourcePixelEndPosition);  //get target Y indecies
                    Double pixelRangeStartSubWeight = 1.0 - (sourcePixelStartPosition % 1.0);
                    Double pixelRangeEndSubWeight = sourcePixelEndPosition % 1.0;
                    Double difference = yPixelStride;

                    //if we end on an even number, don't sample and back up. Also, if I exceed the height, back up.
                    if (sourcePixelEndPosition % 1.0 == 0.0 || sourcePixelEndPosition >= actualHeight)
                    {
                        difference -= pixelRangeEndSubWeight;
                        sourcePost--;
                        pixelRangeEndSubWeight = yPixelStride;
                    }
                    Int32 baseY = (sourcePre * targetWidth);

                    if (sourcePre == sourcePost)    //short-circuit the resampling. this will be 100% of the only sample being resampled
                        for (Int32 x = 0; x < targetWidth; ++x)
                            row.Add(xTransform[baseY + x]);
                    else    //I should ALWAYS have more than 1 sample returned, now, as the range between sourcePre and sourcePost
                    {
                        //first sample row
                        for (Int32 x = 0; x < targetWidth; ++x)       
                            row.Add(xTransform[baseY + x] * pixelRangeStartSubWeight);
                        
                        //any intermediate sample rows
                        for (Int32 i = (sourcePre + 1); i < (sourcePost - 1); ++i)  
                        {
                            baseY = ((sourcePre + 1) * targetWidth);

                            for (Int32 x = 0; x < targetWidth; ++x)
                                row[x] += xTransform[baseY + x];
                        }

                        //last sample row
                        baseY = (sourcePost * targetWidth);
                        for (Int32 x = 0; x < targetWidth; ++x)
                            row[x] += xTransform[baseY + x] * pixelRangeEndSubWeight;

                        //if stride =< 1, there will be only 2 samples, the start and end, skipping the multiple y loop.
                        //if stride >1, then it is possible to get three or more. here, the total exceeds a single sample
                        // either way, dividing the difference (usually stride) out will give the the weighted average.
                        for (Int32 rowIndex = 0; rowIndex < row.Count; ++rowIndex)
                            row[rowIndex] /= difference;
                    }

                    //add the row to the output
                    yTransform.AddRange(row);
                }
            }

            xTransform = null;

            return yTransform;
        }

        /// <summary>Gets the two indecies associated with a target sample, based on the source image's step per pixel</summary>
        /// <param name="left">Preceeding pixel of source to sample</param>
        /// <param name="right">Succeedin pixel of source to sample</param>
        /// <param name="target">Desired index of destination pixel</param>
        /// <param name="sourceStep">The weight per source pixel when mapping to a new resolution</param>
        private static void GetBilinearIndeces(Int32 target, Double sourcePixelWeight, Double sourceStep, out Int32 left, out Int32 right, out Double startPosition, out Double endPosition)
        {
            startPosition = sourceStep * target;
            endPosition = startPosition + sourceStep;
            left = Convert.ToInt32(Math.Floor(startPosition));
            right = Convert.ToInt32(Math.Floor(endPosition));
        }
    }
}