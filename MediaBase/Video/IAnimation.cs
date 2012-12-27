using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video
{
    /// <summary>Interface to pull information indicating animation set information</summary>
    public interface IAnimation : IImageSet
    {
        /// <summary>Returns an IList containing an IList of indeces meant to be used in conjunction with <see cref="IImageSet.GetFrame(Int32)"/></summary>
        /// <returns></returns>
        IList<IList<Int32>> GetFrameAnimations();
    }
}