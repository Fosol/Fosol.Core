using Fosol.Core.Extensions.Bytes;
using Fosol.Core.Extensions.Strings;
using System;

namespace Fosol.Core.Drawing.Extensions.Colors
{
    /// <summary>
    /// ColorExtensions static class, provides extension methods for Drawing.Color objects.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Translates a html hexadecimal definition of a color into a .NET Framework Color.
        /// The input string may start with a '#' character and be followed by 6 to 8 hexadecimal
        /// digits. The hex values A-F are not case sensitive. 
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "value" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "value" cannot be null.</exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        /// <param name="hexString">Hex color value.</param>
        /// <returns>Color object.</returns>
        public static System.Drawing.Color HexToColor(this String value)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(value, "value");

            byte a = 255, r = 0, g = 0, b = 0;
            // #123
            if (value.StartsWith("#") && value.Length == 4)
            {
                r = value.Substring(1, 1).HexToByte();
                g = value.Substring(2, 1).HexToByte();
                b = value.Substring(3, 1).HexToByte();

                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
            // #1234
            else if (value.StartsWith("#") && value.Length == 5)
            {
                a = value.Substring(1, 1).HexToByte();
                r = value.Substring(2, 1).HexToByte();
                g = value.Substring(3, 1).HexToByte();
                b = value.Substring(4, 1).HexToByte();

                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
            // #010203
            else if (value.StartsWith("#") && value.Length == 7)
            {
                r = value.Substring(1, 2).HexToByte();
                g = value.Substring(3, 2).HexToByte();
                b = value.Substring(5, 2).HexToByte();

                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
            // #01020304
            else if (value.StartsWith("#") && value.Length == 9)
            {
                a = value.Substring(1, 2).HexToByte();
                r = value.Substring(3, 2).HexToByte();
                g = value.Substring(5, 2).HexToByte();
                b = value.Substring(7, 2).HexToByte();

                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
            else if (!value.StartsWith("#"))
            {
                try
                {
                    // Attempt to parse the color as a name.
                    return System.Drawing.Color.FromName(value.Substring(1));
                }
                catch
                {
                    // 123
                    if (value.Length == 3)
                    {
                        r = value.Substring(0, 1).HexToByte();
                        g = value.Substring(1, 1).HexToByte();
                        b = value.Substring(2, 1).HexToByte();

                        return System.Drawing.Color.FromArgb(a, r, g, b);
                    }
                    // 1234
                    else if (value.Length == 4)
                    {
                        a = value.Substring(0, 1).HexToByte();
                        r = value.Substring(1, 1).HexToByte();
                        g = value.Substring(2, 1).HexToByte();
                        b = value.Substring(3, 1).HexToByte();

                        return System.Drawing.Color.FromArgb(a, r, g, b);
                    }
                    // 010203
                    else if (value.Length == 6)
                    {
                        r = value.Substring(0, 2).HexToByte();
                        g = value.Substring(2, 2).HexToByte();
                        b = value.Substring(4, 2).HexToByte();

                        return System.Drawing.Color.FromArgb(a, r, g, b);
                    }
                    // 01020304
                    else if (value.Length == 8)
                    {
                        a = value.Substring(0, 2).HexToByte();
                        r = value.Substring(2, 2).HexToByte();
                        g = value.Substring(4, 2).HexToByte();
                        b = value.Substring(6, 2).HexToByte();

                        return System.Drawing.Color.FromArgb(a, r, g, b);
                    }
                }
            }

            // An invalid color was supplied, throw exceptoin.
            throw new ArgumentException("value");
        }

        /// <summary>
        /// Translates a .NET Framework Color into a string containing the html hexadecimal 
        /// representation of a color. The string has a leading '#' character that is followed 
        /// by 6 hexadecimal digits.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "color" cannot be null.</exception>
        /// <param name="actColor">Color object.</param>
        /// <returns>Hex value</returns>
        public static String ColorToHex(this System.Drawing.Color color)
        {
            Validation.Argument.Assert.IsNotNull(color, "color");
            return "#" + color.A.ToHex() + color.R.ToHex() + color.G.ToHex() + color.B.ToHex();
        }
    }
}
