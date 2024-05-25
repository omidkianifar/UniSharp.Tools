using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Confirms that a string is a well-formed URL by using standard URI validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class UrlAttribute : PropertyValidationAttribute
    {
        public UrlAttribute()
        {
            ErrorMessage ??= "The field must be a valid URL.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return value.ToString().IsUrl();
        }
    }
}