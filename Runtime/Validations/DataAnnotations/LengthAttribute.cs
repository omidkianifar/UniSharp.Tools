using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Verifies that a string has an exact length specified by the attribute parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class LengthAttribute : PropertyValidationAttribute
    {
        private readonly int _exactLength;

        public LengthAttribute(int exactLength)
        {
            _exactLength = exactLength;

            ErrorMessage ??= $"The field must be {_exactLength} characters long.";
        }

        public override bool IsValid(object value)
        {
            if (value is null) return false;

            return value.ToString().HasLength(_exactLength);
        }
    }
}