using System.Collections;
using FFlow.Core;

namespace FFlow.Extensions;

public static class IFlowContextExtensions
{
    /// <summary>
    /// Loads environment variables into the flow context.
    /// </summary>
    /// <param name="context">The flow context in which to load environment variables into</param>
    /// <returns>The same <see cref="IFlowContext"/> instance.</returns>
    public static IFlowContext LoadEnvironmentVariables(this IFlowContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
        {
            if (env.Key is string key && env.Value is string value)
            {
                context.Set(key, value);
            }
        }

        return context;
    }

    public static TListener? GetEventListener<TListener>(this IFlowContext context) where TListener : class, IFlowEventListener
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var key = Internals.BuildEventListenerKey<TListener>();
        if (context.TryGet(key, out IFlowEventListener? listener))
        {
            return (TListener) listener;
        }

        throw new InvalidOperationException($"Event listener of type {typeof(TListener).FullName} is not registered in the context.");
    }
    
    
}