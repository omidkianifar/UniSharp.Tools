using System;
using System.Collections.Generic;

namespace UniSharp.Tools.Validations
{
    /// <summary>
    /// Encapsulates the outcome of validation operations, indicating whether the validation succeeded and storing any error messages.
    /// It supports merging results from multiple validation steps, enabling complex validation scenarios.
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public IEnumerable<string> Errors => _errors.AsReadOnly();
        private List<string> _errors { get; set; } = new();

        public static readonly ValidationResult Ok = new() { IsValid = true };
        public static readonly ValidationResult Failed = new() { IsValid = false, _errors = new List<string> { "Invalid" } };
        public static implicit operator ValidationResult(bool isValid) => isValid ? Ok : Failed;

        protected ValidationResult() { }

        public static ValidationResult CreateInvalid(params string[] errors)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));
            if (errors.Length == 0) throw new ArgumentException("Error list cannot be empty.", nameof(errors));

            return new ValidationResult { IsValid = false, _errors = new List<string>(errors) };
        }

        public static ValidationResult CreateFromResults(params ValidationResult[] results)
        {
            if (results is null)
                throw new ArgumentNullException(nameof(results));

            if (results.Length == 0)
                return ValidationResult.Ok;

            for (int i = 1; i < results.Length; i++)
                results[0].Merge(results[i]);

            return results[0];
        }

        public void Merge(ValidationResult other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            IsValid &= other.IsValid;
            _errors.AddRange(other.Errors);
        }
    }
}
