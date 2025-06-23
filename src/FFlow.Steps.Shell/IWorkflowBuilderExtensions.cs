using FFlow.Core;

namespace FFlow.Steps.Shell;

public static class IWorkflowBuilderExtensions
{
    public static IConfigurableStepBuilder RunCommand(this IWorkflowBuilder builder, string command, Action<RunCommandStep>? configure = null)
    {
        if (string.IsNullOrEmpty(command))
            throw new ArgumentException("Command cannot be null or empty.", nameof(command));

        var step = new RunCommandStep { Command = command };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }
    
    public static IConfigurableStepBuilder RunScriptRaw(this IWorkflowBuilder builder, string script, Action<RunScriptRawStep>? configure = null)
    {
        if (string.IsNullOrEmpty(script))
            throw new ArgumentException("Script cannot be null or empty.", nameof(script));

        var step = new RunScriptRawStep { Script = script };
        configure?.Invoke(step);
        return builder.AddStep(step);
    }
    
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