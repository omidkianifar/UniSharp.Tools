using System.Threading;
using System.Threading.Tasks;
using UniSharp.Tools.Runtime.Pipelines.Requests;

namespace UniSharp.Tools.Runtime.Pipelines
{
    public interface IPipeline
    {
        void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) where TRequest : IRequest<TResponse>;

        Task<TResponse> Process<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
