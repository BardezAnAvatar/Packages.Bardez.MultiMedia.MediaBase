using System;
using Bardez.Projects.MultiMedia.LibAV;

namespace Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums
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

    /// <summary>Extension class for converting the PixelFormat enum to other destination pixel format enumerators</summary>
    public static class PixelFormatExtender
    {
        /// <summary>Converts the PixelFormat instance to a LibAV Pixel Format enumerator</summary>
        /// <param name="source">Source PixelFormat to translate</param>
        /// <returns>The translated pixel format, or LibAVPixelFormat.PIX_FMT_NONE if no translation available.</returns>
        public static LibAVPixelFormat ToLibAVPixelFormat(this PixelFormat source)
        {
            LibAVPixelFormat format = LibAVPixelFormat.PIX_FMT_NONE;

            switch (source)
            {
                case PixelFormat.RGB_B5G5R5X1:
                    format = LibAVPixelFormat.PIX_FMT_BGR555LE;
                    break;
                case PixelFormat.RGB_B8G8R8:
                    format = LibAVPixelFormat.PIX_FMT_BGR24;
                    break;
                case PixelFormat.RGB_R5G5B5X1:
                    format = LibAVPixelFormat.PIX_FMT_RGB555LE;
                    break;
                case PixelFormat.RGB_R8G8B8:
                    format = LibAVPixelFormat.PIX_FMT_RGB24;
                    break;
                case PixelFormat.RGBA_B8G8R8A8:
                    format = LibAVPixelFormat.PIX_FMT_BGRA;
                    break;
                case PixelFormat.RGBA_R8G8B8A8:
                    format = LibAVPixelFormat.PIX_FMT_RGBA;
                    break;
                case PixelFormat.YUV_YUV_Y4Cb1Cr1:
                    format = LibAVPixelFormat.PIX_FMT_YUV420P;
                    break;
                case PixelFormat.RGB_32_Paletted:
                    format = LibAVPixelFormat.PIX_FMT_PAL8;
                    break;
            }

            return format;
        }

        /// <summary>Converts the LibAV Pixel Format instance to a PixelFormat enumerator</summary>
        /// <param name="source">Source LibAVPixelFormat to translate</param>
        /// <returns>The translated pixel format, or LibAVPixelFormat.PIX_FMT_NONE if no translation available.</returns>
        public static PixelFormat FromLibAVPixelFormat(LibAVPixelFormat source)
        {
            PixelFormat format = PixelFormat.Invalid;

            switch (source)
            {
                case LibAVPixelFormat.PIX_FMT_BGR555LE:
                    format = PixelFormat.RGB_B5G5R5X1;
                    break;
                case LibAVPixelFormat.PIX_FMT_BGR24:
                    format = PixelFormat.RGB_B8G8R8;
                    break;
                case LibAVPixelFormat.PIX_FMT_RGB555LE:
                    format = PixelFormat.RGB_R5G5B5X1;
                    break;
                case LibAVPixelFormat.PIX_FMT_RGB24:
                    format = PixelFormat.RGB_R8G8B8;
                    break;
                case LibAVPixelFormat.PIX_FMT_BGRA:
                    format = PixelFormat.RGBA_B8G8R8A8;
                    break;
                case LibAVPixelFormat.PIX_FMT_RGBA:
                    format = PixelFormat.RGBA_R8G8B8A8;
                    break;
                case LibAVPixelFormat.PIX_FMT_YUV420P:
                    format = PixelFormat.YUV_YUV_Y4Cb1Cr1;
                    break;
                case LibAVPixelFormat.PIX_FMT_PAL8:
                    format = PixelFormat.RGB_32_Paletted;
                    break;
            }

            return format;
        }
    }
}