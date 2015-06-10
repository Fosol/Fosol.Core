using System;
using System.Linq;

namespace Fosol.Core.Security
{
    /// <summary>
    /// PasswordComposition class provides a way to catalog the composition of a given password.
    /// This provides a way to determine the strength of the password.
    /// </summary>
    public class PasswordComposition
    {
        #region Properties
        /// <summary>
        /// get - Number of characters.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// get - Number of nonalphabetic characters.
        /// </summary>
        public int NonalphaCharacters { get; private set; }

        /// <summary>
        /// get - Number of nonalphabetic and nondigit characters.
        /// </summary>
        public int NonalphaDigitCharacters { get; private set; }

        /// <summary>
        /// get - Number of alphabetic characters.
        /// </summary>
        public int AlphaCharacters { get; private set; }

        /// <summary>
        /// get - Number of digit characters.
        /// </summary>
        public int DigitCharacters { get; private set; }

        /// <summary>
        /// get - Number of uppercase characters.
        /// </summary>
        public int UppercaseCharacters { get; private set; }

        /// <summary>
        /// get - Number of lowercase characters.
        /// </summary>
        public int LowercaseCharacters { get; private set; }

        /// <summary>
        /// get - An arbitrary (inherited classes may provide a viable statistic) measurement which reflects the strength of the password as a percentage.
        /// </summary>
        public int Strength { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PasswordStrength object.
        /// </summary>
        /// <param name="password">Password to evaluate the strength of.  The password is not stored.</param>
        public PasswordComposition(string password)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrEmpty(password, nameof(password));

            // Check each character and increment the various strength indicators.
            foreach (var c in password.ToArray())
            {
                if (Char.IsLetter(c))
                    this.AlphaCharacters++;
                else
                    this.NonalphaCharacters++;
                if (Char.IsDigit(c))
                    this.DigitCharacters++;
                if (Char.IsLower(c))
                    this.LowercaseCharacters++;
                if (Char.IsUpper(c))
                    this.UppercaseCharacters++;
                if (!Char.IsLetterOrDigit(c))
                    this.NonalphaDigitCharacters++;
            }

            // Determine the strength of the password.
            this.Strength = this.Evaluate();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Evaluates this PasswordStrength object and returns a percentage that represents the strength of the given password.
        /// This is a arbitrary result and is mostly meaningless.
        /// In reality attributing a percentage to represent a password strength is meaningless without the context of a comparing metric.
        /// This method should also check for dictionary and commong passwords to determine strength.
        /// </summary>
        /// <returns>Percentage to represent the strength of this password.</returns>
        protected int Evaluate()
        {
            var base_strength = 0;

            base_strength += this.Length * 5;

            base_strength += this.NonalphaDigitCharacters * 5;

            if (this.NonalphaDigitCharacters > 0 && this.AlphaCharacters > 0)
                base_strength += Math.Abs(this.NonalphaDigitCharacters / this.AlphaCharacters) * 5;

            if (this.UppercaseCharacters > 0 && this.LowercaseCharacters > 0)
                base_strength += Math.Abs(this.UppercaseCharacters / this.LowercaseCharacters) * 5;

            return base_strength > 100 ? 100 : base_strength;
        }

        /// <summary>
        /// Creates a new instance of a PasswordComposition class for the specified password.
        /// </summary>
        /// <param name="password">Password to evaluate.</param>
        /// <returns>New instance of a PasswordComposition class.</returns>
        public static PasswordComposition Evaluate(string password)
        {
            return new PasswordComposition(password);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
