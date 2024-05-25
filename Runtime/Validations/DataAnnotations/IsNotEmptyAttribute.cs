using System;
using System.Collections;
using UniSharp.Tools.Validations.Attributes;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Checks that a value is not null or empty.
    /// It supports strings, collections, and arrays, ensuring they contain elements or characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class IsNotEmptyAttribute : PropertyValidationAttribute
    {
        public IsNotEmptyAttribute()
        {
            ErrorMessage = "The value must not be empty.";
        }

        public override bool IsValid(object value)
        {
            // Null values are considered empty.
            if (value == null) return false;

            if (value is string stringValue)
            {
                return !string.IsNullOrEmpty(stringValue);
            }

            if (value is ICollection collectionValue)
            {
                return collectionValue.Count > 0;
            }

            if (value is Array arrayValue)
            {
                return arrayValue.Length > 0;
            }

            // You could consider non-null value types (e.g., int, DateTime) as inherently not empty
            if (value.GetType().IsValueType)
            {
                return true;
            }

            // Default case for types not explicitly checked
            return false;
        }
    }
}