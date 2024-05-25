using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UniSharp.Tools.Validations.Extensions
{
    public static class ValidationExtensions
    {
        private const string EmailPattern = @"^(?=.{1,256}$)[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9][a-zA-Z0-9.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$";

        public static bool IsInRange(this DateTime value, DateTime min, DateTime max)
        {
            return value.IsInRange<DateTime>(min, max);
        }

        public static bool IsInRange(this TimeSpan value, TimeSpan min, TimeSpan max)
        {
            return value.IsInRange<TimeSpan>(min, max);
        }

        public static bool IsInRange(this double value, double min, double max)
        {
            return value.IsInRange<double>(min, max);
        }

        public static bool IsInRange<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }

        public static bool IsMatch(this string value, string pattern)
        {
            if (value is null) return false;

            if (pattern is null) return false;

            return Regex.IsMatch(value, pattern);
        }

        public static bool IsEmail(this string value)
        {
            if (value is null) return false;

            return value.IsMatch(EmailPattern);
        }

        public static bool IsUrl(this string value)
        {
            if (value is null) return false;

            return Uri.IsWellFormedUriString(value, UriKind.Absolute);
        }

        public static bool HasLength(this string value, int length)
        {
            if (value is null) return false;

            return value.Length == length;
        }

        public static bool HasMaxLength(this string value, int maxLength)
        {
            if (value is null) return false;

            return value.Length <= maxLength;
        }

        public static bool HasMinLength(this string value, int minLength)
        {
            if (value is null) return false;

            return value.Length >= minLength;
        }

        public static bool IsValidUserName(this string value, int minLength = 4, int maxLength = 20, bool allowDigits = true, bool allowSpecialCharacters = false)
        {
            if (value == null) return false;

            if (value.Length < minLength || value.Length > maxLength)
            {
                return false;
            }
            if (!allowDigits && Regex.IsMatch(value, "[0-9]"))
            {
                return false;
            }
            if (!allowSpecialCharacters && Regex.IsMatch(value, "[^a-zA-Z0-9]"))
            {
                return false;
            }

            return true;
        }

        public static bool IsValidPassword(this string value, int minLength = 4, int maxLength = 20, bool requireUppercase = true, bool requireLowercase = true, bool requireDigit = true, bool requireSpecialCharacter = true)
        {
            if (value == null) return false;

            if (value.Length < minLength)
            {
                return false;
            }
            if (value.Length > maxLength)
            {
                return false;
            }
            if (requireUppercase && !Regex.IsMatch(value, "[A-Z]"))
            {
                return false;
            }
            if (requireLowercase && !Regex.IsMatch(value, "[a-z]"))
            {
                return false;
            }
            if (requireDigit && !Regex.IsMatch(value, "[0-9]"))
            {
                return false;
            }
            if (requireSpecialCharacter && !Regex.IsMatch(value, "[!@#$%^&*(),.?\":{}|<>]"))
            {
                return false;
            }

            return true;
        }

        internal static ValidationResult Validate(this IEnumerable<ValidationResult> validationResults)
        {
            return ValidationResult.CreateFromResults(validationResults.ToArray());
        }

        internal static ValidationResult Validate<T>(this IEnumerable<IValidator<T>> validators, T instance)
        {
            return validators.Select(v => v.Validate(instance)).Validate();
        }
    }
}