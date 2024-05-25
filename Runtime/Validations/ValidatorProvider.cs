using UniSharp.Tools.Validations.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UniSharp.Tools.Validations.DataAnnotations;
using UniSharp.Tools.Validations.Fluents;
using UniSharp.Tools.Validations.Extensions;

namespace UniSharp.Tools.Validations
{
    /// <summary>
    /// Serves as the central registry for validators, dynamically discovering and storing validators based on the IValidator<> interface.
    /// It offers methods to retrieve validators for specific types, supporting dependency injection and dynamic validator resolution.
    /// </summary>
    public class ValidatorProvider
    {
        private readonly Dictionary<Type, List<Type>> validatorTypes = new();

        public ValidatorProvider()
        {
            var validatorInterfaceType = typeof(IValidator<>);
            var assemblyValidators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorInterfaceType))
                .ToList();

            foreach (var validator in assemblyValidators)
            {
                var requestTypes = validator.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorInterfaceType)
                    .Select(i => i.GetGenericArguments().First());

                foreach (var requestType in requestTypes)
                {
                    if (!validatorTypes.ContainsKey(requestType))
                    {
                        validatorTypes[requestType] = new List<Type>();
                    }

                    validatorTypes[requestType].Add(validator);
                }
            }
        }

        public IEnumerable<IValidator<T>> GetValidators<T>()
        {
            if (!validatorTypes.TryGetValue(typeof(T), out var types))
                return Enumerable.Empty<IValidator<T>>();

            return types.Select(x => (IValidator<T>)Activator.CreateInstance(x));
        }

        public static ValidationResult Validate<T>(T instance, params Type[] validatorTypes)
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            if (validatorTypes is null || validatorTypes.Length == 0)
                throw new ArgumentNullException(nameof(validatorTypes));

            var validators = CreateValidators<T>(validatorTypes);

            return validators.Validate(instance);
        }

        public static ValidationResult Validate<T>(T instance, bool usePropertyValidator = true)
        {
            var validationResult = GetValidatorAttributes<T>().Validate(instance);

            if (usePropertyValidator)
                validationResult.Merge(ValidateProperties(instance));

            return validationResult;
        }

        public static ValidationResult ValidateProperties<T>(T instance)
        {
            var errorMessages = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .SelectMany(property
                    => property.GetCustomAttributes(typeof(PropertyValidationAttribute), true)
                               .Cast<PropertyValidationAttribute>(), (property, validator) => new { property, validator })
                .Where(x => !x.validator.IsValid(x.property.GetValue(instance)))
                .Select(x => x.validator.ErrorMessage ?? $"Validation failed for {x.property.Name}")
                .ToList();

            return errorMessages.Any() ?
                ValidationResult.CreateInvalid(errorMessages.ToArray()) :
                ValidationResult.Ok;
        }

        public static IFluentValidatorBuilder<T> For<T>()
        {
            return new FluentValidatorBuilder<T>();
        }

        private static IEnumerable<IValidator<T>> GetValidatorAttributes<T>()
        {
            var targetType = typeof(T);
            var validatorAttributes = Attribute.GetCustomAttributes(targetType, typeof(ValidationAttribute))
                                               .Cast<ValidationAttribute>().Select(x => x.ValidatorType);

            var validators = CreateValidators<T>(validatorAttributes);

            return validators;
        }

        private static IEnumerable<IValidator<T>> CreateValidators<T>(IEnumerable<Type> validatorTypes)
        {
            var targetType = typeof(T);
            var validators = new List<IValidator<T>>();
            foreach (var validatorType in validatorTypes)
            {
                if (typeof(IValidator<T>).IsAssignableFrom(validatorType))
                {
                    if (Activator.CreateInstance(validatorType) is IValidator<T> validator)
                    {
                        validators.Add(validator);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"The specified validator for {targetType.Name} does not implement the IValidator<T> interface properly.");
                }
            }

            return validators;
        }

        public static async Task<ValidationResult> ValidateAsync<T>(T instance)
        {
            var validators = GetValidatorAttributes<T>();
            var validationTasks = validators.Select(validator => validator.ValidateAsync(instance));
            var validationResults = await Task.WhenAll(validationTasks);

            var validationResult = ValidationResult.CreateFromResults(validationResults);

            var propertyValidationResult = await Task.FromResult(ValidateProperties(instance));
            validationResult.Merge(propertyValidationResult);

            return validationResult;
        }
    }
}
