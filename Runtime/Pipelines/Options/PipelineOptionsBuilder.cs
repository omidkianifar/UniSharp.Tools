using System;
using System.Linq;
using System.Reflection;
using UniSharp.Tools.Runtime.Pipelines.Behaviours;

namespace UniSharp.Tools.Runtime.Pipelines.Options
{
    public class PipelineOptionsBuilder
    {
        internal IPipelineOptions Options => _options;
        private readonly PipelineOptions _options = new();

        public void AddBehavior(Type behaviorType)
        {
            if (!typeof(IPipelineBehavior).IsAssignableFrom(behaviorType))
            {
                throw new ArgumentException("Behavior type must implement IPipelineBehavior", nameof(behaviorType));
            }

            if (!behaviorType.IsGenericTypeDefinition)
            {
                // throw new ArgumentException("Behavior type must be an open generic type", nameof(behaviorType));
            }

            if (_options.Behaviours.Contains(behaviorType))
            {
                throw new ArgumentException("Behavior type added before", nameof(behaviorType));
            }

            _options.Behaviours = _options.Behaviours.Append(behaviorType);
        }

        public void AddBehaviors(params Type[] behaviorTypes)
        {
            foreach (var behaviorType in behaviorTypes)
                AddBehavior(behaviorType);
        }

        //public void RestrictRequestHandler(Type type)
        //{
        //    if (!type.IsGenericTypeDefinition ||
        //        !typeof(IRequestHandler<,>).IsAssignableFrom(type.GetGenericTypeDefinition()))
        //    {
        //        throw new ArgumentException("Restricted type must be a generic type definition that extends IRequestHandler<TRequest, TResponse>.", nameof(type));
        //    }

        //    _options.RequestHandlerBaseType = type;
        //}

        public void RestrictAssemblies(params Assembly[] assemblies)
        {
            _options.RestrictedAssemblies = _options.RestrictedAssemblies.Concat(assemblies);
        }

        public void DisableAutoRegisterHandlers()
        {
            _options.UseAutoRegistration = false;
        }

        public void AddDefaultBehaviours()
        {
            AddBehavior(typeof(LoggingBehavior<,>));
            AddBehavior(typeof(ExceptionHandlingBehavior<,>));
            AddBehavior(typeof(PerformanceMonitoringBehavior<,>));
            //AddBehavior(typeof(RequestPreProcessingBehavior<,>));
            AddBehavior(typeof(ValidationBehavior<,>));
            //AddBehavior(typeof(ResponsePostProcessingBehavior<,>));
        }
    }
}
