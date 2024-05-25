using UniSharp.Tools.Runtime.Pipelines.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UniSharp.Tools.Runtime.Pipelines.Behaviours
{
    public abstract class RequestPreProcessingBehavior<TRequest, TResponse> : PipelineBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async override Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default)
        {
            ModifyRequest(ref request);

            var response = await next(cancellationToken).ConfigureAwait(false);

            return response;
        }

        protected abstract void ModifyRequest(ref TRequest request);
    }
}
