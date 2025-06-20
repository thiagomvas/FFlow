using FFlow.Core;

namespace FFlow.Extensions;

/// <summary>
/// Provides extension methods for configuring workflows with options and decorators.
/// </summary>
public static class IWorkflowBuilderExtensions
{
    /// <summary>
    /// Configures the workflow builder with the specified workflow options.
    /// </summary>
    /// <param name="builder">The configurable workflow builder.</param>
    /// <param name="options">The workflow options to apply.</param>
    /// <returns>The updated workflow builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> or <paramref name="options"/> is null.</exception>
    public static IWorkflowBuilder WithOptions(this IConfigurableWorkflowBuilder builder, WorkflowOptions options)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));
        if (options is null) throw new ArgumentNullException(nameof(options));

        return builder.WithOptions(options);
    }

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
}