using System;
using UniSharp.Tools.Validations.Attributes;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Ensures a field or property is provided and is not empty or whitespace, specifically handling strings with additional scrutiny.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredAttribute : PropertyValidationAttribute
    {
        public RequiredAttribute()
        {
            ErrorMessage = "The field is required.";
        }

        public override bool IsValid(object value)
        {
            if (value is null)
                return false;

            if (value is string strValue)
                return !string.IsNullOrWhiteSpace(strValue);

            return true;
        }
    }
}