using FFlow.Core;

namespace FFlow.Extensions;

public static class FFlowBuilderStepConfigurationExtensions
{
    
    public static nFFlowBuilder Input<TStep>(this nFFlowBuilder builder, Action<TStep, IFlowContext> configure)
        where TStep : FlowStep
    {
        var lastStep = builder.Steps[^1];
        if (lastStep is null) throw new InvalidOperationException("No steps have been added to the flow.");
        if (lastStep is not TStep step)
            throw new InvalidOperationException($"Last step is not of type {typeof(TStep).Name}.");
        
        step.OnBeforeRun += (flowStep, ctx) => configure((TStep) flowStep, ctx);
        return builder;
    }
    
    public static nFFlowBuilder Output<TStep>(this nFFlowBuilder builder, Action<TStep, IFlowContext> configure)
        where TStep : FlowStep
    {
        var lastStep = builder.Steps[^1];
        if (lastStep is null) throw new InvalidOperationException("No steps have been added to the flow.");
        if (lastStep is not TStep step)
            throw new InvalidOperationException($"Last step is not of type {typeof(TStep).Name}.");
        
        step.OnAfterRun += (flowStep, ctx) => configure((TStep) flowStep, ctx);
        return builder;
    }
}