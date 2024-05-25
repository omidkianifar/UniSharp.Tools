using System.Threading;
using System.Threading.Tasks;

namespace UniSharp.Tools.Runtime.Pipelines.Requests
{
    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }
}
