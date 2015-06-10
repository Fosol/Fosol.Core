using System;

namespace Fosol.Core.Security
{
    /// <summary>
    /// PasswordRequirement class provides a way to define the required composition of a password.
    /// </summary>
    public class PasswordRequirement
    {
        #region Properties
        /// <summary>
        /// get - Minimum number of characters.
        /// </summary>
        public int MinLength { get; private set; }

        /// <summary>
        /// get - Maximum number of characters.
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// get - Minimum number of nonalphabetic characters.
        /// </summary>
        public int MinNonalphaCharacters { get; private set; }

        /// <summary>
        /// get - Minimum number of nonalphabetic and nondigit characters.
        /// </summary>
        public int MinNonalphaDigitCharacters { get; private set; }

        /// <summary>
        /// get - Minimum number of alphabetic characters.
        /// </summary>
        public int MinAlphaCharacters { get; private set; }

        /// <summary>
        /// get - Minimum number of digit characters.
        /// </summary>
        public int MinDigitCharacters { get; private set; }

        /// <summary>
        /// get - Minimum number of uppercase characters.
        /// </summary>
        public int MinUppercaseCharacters { get; private set; }

        /// <summary>
        /// get - Minimum number of lowercase characters.
        /// </summary>
        public int MinLowercaseCharacters { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PasswordRequirement class.
        /// </summary>
        /// <param name="minLength">Minimum length of password.</param>
        /// <param name="maxLength">Maximum length of password.</param>
        /// <param name="minAlphaCharacters">Minimum number of alphabet characters.</param>
        /// <param name="minNonalphaCharacters">Minimum number of nonalphabet characters (digits and symbols).</param>
        public PasswordRequirement(int minLength, int maxLength, int minAlphaCharacters, int minNonalphaCharacters)
        {
            Fosol.Core.Validation.Argument.Assert.IsInRange(minLength, 0, maxLength, nameof(minLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(maxLength, minLength, nameof(maxLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minAlphaCharacters, 0, nameof(minAlphaCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minNonalphaCharacters, 0, nameof(minNonalphaCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMaximum(minAlphaCharacters + minNonalphaCharacters, maxLength, nameof(maxLength));

            this.MinLength = minLength;
            this.MaxLength = maxLength;
            this.MinAlphaCharacters = minAlphaCharacters;
            this.MinNonalphaCharacters = minNonalphaCharacters;
        }

        /// <summary>
        /// Creates a new instance of a PasswordRequirement class.
        /// </summary>
        /// <param name="minLength">Minimum length of password.</param>
        /// <param name="maxLength">Maximum length of password.</param>
        /// <param name="minNonalphaCharacters">Minimum number of nonalphabet characters (digits and symbols).</param>
        /// <param name="minUppercaseCharacters">Minimum number of uppercase characters.</param>
        /// <pparam name="minLowercaseCharacters">Minimum number of lowercase characters.</pparam>
        public PasswordRequirement(int minLength, int maxLength, int minNonalphaCharacters, int minUppercaseCharacters, int minLowercaseCharacters)
        {
            Fosol.Core.Validation.Argument.Assert.IsInRange(minLength, 0, maxLength, nameof(minLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(maxLength, minLength, nameof(maxLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minNonalphaCharacters, 0, nameof(minNonalphaCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minUppercaseCharacters, 0, nameof(minUppercaseCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minLowercaseCharacters, 0, nameof(minLowercaseCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMaximum(minNonalphaCharacters + minUppercaseCharacters + minLowercaseCharacters, maxLength, nameof(maxLength));

            this.MinLength = minLength;
            this.MaxLength = maxLength;
            this.MinNonalphaCharacters = minNonalphaCharacters;
            this.MinUppercaseCharacters = minUppercaseCharacters;
            this.MinLowercaseCharacters = minLowercaseCharacters;
        }

        /// <summary>
        /// Creates a new instance of a PasswordRequirement class.
        /// </summary>
        /// <param name="minLength">Minimum length of password.</param>
        /// <param name="maxLength">Maximum length of password.</param>
        /// <param name="minUppercaseCharacters">Minimum number of uppercase characters.</param>
        /// <pparam name="minLowercaseCharacters">Minimum number of lowercase characters.</pparam>
        /// <param name="minNonalphaDigitCharacters">Minimum number of nonalphabet and nondigit characters (symbols).</param>
        /// <param name="minDigitCharacters">Minimum number of digit characters (0-9).</param>
        public PasswordRequirement(int minLength, int maxLength, int minUppercaseCharacters, int minLowercaseCharacters, int minNonalphaDigitCharacters, int minDigitCharacters)
        {
            Fosol.Core.Validation.Argument.Assert.IsInRange(minLength, 0, maxLength, nameof(minLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(maxLength, minLength, nameof(maxLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minUppercaseCharacters, 0, nameof(minUppercaseCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minLowercaseCharacters, 0, nameof(minLowercaseCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minNonalphaDigitCharacters, 0, nameof(minNonalphaDigitCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minDigitCharacters, 0, nameof(minDigitCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMaximum(minUppercaseCharacters + minLowercaseCharacters + minNonalphaDigitCharacters + minDigitCharacters, maxLength, nameof(maxLength));

            this.MinLength = minLength;
            this.MaxLength = maxLength;
            this.MinUppercaseCharacters = minUppercaseCharacters;
            this.MinLowercaseCharacters = minLowercaseCharacters;
            this.MinNonalphaDigitCharacters = minNonalphaDigitCharacters;
            this.MinDigitCharacters = minDigitCharacters;
        }
        #endregion

        #region Methods
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
