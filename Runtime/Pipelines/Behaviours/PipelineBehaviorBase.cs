using System;
using System.Threading;
using System.Threading.Tasks;
using UniSharp.Tools.Runtime.Pipelines.Requests;

namespace UniSharp.Tools.Runtime.Pipelines.Behaviours
{
    public abstract class PipelineBehaviorBase<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        async Task<object> IPipelineBehavior.Handle(IRequest request, Func<CancellationToken, Task<object>> next, CancellationToken cancellationToken = default)
        {
            async Task<TResponse> adaptedNext(CancellationToken ct)
            {
                object result = await next(ct).ConfigureAwait(false);
                return (TResponse)result;
            }

            var response = await Handle((TRequest)request, adaptedNext, cancellationToken).ConfigureAwait(false);

            return response;
        }


        public abstract Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default);
    }
}
