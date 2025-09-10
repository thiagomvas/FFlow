using FFlow.Core;

namespace FFlow.Steps.FileIO;

/// <summary>
/// A workflow step that renames a file within its current directory.
/// The new name must not conflict with an existing file.
/// </summary>
[StepName("Rename File")]
[StepTags("file", "io")]
public class RenameFileStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the file to rename.
    /// </summary>
    public string SourcePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new file name.
    /// </summary>
    public string NewName { get; set; } = string.Empty;
    
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(SourcePath))
        {
            throw new ArgumentException("SourcePath cannot be null or empty.", nameof(SourcePath));
        }

        if (string.IsNullOrWhiteSpace(NewName))
        {
            throw new ArgumentException("NewName cannot be null or empty.", nameof(NewName));
        }

        if (!File.Exists(SourcePath))
        {
            throw new FileNotFoundException($"Source file not found: {SourcePath}", SourcePath);
        }

        var directory = Path.GetDirectoryName(SourcePath);
        if (directory == null)
        {
            throw new InvalidOperationException("Could not determine the directory of the source file.");
        }

        var destinationPath = Path.Combine(directory, NewName);
        if (File.Exists(destinationPath))
        {
            throw new IOException($"A file with the name '{NewName}' already exists in the directory '{directory}'.");
        }

        File.Move(SourcePath, destinationPath);
        return Task.CompletedTask;
    }
}