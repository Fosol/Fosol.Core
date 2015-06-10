using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fosol.Core.Drawing.Extensions.Images
{
    /// <summary>
    /// ImageExtensions static class, provides extension methods for Image objects.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Converts an image to a byte array using the specified ImageFormat.
        /// </summary>
        /// <param name="image">Image to convert into a byte array.</param>
        /// <param name="format">ImageFormat of the image.  If null it will use the image.RawFormat.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToByteArray(this Image image, ImageFormat format = null)
        {
            Initialization.Assert.IsNotNull(ref format, image.RawFormat);

            using (var stream = new MemoryStream())
            {
                image.Save(stream, format);
                return stream.ToArray();
            }
        }
    }
}
