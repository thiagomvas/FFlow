using FFlow.Core;
using FFlow.Exceptions;

namespace FFlow;

internal class Internals
{
    internal const string FFlowContextInputKey = "fflow.ctx.input";

    internal static string BuildInputKey(IFlowStep step, string key)
    {
        ArgumentNullException.ThrowIfNull(step);
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        var stepName = step.GetType().Name;

        return $"input:{stepName}.{key}";
    }
    
    internal static string BuildOutputKey(IFlowStep step, string key)
    {
        ArgumentNullException.ThrowIfNull(step);
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        var stepName = step.GetType().Name;

        return $"output:{stepName}.{key}";
    }

    internal static string BuildEventListenerKey<TEventListener>()
        where TEventListener : class, IFlowEventListener
    {
        return $"__event_listener:{typeof(TEventListener).FullName}";
    }

    internal static string BuildEventListenerKey(IFlowEventListener listener)
    {
        return $"__event_listener:{listener.GetType().FullName}";
    }
    
    internal static TStep GetOrCreateStep<TStep>(IServiceProvider? serviceProvider) where TStep : class, IFlowStep
    {
        if (serviceProvider != null)
        {
            var step = (TStep) serviceProvider.GetService(typeof(TStep));
            if (step != null)
                return step;
        }

        // Fallback only if TStep has a public parameterless constructor
        var constructor = typeof(TStep).GetConstructor(Type.EmptyTypes);
        if (constructor != null)
        {
            return Activator.CreateInstance<TStep>();
        }

        throw new StepCreationException(typeof(TStep));
    }

    
}