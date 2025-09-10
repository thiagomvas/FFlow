using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("Delete File")]
[StepTags("file", "io")]
public class DeleteFileStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new InvalidOperationException("Path cannot be null or empty.");
        }

        if (File.Exists(Path))
        {
            File.Delete(Path);
        }
        
        return Task.CompletedTask;
    }
}