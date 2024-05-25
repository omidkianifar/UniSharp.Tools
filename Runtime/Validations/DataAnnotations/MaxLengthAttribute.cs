using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Ensures a string does not exceed a specified maximum length.
    /// It considers null values as valid, allowing for optional fields that have a length constraint when provided.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class MaxLengthAttribute : PropertyValidationAttribute
    {
        private readonly int _maxLength;

        public MaxLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;

            ErrorMessage ??= $"The field cannot be more than {_maxLength} characters long.";
        }

        public override bool IsValid(object value)
        {
            // Consider empty values as valid. Use in conjunction with RequiredAttribute if the field is also required.
            if (value is null) return true;

            return value.ToString().HasMaxLength(_maxLength);
        }
    }
}