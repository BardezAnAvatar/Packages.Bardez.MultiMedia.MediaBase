using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.Pixels
{
    /// <summary>Describes a 32-bit value of RGBA data</summary>
    public class RgbQuad : RgbTriplet
    {
        #region Static Fields
        /// <summary>Bits per pixel for this pixel type</summary>
        public new const Int32 BitsPerPixel = 32;

        /// <summary>Bytes per pixel for this pixel type</summary>
        public new const Int32 BytesPerPixel = 4;
        #endregion


        #region Fields
        /// <summary>Represents the alpha transparency component value</summary>
        public Byte Alpha { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public RgbQuad() { }

        /// <summary>Definition constructor</summary>
        /// <param name="red">The red component value</param>
        /// <param name="green">The green component value</param>
        /// <param name="blue">The blue component value</param>
        /// <param name="alpha">The alpha transparency component value</param>
        public RgbQuad(Byte red, Byte green, Byte blue, Byte alpha) : base (red, green, blue)
        {
            this.Alpha = alpha;
        }
        #endregion


        #region Methods
        /// <summary>Gets the underlying bytes of the pixel</summary>
        /// <returns>A byte array of the underlying bytes</returns>
        public override Byte[] GetBytes()
        {
            return new Byte[] { this.Blue, this.Green, this.Red, this.Alpha };
        }
        #endregion
    }
}