using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UniSharp.Tools.Validations.Fluents
{
    /// <summary>
    /// Enables the construction of validation rules using a fluent API, allowing for inline validation rules to be defined with clear, readable syntax.
    /// It supports adding rules for specific properties, custom error messages, and building a composite validator that encapsulates these rules.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FluentValidatorBuilder<T> : IFluentValidatorBuilder<T>
    {
        private readonly List<Func<T, ValidationResult>> _validationRules = new();

        public IFluentValidatorBuilder<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Func<TProperty, bool> validationRule, string errorMessage)
        {
            _validationRules.Add(instance =>
            {
                var compiledExpression = propertyExpression.Compile();
                var propertyValue = compiledExpression(instance);
                if (!validationRule(propertyValue))
                {
                    return ValidationResult.CreateInvalid(errorMessage);
                }
                return ValidationResult.Ok;
            });

            return this;
        }

        public IFluentValidatorBuilder<T> WithMessage(string errorMessage)
        {
            if (_validationRules.Any())
            {
                var lastRule = _validationRules.Last();
                _validationRules.Remove(lastRule);
                _validationRules.Add(instance => {
                    var result = lastRule(instance);
                    if (!result.IsValid)
                    {
                        // Replace the error message of the last rule
                        return ValidationResult.CreateInvalid(errorMessage);
                    }
                    return result;
                });
            }

            return this;
        }

        public IValidator<T> Build()
        {
            return new InlineValidator<T>(_validationRules);
        }
    }
}
