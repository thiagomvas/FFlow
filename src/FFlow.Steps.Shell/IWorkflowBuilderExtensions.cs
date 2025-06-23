using FFlow.Core;
using FFlow.Steps.Shell;

namespace FFlow.Extensions;

/// <summary>
/// Extension methods for <see cref="IWorkflowBuilder"/> to run shell commands and scripts.
/// </summary>
public static class IWorkflowBuilderExtensions
{
    /// <summary>
    /// Adds a step to run a shell command with optional configuration.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="configure">Optional configuration action for the <see cref="RunCommandStep"/>.</param>
    /// <returns>A configurable step builder.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="command"/> is null or empty.</exception>
    public static IConfigurableStepBuilder RunCommand(this IWorkflowBuilder builder, string command, Action<RunCommandStep>? configure = null)
    {
        if (string.IsNullOrEmpty(command))
            throw new ArgumentException("Command cannot be null or empty.", nameof(command));

        var step = new RunCommandStep { Command = command };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }
    
    /// <summary>
    /// Adds a step to run a raw script with optional configuration.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="script">The script content to execute.</param>
    /// <param name="configure">Optional configuration action for the <see cref="RunScriptRawStep"/>.</param>
    /// <returns>A configurable step builder.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="script"/> is null or empty.</exception>
    public static IConfigurableStepBuilder RunScriptRaw(this IWorkflowBuilder builder, string script, Action<RunScriptRawStep>? configure = null)
    {
        if (string.IsNullOrEmpty(script))
            throw new ArgumentException("Script cannot be null or empty.", nameof(script));

        var step = new RunScriptRawStep { Script = script };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }
    
    /// <summary>
    /// Adds a step to run a script loaded from a file, with optional configuration.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="scriptFilePath">The path to the script file to execute.</param>
    /// <param name="configure">Optional configuration action for the <see cref="RunScriptRawStep"/>.</param>
    /// <returns>A configurable step builder.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="scriptFilePath"/> is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the script file does not exist.</exception>
    public static IConfigurableStepBuilder RunScriptFile(this IWorkflowBuilder builder, string scriptFilePath, Action<RunScriptRawStep>? configure = null)
    {
        if (string.IsNullOrEmpty(scriptFilePath))
            throw new ArgumentException("Script file path cannot be null or empty.", nameof(scriptFilePath));

        if (!System.IO.File.Exists(scriptFilePath))
            throw new FileNotFoundException("Script file not found.", scriptFilePath);

        var script = System.IO.File.ReadAllText(scriptFilePath);
        return builder.RunScriptRaw(script, configure);
    }
}
