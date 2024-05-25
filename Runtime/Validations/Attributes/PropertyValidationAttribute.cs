using System;

namespace UniSharp.Tools.Validations.Attributes
{
    /// <summary>
    /// Serves as the base class for custom validation attributes that can be applied to properties, enabling attribute-based validation rules that are automatically enforced by the framework.
    /// </summary>
    public abstract class PropertyValidationAttribute : Attribute
    {
        public abstract bool IsValid(object value);
        public string ErrorMessage { get; set; }
    }
}