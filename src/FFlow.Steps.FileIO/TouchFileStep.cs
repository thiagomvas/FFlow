using FFlow.Core;

namespace FFlow.Steps.FileIO;

/// <summary>
/// A workflow step that creates a new file if it does not exist,
/// or updates the last access and/or modification timestamps of an existing file.
/// </summary>
[StepName("Touch File")]
[StepTags("file", "io")]
public class TouchFileStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the file to touch.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional access time to apply to the file.
    /// If not provided, the access time remains unchanged.
    /// </summary>
    public DateTime? AccessTime { get; set; }

    /// <summary>
    /// Gets or sets an optional modification time to apply to the file.
    /// If not provided, the modification time remains unchanged.
    /// </summary>
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