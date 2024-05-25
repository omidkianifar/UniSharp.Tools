using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UniSharp.Tools.Runtime.Pipelines.Behaviours;
using UniSharp.Tools.Runtime.Pipelines.Options;
using UniSharp.Tools.Runtime.Pipelines.Requests;

namespace UniSharp.Tools.Runtime.Pipelines
{
    public class Pipeline : IPipeline
    {
        private readonly Dictionary<Type, Func<IRequest, CancellationToken, Task<object>>> _handlers = new();
        private Func<IRequest, CancellationToken, Task<object>> _defaultHandler;
        private readonly IPipelineOptions _options;

        public Pipeline(Action<PipelineOptionsBuilder> setup = null)
        {
            var builder = new PipelineOptionsBuilder();
            setup?.Invoke(builder);

            _options = builder.Options;

            if (_options.UseAutoRegistration)
                AutoRegisterHandlers();
        }

        public async Task<TResponse> Process<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            var pipelineBehaviors = CreatePiplineBehaviours(requestType, responseType);

            Func<CancellationToken, Task<object>> handlerCall = (ct) =>
            {
                if (_handlers.ContainsKey(requestType))
                    return _handlers[requestType](request, ct);

                return _defaultHandler != null ? _defaultHandler(request, ct) : Task.FromResult<object>(null);
            };

            foreach (var behavior in pipelineBehaviors.Reverse())
            {
                var next = handlerCall;
                handlerCall = ct => behavior.Handle(request, ct => next(ct), ct);
            }

            var result = await handlerCall(cancellationToken).ConfigureAwait(false);

            return (TResponse)result;
        }

        public void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) where TRequest : IRequest<TResponse>
        {
            _handlers[typeof(TRequest)] = (request, cancellationToken) => Task.FromResult<object>(handler.Handle((TRequest)request, cancellationToken).Result);
        }

        public void RegisterDefaultHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) where TRequest : IRequest<TResponse>
        {
            _defaultHandler = (request, cancellationToken) => Task.FromResult<object>(handler.Handle((TRequest)request, cancellationToken).Result);
        }

        protected void AutoRegisterHandlers()
        {
            var handlerTypes = GetHandlerTypes();

            var registerMethod = GetType().GetMethod(nameof(RegisterHandler), BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var type in handlerTypes)
            {
                try
                {
                    var interfaceType = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
                    var requestType = interfaceType.GetGenericArguments()[0];
                    var responseType = interfaceType.GetGenericArguments()[1];
                    var handlerInstance = Activator.CreateInstance(type);

                    var genericRegisterMethod = registerMethod.MakeGenericMethod(new Type[] { requestType, responseType });
                    genericRegisterMethod.Invoke(this, new[] { handlerInstance });
                }
                catch (InvalidOperationException ex) // Catch more specific exceptions if possible
                {
                    UnityEngine.Debug.LogError($"Error registering handler {type.Name}: {ex.Message}");
                }
                catch (TargetInvocationException ex)
                {
                    UnityEngine.Debug.LogError($"Error invoking RegisterHandler for {type.Name}: {ex.InnerException?.Message}");
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"Unexpected error during auto-registration of {type.Name}: {ex.Message}");
                }
            }
        }

        protected virtual IEnumerable<Type> GetHandlerTypes()
        {
            var typesToSearch = FilterAssemblies().SelectMany(assembly => assembly.GetTypes());

            //if (_options.RequestHandlerBaseType != null)
            //{
            //    // Adjusted to check for ITestRequestHandler<TRequest, TResponse> implementation
            //    return typesToSearch
            //        .Where(t => t.GetInterfaces()
            //            .Any(i => i.IsGenericType &&
            //                      i.GetGenericTypeDefinition() == _options.RequestHandlerBaseType &&
            //                      typeof(IRequestHandler<,>).IsAssignableFrom(i.GetGenericTypeDefinition())));
            //}

            return typesToSearch
                .Where(t => t.GetInterfaces()
                            .Any(i => i.IsGenericType &&
                                      i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));
        }


        protected IEnumerable<Assembly> FilterAssemblies()
        {
            return _options.RestrictedAssemblies.Any() ?
                _options.RestrictedAssemblies.Distinct() :
                AppDomain.CurrentDomain.GetAssemblies();
        }

        protected IEnumerable<IPipelineBehavior> CreatePiplineBehaviours(Type requestType, Type responseType)
        {
            var behaviors = new List<IPipelineBehavior>();

            foreach (var behaviorType in _options.Behaviours)
            {
                object instance;

                if (!behaviorType.IsGenericType)
                {
                    instance = Activator.CreateInstance(behaviorType);
                }
                else
                {
                    var concreteType = behaviorType.MakeGenericType(requestType, responseType);
                    instance = Activator.CreateInstance(concreteType);
                }

                if (instance is IPipelineBehavior behavior)
                    behaviors.Add(behavior);
            }

            return behaviors;
        }
    }


}
