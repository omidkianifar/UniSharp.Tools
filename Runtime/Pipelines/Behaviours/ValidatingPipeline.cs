using System;
using System.Threading;
using System.Threading.Tasks;
using UniSharp.Tools.Runtime.Pipelines.Requests;
using UniSharp.Tools.Validations;

namespace UniSharp.Tools.Runtime.Pipelines.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : PipelineBehaviorBase<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        public async override Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default)
        {
            var validationResult = Validate(request);

            if (!validationResult.IsValid)
            {
                return CreateErrorResponse(validationResult);
            }

            return await next(cancellationToken).ConfigureAwait(false);
        }

        protected virtual ValidationResult Validate(TRequest request)
        {
            return ValidatorProvider.Validate(request);
        }

        protected virtual TResponse CreateErrorResponse(ValidationResult validationResult)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }

}
