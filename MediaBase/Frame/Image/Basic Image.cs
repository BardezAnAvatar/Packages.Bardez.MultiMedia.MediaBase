using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.Multimedia.MediaBase.Frame.Image
{
    /// <summary>This class is a wrapper for IMultimediaImage</summary>
    public class BasicImage : IImage
    {
        #region Fields
        /// <summary>The underlying data</summary>
        protected IMultimediaImageFrame sourceFrame;
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="data">Source data this class wraps</param>
        public BasicImage(IMultimediaImageFrame data)
        {
            this.sourceFrame = data;
        }
        #endregion


        #region IImage methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public IMultimediaImageFrame GetFrame()
        {
            return sourceFrame;
        }

        /// <summary>Gets a sub-image of the current image</summary>
        /// <param name="x">Source X position</param>
        /// <param name="y">Source Y position</param>
        /// <param name="width">Width of sub-image</param>
        /// <param name="height">Height of sub-image</param>
        /// <returns>The requested sub-image</returns>
        public IImage GetSubImage(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            return new BasicImage(ImageManipulation.GetSubImage(this.sourceFrame, x, y, width, height));
        }
        #endregion
    }
}