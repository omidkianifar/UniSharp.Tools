using System;
using System.Text.RegularExpressions;
using UniSharp.Tools.Validations.Attributes;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Enforces complex password rules, including minimum length, and the presence of uppercase letters, lowercase letters, digits, and special characters.
    /// Each rule contributes to the password's overall complexity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class PasswordFormatAttribute : PropertyValidationAttribute
    {
        public int MinimumLength { get; set; } = 8; // Default minimum length
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireDigit { get; set; } = true;
        public bool RequireSpecialCharacter { get; set; } = true;

        public PasswordFormatAttribute()
        {
            ErrorMessage = "Password does not meet complexity requirements.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var password = value.ToString();

            if (password.Length < MinimumLength)
            {
                ErrorMessage = $"Password must be at least {MinimumLength} characters long.";
                return false;
            }
            if (RequireUppercase && !Regex.IsMatch(password, "[A-Z]"))
            {
                ErrorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }
            if (RequireLowercase && !Regex.IsMatch(password, "[a-z]"))
            {
                ErrorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }
            if (RequireDigit && !Regex.IsMatch(password, "[0-9]"))
            {
                ErrorMessage = "Password must contain at least one digit.";
                return false;
            }
            if (RequireSpecialCharacter && !Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]"))
            {
                ErrorMessage = "Password must contain at least one special character.";
                return false;
            }

            return true;
        }
    }
}