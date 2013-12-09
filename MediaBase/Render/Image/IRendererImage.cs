using System;
using System.Drawing;

using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Image
{
    /// <summary>Defines an interface for image renderers</summary>
    /// <remarks>
    ///     An image renderer should have the following abilities:
    ///         1) have assets sent to it for composite drawing (layering, drawing order, etc.)
    ///         2) have assets referenced rendered
    ///     
    ///     To facilitate this, I think the following types of operations are necessary:
    ///         1) Submission of data and return of a key
    ///         2) Begin drawing operation
    ///         3) Various drawing operations
    ///             a) Create a layer (creates a new layer for all drawing)
    ///             b) Commit layer (finished drawing everything on the specified layer)
    ///                 i) Layering could just be drawing in order; why is this really needed?
    ///             c) Single drawing (draw image A at coordinates X, Y)
    ///             d) Multiple Drawing (sequential calls to above)
    ///             e) Draw a line from X to Y
    ///             f) Draw ellipse, circle, rectangle, etc.
    ///         4) Render temporary image to target
    /// </remarks>
    public interface IRendererImage : IDisposable
    {
        #region Properties
        /// <summary>
        ///     This exposes the scaling factor of the renderer. It will scale the finished image, rendering
        ///     the scaled composite image in the middle anchor point.
        /// </summary>
        /// <value>Should be withing reasonable scaling. 1/16 to 16x?</value>
        Single ScaleFactor { get; set; }
        #endregion


        #region Events
        /// <summary>Event that occurs when the renderer has finished rendering the provided data. Used to signal expiration and ready for disposal.</summary>
        event EventHandler FinishedRendering;
        #endregion


        #region Resource Management
        /// <summary>Posts an <see cref="IMultimediaImageFrame" /> resource to the renderer and returns a unique key to access it.</summary>
        /// <param name="resource"><see cref="IMultimediaImageFrame" /> to be submitted.</param>
        /// <returns>A unique UInt32 key</returns>
        Int32 SubmitImageResource(IMultimediaImageFrame resource);

        /// <summary>Frees a submitted resource and Disposes of it.</summary>
        /// <param name="key">Unique UInt32 key of the resource to be disposed</param>
        void FreeImageResource(Int32 key);
        #endregion


        #region Rendering operations
        /// <summary>Begins the drawing operation of the renderer</summary>
        void StartDrawing();

        /// <summary>Finishes the drawing operation of the renderer, and redners the image</summary>
        void FinishDrawing();

        /// <summary>Draws the specified image to the renderer</summary>
        /// <param name="key">Unique UInt32 key of the resource to be drawn</param>
        /// <param name="originX">X coordinate to start drawing from</param>
        /// <param name="originY">Y coordinate to start drawing from</param>
        void DrawImage(Int32 key, Int64 originX, Int64 originY);

        /// <summary>Draws the specified image to the renderer</summary>
        /// <param name="key">Unique UInt32 key of the resource to be drawn</param>
        void DrawImage(Int32 key);

        /// <summary>Draws a line segment from one point to another</summary>
        /// <param name="start">Start point of the line segment</param>
        /// <param name="end">End point of the line segment</param>
        /// <param name="color">Color of the line segment to draw</param>
        /// <param name="width">Width, in pixels of the line segment to draw</param>
        /// <param name="style">Style of the line segment to draw</param>
        void DrawLine(Point start, Point end, Color color, Single width, LineStyle style);

        /// <summary>Draws an ellipse at a center point</summary>
        /// <param name="origin">Origin of the elliptical object to draw</param>
        /// <param name="radiusX">X-radius of the elliptoid</param>
        /// <param name="radiuxY">Y-radius of the elliptoid</param>
        /// <param name="color">Color of the elliptoid to draw</param>
        /// <param name="width">Width, in pixels of the elliptoid to draw</param>
        /// <param name="style">Style of the elliptoid to draw</param>
        void DrawEllipse(Point origin, Single radiusX, Single radiuxY, Color color, Single width, LineStyle style);

        /// <summary>Draws a rectangle at a the specified points</summary>
        /// <param name="upperLeft">Upper-left point of the line segment</param>
        /// <param name="lowerRight">Lower-right point of the line segment</param>
        /// <param name="color">Color of the rectangle to draw</param>
        /// <param name="width">Width, in pixels of the rectangle to draw</param>
        /// <param name="style">Style of the rectangle to draw</param>
        void DrawRectangle(Point upperLeft, Point lowerRight, Color color, Single width, LineStyle style);
        #endregion
    }
}