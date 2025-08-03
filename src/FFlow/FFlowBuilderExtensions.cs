using System.Net.NetworkInformation;
using FFlow.Core;
using FFlow.Exceptions;

namespace FFlow;

public static class FFlowBuilderExtensions
{
    public static WorkflowBuilderBase StartWith<TStep>(this WorkflowBuilderBase builder)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        builder.InsertStepAt(0, step);
        return builder;
    }

    public static WorkflowBuilderBase StartWith<TStep>(this WorkflowBuilderBase builder, Action<TStep> configure)
        where TStep : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(configure);
        return builder.StartWith<TStep>((step, _) => configure(step));
    }

    public static WorkflowBuilderBase StartWith<TStep>(this WorkflowBuilderBase builder,
        StepConfigurator<TStep> configure)
        where TStep : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(configure);
        var step = builder.ResolveAndConfigure(configure);
        builder.InsertStepAt(0, step);
        return builder;
    }


    public static WorkflowBuilderBase StartWith<TStep>(this WorkflowBuilderBase builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        builder.InsertStepAt(0, step);
        return builder;
    }

    public static WorkflowBuilderBase StartWith(this WorkflowBuilderBase builder, AsyncFlowAction step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.StartWith(new DelegateFlowStep(step));
    }

    public static WorkflowBuilderBase StartWith(this WorkflowBuilderBase builder, Action<IFlowContext> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.StartWith(new DelegateFlowStep((ctx, ct) => Task.Run(() => step(ctx), ct)));
    }

    public static WorkflowBuilderBase StartWith(this WorkflowBuilderBase builder, Action step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.StartWith(new DelegateFlowStep((_, ct) => Task.Run(step, ct)));
    }

    public static WorkflowBuilderBase Then<TStep>(this WorkflowBuilderBase builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Then<TStep>(this WorkflowBuilderBase builder)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Then<TStep>(this WorkflowBuilderBase builder, Action<TStep> configure)
        where TStep : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(configure);
        return builder.Then<TStep>((step, _) => configure(step));
    }

    public static WorkflowBuilderBase Then<TStep>(this WorkflowBuilderBase builder, StepConfigurator<TStep> configure)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure(configure);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Then(this WorkflowBuilderBase builder, AsyncFlowAction step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep(step));
    }

    public static WorkflowBuilderBase Then(this WorkflowBuilderBase builder, Action<IFlowContext> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep((ctx, ct) => Task.Run(() => step(ctx), ct)));
    }

    public static WorkflowBuilderBase Then(this WorkflowBuilderBase builder, Action step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep((_, ct) => Task.Run(step, ct)));
    }

    public static WorkflowBuilderBase Finally<TStep>(this WorkflowBuilderBase builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        if (builder is FFlowBuilder flowBuilder)
        {
            flowBuilder.WithStarter(step);
            return flowBuilder;
        }
        else
        {
            builder.AddStep(step);
            return builder;
        }
    }

    public static WorkflowBuilderBase Finally<TStep>(this WorkflowBuilderBase builder)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        if (builder is FFlowBuilder flowBuilder)
        {
            flowBuilder.WithStarter(step);
            return flowBuilder;
        }
        else
        {
            builder.AddStep(step);
            return builder;
        }
    }

    public static WorkflowBuilderBase Finally<TStep>(this WorkflowBuilderBase builder, Action<TStep> configure)
        where TStep : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(configure);
        return builder.Finally<TStep>((step, _) => configure(step));
    }

    public static WorkflowBuilderBase Finally<TStep>(this WorkflowBuilderBase builder,
        StepConfigurator<TStep> configure)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure(configure);
        if (builder is FFlowBuilder flowBuilder)
        {
            flowBuilder.WithStarter(step);
            return flowBuilder;
        }
        else
        {
            builder.AddStep(step);
            return builder;
        }
    }

    public static WorkflowBuilderBase Finally(this WorkflowBuilderBase builder, AsyncFlowAction step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep(step));
    }

    public static WorkflowBuilderBase Finally(this WorkflowBuilderBase builder, Action<IFlowContext> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep((ctx, ct) => Task.Run(() => step(ctx), ct)));
    }

    public static WorkflowBuilderBase Finally(this WorkflowBuilderBase builder, Action step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep((_, ct) => Task.Run(step, ct)));
    }

    public static WorkflowBuilderBase ContinueWith<TWorkflowDefinition>(this WorkflowBuilderBase builder)
        where TWorkflowDefinition : class, IWorkflowDefinition
    {
        var workflowDefinition =
            ((FFlowBuilder)builder)?.Services?.GetService(typeof(TWorkflowDefinition)) as TWorkflowDefinition
            ?? Activator.CreateInstance<TWorkflowDefinition>();
        if (workflowDefinition == null)
        {
            throw new InvalidOperationException($"Could not create instance of {typeof(TWorkflowDefinition).Name}");
        }

        var step = new WorkflowContinuationStep(workflowDefinition);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Delay(this WorkflowBuilderBase builder, TimeSpan delay)
    {
        if (delay <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(delay), "Delay must be greater than zero.");

        var step = new DelayStep(delay);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Delay(this WorkflowBuilderBase builder, int milliseconds)
    {
        if (milliseconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(milliseconds), "Delay must be greater than zero.");

        return builder.Delay(TimeSpan.FromMilliseconds(milliseconds));
    }

    public static WorkflowBuilderBase Fork(this WorkflowBuilderBase builder, params Func<IWorkflow>[] workflowFactories)
    {
        if (workflowFactories == null || workflowFactories.Length == 0)
            throw new ArgumentNullException(nameof(workflowFactories), "At least one workflow must be provided.");

        var step = new ForkStep(ForkStrategy.FireAndForget, workflowFactories);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Fork(this WorkflowBuilderBase builder, ForkStrategy strategy,
        params Func<IWorkflow>[] workflowFactories)
    {
        if (workflowFactories == null || workflowFactories.Length == 0)
            throw new ArgumentNullException(nameof(workflowFactories), "At least one workflow must be provided.");

        var step = new ForkStep(strategy, workflowFactories);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Fork(this WorkflowBuilderBase builder,
        params Func<WorkflowBuilderBase>[] workflowBuilders)
    {
        if (workflowBuilders == null || workflowBuilders.Length == 0)
            throw new ArgumentNullException(nameof(workflowBuilders),
                "At least one workflow builder must be provided.");

        var step = new ForkStep(ForkStrategy.FireAndForget, workflowBuilders);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Fork(this WorkflowBuilderBase builder, ForkStrategy strategy,
        params Func<WorkflowBuilderBase>[] workflowBuilders)
    {
        if (workflowBuilders == null || workflowBuilders.Length == 0)
            throw new ArgumentNullException(nameof(workflowBuilders),
                "At least one workflow builder must be provided.");

        var step = new ForkStep(strategy, workflowBuilders);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase If<TTrue>(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition)
        where TTrue : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(condition);

        var trueStep = builder.ResolveAndConfigure<TTrue>();
        var step = new IfStep(condition, trueStep);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase If<TTrue, TFalse>(this WorkflowBuilderBase builder,
        Func<IFlowContext, bool> condition)
        where TTrue : class, IFlowStep
        where TFalse : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(condition);

        var trueStep = builder.ResolveAndConfigure<TTrue>();
        var falseStep = builder.ResolveAndConfigure<TFalse>();
        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase If<TTrue, TFalse>(this WorkflowBuilderBase builder,
        Func<IFlowContext, bool> condition,
        StepConfigurator<TTrue>? trueStepConfigurator = null,
        StepConfigurator<TFalse>? falseStepConfigurator = null)
        where TTrue : class, IFlowStep
        where TFalse : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(condition);

        var trueStep = builder.ResolveAndConfigure(trueStepConfigurator);
        var falseStep = builder.ResolveAndConfigure(falseStepConfigurator);

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase If(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition,
        AsyncFlowAction trueStepAction, AsyncFlowAction? falseStepAction = null)
    {
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(trueStepAction);

        var trueStep = new DelegateFlowStep(trueStepAction);
        var falseStep = falseStepAction != null ? new DelegateFlowStep(falseStepAction) : null;

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase If(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition,
        Action<IFlowContext> trueStepAction, Action<IFlowContext>? falseStepAction = null)
    {
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(trueStepAction);

        var trueStep = new DelegateFlowStep((ctx, ct) => Task.Run(() => trueStepAction(ctx), ct));
        var falseStep = falseStepAction != null
            ? new DelegateFlowStep((ctx, ct) => Task.Run(() => falseStepAction(ctx), ct))
            : null;

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase If(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition,
        Action stepAction, Action<IFlowContext>? falseStepAction = null)
    {
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(stepAction);

        var trueStep = new DelegateFlowStep((_, ct) => Task.Run(stepAction, ct));
        var falseStep = falseStepAction != null
            ? new DelegateFlowStep((ctx, ct) => Task.Run(() => falseStepAction(ctx), ct))
            : null;

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase If(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition,
        IFlowStep trueStep, IFlowStep? falseStep = null)
    {
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(trueStep);

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase If(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition,
        Func<WorkflowBuilderBase> trueFactory, Func<WorkflowBuilderBase>? falseFactory = null)
    {
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(trueFactory);

        var trueStep = new BuilderStep(trueFactory.Invoke(), ((FFlowBuilder)builder)?.Services ?? null);
        IFlowStep? falseStep = null;

        if (falseFactory != null)
        {
            falseStep = new BuilderStep(falseFactory.Invoke(), ((FFlowBuilder)builder)?.Services ?? null);
        }

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;

    }

    public static WorkflowBuilderBase Stop(this WorkflowBuilderBase builder)
    {
        var step = new StopExecutionStep();
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase StopIf(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        var step = new StopExecutionIfStep(condition);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Switch(this WorkflowBuilderBase builder, Action<SwitchCaseBuilder> caseBuilder)
    {
        ArgumentNullException.ThrowIfNull(caseBuilder);

        var switchCaseBuilder = new SwitchCaseBuilder { _serviceProvider = ((FFlowBuilder)builder)?.Services ?? null };
        caseBuilder(switchCaseBuilder);

        var step = switchCaseBuilder.Build();

        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Throw(this WorkflowBuilderBase builder, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));

        var step = new ThrowExceptionStep(message);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase Throw<TException>(this WorkflowBuilderBase builder, string message)
        where TException : Exception
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));

        var exception = (TException)Activator.CreateInstance(typeof(TException), message)!
                        ?? throw new InvalidOperationException(
                            $"Could not create instance of {typeof(TException)} with message.");

        var step = new ThrowExceptionStep(exception);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase ThrowIf(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition,
        string message)
    {
        ArgumentNullException.ThrowIfNull(condition);
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));

        var step = new ThrowExceptionIfStep(condition, message);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase ThrowIf<TException>(this WorkflowBuilderBase builder,
        Func<IFlowContext, bool> condition, string message)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(condition);
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));

        var exception = (TException)Activator.CreateInstance(typeof(TException), message)!
                        ?? throw new InvalidOperationException(
                            $"Could not create instance of {typeof(TException)} with message.");

        var step = new ThrowExceptionIfStep(condition, exception);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase LogToConsole(this WorkflowBuilderBase builder, string message)
    {
        var step = new LogToConsoleStep(message);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase OnAnyError<TStep>(this WorkflowBuilderBase builder) where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        builder.SetErrorHandlingStep(step);
        return builder;
    }

    public static WorkflowBuilderBase OnAnyError(this WorkflowBuilderBase builder, AsyncFlowAction errorAction)
    {
        ArgumentNullException.ThrowIfNull(errorAction);
        var step = new DelegateFlowStep(errorAction);
        return builder.OnAnyError(step);
    }

    public static WorkflowBuilderBase OnAnyError(this WorkflowBuilderBase builder, Action<IFlowContext> errorAction)
    {
        ArgumentNullException.ThrowIfNull(errorAction);
        var step = new DelegateFlowStep((ctx, ct) => Task.Run(() => errorAction(ctx), ct));
        return builder.OnAnyError(step);
    }

    public static WorkflowBuilderBase OnAnyError(this WorkflowBuilderBase builder, Action errorAction)
    {
        ArgumentNullException.ThrowIfNull(errorAction);
        var step = new DelegateFlowStep((_, ct) => Task.Run(errorAction, ct));
        return builder.OnAnyError(step);
    }

    public static WorkflowBuilderBase OnAnyError(this WorkflowBuilderBase builder, IFlowStep errorStep)
    {
        ArgumentNullException.ThrowIfNull(errorStep);
        builder.SetErrorHandlingStep(errorStep);
        return builder;
    }

    [Obsolete("This uses the legacy implementation of the ForEach step.")]
    public static WorkflowBuilderBase ForEach(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<object>> itemsSelector, AsyncFlowAction action)
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(action);

        var step = new DeprecatedForEachStep(itemsSelector, new DelegateFlowStep(action));
        builder.AddStep(step);
        return builder;
    }

    [Obsolete("This uses the legacy implementation of the ForEach step.")]
    public static WorkflowBuilderBase ForEach<TItem>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<TItem>> itemsSelector, AsyncFlowAction action)
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(action);

        var step = new DeprecatedForEachStep<TItem>(itemsSelector, new DelegateFlowStep(action));
        builder.AddStep(step);
        return builder;
    }

    [Obsolete("This uses the legacy implementation of the ForEach step.")]
    public static WorkflowBuilderBase ForEach<TStepIterator>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<object>> itemsSelector)
        where TStepIterator : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);

        var step = new DeprecatedForEachStep(itemsSelector, builder.ResolveAndConfigure<TStepIterator>());
        builder.AddStep(step);
        return builder;
    }

    [Obsolete("This uses the legacy implementation of the ForEach step.")]
    public static WorkflowBuilderBase ForEach<TStepIterator, TItem>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<TItem>> itemsSelector)
        where TStepIterator : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);

        var step = new DeprecatedForEachStep<TItem>(itemsSelector, builder.ResolveAndConfigure<TStepIterator>());
        builder.AddStep(step);
        return builder;
    }

    [Obsolete("This uses the legacy implementation of the ForEach step.")]
    public static WorkflowBuilderBase ForEach<TStepIterator>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<object>> itemsSelector, StepConfigurator<TStepIterator> configure)
        where TStepIterator : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(configure);

        var step = new DeprecatedForEachStep(itemsSelector, builder.ResolveAndConfigure(configure));
        builder.AddStep(step);
        return builder;
    }

    [Obsolete("This uses the legacy implementation of the ForEach step.")]
    public static WorkflowBuilderBase ForEach<TStepIterator, TItem>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<TItem>> itemsSelector, StepConfigurator<TStepIterator> configure)
        where TStepIterator : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(configure);

        var step = new DeprecatedForEachStep<TItem>(itemsSelector, builder.ResolveAndConfigure(configure));
        builder.AddStep(step);
        return builder;
    }

    [Obsolete("This uses the legacy implementation of the ForEach step.")]
    public static WorkflowBuilderBase ForEach<TItem>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Func<WorkflowBuilderBase> action)
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(action);

        var step = new DeprecatedForEachStep<TItem>(itemsSelector,
            new BuilderStep(action.Invoke(), ((FFlowBuilder)builder)?.Services ?? null));
        builder.AddStep(step);
        return builder;
    }
    
    public static WorkflowBuilderBase ForEach<TItem>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem> action)
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(action);

        var step = new ForEachStep<TItem>(itemsSelector, action);
        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase ForEach<TItem, TStepIterator>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem, TStepIterator> configure, Func<TStepIterator>? stepFactory = null)
        where TStepIterator : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(configure);

        stepFactory ??= () => builder.ResolveAndConfigure<TStepIterator>();
        
        var step = new ForEachStep<TItem, TStepIterator>(itemsSelector, stepFactory, configure);
        builder.AddStep(step);
        return builder;
        
    }

    public static FFlowBuilder WithDecorator<TDecorator>(this FFlowBuilder builder, Func<IFlowStep, TDecorator> decoratorFactory)
        where TDecorator : BaseStepDecorator
    {
        ArgumentNullException.ThrowIfNull(decoratorFactory);

        builder.WithOptions(options =>
            options.AddStepDecorator(decoratorFactory));
        return builder;
    }


internal static TStep ResolveAndConfigure<TStep>(
        this WorkflowBuilderBase builder,
        StepConfigurator<TStep>? configure = null)
        where TStep : class, IFlowStep
    {
        if (builder is FFlowBuilder flowBuilder && flowBuilder.TryResolveStep(out TStep? step))
            return step;
        try
        {
            return Activator.CreateInstance<TStep>();

        }
        catch (Exception e)
        {
            throw new StepCreationException(typeof(TStep), e);
        }
    }
}

public delegate void StepConfigurator<TStep>(TStep step, IFlowContext context) where TStep : IFlowStep;