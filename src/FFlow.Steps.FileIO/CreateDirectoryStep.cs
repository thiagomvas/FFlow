using FFlow.Core;

namespace FFlow.Steps.FileIO;
/// <summary>
/// A workflow step that creates a directory at the specified path
/// if it does not already exist.
/// </summary>
[StepName("Create Directory")]
[StepTags("file", "io")]
public class CreateDirectoryStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the directory to create.
    /// </summary>
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