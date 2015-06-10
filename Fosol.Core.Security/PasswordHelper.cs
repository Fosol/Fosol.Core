using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Fosol.Core.Security
{
    /// <summary>
    /// PasswordHelper provides methods to assit in password generation and testing.
    /// 
    /// Initialize a new instance of PasswordHelper with your password requirements, then use the Validate() method to validate any password.
    /// </summary>
    public sealed class PasswordHelper
    {
        #region Variables
        private static readonly string _NonalphaDigitCharacters = ".*[^A-Za-z0-9]";
        private string _PasswordStrengthRegularExpression;
        private PasswordRequirement _Requirements;
        private Regex _Validator;
        #endregion

        #region Properties
        /// <summary>
        /// get - The PasswordRequirement object.
        /// </summary>
        public PasswordRequirement Requirements
        {
            get { return _Requirements; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of PasswordHelper object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter "requirements" must not be null.</exception>
        /// <param name="requirements">PasswordRequirment object to intialize with.</param>
        public PasswordHelper(PasswordRequirement requirements)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(requirements, nameof(requirements));

            _Requirements = requirements;
        }

        /// <summary>
        /// Creates a new instance of PasswordHelper object.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameters 'minLength', 'maxLength', 'minRequiredLowercaseCharacters', 'minRequiredUppercaseCharacters', 'minRequiredNumericCharacters', 'minRequiredSpecialCharacters' cannot be less than zero.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameters 'maxLength' must be greater than the sum of the other minimum requirements.</exception>
        /// <param name="minLength">Minimum length of characters required.</param>
        /// <param name="maxLength">Maximum length of characters required.</param>
        /// <param name="minRequiredNonalphaDigitCharacters">Minimum number of nonalphadigit characters required.</param>
        /// <param name="minRequiredLowercaseCharacters">Minimum number of lower-case characters required.</param>
        /// <param name="minRequiredUppercaseCharacters">Minimum number of upper-case characters required.</param>
        /// <param name="minRequiredDigitCharacters">Minimum number of digit characters required.</param>
        public PasswordHelper(int minLength, int maxLength, int minRequiredNonalphaDigitCharacters, int minRequiredLowercaseCharacters, int minRequiredUppercaseCharacters, int minRequiredDigitCharacters)
        {
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minLength, 0, nameof(minLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(maxLength, 0, nameof(maxLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredNonalphaDigitCharacters, 0, nameof(minRequiredNonalphaDigitCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredLowercaseCharacters, 0, nameof(minRequiredLowercaseCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredUppercaseCharacters, 0, nameof(minRequiredUppercaseCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredDigitCharacters, 0, nameof(minRequiredDigitCharacters));

            if (maxLength > 0)
            {
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minLength, maxLength, nameof(minLength));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredNonalphaDigitCharacters, maxLength, nameof(minRequiredNonalphaDigitCharacters));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredLowercaseCharacters, maxLength, nameof(minRequiredLowercaseCharacters));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredUppercaseCharacters, maxLength, nameof(minRequiredUppercaseCharacters));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredDigitCharacters, maxLength, nameof(minRequiredDigitCharacters));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredNonalphaDigitCharacters + minRequiredLowercaseCharacters + minRequiredUppercaseCharacters + minRequiredDigitCharacters, maxLength, "maxLength", "Parameter 'maxLength' is not large enough to accept your other minimum requirements.");
            }

            _Requirements = new PasswordRequirement(minLength, maxLength, minRequiredUppercaseCharacters, minRequiredLowercaseCharacters, minRequiredNonalphaDigitCharacters, minRequiredDigitCharacters);
        }

        /// <summary>
        /// Creates a new instance of PasswordHelper object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'passwordStrengthRegularExpression' cannot be empty or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'passwordStrengthRegularExpression' cannot be null.</exception>
        /// <param name="passwordStrengthRegularExpression">Regular expression string to validate the password.</param>
        public PasswordHelper(string passwordStrengthRegularExpression)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(passwordStrengthRegularExpression, nameof(passwordStrengthRegularExpression));

            _PasswordStrengthRegularExpression = passwordStrengthRegularExpression;
            _Validator = new Regex(passwordStrengthRegularExpression);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validates the specified password to determine if it adheres to the configured requirements.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'password' cannot be null.</exception>
        /// <param name="password">Password string to test.</param>
        /// <returns>True if the password is valid.</returns>
        public bool Validate(string password)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(password, nameof(password));

            if (_Validator != null)
                return _Validator.IsMatch(password);

            return this.Validate(new PasswordComposition(password));
        }

        /// <summary>
        /// Evaluates the password strength with the password requirements.
        /// </summary>
        /// <param name="strength">Strength of the password being validated.</param>
        /// <returns>True if the password strength is valid.</returns>
        private bool Validate(PasswordComposition strength)
        {
            return this.Requirements.MinLength <= strength.Length
                && this.Requirements.MaxLength >= strength.Length
                && this.Requirements.MinLowercaseCharacters <= strength.LowercaseCharacters
                && this.Requirements.MinUppercaseCharacters <= strength.UppercaseCharacters
                && this.Requirements.MinDigitCharacters <= strength.DigitCharacters
                && this.Requirements.MinNonalphaCharacters <= strength.NonalphaCharacters
                && this.Requirements.MinNonalphaDigitCharacters <= strength.NonalphaDigitCharacters;
        }

        /// <summary>
        /// Validates the specified password to determine if it adheres to the specified regular expression.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'passwordStrengthRegularExpression' cannot be empty or white space.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters 'password', and 'passwordStrengthRegularExpression' cannot be null.</exception>
        /// <param name="password">Password string to test.</param>
        /// <param name="passwordStrengthRegularExpression">Regular expression to test the strength of the password.</param>
        /// <returns>True if the password if valid.</returns>
        public static bool IsValid(string password, string passwordStrengthRegularExpression)
        {
            Fosol.Core.Validation.Argument.Assert.IsNotNull(password, nameof(password));
            Fosol.Core.Validation.Argument.Assert.IsNotNullOrWhitespace(passwordStrengthRegularExpression, nameof(passwordStrengthRegularExpression));

            return Regex.IsMatch(password, passwordStrengthRegularExpression);
        }

        /// <summary>
        /// Creates a regular expression the validates the specified rules.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameters 'minLength', 'maxLength', 'minRequiredNonalphaDigitCharacters', 'minRequiredLowercaseCharacters', 'minRequiredUpperCaseCharacters', 'minRequiredDigitCharacters' cannot be less than zero.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameters 'maxLength' must be creater than the sum of the other minimum requirements.</exception>
        /// <param name="minLength">Minimum length of the password required.</param>
        /// <param name="maxLength">Maximum length of the password required.</param>
        /// <param name="minRequiredNonalphaDigitCharacters">Minimum number of nonalphadigit characters required.</param>
        /// <param name="minRequiredLowercaseCharacters">Minimum number of lower-case characters required.</param>
        /// <param name="minRequiredUppercaseCharacters">Minimum number of upper-case characters require.</param>
        /// <param name="minRequiredDigitCharacters">Minimum number of digit characters required.</param>
        /// <returns>A new regular expression string.</returns>
        public static string CreatePasswordStrengthRegularExpression(int minLength, int maxLength, int minRequiredNonalphaDigitCharacters, int minRequiredLowercaseCharacters, int minRequiredUppercaseCharacters, int minRequiredDigitCharacters)
        {
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minLength, 0, nameof(minLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(maxLength, 0, nameof(maxLength));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredNonalphaDigitCharacters, 0, nameof(minRequiredNonalphaDigitCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredLowercaseCharacters, 0, nameof(minRequiredLowercaseCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredUppercaseCharacters, 0, nameof(minRequiredUppercaseCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredDigitCharacters, 0, nameof(minRequiredDigitCharacters));

            if (maxLength > 0)
            {
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minLength, maxLength, nameof(minLength));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredNonalphaDigitCharacters, maxLength, nameof(minRequiredNonalphaDigitCharacters));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredLowercaseCharacters, maxLength, nameof(minRequiredLowercaseCharacters));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredUppercaseCharacters, maxLength, nameof(minRequiredUppercaseCharacters));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredDigitCharacters, maxLength, nameof(minRequiredDigitCharacters));
                Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredLowercaseCharacters + minRequiredUppercaseCharacters + minRequiredDigitCharacters + minRequiredNonalphaDigitCharacters, maxLength, "maxLength", "Parameter 'maxLength' is not large enough to accept your other minimum requirements.");
            }

            var expression = new StringBuilder();

            expression.Append("^");

            if (minRequiredNonalphaDigitCharacters > 0)
            {
                expression.Append("(?=");
                for (var i = 0; i < minRequiredNonalphaDigitCharacters; i++)
                    expression.Append(_NonalphaDigitCharacters);
                expression.Append(")");
            }

            if (minRequiredLowercaseCharacters > 0)
            {
                expression.Append("(?=");
                for (var i = 0; i < minRequiredLowercaseCharacters; i++)
                    expression.Append(".*[a-z]");
                expression.Append(")");
            }

            if (minRequiredUppercaseCharacters > 0)
            {
                expression.Append("(?=");
                for (var i = 0; i < minRequiredUppercaseCharacters; i++)
                    expression.Append(".*[A-Z]");
                expression.Append(")");
            }

            if (minRequiredDigitCharacters > 0)
            {
                expression.Append("(?=");
                for (var i = 0; i < minRequiredDigitCharacters; i++)
                    expression.Append(".*[0-9]");
                expression.Append(")");
            }

            if (minLength > 0 && maxLength > 0)
                expression.Append(string.Format(".{{{0},{1}}}", minLength, maxLength));
            else if (minLength > 0)
                expression.Append(string.Format(".{{{0}}}", minLength));
            else if (maxLength > 0)
                expression.Append(string.Format(".{{{0}}}", maxLength));

            expression.Append("$");

            return expression.ToString();
        }

        /// <summary>
        /// Generates a random password that follows the specified rules.
        /// Also ensures that the password itself is not a dangerous cross site script.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'length' must be greater than or equal to one, and 'minRequiredNonalphaDigitCharacters' must be greater than or equal to zero (as well as less than or equal to 'length').</exception>
        /// <param name="length">Length of the password you would like to create.</param>
        /// <param name="minRequiredNonalphaDigitCharacters">Minimum number of nonalphadigit characters to be included in the password.</param>
        /// <returns>A new password.</returns>
        public static string GeneratePassword(int length, int minRequiredNonalphaDigitCharacters)
        {
            Fosol.Core.Validation.Argument.Assert.IsMinimum(length, 1, nameof(length));
            Fosol.Core.Validation.Argument.Assert.IsMinimum(minRequiredNonalphaDigitCharacters, 0, nameof(minRequiredNonalphaDigitCharacters));
            Fosol.Core.Validation.Argument.Assert.IsMaximum(minRequiredNonalphaDigitCharacters, length, String.Format(Resources.Multilingual.PasswordHelper_GeneratePassword, nameof(minRequiredNonalphaDigitCharacters), nameof(length)));

            string password;

            do
            {
                var random_bytes = new byte[length];
                var random_characters = new char[length];
                new RNGCryptoServiceProvider().GetBytes(random_bytes);

                var special_character_count = 0;
                for (var i = 0; i < length; i++)
                {
                    var value = (int)random_bytes[i] % 87;

                    if (value < 10)
                    {
                        random_characters[i] = (char)(48 + value);
                    }
                    else
                    {
                        if (value < 36)
                        {
                            random_characters[i] = (char)(65 + value - 10);
                        }
                        else
                        {
                            if (value < 62)
                            {
                                random_characters[i] = (char)(97 + value - 36);
                            }
                            else
                            {
                                random_characters[i] = _NonalphaDigitCharacters[value - 62];
                                special_character_count++;
                            }
                        }
                    }
                }

                // Ensure there are enough nonalphadigit characters in the password.
                // If there are not enough, replace some of the alphadigit characters.
                if (special_character_count < minRequiredNonalphaDigitCharacters)
                {
                    var random = new Random();
                    for (var i = 0; i < minRequiredNonalphaDigitCharacters - special_character_count; i++)
                    {
                        int pos;
                        do
                        {
                            pos = random.Next(0, length);
                        }
                        while (!char.IsLetterOrDigit(random_characters[pos]));

                        // Replace 
                        random_characters[pos] = _NonalphaDigitCharacters[random.Next(0, _NonalphaDigitCharacters.Length)];
                    }
                }

                password = new String(random_characters);
            } while (Fosol.Core.Net.CrossSiteScriptingHelper.IsDangerousString(password));

            return password;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
