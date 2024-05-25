using System;
using System.Threading.Tasks;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations
{
    /// <summary>
    /// An abstract base class for validators, providing default implementations for common validation tasks, such as property validation, range checks, and regex matching.
    /// It can be extended to create custom validators for specific types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Validator<T> : IValidator<T>
    {
        public virtual ValidationResult Validate(T instance)
        {
            return ValidatorProvider.ValidateProperties(instance);
        }

        public async Task<ValidationResult> ValidateAsync(T instance)
        {
            return await Task.Run(() => Validate(instance));
        }

        public ValidationResult ValidateProperty<TProperty>(TProperty property, Func<TProperty, bool> validationRule, string errorMessage)
        {
            var isValid = validationRule(property);

            if (!isValid)
            {
                return ValidationResult.CreateInvalid(errorMessage);
            }

            return ValidationResult.Ok;
        }

        public ValidationResult ValidateAll(params ValidationResult[] results)
        {
            if (results is null)
                throw new ArgumentNullException(nameof(results));

            if (results.Length == 0)
                return ValidationResult.Ok;

            for (int i = 1; i < results.Length; i++)
                results[0].Merge(results[i]);

            return results[0];
        }

        public ValidationResult Required<TProperty>(TProperty value, string fieldName) where TProperty : class
        {
            if (value is null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                return ValidationResult.CreateInvalid($"{fieldName} is required.");
            }

            return ValidationResult.Ok;
        }

        public ValidationResult ValidateRange<TProperty>(TProperty value, TProperty min, TProperty max, string fieldName) where TProperty : IComparable<TProperty>
        {
            if (!ValidationExtensions.IsInRange(value, min, max))
            {
                return ValidationResult.CreateInvalid($"{fieldName} must be between {min} and {max}.");
            }

            return ValidationResult.Ok;
        }

        public ValidationResult ValidateRegex(string value, string pattern, string errorMessage)
        {
            if (!ValidationExtensions.IsMatch(value, pattern))
            {
                return ValidationResult.CreateInvalid(errorMessage);
            }

            return ValidationResult.Ok;
        }

        public ValidationResult ValidateWithCustomMethod<TProperty>(TProperty value, Func<TProperty, ValidationResult> validationMethod)
        {
            return validationMethod(value);
        }
    }
}
