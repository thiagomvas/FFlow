using FFlow.Core;

namespace FFlow.Steps.FileIO;
/// <summary>
/// A workflow step that copies a file from a source path to a destination path.
/// </summary>
[StepName("Copy File")]
[StepTags("file", "io")]
public class CopyFileStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the file to copy.
    /// </summary>
    public string SourcePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path where the file will be copied to.
    /// </summary>
    public string DestinationPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to overwrite the destination file if it already exists.
    /// </summary>
    public bool Overwrite { get; set; } = false;

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(SourcePath))
        {
            throw new ArgumentException("SourcePath cannot be null or empty.", nameof(SourcePath));
        }

        if (string.IsNullOrWhiteSpace(DestinationPath))
        {
            throw new ArgumentException("DestinationPath cannot be null or empty.", nameof(DestinationPath));
        }

        if (!File.Exists(SourcePath))
        {
            throw new FileNotFoundException($"Source file not found: {SourcePath}", SourcePath);
        }

        File.Copy(SourcePath, DestinationPath, Overwrite);
        return Task.CompletedTask;
    }
    
}