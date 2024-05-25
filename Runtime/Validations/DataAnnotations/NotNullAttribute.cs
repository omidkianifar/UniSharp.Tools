using System;
using UniSharp.Tools.Validations.Attributes;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Checks for non-null values, ensuring that a field or property is provided.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class NotNullAttribute : PropertyValidationAttribute
    {
        public NotNullAttribute()
        {
            ErrorMessage = "The value cannot be null.";
        }

        public override bool IsValid(object value)
        {
            return value is not null;
        }
    }
}