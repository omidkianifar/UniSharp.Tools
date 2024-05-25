using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Validates that a string meets or exceeds a specified minimum length.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class MinLengthAttribute : PropertyValidationAttribute
    {
        private readonly int _minLength;

        public MinLengthAttribute(int minLength)
        {
            _minLength = minLength;

            ErrorMessage ??= $"The field must be at least {_minLength} characters long.";
        }

        public override bool IsValid(object value)
        {
            // Consider null values as invalid. Use in conjunction with RequiredAttribute if needed.
            if (value is null) return false;

            return value.ToString().HasMinLength(_minLength);
        }
    }
}