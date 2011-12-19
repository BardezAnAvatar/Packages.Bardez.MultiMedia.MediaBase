using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.Pixels
{
    /// <summary>Describes a 24-bit value of Y'UV 4:4:4 data</summary>
    /// <remarks>See http://en.wikipedia.org/wiki/YUV for YUV info</remarks>
    public class YUV_444 : PixelBase
    {
        #region Properties
        /// <summary>Represents the luma (Y') component value</summary>
        public Byte Y { get; set; }

        /// <summary>Represents the blue − luma (B' - Y' = U) component value</summary>
        public Byte U { get; set; }
        
        /// <summary>Represents the red − luma (R' - Y' = U) component value</summary>
        public Byte V { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public YUV_444() { }

        /// <summary>Definition constructor</summary>
        /// <param name="y">The luma (Y') component value</param>
        /// <param name="u">The blue − luma (B' - Y' = U) component value</param>
        /// <param name="v">The red − luma (R' - Y' = U) component value</param>
        public YUV_444(Byte y, Byte u, Byte v)
        {
            this.Y = y;
            this.U = u;
            this.V = v;
        }
        #endregion


        #region Methods
        /// <summary>Gets the underlying bytes of the pixel</summary>
        /// <returns>A byte array of the underlying bytes</returns>
        public override Byte[] GetBytes()
        {
            return new Byte[] { this.Y, this.U, this.V };
        }
        #endregion
    }
}