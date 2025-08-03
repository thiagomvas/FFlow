using System.Net.NetworkInformation;
using FFlow.Core;
using FFlow.Exceptions;

namespace FFlow;

/// <summary>
/// Flow Control extensions for <see cref="WorkflowBuilderBase"/>.
/// </summary>
public static class FFlowBuilderExtensions
{
    /// <summary>
    /// Adds the specified step type as the first step in the workflow.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to insert.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <returns>The workflow builder with the inserted step.</returns>
    /// <remarks>
    /// The step is first resolved from the dependency injection container. If not registered, it is instantiated using a parameterless constructor.
    /// </remarks>
    public static WorkflowBuilderBase StartWith<TStep>(this WorkflowBuilderBase builder)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        builder.InsertStepAt(0, step);
        return builder;
    }
    
    /// <summary>
    /// Adds the specified step type as the first step in the workflow, with additional configuration.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to insert.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">The delegate to configure the step instance.</param>
    /// <returns>The workflow builder with the configured and inserted step.</returns>
    /// <remarks>
    /// The step is first resolved from the dependency injection container. If not registered, it is instantiated using a parameterless constructor.
    /// </remarks>
    public static WorkflowBuilderBase StartWith<TStep>(this WorkflowBuilderBase builder, Action<TStep> configure)
        where TStep : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(configure);
        return builder.StartWith<TStep>((step, _) => configure(step));
    }

    /// <summary>
    /// Adds the specified step type as the first step in the workflow, with configurator logic.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to insert.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">The configurator delegate for the step instance.</param>
    /// <returns>The workflow builder with the configured and inserted step.</returns>
    /// <remarks>
    /// The step is first resolved from the dependency injection container. If not registered, it is instantiated using a parameterless constructor.
    /// </remarks>
    public static WorkflowBuilderBase StartWith<TStep>(this WorkflowBuilderBase builder,
        StepConfigurator<TStep> configure)
        where TStep : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(configure);
        var step = builder.ResolveAndConfigure(configure);
        builder.InsertStepAt(0, step);
        return builder;
    }

    /// <summary>
    /// Adds the given step instance as the first step in the workflow.
    /// </summary>
    /// <typeparam name="TStep">The type of the step.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The step instance to insert.</param>
    /// <returns>The workflow builder with the inserted step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>

    public static WorkflowBuilderBase StartWith<TStep>(this WorkflowBuilderBase builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        builder.InsertStepAt(0, step);
        return builder;
    }

    /// <summary>
    /// Adds an asynchronous delegate as the first step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The asynchronous step delegate.</param>
    /// <returns>The workflow builder with the inserted delegate step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is wrapped in a <see cref="DelegateFlowStep"/>.
    /// </remarks>
    public static WorkflowBuilderBase StartWith(this WorkflowBuilderBase builder, AsyncFlowAction step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.StartWith(new DelegateFlowStep(step));
    }

    /// <summary>
    /// Adds a synchronous delegate with access to <see cref="IFlowContext"/> as the first step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The delegate representing the step logic.</param>
    /// <returns>The workflow builder with the inserted delegate step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is executed using <see cref="Task.Run"/> and wrapped in a <see cref="DelegateFlowStep"/>.
    /// </remarks>
    public static WorkflowBuilderBase StartWith(this WorkflowBuilderBase builder, Action<IFlowContext> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.StartWith(new DelegateFlowStep((ctx, ct) => Task.Run(() => step(ctx), ct)));
    }

    /// <summary>
    /// Adds a parameterless synchronous delegate as the first step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The delegate representing the step logic.</param>
    /// <returns>The workflow builder with the inserted delegate step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is executed using <see cref="Task.Run"/> and wrapped in a <see cref="DelegateFlowStep"/>.
    /// </remarks>
    public static WorkflowBuilderBase StartWith(this WorkflowBuilderBase builder, Action step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.StartWith(new DelegateFlowStep((_, ct) => Task.Run(step, ct)));
    }

    /// <summary>
    /// Adds the given step instance as the next step in the workflow.
    /// </summary>
    /// <typeparam name="TStep">The type of the step.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The step instance to add.</param>
    /// <returns>The workflow builder with the added step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    public static WorkflowBuilderBase Then<TStep>(this WorkflowBuilderBase builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds the specified step type as the next step in the workflow.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to add.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <returns>The workflow builder with the added step.</returns>
    /// <remarks>
    /// The step is first resolved from the dependency injection container. If not registered, it is instantiated using a parameterless constructor.
    /// </remarks>
    public static WorkflowBuilderBase Then<TStep>(this WorkflowBuilderBase builder)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds the specified step type as the next step in the workflow, with additional configuration.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to add.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">The delegate to configure the step instance.</param>
    /// <returns>The workflow builder with the configured step added.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    /// <remarks>
    /// The step is first resolved from the dependency injection container. If not registered, it is instantiated using a parameterless constructor.
    /// </remarks>
    public static WorkflowBuilderBase Then<TStep>(this WorkflowBuilderBase builder, Action<TStep> configure)
        where TStep : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(configure);
        return builder.Then<TStep>((step, _) => configure(step));
    }

    /// <summary>
    /// Adds the specified step type as the next step in the workflow, using a configurator delegate.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to add.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">The configurator delegate for the step.</param>
    /// <returns>The workflow builder with the configured step added.</returns>
    /// <remarks>
    /// The step is first resolved from the dependency injection container. If not registered, it is instantiated using a parameterless constructor.
    /// </remarks>
    public static WorkflowBuilderBase Then<TStep>(this WorkflowBuilderBase builder, StepConfigurator<TStep> configure)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure(configure);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds an asynchronous delegate as the next step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The asynchronous delegate representing the step logic.</param>
    /// <returns>The workflow builder with the added delegate step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is wrapped in a <see cref="DelegateFlowStep"/>.
    /// </remarks>
    public static WorkflowBuilderBase Then(this WorkflowBuilderBase builder, AsyncFlowAction step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep(step));
    }

    /// <summary>
    /// Adds a synchronous delegate with access to <see cref="IFlowContext"/> as the next step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The delegate representing the step logic.</param>
    /// <returns>The workflow builder with the added delegate step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is executed using <see cref="Task.Run"/> and wrapped in a <see cref="DelegateFlowStep"/>.
    /// </remarks>
    public static WorkflowBuilderBase Then(this WorkflowBuilderBase builder, Action<IFlowContext> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep((ctx, ct) => Task.Run(() => step(ctx), ct)));
    }
    
    /// <summary>
    /// Adds a parameterless synchronous delegate as the next step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The delegate representing the step logic.</param>
    /// <returns>The workflow builder with the added delegate step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is executed using <see cref="Task.Run"/> and wrapped in a <see cref="DelegateFlowStep"/>.
    /// </remarks>
    public static WorkflowBuilderBase Then(this WorkflowBuilderBase builder, Action step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Then(new DelegateFlowStep((_, ct) => Task.Run(step, ct)));
    }

    /// <summary>
    /// Adds the given step as the final step in the workflow.
    /// </summary>
    /// <typeparam name="TStep">The type of the step.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The step instance to add.</param>
    /// <returns>The updated workflow builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// If the builder is an <see cref="FFlowBuilder"/>, the step is registered as the finalizer via <c>WithFinalizer</c>. Otherwise, it is added as a regular step.
    /// </remarks>
    public static WorkflowBuilderBase Finally<TStep>(this WorkflowBuilderBase builder, TStep step)
        where TStep : IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        if (builder is FFlowBuilder flowBuilder)
        {
            flowBuilder.WithFinalizer(step);
            return flowBuilder;
        }
        else
        {
            builder.AddStep(step);
            return builder;
        }
    }

    /// <summary>
    /// Adds the specified step type as the final step in the workflow.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to add.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <returns>The updated workflow builder.</returns>
    /// <remarks>
    /// The step is resolved from the dependency injection container, or created via a parameterless constructor if not registered.
    /// If the builder is an <see cref="FFlowBuilder"/>, the step is registered as the finalizer via <c>WithFinalizer</c>. Otherwise, it is added as a regular step.
    /// </remarks>
    public static WorkflowBuilderBase Finally<TStep>(this WorkflowBuilderBase builder)
        where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        if (builder is FFlowBuilder flowBuilder)
        {
            flowBuilder.WithFinalizer(step);
            return flowBuilder;
        }
        else
        {
            builder.AddStep(step);
            return builder;
        }
    }

    /// <summary>
    /// Adds the specified step type as the final step in the workflow, with configuration logic.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to add.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">The delegate to configure the step instance.</param>
    /// <returns>The updated workflow builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    /// <remarks>
    /// The step is resolved from the dependency injection container, or created via a parameterless constructor if not registered.
    /// If the builder is an <see cref="FFlowBuilder"/>, the step is registered as the finalizer via <c>WithFinalizer</c>. Otherwise, it is added as a regular step.
    /// </remarks>
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
            flowBuilder.WithFinalizer(step);
            return flowBuilder;
        }
        else
        {
            builder.AddStep(step);
            return builder;
        }
    }
    
    /// <summary>
    /// Adds an asynchronous delegate as the final step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The asynchronous delegate representing the step logic.</param>
    /// <returns>The updated workflow builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is wrapped in a <see cref="DelegateFlowStep"/> and added as the final step.
    /// If the builder is an <see cref="FFlowBuilder"/>, it is registered as the finalizer.
    /// </remarks>

    public static WorkflowBuilderBase Finally(this WorkflowBuilderBase builder, AsyncFlowAction step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep(step));
    }

    /// <summary>
    /// Adds a synchronous delegate with access to <see cref="IFlowContext"/> as the final step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The delegate representing the step logic.</param>
    /// <returns>The updated workflow builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is executed using <see cref="Task.Run"/> and wrapped in a <see cref="DelegateFlowStep"/>.
    /// If the builder is an <see cref="FFlowBuilder"/>, it is registered as the finalizer.
    /// </remarks>
    public static WorkflowBuilderBase Finally(this WorkflowBuilderBase builder, Action<IFlowContext> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep((ctx, ct) => Task.Run(() => step(ctx), ct)));
    }

    
    /// <summary>
    /// Adds a parameterless synchronous delegate as the final step in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="step">The delegate representing the step logic.</param>
    /// <returns>The updated workflow builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="step"/> is null.</exception>
    /// <remarks>
    /// The delegate is executed using <see cref="Task.Run"/> and wrapped in a <see cref="DelegateFlowStep"/>.
    /// If the builder is an <see cref="FFlowBuilder"/>, it is registered as the finalizer.
    /// </remarks>
    public static WorkflowBuilderBase Finally(this WorkflowBuilderBase builder, Action step)
    {
        ArgumentNullException.ThrowIfNull(step);
        return builder.Finally(new DelegateFlowStep((_, ct) => Task.Run(step, ct)));
    }

    /// <summary>
    /// Continues the workflow by adding a continuation step based on the specified workflow definition type.
    /// </summary>
    /// <typeparam name="TWorkflowDefinition">The workflow definition type to continue with.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <returns>The workflow builder with the added continuation step.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the workflow definition instance cannot be created.</exception>
    /// <remarks>
    /// The method tries to resolve <typeparamref name="TWorkflowDefinition"/> from the DI container via the builder's services. 
    /// If not found, it attempts to create an instance using a parameterless constructor.
    /// </remarks>
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
    
    /// <summary>
    /// Adds a delay step to the workflow with the specified <see cref="TimeSpan"/> duration.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="delay">The delay duration. Must be greater than zero.</param>
    /// <returns>The workflow builder with the added delay step.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="delay"/> is zero or negative.</exception>

    public static WorkflowBuilderBase Delay(this WorkflowBuilderBase builder, TimeSpan delay)
    {
        if (delay <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(delay), "Delay must be greater than zero.");

        var step = new DelayStep(delay);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a delay step to the workflow with the specified delay in milliseconds.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="milliseconds">The delay duration in milliseconds. Must be greater than zero.</param>
    /// <returns>The workflow builder with the added delay step.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="milliseconds"/> is zero or negative.</exception>
    public static WorkflowBuilderBase Delay(this WorkflowBuilderBase builder, int milliseconds)
    {
        if (milliseconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(milliseconds), "Delay must be greater than zero.");

        return builder.Delay(TimeSpan.FromMilliseconds(milliseconds));
    }

    /// <summary>
    /// Adds a fork step to the workflow that executes multiple workflows concurrently using the default <see cref="ForkStrategy.FireAndForget"/> strategy.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="workflowFactories">Factories producing workflows to execute in parallel.</param>
    /// <returns>The workflow builder with the added fork step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="workflowFactories"/> is null or empty.</exception>
    public static WorkflowBuilderBase Fork(this WorkflowBuilderBase builder, params Func<IWorkflow>[] workflowFactories)
    {
        if (workflowFactories == null || workflowFactories.Length == 0)
            throw new ArgumentNullException(nameof(workflowFactories), "At least one workflow must be provided.");

        var step = new ForkStep(ForkStrategy.FireAndForget, workflowFactories);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a fork step to the workflow that executes multiple workflows concurrently using the specified fork strategy.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="strategy">The fork strategy to control workflow execution behavior.</param>
    /// <param name="workflowFactories">Factories producing workflows to execute in parallel.</param>
    /// <returns>The workflow builder with the added fork step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="workflowFactories"/> is null or empty.</exception>
    public static WorkflowBuilderBase Fork(this WorkflowBuilderBase builder, ForkStrategy strategy,
        params Func<IWorkflow>[] workflowFactories)
    {
        if (workflowFactories == null || workflowFactories.Length == 0)
            throw new ArgumentNullException(nameof(workflowFactories), "At least one workflow must be provided.");

        var step = new ForkStep(strategy, workflowFactories);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a fork step to the workflow that executes multiple nested workflow builders concurrently using the default <see cref="ForkStrategy.FireAndForget"/> strategy.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="workflowBuilders">Factories producing nested workflow builders to execute in parallel.</param>
    /// <returns>The workflow builder with the added fork step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="workflowBuilders"/> is null or empty.</exception>
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

    /// <summary>
    /// Adds a fork step to the workflow that executes multiple nested workflow builders concurrently using the specified fork strategy.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="strategy">The fork strategy to control workflow execution behavior.</param>
    /// <param name="workflowBuilders">Factories producing nested workflow builders to execute in parallel.</param>
    /// <returns>The workflow builder with the added fork step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="workflowBuilders"/> is null or empty.</exception>
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

    /// <summary>
    /// Adds an if step with a condition and a single true step type.
    /// </summary>
    /// <typeparam name="TTrue">The type of the step to execute if the condition is true.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition delegate to evaluate.</param>
    /// <returns>The workflow builder with the added if step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> is null.</exception>
    /// <remarks>
    /// The true step is resolved from DI or created via parameterless constructor.
    /// </remarks>
    public static WorkflowBuilderBase If<TTrue>(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition)
        where TTrue : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(condition);

        var trueStep = builder.ResolveAndConfigure<TTrue>();
        var step = new IfStep(condition, trueStep);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds an if step with a condition and separate true and false step types.
    /// </summary>
    /// <typeparam name="TTrue">The step type for the true branch.</typeparam>
    /// <typeparam name="TFalse">The step type for the false branch.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition delegate to evaluate.</param>
    /// <returns>The workflow builder with the added if step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> is null.</exception>
    /// <remarks>
    /// Both true and false steps are resolved from DI or created via parameterless constructors.
    /// </remarks>
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
    
    /// <summary>
    /// Adds an if step with a condition and optional configurators for true and false steps.
    /// </summary>
    /// <typeparam name="TTrue">The step type for the true branch.</typeparam>
    /// <typeparam name="TFalse">The step type for the false branch.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition delegate to evaluate.</param>
    /// <param name="trueStepConfigurator">Optional configurator for the true step.</param>
    /// <param name="falseStepConfigurator">Optional configurator for the false step.</param>
    /// <returns>The workflow builder with the added if step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> is null.</exception>

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
    
    /// <summary>
    /// Adds an if step with a condition and asynchronous delegates for true and optional false steps.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition delegate to evaluate.</param>
    /// <param name="trueStepAction">The asynchronous action to run if true.</param>
    /// <param name="falseStepAction">Optional asynchronous action to run if false.</param>
    /// <returns>The workflow builder with the added if step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> or <paramref name="trueStepAction"/> is null.</exception>
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

    /// <summary>
    /// Adds an if step with a condition and synchronous delegates with context for true and optional false steps.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition delegate to evaluate.</param>
    /// <param name="trueStepAction">The synchronous action to run if true.</param>
    /// <param name="falseStepAction">Optional synchronous action to run if false.</param>
    /// <returns>The workflow builder with the added if step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> or <paramref name="trueStepAction"/> is null.</exception>
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

    /// <summary>
    /// Adds an if step with a condition and parameterless synchronous delegate for true and optional context-aware false steps.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition delegate to evaluate.</param>
    /// <param name="stepAction">The synchronous action to run if true.</param>
    /// <param name="falseStepAction">Optional synchronous action to run if false.</param>
    /// <returns>The workflow builder with the added if step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> or <paramref name="stepAction"/> is null.</exception>
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

    /// <summary>
    /// Adds an if step with a condition and explicit true and optional false step instances.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition delegate to evaluate.</param>
    /// <param name="trueStep">The step to execute if true.</param>
    /// <param name="falseStep">Optional step to execute if false.</param>
    /// <returns>The workflow builder with the added if step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> or <paramref name="trueStep"/> is null.</exception>
    public static WorkflowBuilderBase If(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition,
        IFlowStep trueStep, IFlowStep? falseStep = null)
    {
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(trueStep);

        var step = new IfStep(condition, trueStep, falseStep);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds an if step with a condition and workflow builder factories for true and optional false branches.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition delegate to evaluate.</param>
    /// <param name="trueFactory">Factory for building the true branch workflow.</param>
    /// <param name="falseFactory">Optional factory for building the false branch workflow.</param>
    /// <returns>The workflow builder with the added if step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> or <paramref name="trueFactory"/> is null.</exception>
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

    /// <summary>
    /// Adds a step that stops workflow execution immediately when reached.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <returns>The workflow builder with the added stop step.</returns>
    public static WorkflowBuilderBase Stop(this WorkflowBuilderBase builder)
    {
        var step = new StopExecutionStep();
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a conditional stop step that stops workflow execution if the specified condition evaluates to true.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition to evaluate for stopping execution.</param>
    /// <returns>The workflow builder with the added conditional stop step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> is null.</exception>
    public static WorkflowBuilderBase StopIf(this WorkflowBuilderBase builder, Func<IFlowContext, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        var step = new StopExecutionIfStep(condition);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a switch step to the workflow, configured via the provided case builder action.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="caseBuilder">An action to configure switch cases.</param>
    /// <returns>The workflow builder with the added switch step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="caseBuilder"/> is null.</exception>
    public static WorkflowBuilderBase Switch(this WorkflowBuilderBase builder, Action<SwitchCaseBuilder> caseBuilder)
    {
        ArgumentNullException.ThrowIfNull(caseBuilder);

        var switchCaseBuilder = new SwitchCaseBuilder { _serviceProvider = ((FFlowBuilder)builder)?.Services ?? null };
        caseBuilder(switchCaseBuilder);

        var step = switchCaseBuilder.Build();

        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step that throws an exception with the specified message when executed.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="message">The exception message. Cannot be null or whitespace.</param>
    /// <returns>The workflow builder with the added throw step.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="message"/> is null or whitespace.</exception>
    public static WorkflowBuilderBase Throw(this WorkflowBuilderBase builder, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));

        var step = new ThrowExceptionStep(message);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step that throws a specified exception type with the given message when executed.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="message">The exception message. Cannot be null or whitespace.</param>
    /// <returns>The workflow builder with the added throw step.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="message"/> is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the exception instance could not be created.</exception>
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

    /// <summary>
    /// Adds a conditional step that throws an exception with the specified message if the condition is true.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="message">The exception message. Cannot be null or whitespace.</param>
    /// <returns>The workflow builder with the added conditional throw step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="message"/> is null or whitespace.</exception>
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

    /// <summary>
    /// Adds a conditional step that throws a specified exception type with the given message if the condition is true.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="message">The exception message. Cannot be null or whitespace.</param>
    /// <returns>The workflow builder with the added conditional throw step.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="message"/> is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the exception instance could not be created.</exception>
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

    /// <summary>
    /// Adds a step that logs the specified message to the console when executed.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="message">The message to log.</param>
    /// <returns>The workflow builder with the added log step.</returns>
    public static WorkflowBuilderBase LogToConsole(this WorkflowBuilderBase builder, string message)
    {
        var step = new LogToConsoleStep(message);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Sets a step to handle any errors in the workflow, resolved and configured by type.
    /// </summary>
    /// <typeparam name="TStep">The error handling step type.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <returns>The workflow builder with the error handling step set.</returns>
    public static WorkflowBuilderBase OnAnyError<TStep>(this WorkflowBuilderBase builder) where TStep : class, IFlowStep
    {
        var step = builder.ResolveAndConfigure<TStep>();
        builder.SetErrorHandlingStep(step);
        return builder;
    }

    /// <summary>
    /// Sets an asynchronous delegate as the error handling step for any errors in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="errorAction">The asynchronous action to run on error.</param>
    /// <returns>The workflow builder with the error handling step set.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="errorAction"/> is null.</exception>
    public static WorkflowBuilderBase OnAnyError(this WorkflowBuilderBase builder, AsyncFlowAction errorAction)
    {
        ArgumentNullException.ThrowIfNull(errorAction);
        var step = new DelegateFlowStep(errorAction);
        return builder.OnAnyError(step);
    }

    /// <summary>
    /// Sets a synchronous delegate with context as the error handling step for any errors in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="errorAction">The synchronous action to run on error.</param>
    /// <returns>The workflow builder with the error handling step set.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="errorAction"/> is null.</exception>
    public static WorkflowBuilderBase OnAnyError(this WorkflowBuilderBase builder, Action<IFlowContext> errorAction)
    {
        ArgumentNullException.ThrowIfNull(errorAction);
        var step = new DelegateFlowStep((ctx, ct) => Task.Run(() => errorAction(ctx), ct));
        return builder.OnAnyError(step);
    }

    /// <summary>
    /// Sets a parameterless synchronous delegate as the error handling step for any errors in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="errorAction">The synchronous action to run on error.</param>
    /// <returns>The workflow builder with the error handling step set.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="errorAction"/> is null.</exception>
    public static WorkflowBuilderBase OnAnyError(this WorkflowBuilderBase builder, Action errorAction)
    {
        ArgumentNullException.ThrowIfNull(errorAction);
        var step = new DelegateFlowStep((_, ct) => Task.Run(errorAction, ct));
        return builder.OnAnyError(step);
    }

    /// <summary>
    /// Sets a custom <see cref="IFlowStep"/> as the error handling step for any errors in the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="errorStep">The error handling step instance.</param>
    /// <returns>The workflow builder with the error handling step set.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="errorStep"/> is null.</exception>
    public static WorkflowBuilderBase OnAnyError(this WorkflowBuilderBase builder, IFlowStep errorStep)
    {
        ArgumentNullException.ThrowIfNull(errorStep);
        builder.SetErrorHandlingStep(errorStep);
        return builder;
    }

    /// <summary>
    /// [Obsolete] Adds a legacy ForEach step with an items selector and async action.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <param name="action">Asynchronous action to execute per item.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="itemsSelector"/> or <paramref name="action"/> is null.</exception>
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

    /// <summary>
    /// [Obsolete] Adds a legacy ForEach step with generic item type and async action.
    /// </summary>
    /// <typeparam name="TItem">Type of items to iterate.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <param name="action">Asynchronous action to execute per item.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
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

    /// <summary>
    /// [Obsolete] Adds a legacy ForEach step with specified iterator step type.
    /// </summary>
    /// <typeparam name="TStepIterator">Type of step to execute for each item.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="itemsSelector"/> is null.</exception>
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

    /// <summary>
    /// [Obsolete] Adds a legacy ForEach step with generic item type and specified iterator step type.
    /// </summary>
    /// <typeparam name="TStepIterator">Type of step to execute for each item.</typeparam>
    /// <typeparam name="TItem">Type of items to iterate.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
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

    /// <summary>
    /// [Obsolete] Adds a legacy ForEach step with iterator step and configurator.
    /// </summary>
    /// <typeparam name="TStepIterator">Type of step to execute for each item.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <param name="configure">Step configurator delegate.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
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

    /// <summary>
    /// [Obsolete] Adds a legacy ForEach step with generic item type, iterator step, and configurator.
    /// </summary>
    /// <typeparam name="TStepIterator">Type of step to execute for each item.</typeparam>
    /// <typeparam name="TItem">Type of items to iterate.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <param name="configure">Step configurator delegate.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
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

    /// <summary>
    /// [Obsolete] Adds a legacy ForEach step with generic item type and workflow builder action.
    /// </summary>
    /// <typeparam name="TItem">Type of items to iterate.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <param name="action">Factory producing workflow builder for each item.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
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

    /// <summary>
    /// Adds a ForEach step with generic item type and synchronous action.
    /// </summary>
    /// <typeparam name="TItem">Type of items to iterate.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <param name="action">Action to execute for each item.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
    public static WorkflowBuilderBase ForEach<TItem>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem> action)
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(action);

        var step = new ForEachStep<TItem>(itemsSelector, action);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a ForEach step with generic item type and configurable iterator step.
    /// </summary>
    /// <typeparam name="TItem">Type of items to iterate.</typeparam>
    /// <typeparam name="TStepIterator">Type of step used for each iteration.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="itemsSelector">Function selecting items to iterate.</param>
    /// <param name="configure">Action to configure each iteration step with the item.</param>
    /// <param name="stepFactory">Optional factory for creating iterator step instances; defaults to resolving from DI or parameterless constructor.</param>
    /// <returns>The workflow builder with the added ForEach step.</returns>
    public static WorkflowBuilderBase ForEach<TItem, TStepIterator>(this WorkflowBuilderBase builder,
        Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem, TStepIterator> configure,
        Func<TStepIterator>? stepFactory = null)
        where TStepIterator : class, IFlowStep
    {
        ArgumentNullException.ThrowIfNull(itemsSelector);
        ArgumentNullException.ThrowIfNull(configure);

        stepFactory ??= () => builder.ResolveAndConfigure<TStepIterator>();

        var step = new ForEachStep<TItem, TStepIterator>(itemsSelector, stepFactory, configure);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step decorator to the workflow builder using the specified decorator factory.
    /// </summary>
    /// <typeparam name="TDecorator">The type of the step decorator, inheriting from <see cref="BaseStepDecorator"/>.</typeparam>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="decoratorFactory">A factory function that takes an <see cref="IFlowStep"/> and returns a decorator instance.</param>
    /// <returns>The workflow builder with the added step decorator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="decoratorFactory"/> is null.</exception>
    public static FFlowBuilder WithDecorator<TDecorator>(this FFlowBuilder builder,
        Func<IFlowStep, TDecorator> decoratorFactory)
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