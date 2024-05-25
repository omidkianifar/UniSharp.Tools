using System;
using System.Collections.Generic;
using System.Reflection;

namespace UniSharp.Tools.Runtime.Pipelines.Options
{
    public interface IPipelineOptions
    {
        IEnumerable<Type> Behaviours { get; }
        IEnumerable<Assembly> RestrictedAssemblies { get; }
        Type RequestHandlerBaseType { get; }
        bool UseAutoRegistration { get; }
    }

    public class PipelineOptions : IPipelineOptions
    {
        public IEnumerable<Type> Behaviours { get; set; } = new List<Type>();
        public IEnumerable<Assembly> RestrictedAssemblies { get; set; } = new List<Assembly>();
        public Type RequestHandlerBaseType { get; set; } = default;
        public bool UseAutoRegistration { get; set; } = true;
    }
}
