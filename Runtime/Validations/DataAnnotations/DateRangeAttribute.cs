using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Validates that a DateTime value falls within a specified range.
    /// It parses string inputs for minimum and maximum dates and checks if the value is within this range.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DateRangeAttribute : PropertyValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;

        public DateRangeAttribute(string minDate, string maxDate)
        {
            _minDate = DateTime.Parse(minDate);
            _maxDate = DateTime.Parse(maxDate);

            ErrorMessage = $"The date must be between {_minDate.ToShortDateString()} and {_maxDate.ToShortDateString()}.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (value is DateTime dateValue)
            {
                return dateValue.IsInRange(_minDate, _maxDate);
            }

            return false;
        }
    }
}