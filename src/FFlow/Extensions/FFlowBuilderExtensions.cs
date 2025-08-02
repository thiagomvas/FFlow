using FFlow.Core;

namespace FFlow.Extensions;

public static class FFlowBuilderExtensions
{
    public static nFFlowBuilder WithStarter<TStep>(this nFFlowBuilder builder)
        where TStep : class, IFlowStep
    {
        if (!builder.TryResolveStep<TStep>(out var step))
            throw new InvalidOperationException($"Could not resolve step of type {typeof(TStep).Name} from the service provider.");
        return builder.WithStarter(step);
    }
    public static nFFlowBuilder StartWith<TStep>(this nFFlowBuilder builder, Action<TStep> configure)
        where TStep : class, IFlowStep
    {
        if (configure == null) throw new ArgumentNullException(nameof(configure));
        return builder.StartWith<TStep>((step, _) => configure(step));
    }
    
    public static nFFlowBuilder StartWith<TStep>(this nFFlowBuilder builder, StepConfigurator<TStep> configure)
        where TStep : class, IFlowStep
    {
        if (configure == null) throw new ArgumentNullException(nameof(configure));
        var step = builder.ResolveAndConfigure(configure);
        return builder.WithStarter(step);
    }
    
    
    public static nFFlowBuilder StartWith<TStep>(this nFFlowBuilder builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        return builder.WithStarter(step);
    }

    public static nFFlowBuilder Then<TStep>(this nFFlowBuilder builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        builder.AddStep(step);
        return builder;
    }

    public static nFFlowBuilder Then<TStep>(this nFFlowBuilder builder)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        builder.AddStep(step);
        return builder;
    }
    public static nFFlowBuilder Then<TStep>(this nFFlowBuilder builder, Action<TStep> configure)
        where TStep : class, IFlowStep
    {
        if (configure == null) throw new ArgumentNullException(nameof(configure));
        return builder.Then<TStep>((step, _) => configure(step));
    }
    
    public static nFFlowBuilder Then<TStep>(this nFFlowBuilder builder, StepConfigurator<TStep> configure) 
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure(configure);
        builder.AddStep(step);
        return builder;
    }

    public static nFFlowBuilder Then(this nFFlowBuilder builder, AsyncFlowAction step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep(step));
    }
    
    public static nFFlowBuilder Then(this nFFlowBuilder builder, Action<IFlowContext> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep((ctx, ct) => Task.Run(() => step(ctx), ct)));
    }
    
    public static nFFlowBuilder Then(this nFFlowBuilder builder, Action step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep((_, ct) => Task.Run(step, ct)));
    }
    
    public static nFFlowBuilder Finally<TStep>(this nFFlowBuilder builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        return builder.WithFinalizer(step);
    }
    
    public static nFFlowBuilder Finally<TStep>(this nFFlowBuilder builder)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        return builder.WithFinalizer(step);
    }
    
    public static nFFlowBuilder Finally<TStep>(this nFFlowBuilder builder, Action<TStep> configure)
        where TStep : class, IFlowStep
    {
        if (configure == null) throw new ArgumentNullException(nameof(configure));
        return builder.Finally<TStep>((step, _) => configure(step));
    }
    
    public static nFFlowBuilder Finally<TStep>(this nFFlowBuilder builder, StepConfigurator<TStep> configure) 
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure(configure);
        return builder.WithFinalizer(step);
    }
    
    public static nFFlowBuilder Finally(this nFFlowBuilder builder, AsyncFlowAction step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep(step));
    }
    
    public static nFFlowBuilder Finally(this nFFlowBuilder builder, Action<IFlowContext> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep((ctx, ct) => Task.Run(() => step(ctx), ct)));
    }
    
    public static nFFlowBuilder Finally(this nFFlowBuilder builder, Action step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep((_, ct) => Task.Run(step, ct)));
    }
    
    
    internal static TStep ResolveAndConfigure<TStep>(
        this nFFlowBuilder builder,
        StepConfigurator<TStep>? configure = null)
        where TStep : class, IFlowStep
    {
        if (!builder.TryResolveStep(out TStep? step))
            throw new InvalidOperationException(
                $"Could not resolve step of type {typeof(TStep).Name} from the service provider.");
        
        configure?.Invoke(step!, builder.FlowContext ?? throw new InvalidOperationException("Flow context is not set."));
        return step!;

    }
}

public delegate void StepConfigurator<TStep>(TStep step, IFlowContext context) where TStep : IFlowStep;