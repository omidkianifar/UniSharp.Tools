using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Ensures that a string value matches a standard email format by utilizing the IsEmail extension method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAttribute : PropertyValidationAttribute
    {
        public EmailAttribute()
        {
            ErrorMessage = "Invalid email format.";
        }

        public override bool IsValid(object value)
        {
            if (value is null) return false;

            return value.ToString().IsEmail();
        }
    }
}