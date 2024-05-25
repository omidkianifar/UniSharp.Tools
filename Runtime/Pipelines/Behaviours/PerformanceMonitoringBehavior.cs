using UniSharp.Tools.Runtime.Pipelines.Behaviours;
using UniSharp.Tools.Runtime.Pipelines.Requests;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace UniSharp.Tools.Runtime.Pipelines.Behaviours
{
    public class PerformanceMonitoringBehavior<TRequest, TResponse> : PipelineBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {

        protected Stopwatch _stopwatch;

        public async override Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default)
        {
            StartMonitoring(ref request);

            try
            {
                var response = await next(cancellationToken).ConfigureAwait(false);

                return response;
            }
            finally
            {
                StopMonitoring(ref request);

                LogPerformance(typeof(TRequest).Name, _stopwatch.Elapsed);
            }
        }

        protected virtual void StartMonitoring(ref TRequest request)
        {
            _stopwatch = Stopwatch.StartNew();
        }

        protected virtual void StopMonitoring(ref TRequest request)
        {
            _stopwatch.Stop();
        }

        protected virtual void LogPerformance(string requestName, TimeSpan duration)
        {
            Debug.Log($"Request [{requestName}] completed in {duration.TotalMilliseconds} ms.");
        }
    }
}
