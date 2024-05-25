using System.Threading.Tasks;

namespace UniSharp.Tools.Validations
{
    public interface IValidator
    {
        // This interface intentionally left blank.
        // It's used only for type-safe storage of generic validators.
    }

    /// <summary>
    /// Define the contract for validators, with IValidator<T> specifying synchronous and asynchronous validation methods for instances of type T.
    /// This allows for easy implementation of custom validation logic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidator<T> : IValidator
    {
        ValidationResult Validate(T instance);
        Task<ValidationResult> ValidateAsync(T instance);
    }
}
