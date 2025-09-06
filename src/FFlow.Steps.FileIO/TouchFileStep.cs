using FFlow.Core;

namespace FFlow.Steps.FileIO;

/// <summary>
/// Step that creates a new file or updates the timestamp of an existing file at the specified path.
/// </summary>
[StepName("Touch File")]
[StepTags("file", "io")]
public class TouchFileStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    public DateTime? AccessTime { get; set; }
    public DateTime? ModificationTime { get; set; }
    
    
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(Path));
        }

        if (!File.Exists(Path))
        {
            using (File.Create(Path)) { }
        }

        if (AccessTime.HasValue)
        {
            File.SetLastAccessTime(Path, AccessTime.Value);
        }

        if (ModificationTime.HasValue)
        {
            File.SetLastWriteTime(Path, ModificationTime.Value);
        }

        return Task.CompletedTask;
    }
}