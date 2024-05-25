using UniSharp.Tools.Runtime.Pipelines.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UniSharp.Tools.Runtime.Pipelines.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : PipelineBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async override Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default)
        {
            await LogBeforeProcess(typeof(TRequest).Name, ref request).ConfigureAwait(false);

            var response = await next(cancellationToken).ConfigureAwait(false);

            await LogAfterProcess(typeof(TRequest).Name, ref request, typeof(TResponse).Name, ref response).ConfigureAwait(false);

            return response;
        }

        protected virtual Task LogBeforeProcess(string requestName, ref TRequest request)
        {
            Debug.Log($"Handling {typeof(TRequest).Name} at {DateTime.Now}");

            return Task.CompletedTask;
        }

        protected virtual Task LogAfterProcess(string requestName, ref TRequest request, string responseName, ref TResponse response)
        {
            Debug.Log($"Handled {typeof(TRequest).Name} at {DateTime.Now}");

            return Task.CompletedTask;
        }
    }
}
