using System;
using System.Collections.Generic;

namespace UniSharp.Tools.Validations.Fluents
{
    /// <summary>
    /// Implements Validator<T>, allowing the use of inline-defined validation rules.
    /// This class makes it possible to apply custom validation logic defined via the fluent API, in addition to or instead of attribute-based validation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InlineValidator<T> : Validator<T>
    {
        private readonly IEnumerable<Func<T, ValidationResult>> _validationRules;

        public InlineValidator(IEnumerable<Func<T, ValidationResult>> validationRules)
        {
            _validationRules = validationRules;
        }

        public override ValidationResult Validate(T instance)
        {
            foreach (var rule in _validationRules)
            {
                var result = rule(instance);
                if (!result.IsValid)
                {
                    return result;
                }
            }

            // Fall back to property validation if defined
            return base.Validate(instance);
        }
    }
}
