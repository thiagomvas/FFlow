using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("Create Directory")]
[StepTags("file", "io")]
public class CreateDirectoryStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new InvalidOperationException("Path cannot be null or empty.");
        }

        var directoryInfo = new DirectoryInfo(Path);
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        return Task.CompletedTask;
    }
}