using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.Pixels
{
    /// <summary>Describes a 24-bit value of RGB data</summary>
    public class RgbTriplet : PixelBase
    {
        #region Static Fields
        /// <summary>Bits per pixel for this pixel type</summary>
        public const Int32 BitsPerPixel = 24;

        /// <summary>Bytes per pixel for this pixel type</summary>
        public const Int32 BytesPerPixel = 3;
        #endregion


        #region Fields
        /// <summary>Represents the red component value</summary>
        public Byte Red { get; set; }

        /// <summary>Represents the green component value</summary>
        public Byte Green { get; set; }

        /// <summary>Represents the blue component value</summary>
        public Byte Blue { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public RgbTriplet() { }

        /// <summary>Definition constructor</summary>
        /// <param name="red">The red component value</param>
        /// <param name="green">The green component value</param>
        /// <param name="blue">The blue component value</param>
        public RgbTriplet(Byte red, Byte green, Byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
        #endregion


        #region Methods
        /// <summary>Gets the underlying bytes of the pixel</summary>
        /// <returns>A byte array of the underlying bytes</returns>
        public override Byte[] GetBytes()
        {
            return new Byte[] { this.Blue, this.Green, this.Red };
        }
        #endregion
    }
}