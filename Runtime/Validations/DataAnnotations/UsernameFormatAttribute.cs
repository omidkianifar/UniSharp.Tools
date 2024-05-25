using System;
using System.Text.RegularExpressions;
using UniSharp.Tools.Validations.Attributes;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Validates usernames against specific rules, such as length constraints and the inclusion or exclusion of digits and special characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class UsernameFormatAttribute : PropertyValidationAttribute
    {
        public int MinimumLength { get; set; } = 4; // Default minimum length
        public int MaximumLength { get; set; } = 20; // Default maximum length
        public bool AllowDigits { get; set; } = true;
        public bool AllowSpecialCharacters { get; set; } = false;

        public UsernameFormatAttribute()
        {
            ErrorMessage = "Username does not meet format requirements.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var username = value.ToString();

            if (username.Length < MinimumLength || username.Length > MaximumLength)
            {
                ErrorMessage = $"Username must be between {MinimumLength} and {MaximumLength} characters long.";
                return false;
            }
            if (!AllowDigits && Regex.IsMatch(username, "[0-9]"))
            {
                ErrorMessage = "Username must not contain digits.";
                return false;
            }
            if (!AllowSpecialCharacters && Regex.IsMatch(username, "[^a-zA-Z0-9]"))
            {
                ErrorMessage = "Username must not contain special characters.";
                return false;
            }

            return true;
        }
    }
}