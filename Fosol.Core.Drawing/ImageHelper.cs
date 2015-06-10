using Fosol.Core.Drawing.Extensions;
using Fosol.Core.Drawing.Extensions.Bytes;
using Fosol.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Fosol.Core.Drawing
{
    /// <summary>
    /// Utility methods to modify image sizes.
    /// Provides the following methods, Canvas, Crop, Resize, Scale and Optimize.
    /// 
    /// Canvas:
    /// The original image will stay the same size, but the output will have white space, or it will crop the image.
    /// 
    /// Crop:
    /// This will allow you to crop an image within the current dimensions.
    /// You cannot crop an image to be larger.
    /// 
    /// Resize:
    /// This will allow you to stretch or shrink an image to the desired size.
    /// You may provide only one of the dimensions (width, height) to resize that particlar dimension only.
    /// 
    /// Scale:
    /// This will allow you enlarge or shrink an image but always keep the original scale ratio.
    /// If you provide a fill color it will maintain scale and will not crop.
    /// If you do not provide a fill color it will maintain scale and will crop.
    /// 
    /// Optimize:
    /// Optimize the size of the image by degrading the quality.
    /// 0 would result in the most optimized image, with the poorest quality.
    /// 100 would be the least optimized image, with the original quality.
    /// </summary>
    public class ImageHelper
    {
        #region Constants
        private static readonly CenterPoint _DefaultOffset = OffsetRule.Center;
        #endregion

        #region Variables
        /// <summary>
        /// Caching dictionary for ImageCodecInfo.
        /// </summary>
        private static Dictionary<ImageFormat, ImageCodecInfo> _CachedImageCodecInfo;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The default cropping offset option.
        /// This provides a way to specify the center of the image to enforce a desired cropping behaviour.
        /// </summary>
        public CenterPoint Offset { get; set; }

        /// <summary>
        /// get/set - The default background color to use for autocropped images which do not fill the requested image size.
        /// If the FillColor is null white space is not allowed.
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// get/set - The algorithm used when images are scaled or rotated.
        /// Default value is HighQualityBicubic.
        /// </summary>
        public System.Drawing.Drawing2D.InterpolationMode InterpolationMode { get; set; }

        /// <summary>
        /// get/set - Collection of encoder parameters to use when generating the new image.
        /// The following parameters can be applied to the generated image;
        /// ChrominanceTable
        /// ColorDepth
        /// Compression
        /// LuminanceTable
        /// Quality
        /// RenderMethod
        /// SaveFlag
        /// ScanMethod
        /// Transformation
        /// Version
        /// </summary>
        public EncoderParameters EncoderParameters { get; set; }

        /// <summary>
        /// get - The image that will be autocropped.
        /// </summary>
        public Image Photo { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an Autocrop object.
        /// Initialize the default properties.
        /// </summary>
        private ImageHelper()
        {
            this.Offset = _DefaultOffset;
            this.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        }

        /// <summary>
        /// Creates a new instace of an Autocrop object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "imageStream" must be readable.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "imageStream" cannot be null.</exception>
        /// <param name="imageStream">Source stream with image that will be autocropped.</param>
        /// <param name="useEmbeddedColorManagement">Determines whether it should use the embedded color management information in the stream.</param>
        /// <param name="validateImageData">Determines whether it should validate the image data after converting.</param>
        public ImageHelper(Stream imageStream, bool useEmbeddedColorManagement = false, bool validateImageData = false)
            : this()
        {
            Validation.Argument.Assert.IsNotNull(imageStream, nameof(imageStream));
            Validation.Argument.Assert.IsValid(imageStream.CanRead, nameof(imageStream), "Parameter '{0}.CanRead' must be true.");

            this.Photo = Image.FromStream(imageStream, useEmbeddedColorManagement, validateImageData);
        }

        /// <summary>
        /// Creates a new instance of an Autocrop object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "image" cannot be null.</exception>
        /// <param name="image">Source image that will be autocropped.</param>
        public ImageHelper(Image image)
            : this()
        {
            Validation.Argument.Assert.IsNotNull(image, nameof(image));

            this.Photo = image;
        }

        /// <summary>
        /// Creates a new instance of an Autocrop object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "image" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "image" cannot be null.</exception>
        /// <param name="image">Source image that will be autocropped.</param>
        /// <param name="useEmbeddedColorManagement">Determines whether it should use the embedded color management information in the stream.</param>
        /// <param name="validateImageData">Determines whether it should validate the image data after converting.</param>
        public ImageHelper(byte[] image, bool useEmbeddedColorManagement = false, bool validateImageData = false)
            : this()
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(image, nameof(image));

            this.Photo = image.ToImage(useEmbeddedColorManagement, validateImageData);
        }

        /// <summary>
        /// Creates a new instacne of an Autocrop object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "filename" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "filename" cannot be null.</exception>
        /// <param name="filename">Name and path to the file of the image.</param>
        /// <param name="useEmbeddedColorManagement">Set to true to use color management information set in the file.</param>
        public ImageHelper(string filename, bool useEmbeddedColorManagement = false)
            : this()
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(filename, nameof(filename));

            this.Photo = Image.FromFile(filename, useEmbeddedColorManagement);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the encoder information for a specified ImageFormat value.
        /// </summary>
        /// <param name="format">ImageFormat to retrieve encoder information for.</param>
        /// <returns>ImageCodecInfo object.</returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            // Cached dictionary needs to be initialized.
            if (_CachedImageCodecInfo == null)
                _CachedImageCodecInfo = new Dictionary<ImageFormat, ImageCodecInfo>();

            // Add it to the cached dictionary.
            if (!_CachedImageCodecInfo.ContainsKey(format))
            {
                var codec = ImageCodecInfo.GetImageDecoders().First(d => d.FormatID == format.Guid);
                _CachedImageCodecInfo.Add(format, codec);
            }

            return _CachedImageCodecInfo[format];
        }

        /// <summary>
        /// Resizing an images canvas.
        /// You must specify at least one dimension (width or height).
        /// If you do not specify one dimension (width or height) that dimension will default to the original image dimension.
        /// You must specify a fill.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="size">Size of the new image.</param>
        /// <param name="fill">Background color of image if the size of the new image is greater than the destination rectangle.</param>
        /// <param name="offset">Control where the image will be drawn in the new canvas.</param>
        /// <param name="quality">Quality of the image.</param>
        /// <param name="graphicsUnit">GraphicsUnit option.</param>
        /// <returns>Size of the new image in bytes</returns>
        public long Canvas(Stream destination, Size size, Color fill, CenterPoint offset = null, long? quality = null, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            Validation.Argument.Assert.IsNotNull(size, nameof(size));
            Validation.Argument.Assert.IsMinimum(size.Width, 0, nameof(size), "Parameter '{0}.Width' is an invalid resize value.");
            Validation.Argument.Assert.IsMinimum(size.Height, 0, nameof(size), "Parameter '{0}.Height' is an invalid resize value.");

            return Canvas(destination, size.Width, size.Height, fill, offset, quality, graphicsUnit);
        }

        /// <summary>
        /// Resizing an images canvas.
        /// You must specify at least one dimension (width or height).
        /// If you do not specify one dimension (width or height) that dimension will default to the original image dimension.
        /// You must specify a fill.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="width">Width of the new image.</param>
        /// <param name="height">Height of the new image.</param>
        /// <param name="fill">Background color of image if the size of the new image is greater than the destination rectangle.</param>
        /// <param name="offset">Control where the image will be drawn in the new canvas.</param>
        /// <param name="quality">Quality of the image.</param>
        /// <param name="graphicsUnit">GraphicsUnit option.</param>
        /// <returns>Size of the new image in bytes</returns>
        public long Canvas(Stream destination, int width, int height, Color fill, CenterPoint offset = null, long? quality = null, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            Validation.Argument.Assert.IsNotNull(destination, nameof(destination));
            Validation.Argument.Assert.IsValid(destination.CanWrite, nameof(destination), "Parameter '{0}.CanWrite' must be true.");
            Validation.Argument.Assert.IsValid(destination.CanSeek, nameof(destination), "Parameter '{0}.CanSeek' must be true.");
            Validation.Argument.Assert.IsMinimum(width, 0, nameof(width));
            Validation.Argument.Assert.IsMinimum(height, 0, nameof(height));
            Validation.Argument.Assert.IsNotNull(fill, nameof(fill));
            Validation.Argument.Assert.IsInRange(quality, 0, 100, nameof(quality));

            // Initialize default values.
            Initialization.Assert.IsNotDefault(ref width, this.Photo.Width);
            Initialization.Assert.IsNotDefault(ref height, this.Photo.Height);
            Initialization.Assert.IsNotNull(ref offset, this.Offset);

            // If height and width are the same as the image, return the original image.
            if (width == this.Photo.Width && height == this.Photo.Height)
            {
                this.Photo.Save(destination, this.Photo.RawFormat);
                return destination.Length;
            }

            // Create the destination and source rectangles.
            // These rectangles are used by the Graphics object to modify the dimensions of the image.
            var size = new Size(width, height);
            var dest_rect = new Rectangle(0, 0, width, height);
            var source_rect = new Rectangle(0, 0, this.Photo.Width, this.Photo.Height);

            CalculateCanvas(ref size, ref dest_rect, ref source_rect, offset);

            return Generate(destination, size, dest_rect, source_rect, fill, quality, graphicsUnit);
        }

        /// <summary>
        /// Crop the image.
        /// You must plot the crop by setting at least one property greater than '0' (X, Y, Width, Height).
        /// Crop cannot polt a larger crop than the size of the original image.
        /// If you want a larger image without resizing use the Canvas method.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="plot">Plot dimensions to crop image.</param>
        /// <param name="quality">Quality of the image.</param>
        /// <param name="graphicsUnit">GraphicsUnit option.</param>
        /// <returns>Size of the new image in bytes</returns>
        public long Crop(Stream destination, Rectangle plot, long? quality = null, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            Validation.Argument.Assert.IsNotNull(plot, nameof(plot));
            Validation.Argument.Assert.IsInRange(plot.X, 0, this.Photo.Width - 1, nameof(plot), "Parameter '{0}.X' is invalid.");
            Validation.Argument.Assert.IsInRange(plot.Y, 0, this.Photo.Height - 1, nameof(plot), "Parameter '{0}.Y' is invalid.");
            Validation.Argument.Assert.IsInRange(plot.Width, 0, this.Photo.Width, nameof(plot), "Parameter '{0}.Width' is invalid.");
            Validation.Argument.Assert.IsInRange(plot.Height, 0, this.Photo.Height, nameof(plot), "Parameter '{0}.Height' is invalid.");
            Validation.Argument.Assert.IsInRange(plot.X + plot.Width, 0, this.Photo.Width - plot.X, nameof(plot), "Parameter '{0}.X' and '{0}.Width' is invalid.");
            Validation.Argument.Assert.IsInRange(plot.Y + plot.Height, 0, this.Photo.Height - plot.Y, nameof(plot), "Parameter '{0}.Y' and '{0}.Height' is invalid.");
            Validation.Argument.Assert.IsValid(plot.X + plot.Y + plot.Width + plot.Height != 0, nameof(plot), "Parameter '{0}' is invalid.");

            return Crop(destination, plot.X, plot.Y, plot.Width, plot.Height, quality, graphicsUnit);
        }

        /// <summary>
        /// Crop the image.
        /// You must plot the crop by setting at least one property greater than '0' (xPosition, yPosition, Width, Height).
        /// Crop cannot polt a larger crop than the size of the original image.
        /// If you want a larger image without resizing use the Canvas method.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="xPosition">X-coordinate which is the upper left corner.</param>
        /// <param name="yPosition">Y-coordinate which is the upper left corner.</param>
        /// <param name="width">Width of new image.</param>
        /// <param name="height">Height of new image.</param>
        /// <param name="quality">Quality of the image.</param>
        /// <param name="graphicsUnit">GraphicsUnit option.</param>
        /// <returns>Size of the new image in bytes</returns>
        public long Crop(Stream destination, int xPosition, int yPosition, int width, int height, long? quality = null, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            Validation.Argument.Assert.IsNotNull(destination, nameof(destination));
            Validation.Argument.Assert.IsValid(destination.CanWrite, nameof(destination), "Parameter '{0}.CanWrite' must be true.");
            Validation.Argument.Assert.IsValid(destination.CanSeek, nameof(destination), "Parameter '{0}.CanSeek' must be true.");
            Validation.Argument.Assert.IsInRange(xPosition, 0, this.Photo.Width - 1, nameof(xPosition));
            Validation.Argument.Assert.IsInRange(yPosition, 0, this.Photo.Height - 1, nameof(yPosition));
            Validation.Argument.Assert.IsInRange(width, 0, this.Photo.Width, nameof(width));
            Validation.Argument.Assert.IsInRange(height, 0, this.Photo.Height, nameof(height));
            Validation.Argument.Assert.IsInRange(xPosition + width, 0, this.Photo.Width - xPosition, "xPosition, width");
            Validation.Argument.Assert.IsInRange(yPosition + height, 0, this.Photo.Height - yPosition, "yPosition, height");
            Validation.Argument.Assert.IsValid(width + height + xPosition + yPosition != 0, "xPosition, yPosition, width, height", "Parameters 'xPosition', 'yPosition', 'width' and 'height' are invalid.");
            Validation.Argument.Assert.IsInRange(quality, 0, 100, nameof(quality));

            // Calculate thee width based on the xPosition.
            if (xPosition > 0 && (width == 0 || (xPosition + width) > this.Photo.Width))
                width = this.Photo.Width - xPosition;
            // Default the width if it wasn't set to the source image width.
            else if (xPosition == 0 && width == 0)
                width = this.Photo.Width;

            // Calculate the height based on the yPosition.
            if (yPosition > 0 && (height == 0 || (yPosition + height) > this.Photo.Height))
                height = this.Photo.Height - yPosition;
            // Default the height if it wasn't set to the source image height.
            else if (yPosition == 0 & height == 0)
                height = this.Photo.Height;

            // If the x and y coordinates are at 0.
            // If height and width are the same as the image, return the original image.
            if (xPosition == 0 && yPosition == 0 && width == this.Photo.Width && height == this.Photo.Height)
            {
                this.Photo.Save(destination, this.Photo.RawFormat);
                return destination.Length;
            }

            // Create the destination and source rectangles.
            // These rectangles are used by the Graphics object to modify the dimensions of the image.
            var size = new Size(width, height);
            var dest_rect = new Rectangle(0, 0, width, height);
            var source_rect = new Rectangle(xPosition, yPosition, width, height);

            CalculateCrop(ref size, ref dest_rect, ref source_rect);

            return Generate(destination, size, dest_rect, source_rect, null, quality, graphicsUnit);
        }

        /// <summary>
        /// Resizing an image.
        /// You must specify at least one dimension (width or height).
        /// If you do not specify one dimension (width or height) it will automatically keep the scale of the original image.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="size">
        ///     Size of the new image.
        ///     When one of the dimensions (width, height) is not provided it will automatically keep the scale of the original image.
        /// </param>
        /// <param name="
        /// <param name="quality">Quality of the image.</param>
        /// <param name="graphicsUnit">GraphicsUnit option.</param>
        /// <returns>Size of the new image in bytes</returns>
        public long Resize(Stream destination, Size size, long? quality = null, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            Validation.Argument.Assert.IsNotNull(size, nameof(size));
            Validation.Argument.Assert.IsMinimum(size.Width, 0, nameof(size));
            Validation.Argument.Assert.IsMinimum(size.Height, 0, nameof(size));
            Validation.Argument.Assert.IsValid(size.Width + size.Height != 0, nameof(size), "Parameter '{0}.Width' and '{0}.Height' must not equal 0.");

            return Resize(destination, size.Width, size.Height, quality, graphicsUnit);
        }

        /// <summary>
        /// Resizing an image.
        /// You must specify at least one dimension (width or height).
        /// If you do not specify one dimension (width or height) it will use the original corresponding dimension.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="width">Width of the new image.</param>
        /// <param name="height">Height of the new image.</param>
        /// <param name="quality">Quality of the image.</param>
        /// <param name="graphicsUnit">GraphicsUnit option.</param>
        /// <returns>Size of the new image in bytes</returns>
        public long Resize(Stream destination, int width, int height, long? quality = null, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            Validation.Argument.Assert.IsNotNull(destination, nameof(destination));
            Validation.Argument.Assert.IsValid(destination.CanWrite, nameof(destination), "Parameter \"{0}.CanWrite\" must be true.");
            Validation.Argument.Assert.IsValid(destination.CanSeek, nameof(destination), "Parameter \"{0}.CanSeek\" must be true.");
            Validation.Argument.Assert.IsMinimum(width, 0, nameof(width));
            Validation.Argument.Assert.IsMinimum(height, 0, nameof(height));
            Validation.Argument.Assert.IsValid(width + height != 0, nameof(width), "Parameter \"" + nameof(width) + "\" and \"" + nameof(height) + "\" must not equal 0.");
            Validation.Argument.Assert.IsInRange(quality, 0, 100, nameof(quality));

            // Intialize default values.
            Initialization.Assert.IsNotDefault(ref width, this.Photo.Width);
            Initialization.Assert.IsNotDefault(ref height, this.Photo.Height);

            // If height and width are the same as the image, return the original image.
            if (width == this.Photo.Width && height == this.Photo.Height)
            {
                this.Photo.Save(destination, this.Photo.RawFormat);
                return destination.Length;
            }

            // Create the destination and source rectangles.
            // These rectangles are used by the Graphics object to modify the dimensions of the image.
            var size = new Size(width, height);
            var dest_rect = new Rectangle(0, 0, width, height);
            var source_rect = new Rectangle(0, 0, this.Photo.Width, this.Photo.Height);

            CalculateResize(ref size, ref dest_rect, ref source_rect);

            return Generate(destination, size, dest_rect, source_rect, null, quality, graphicsUnit);
        }

        /// <summary>
        /// Resize an image but maintain the original images scale ratio.
        /// If you provide a fill color it will enable whitespace, thus the image will never crop.
        /// If you do not provide a fill color it will crop the image to fit the new size.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="width">The new width of the image.</param>
        /// <param name="height">The new height of the image.</param>
        /// <param name="fill">Background color used to fill whitespace.</param>
        /// <param name="offset">Control how cropping will be determine.</param>
        /// <param name="quality">Quality of the image.</param>
        /// <param name="graphicsUnit">GraphicsUnit option.</param>
        /// <returns>Size of the new image in bytes</returns>
        public long Scale(Stream destination, int width, int height, Color? fill = null, CenterPoint offset = null, long? quality = null, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            Validation.Argument.Assert.IsNotNull(destination, nameof(destination));
            Validation.Argument.Assert.IsValid(destination.CanWrite, nameof(destination), "Parameter \"{0}.CanWrite\" must be true.");
            Validation.Argument.Assert.IsValid(destination.CanSeek, nameof(destination), "Parameter \"{0}.CanSeek\" must be true.");
            Validation.Argument.Assert.IsMinimum(width, 0, nameof(width));
            Validation.Argument.Assert.IsMinimum(height, 0, nameof(height));
            Validation.Argument.Assert.IsMinimum(width + height, 1, nameof(width), "Parameter \"" + nameof(width) + "\" and \"" + nameof(height) + "\" must not equal 0.");
            Validation.Argument.Assert.IsInRange(quality, 0, 100, nameof(quality));

            // Initialize default values.
            Initialization.Assert.IsNotNull(ref offset, this.Offset);

            // If height and width are the same as the image, return the original image.
            if (width == this.Photo.Width && height == this.Photo.Height)
            {
                this.Photo.Save(destination, this.Photo.RawFormat);
                return destination.Length;
            }

            // Create the destination and source rectangles.
            // These rectangles are used by the Graphics object to modify the dimensions of the image.
            var image_size = new Size(width, height);
            var dest_rect = new Rectangle(0, 0, width, height);
            var source_rect = new Rectangle(0, 0, this.Photo.Width, this.Photo.Height);

            CalculateScale(ref image_size, ref dest_rect, ref source_rect, fill.HasValue, offset);

            return Generate(destination, image_size, dest_rect, source_rect, fill, quality, graphicsUnit);
        }

        /// <summary>
        /// Optimize the image size by lowering the quality of the image.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="quality">Quality of the image.</param>
        /// <returns>Size of the new image in bytes</returns>
        public long Optimize(Stream destination, long quality)
        {
            Validation.Argument.Assert.IsNotNull(destination, nameof(destination));
            Validation.Argument.Assert.IsValid(destination.CanWrite, nameof(destination), "Parameter \"{0}.CanWrite\" must be true.");
            Validation.Argument.Assert.IsValid(destination.CanSeek, nameof(destination), "Parameter \"{0}.CanSeek\" must be true.");
            Validation.Argument.Assert.IsInRange(quality, 1, 100, nameof(quality));

            // Create the destination and source rectangles.
            // These rectangles are used by the Graphics object to modify the dimensions of the image.
            var dest_rect = new Rectangle(0, 0, this.Photo.Width, this.Photo.Height);
            var source_rect = new Rectangle(0, 0, this.Photo.Width, this.Photo.Height);

            return Generate(destination, this.Photo.Size, dest_rect, source_rect, null, quality, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Generate the new image.
        /// </summary>
        /// <param name="destination">Stream to place the new image.</param>
        /// <param name="imageSize">Size of the new image.</param>
        /// <param name="destRect">Graphics destination rectangle.</param>
        /// <param name="sourceRect">Graphics source rectangle.</param>
        /// <param name="fill">Fill color if the new image is resized to larger dimensions.</param>
        /// <param name="quality">Quality of the image.</param>
        /// <param name="graphicsUnit">GraphicsUnit option.</param>
        /// <returns>Size of the new image in bytes</returns>
        private long Generate(Stream destination, Size imageSize, Rectangle destRect, Rectangle sourceRect, Color? fill = null, long? quality = null, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            using (var bitmap = new Bitmap(imageSize.Width, imageSize.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = this.InterpolationMode;

                    // Fill graphics with a background color.
                    if (fill.HasValue)
                        graphics.Clear(fill.Value);

                    graphics.DrawImage(this.Photo, destRect, sourceRect, graphicsUnit);

                    EncoderParameters encoder_params = null;
                    if (this.EncoderParameters == null && quality.HasValue)
                    {
                        encoder_params = new EncoderParameters(1);
                        encoder_params.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality.Value);
                    }
                    if (this.EncoderParameters != null)
                    {
                        // Use the overridden quality parameter and make sure there arent duplicate quality params in the EncoderParameters.
                        if (quality.HasValue)
                        {
                            var duplicate = this.EncoderParameters.Param.Count(p => p.Encoder == Encoder.Quality);
                            encoder_params = new EncoderParameters(this.EncoderParameters.Param.Length + 1 - duplicate);
                            // Make sure there isn't two Encoder.Quality parameters.
                            for (var i = 0; i < this.EncoderParameters.Param.Length; i++)
                            {
                                if (this.EncoderParameters.Param[i].Encoder == Encoder.Quality)
                                    encoder_params.Param[i] = new EncoderParameter(Encoder.Quality, quality.Value);
                                else
                                    encoder_params.Param[i] = this.EncoderParameters.Param[i];
                            }
                        }
                        else
                            encoder_params = this.EncoderParameters;
                    }

                    bitmap.Save(destination, ImageHelper.GetEncoder(this.Photo.RawFormat), encoder_params);
                }
            }

            return destination.Length;
        }

        /// <summary>
        /// Calculate the size, destination rectangle and source rectangle that will be used to create the new image.
        /// </summary>
        /// <param name="size">The size of the new image.</param>
        /// <param name="dest">Rectangle for destination image.</param>
        /// <param name="source">Rectangle for source image.</param>
        /// <param name="offset">Offset point used to control cropping.</param>
        protected void CalculateCanvas(ref Size size, ref Rectangle dest, ref Rectangle source, CenterPoint offset)
        {
            source.Width = this.Photo.Width;
            source.Height = this.Photo.Height;
            dest.Width = this.Photo.Width;
            dest.Height = this.Photo.Height;

            // Center image inside new larger canvas.
            dest.X = (int)Mathematics.Formula.OffsetCenter(size.Width - this.Photo.Width, offset.X);
            dest.Y = (int)Mathematics.Formula.OffsetCenter(size.Height - this.Photo.Height, offset.Y * -1);
        }

        /// <summary>
        /// Calculate the size, destination rectangle and source rectangle that will be used to create the new image.
        /// </summary>
        /// <param name="size">The size of the new image.</param>
        /// <param name="dest">Rectangle for destination image.</param>
        /// <param name="source">Rectangle for source image.</param>
        protected void CalculateCrop(ref Size size, ref Rectangle dest, ref Rectangle source)
        {
            // At present this is not required, but if the class is overridden that class can calculate crop rectangles.
        }

        /// <summary>
        /// Calculate the size, destination rectangle and source rectangle that will be used to create the new image.
        /// </summary>
        /// <param name="size">The size of the new image.</param>
        /// <param name="dest">Rectangle for destination image.</param>
        /// <param name="source">Rectangle for source image.</param>
        protected void CalculateResize(ref Size size, ref Rectangle dest, ref Rectangle source)
        {
            dest.Width = size.Width;
            dest.Height = size.Height;
        }

        /// <summary>
        /// Calculate the size, destination rectangle and source rectangle that will be used to create the new image.
        /// If one of the dimensions (width, height) is not provided it will be automically calculated to maintain scale.
        /// </summary>
        /// <param name="size">The size of the new image.</param>
        /// <param name="dest">Rectangle for destination image.</param>
        /// <param name="source">Rectangle for source image.</param>
        /// <param name="allowWhitespace">Determines whether the new image has whitespace.</param>
        /// <param name="offset">Offset point used to control cropping.</param>
        protected void CalculateScale(ref Size size, ref Rectangle dest, ref Rectangle source, bool allowWhitespace, CenterPoint offset)
        {
            // Calculate the width based on the ratio.
            if (size.Width == 0)
            {
                var ratio = Formula.Ratio(size.Height, source.Height);
                size.Width = Formula.Resize(source.Width, ratio);
            }
            // Calculate the height based on the ratio.
            else if (size.Height == 0)
            {
                var ratio = Formula.Ratio(size.Width, source.Width);
                size.Height = Formula.Resize(source.Width, ratio);
            }

            dest = Mathematics.Formula.Scale(this.Photo.Size, size, offset, allowWhitespace);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
