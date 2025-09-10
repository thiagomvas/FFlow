using FFlow.Core;

namespace FFlow.Steps.FileIO;

/// <summary>
/// A workflow step that deletes a specified directory.
/// </summary>
[StepName("Delete Directory")]
[StepTags("directory", "folder", "io")]
public class DeleteDirectoryStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the directory to delete.
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether to delete directories, subdirectories, and files in the specified path.
    /// </summary>
    public bool Recursive { get; set; } = false;
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(Path));
        }

        if (!Directory.Exists(Path))
        {
            throw new DirectoryNotFoundException($"Directory not found: {Path}");
        }

        Directory.Delete(Path, Recursive);
        return Task.CompletedTask;
    }
}