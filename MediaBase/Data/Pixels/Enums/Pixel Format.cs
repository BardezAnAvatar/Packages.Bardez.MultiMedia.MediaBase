using System;

namespace Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums
{
    /// <summary>Represents a number of pixel formats that the binary data represents</summary>
    public enum PixelFormat
    {
        /// <summary>An invalid pixel format type</summary>
        Invalid,

        /// <summary>32-bit RGB with an alpha channel. Represented in R, G, B, A order.</summary>
        RGBA_R8G8B8A8,

        /// <summary>32-bit RGB with an alpha channel. Represented in B, G, R, A order.</summary>
        RGBA_B8G8R8A8,

        /// <summary>24-bit RGB with no alpha channel. Represented in R, G, B order.</summary>
        RGB_R8G8B8,

        /// <summary>24-bit RGB with no alpha channel. Represented in B, G, R order.</summary>
        RGB_B8G8R8,

        /// <summary>16-bit RGB with no alpha channel. Represented in B, G, R order, discarding the 16th bit.</summary>
        RGB_B5G5R5X1,

        /// <summary>16-bit RGB with no alpha channel. Represented in R, G, B order, discarding the 16th bit.</summary>
        RGB_R5G5B5X1,

        /// <summary>Represents the YCbCr colorspace pixel format used by JFIF (it specifies slightly different color conversions than other sources). Represented in Y, Cb, Cr order.</summary>
        YCbCrJpeg,

        /// <summary>Represents YUV colorspace, with 4 Y pixels (square) to every one Cb and Cr pixel</summary>
        YUV_YUV_Y4Cb1Cr1,

        /// <summary>Represents RBG32 with a palette</summary>
        RGB_32_Paletted,
    }
}