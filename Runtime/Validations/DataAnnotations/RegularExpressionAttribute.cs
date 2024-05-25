using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Checks if a string matches a given regular expression pattern, allowing for versatile pattern-based validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class RegularExpressionAttribute : PropertyValidationAttribute
    {
        private readonly string _pattern;

        public RegularExpressionAttribute(string pattern)
        {
            _pattern = pattern;

            ErrorMessage ??= "The field does not match the required pattern.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return value.ToString().IsMatch(_pattern);
        }
    }
}