using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Validates that a numeric value falls within a specified range.
    /// It converts the value to a double for comparison.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RangeAttribute : PropertyValidationAttribute
    {
        private readonly double _minimum;
        private readonly double _maximum;

        public RangeAttribute(double minimum, double maximum)
        {
            _minimum = minimum;
            _maximum = maximum;

            ErrorMessage ??= $"The field must be between {_minimum} and {_maximum}.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (double.TryParse(value.ToString(), out double valueAsDouble))
            {
                return valueAsDouble.IsInRange(_minimum, _maximum);
            }

            return false;
        }
    }
}