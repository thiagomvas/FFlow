using System.Collections;
using FFlow.Core;

namespace FFlow.Extensions;

public static class IFlowContextExtensions
{
    private const string FlowContextIdKey = "__context.id";
    private const string FlowContextRootKey = "__context.root";
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
                context.SetValue(key, value);
            }
        }

        return context;
    }

    public static TListener? GetEventListener<TListener>(this IFlowContext context) where TListener : class, IFlowEventListener
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var key = Internals.BuildEventListenerKey<TListener>();
        if (context.GetValue<TListener>(key) is TListener listener)
        {
            return listener;
        }

        throw new InvalidOperationException($"Event listener of type {typeof(TListener).FullName} is not registered in the context.");
    }
    
    public static Guid GetId(this IFlowContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var id = context.GetValue<Guid>(FlowContextIdKey);
        if (id == default)
        {
            throw new InvalidOperationException("Flow context ID is not set.");
        }

        return id;
    }
    
    public static void SetId(this IFlowContext context, Guid id)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        if (id == default)
        {
            throw new ArgumentException("ID cannot be default value.", nameof(id));
        }

        context.SetValue(FlowContextIdKey, id);
    }
    
    public static void SetRoot(this IFlowContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        context.SetValue(FlowContextRootKey, context);
    }
    
    public static IFlowContext GetRoot(this IFlowContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var root = context.GetValue<IFlowContext>(FlowContextRootKey);
        if (root is null)
        {
            throw new InvalidOperationException("Root flow context is not set.");
        }

        return root;
    }
}