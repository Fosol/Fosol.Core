using System.Drawing;
using System.IO;

namespace Fosol.Core.Drawing.Extensions.Bytes
{
    /// <summary>
    /// ByteExtensions static class, provides extension methods for Byte objects.
    /// </summary>
    public static class ByteExtensions
    {

        /// <summary>
        /// Converts a byte array into an image.
        /// </summary>
        /// <param name="image">Byte array of data.</param>
        /// <param name="useEmbeddedColorManagement">Use embedded color management information in stream for colouring.</param>
        /// <param name="validateImageData">True to validate image data, otherwise false.</param>
        /// <returns>Image object.</returns>
        public static Image ToImage(this byte[] image, bool useEmbeddedColorManagement = false, bool validateImageData = false)
        {
            using (var stream = new MemoryStream(image))
            {
                return Image.FromStream(stream, useEmbeddedColorManagement, validateImageData);
            }
        }
    }
}
