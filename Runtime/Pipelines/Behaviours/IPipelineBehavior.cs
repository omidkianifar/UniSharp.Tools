using System;
using System.Threading;
using System.Threading.Tasks;
using UniSharp.Tools.Runtime.Pipelines.Requests;

namespace UniSharp.Tools.Runtime.Pipelines.Behaviours
{
    public interface IPipelineBehavior
    {
        Task<object> Handle(IRequest request, Func<CancellationToken, Task<object>> next, CancellationToken cancellationToken);
    }

    public interface IPipelineBehavior<TRequest, TResponse> : IPipelineBehavior where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken);
    }
}
