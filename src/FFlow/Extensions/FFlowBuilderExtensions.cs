using FFlow.Core;

namespace FFlow.Extensions;

public static class FFlowBuilderExtensions
{
    public static nFFlowBuilder WithStarter<TStep>(this nFFlowBuilder builder)
        where TStep : class, IFlowStep
    {
        if (!builder.TryResolveStep<TStep>(out var step))
            throw new InvalidOperationException(
                $"Could not resolve step of type {typeof(TStep).Name} from the service provider.");
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

    public static nFFlowBuilder ContinueWith<TWorkflowDefinition>(this nFFlowBuilder builder)
        where TWorkflowDefinition : class, IWorkflowDefinition
    {
        var workflowDefinition = builder.Services?.GetService(typeof(TWorkflowDefinition)) as TWorkflowDefinition
                                 ?? Activator.CreateInstance<TWorkflowDefinition>();
        if (workflowDefinition == null)
        {
            throw new InvalidOperationException($"Could not create instance of {typeof(TWorkflowDefinition).Name}");
        }

        var step = new WorkflowContinuationStep(workflowDefinition);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder Delay(this nFFlowBuilder builder, TimeSpan delay)
    {
        if (delay <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(delay), "Delay must be greater than zero.");

        var step = new DelayStep(delay);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder Delay(this nFFlowBuilder builder, int milliseconds)
    {
        if (milliseconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(milliseconds), "Delay must be greater than zero.");

        return builder.Delay(TimeSpan.FromMilliseconds(milliseconds));
    }
    
    public static nFFlowBuilder Fork(this nFFlowBuilder builder, params Func<IWorkflow>[] workflowFactories)
    {
        if (workflowFactories == null || workflowFactories.Length == 0)
            throw new ArgumentNullException(nameof(workflowFactories), "At least one workflow must be provided.");

        var step = new ForkStep(ForkStrategy.FireAndForget, workflowFactories);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder Fork(this nFFlowBuilder builder, ForkStrategy strategy, params Func<IWorkflow>[] workflowFactories)
    {
        if (workflowFactories == null || workflowFactories.Length == 0)
            throw new ArgumentNullException(nameof(workflowFactories), "At least one workflow must be provided.");

        var step = new ForkStep(strategy, workflowFactories);
        builder.AddStep(step);
        return builder;
    }

    public static nFFlowBuilder If<TTrue>(this nFFlowBuilder builder, Func<IFlowContext, bool> condition)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));

        if (!builder.TryResolveStep(out IfStep? trueStep))
            throw new InvalidOperationException(
                $"Could not resolve step of type {typeof(IfStep).Name} from the service provider.");
        var step = new IfStep(condition, trueStep);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder If<TTrue, TFalse>(this nFFlowBuilder builder, Func<IFlowContext, bool> condition)
        where TTrue : class, IFlowStep
        where TFalse : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));

        if (!builder.TryResolveStep<TTrue>(out var trueStep))
            throw new InvalidOperationException(
                $"Could not resolve step of type {typeof(TTrue).Name} from the service provider.");
        if (!builder.TryResolveStep<TFalse>(out var falseStep))
            throw new InvalidOperationException(
                $"Could not resolve step of type {typeof(TFalse).Name} from the service provider.");

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder If<TTrue, TFalse>(this nFFlowBuilder builder, Func<IFlowContext, bool> condition,
        StepConfigurator<TTrue>? trueStepConfigurator = null,
        StepConfigurator<TFalse>? falseStepConfigurator = null)
        where TTrue : class, IFlowStep
        where TFalse : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));

        var trueStep = builder.ResolveAndConfigure(trueStepConfigurator);
        var falseStep = builder.ResolveAndConfigure(falseStepConfigurator);

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder If<TTrue>(this nFFlowBuilder builder, Func<IFlowContext, bool> condition,
        StepConfigurator<TTrue>? trueStepConfigurator = null)
        where TTrue : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));

        var trueStep = builder.ResolveAndConfigure(trueStepConfigurator);

        if (!builder.TryResolveStep(out IfStep? falseStep))
            throw new InvalidOperationException(
                $"Could not resolve step of type {typeof(IfStep).Name} from the service provider.");

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder If(this nFFlowBuilder builder, Func<IFlowContext, bool> condition,
        AsyncFlowAction trueStepAction, AsyncFlowAction? falseStepAction = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (trueStepAction == null) throw new ArgumentNullException(nameof(trueStepAction));

        var trueStep = new DelegateFlowStep(trueStepAction);
        var falseStep = falseStepAction != null ? new DelegateFlowStep(falseStepAction) : null;

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder If(this nFFlowBuilder builder, Func<IFlowContext, bool> condition,
        Action<IFlowContext> trueStepAction, Action<IFlowContext>? falseStepAction = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (trueStepAction == null) throw new ArgumentNullException(nameof(trueStepAction));

        var trueStep = new DelegateFlowStep((ctx, ct) => Task.Run(() => trueStepAction(ctx), ct));
        var falseStep = falseStepAction != null
            ? new DelegateFlowStep((ctx, ct) => Task.Run(() => falseStepAction(ctx), ct))
            : null;

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder If(this nFFlowBuilder builder, Func<IFlowContext, bool> condition,
        Action stepAction, Action<IFlowContext>? falseStepAction = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (stepAction == null) throw new ArgumentNullException(nameof(stepAction));

        var trueStep = new DelegateFlowStep((_, ct) => Task.Run(stepAction, ct));
        var falseStep = falseStepAction != null
            ? new DelegateFlowStep((ctx, ct) => Task.Run(() => falseStepAction(ctx), ct))
            : null;

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }
    
    public static nFFlowBuilder If(this nFFlowBuilder builder, Func<IFlowContext, bool> condition,
        IFlowStep trueStep, IFlowStep? falseStep = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (trueStep == null) throw new ArgumentNullException(nameof(trueStep));

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }
    
    internal static TStep ResolveAndConfigure<TStep>(
        this nFFlowBuilder builder,
        StepConfigurator<TStep>? configure = null)
        where TStep : class, IFlowStep
    {
        if (!builder.TryResolveStep(out TStep? step))
            throw new InvalidOperationException(
                $"Could not resolve step of type {typeof(TStep).Name} from the service provider.");

        configure?.Invoke(step!,
            builder.FlowContext ?? throw new InvalidOperationException("Flow context is not set."));
        return step!;
    }
}

public delegate void StepConfigurator<TStep>(TStep step, IFlowContext context) where TStep : IFlowStep;