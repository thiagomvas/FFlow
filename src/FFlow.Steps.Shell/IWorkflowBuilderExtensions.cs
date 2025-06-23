using FFlow.Core;

namespace FFlow.Steps.Shell;

public static class IWorkflowBuilderExtensions
{
    public static IConfigurableStepBuilder RunCommand(this IWorkflowBuilder builder, string command)
    {
        if (string.IsNullOrEmpty(command))
            throw new ArgumentException("Command cannot be null or empty.", nameof(command));

        var step = new RunCommandStep { Command = command };
        return builder.AddStep(step);
    }
}