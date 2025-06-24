using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// Extension methods for <see cref="IWorkflowBuilder"/> to add common .NET CLI steps
/// such as build, run, test, pack, restore, and publish to a workflow.
/// </summary>
public static class IWorkflowBuilderExtensions
{
    /// <summary>
    /// Adds a <see cref="DotnetBuildStep"/> to the workflow and allows configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="configure">An action to configure the <see cref="DotnetBuildStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    public static IConfigurableStepBuilder DotnetBuild(this IWorkflowBuilder builder,
        Action<DotnetBuildStep> configure)
    {
        var step = new DotnetBuildStep();
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetBuildStep"/> to the workflow for the specified project or solution,
    /// and allows optional configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="projectOrSolution">The project or solution file path.</param>
    /// <param name="configure">An optional action to configure the <see cref="DotnetBuildStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="projectOrSolution"/> is null or empty.</exception>
    public static IConfigurableStepBuilder DotnetBuild(this IWorkflowBuilder builder, string projectOrSolution,
        Action<DotnetBuildStep>? configure = null)
    {
        if (string.IsNullOrEmpty(projectOrSolution))
            throw new ArgumentException("Project file path cannot be null or empty.", nameof(projectOrSolution));

        var step = new DotnetBuildStep { ProjectOrSolution = projectOrSolution };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetRunStep"/> to the workflow and allows configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="configure">An action to configure the <see cref="DotnetRunStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    public static IConfigurableStepBuilder DotnetRun(this IWorkflowBuilder builder,
        Action<DotnetRunStep> configure)
    {
        var step = new DotnetRunStep();
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetRunStep"/> to the workflow for the specified project,
    /// and allows optional configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="project">The project file path.</param>
    /// <param name="configure">An optional action to configure the <see cref="DotnetRunStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="project"/> is null or empty.</exception>
    public static IConfigurableStepBuilder DotnetRun(this IWorkflowBuilder builder, string project,
        Action<DotnetRunStep>? configure = null)
    {
        if (string.IsNullOrEmpty(project))
            throw new ArgumentException("Project file path cannot be null or empty.", nameof(project));

        var step = new DotnetRunStep { Project = project };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetTestStep"/> to the workflow and allows configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="configure">An action to configure the <see cref="DotnetTestStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    public static IConfigurableStepBuilder DotnetTest(this IWorkflowBuilder builder,
        Action<DotnetTestStep> configure)
    {
        var step = new DotnetTestStep();
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetTestStep"/> to the workflow for the specified project or solution,
    /// and allows optional configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="projectOrSolution">The project or solution file path.</param>
    /// <param name="configure">An optional action to configure the <see cref="DotnetTestStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="projectOrSolution"/> is null or empty.</exception>
    public static IConfigurableStepBuilder DotnetTest(this IWorkflowBuilder builder, string projectOrSolution,
        Action<DotnetTestStep>? configure = null)
    {
        if (string.IsNullOrEmpty(projectOrSolution))
            throw new ArgumentException("Project file path cannot be null or empty.", nameof(projectOrSolution));

        var step = new DotnetTestStep { ProjectOrSolution = projectOrSolution };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetPackStep"/> to the workflow and allows configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="configure">An action to configure the <see cref="DotnetPackStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    public static IConfigurableStepBuilder DotnetPack(this IWorkflowBuilder builder,
        Action<DotnetPackStep> configure)
    {
        var step = new DotnetPackStep();
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetPackStep"/> to the workflow for the specified project or solution,
    /// and allows optional configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="projectOrSolution">The project or solution file path.</param>
    /// <param name="configure">An optional action to configure the <see cref="DotnetPackStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="projectOrSolution"/> is null or empty.</exception>
    public static IConfigurableStepBuilder DotnetPack(this IWorkflowBuilder builder, string projectOrSolution,
        Action<DotnetPackStep>? configure = null)
    {
        if (string.IsNullOrEmpty(projectOrSolution))
            throw new ArgumentException("Project file path cannot be null or empty.", nameof(projectOrSolution));

        var step = new DotnetPackStep { ProjectOrSolution = projectOrSolution };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetRestoreStep"/> to the workflow and allows configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="configure">An action to configure the <see cref="DotnetRestoreStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    public static IConfigurableStepBuilder DotnetRestore(this IWorkflowBuilder builder,
        Action<DotnetRestoreStep> configure)
    {
        var step = new DotnetRestoreStep();
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetRestoreStep"/> to the workflow for the specified project or solution,
    /// and allows optional configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="projectOrSolution">The project or solution file path.</param>
    /// <param name="configure">An optional action to configure the <see cref="DotnetRestoreStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="projectOrSolution"/> is null or empty.</exception>
    public static IConfigurableStepBuilder DotnetRestore(this IWorkflowBuilder builder, string projectOrSolution,
        Action<DotnetRestoreStep>? configure = null)
    {
        if (string.IsNullOrEmpty(projectOrSolution))
            throw new ArgumentException("Project file path cannot be null or empty.", nameof(projectOrSolution));

        var step = new DotnetRestoreStep { ProjectOrSolution = projectOrSolution };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetPublishStep"/> to the workflow and allows configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="configure">An action to configure the <see cref="DotnetPublishStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    public static IConfigurableStepBuilder DotnetPublish(this IWorkflowBuilder builder,
        Action<DotnetPublishStep> configure)
    {
        var step = new DotnetPublishStep();
        configure?.Invoke(step);
        return builder.AddStep(step);
    }

    /// <summary>
    /// Adds a <see cref="DotnetPublishStep"/> to the workflow for the specified project or solution,
    /// and allows optional configuration via delegate.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="projectOrSolution">The project or solution file path.</param>
    /// <param name="configure">An optional action to configure the <see cref="DotnetPublishStep"/>.</param>
    /// <returns>The step builder for further configuration.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="projectOrSolution"/> is null or empty.</exception>
    public static IConfigurableStepBuilder DotnetPublish(this IWorkflowBuilder builder, string projectOrSolution,
        Action<DotnetPublishStep>? configure = null)
    {
        if (string.IsNullOrEmpty(projectOrSolution))
            throw new ArgumentException("Project file path cannot be null or empty.", nameof(projectOrSolution));

        var step = new DotnetPublishStep { ProjectOrSolution = projectOrSolution };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }
}