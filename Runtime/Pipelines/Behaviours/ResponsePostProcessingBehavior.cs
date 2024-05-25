using UniSharp.Tools.Runtime.Pipelines.Behaviours;
using UniSharp.Tools.Runtime.Pipelines.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UniSharp.Tools.Runtime.Pipelines.Behaviours
{
    public abstract class ResponsePostProcessingBehavior<TRequest, TResponse> : PipelineBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async override Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default)
        {
            var response = await next(cancellationToken).ConfigureAwait(false);

            ModifyResponse(ref response);

            return response;
        }

        protected abstract void ModifyResponse(ref TResponse response);
    }
}
