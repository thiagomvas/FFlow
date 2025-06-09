using FFlow.Core;

namespace FFlow;

internal class Internals
{
    internal const string FFlowContextInputKey = "fflow.ctx.input";

    internal static TStep GetOrCreateStep<TStep>(IServiceProvider? serviceProvider) where TStep : class, IFlowStep
    {
        var step = serviceProvider?.GetService(typeof(TStep)) as TStep
                   ?? Activator.CreateInstance<TStep>();
        return step;
    }
    
}