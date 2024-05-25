using System;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations.DataAnnotations
{
    /// <summary>
    /// Validates that a TimeSpan value is within a specified range, similar to the DateRangeAttribute but for durations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TimeSpanRangeAttribute : PropertyValidationAttribute
    {
        private readonly TimeSpan _minDuration;
        private readonly TimeSpan _maxDuration;

        public TimeSpanRangeAttribute(string minDuration, string maxDuration)
        {
            _minDuration = TimeSpan.Parse(minDuration);
            _maxDuration = TimeSpan.Parse(maxDuration);

            ErrorMessage = $"The duration must be between {_minDuration} and {_maxDuration}.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (value is TimeSpan timeSpanValue)
            {
                return timeSpanValue.IsInRange(_minDuration, _maxDuration);
            }

            return false;
        }
    }
}