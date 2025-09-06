using FFlow.Core;

namespace FFlow.Steps.FileIO;

public static class WorkflowBuilderExtensions
{
    /// <summary>
    /// Adds a step to check if a file exists at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileExists(this WorkflowBuilderBase builder, Action<FileExistsStep> configure)
    {
        var step = new FileExistsStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to check if a file exists at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to check existence of.</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileExists(this WorkflowBuilderBase builder, string path, Action<FileExistsStep>? configure = null)
    {
        var step = new FileExistsStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to read all text from a file at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileReadAllText(this WorkflowBuilderBase builder, Action<FileReadAllTextStep> configure)
    {
        var step = new FileReadAllTextStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to read all text from a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to read all the text from</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileReadAllText(this WorkflowBuilderBase builder, string path, Action<FileReadAllTextStep>? configure = null)
    {
        var step = new FileReadAllTextStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to read all text from a file at the specified path and save it to a context key.
    /// </summary>
    /// <param name="path">The path of the file to read all the text from</param>
    /// <param name="saveToKey">The key to store the contents into a <see cref="IFlowContext"/></param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    
    public static WorkflowBuilderBase FileReadAllText(this WorkflowBuilderBase builder, string path, string saveToKey, Action<FileReadAllTextStep>? configure = null)
    {
        var step = new FileReadAllTextStep
        {
            Path = path,
            SaveToKey = saveToKey
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
}