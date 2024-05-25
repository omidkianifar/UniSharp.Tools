using System;
using System.Linq.Expressions;

namespace UniSharp.Tools.Validations.Fluents
{
    public interface IFluentValidatorBuilder<T>
    {
        IFluentValidatorBuilder<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Func<TProperty, bool> validationRule, string errorMessage);
        IFluentValidatorBuilder<T> WithMessage(string errorMessage);
        IValidator<T> Build();
    }
}
