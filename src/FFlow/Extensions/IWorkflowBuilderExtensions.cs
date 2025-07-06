using FFlow.Core;

namespace FFlow.Extensions;

/// <summary>
/// Provides extension methods for configuring workflows with options and decorators.
/// </summary>
public static class IWorkflowBuilderExtensions
{
    /// <summary>
    /// Adds a step decorator to the workflow builder using the specified factory function.
    /// </summary>
    /// <param name="builder">The configurable workflow builder.</param>
    /// <param name="factory">The factory function to create the step decorator.</param>
    /// <returns>The updated workflow builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> or <paramref name="factory"/> is null.</exception>
    public static IWorkflowBuilder WithDecorator(
        this IConfigurableWorkflowBuilder builder,
        Func<IFlowStep, IFlowStep> factory)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));
        if (factory is null)
            throw new ArgumentNullException(nameof(factory));

        return builder.WithOptions(options =>
            options.AddStepDecorator(factory));
    }

    public static IWorkflowBuilder WithPipelineLogger(
        this IConfigurableWorkflowBuilder builder)
    {
        return builder.WithOptions(options => options.WithEventListener(new PipelineLoggerEventListener()));
    }

    /// <summary>
    /// Adds a step to stop the execution of the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder to add the step to.</param>
    /// <returns>The same <see cref="IWorkflowBuilder"/> for further configuration.</returns>
    public static IWorkflowBuilder StopExecution(this IWorkflowBuilder builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        return builder.Then<StopExecutionStep>();
        
    }
    
    /// <summary>
    /// Stops the execution of the workflow if the specified condition is met.
    /// </summary>
    /// <param name="builder">The workflow builder to add the step to.</param>
    /// <param name="condition">The condition that determines whether the execution should stop or not.</param>
    /// <returns>The same <see cref="IWorkflowBuilder"/> for further configuration.</returns>
    public static IWorkflowBuilder StopExecutionIf(
        this IWorkflowBuilder builder,
        Func<IFlowContext, bool> condition)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));
        if (condition is null)
            throw new ArgumentNullException(nameof(condition));

        return builder.AddStep(new StopExecutionIfStep(condition));
    }

}