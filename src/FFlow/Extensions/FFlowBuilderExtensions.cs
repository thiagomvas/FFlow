using FFlow.Core;

namespace FFlow.Extensions;

public static class FFlowBuilderExtensions
{
    public static nFFlowBuilder StartWith<TStep>(this nFFlowBuilder builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        return builder.WithStarter(step);
    }

    public static nFFlowBuilder Then<TStep>(this nFFlowBuilder builder) where TStep : IFlowStep
    {
        if (builder.TryAddStepResolved(out TStep? step))
        {
            return builder;
        }
        throw new InvalidOperationException($"Could not resolve step of type {typeof(TStep).Name} from the service provider.");
    }
}