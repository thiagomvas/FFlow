using FFlow.Core;

namespace FFlow.Steps.FileIO;

public static class WorkflowBuilderExtensions
{
    public static WorkflowBuilderBase FileExists(this WorkflowBuilderBase builder, Action<FileExistsStep> configure)
    {
        var step = new FileExistsStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }
    
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
}