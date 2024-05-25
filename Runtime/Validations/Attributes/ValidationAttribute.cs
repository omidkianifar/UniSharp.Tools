using System;

namespace UniSharp.Tools.Validations.Attributes
{
    /// <summary>
    /// Allows marking classes with attributes that specify their associated validators.
    /// This facilitates the automatic discovery of validators and their association with target classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ValidationAttribute : Attribute
    {
        public Type ValidatorType { get; }

        public ValidationAttribute(Type validatorType)
        {
            ValidatorType = validatorType;
        }
    }
}