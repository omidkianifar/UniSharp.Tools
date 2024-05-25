namespace UniSharp.Tools.Runtime.Pipelines.Requests
{
    public interface IRequest
    {
    }

    public interface IRequest<TResponse> : IRequest
    {
    }
}
