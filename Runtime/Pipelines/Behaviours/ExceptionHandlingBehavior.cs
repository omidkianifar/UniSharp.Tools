using System;
using System.Threading;
using System.Threading.Tasks;
using UniSharp.Tools.Runtime.Pipelines.Requests;

namespace UniSharp.Tools.Runtime.Pipelines.Behaviours
{
    public class ExceptionHandlingBehavior<TRequest, TResponse> : PipelineBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async override Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default)
        {
            try
            {
                return await next(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        protected virtual TResponse HandleException(Exception exception)
        {
            //Debug.LogError($"Exception caught in {typeof(TRequest).Name}: {exception.Message}");

            if (ShouldContinueWithDefaultResponse(exception))
            {
                return GetDefaultResponse(exception);
            }
            else
            {
                throw exception;
            }
        }

        // Implement logic to decide based on exception type or content
        // For example, return true for known recoverable errors
        // Return True by default
        protected virtual bool ShouldContinueWithDefaultResponse(Exception exception)
        {
            return true;
        }

        protected virtual TResponse GetDefaultResponse(Exception exception)
        {
            return Activator.CreateInstance<TResponse>();
        }
    }
}
