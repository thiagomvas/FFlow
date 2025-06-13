using FFlow.Core;

namespace FFlow;

public static class IWorkflowBuilderExtensions
{
    public static IWorkflowBuilder WithOptions(this IConfigurableWorkflowBuilder builder, WorkflowOptions options)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));
        if (options is null) throw new ArgumentNullException(nameof(options));

        return builder.WithOptions(options);
    }

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