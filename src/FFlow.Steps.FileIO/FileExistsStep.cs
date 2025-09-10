using FFlow.Core;

namespace FFlow.Steps.FileIO;

/// <summary>
/// A workflow step that checks whether a file exists at the specified path.
/// Optionally throws an exception if the file does not exist.
/// </summary>
[StepName("File Exists")]
[StepTags("file", "io")]
public class FileExistsStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the file to check.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether an exception should be thrown
    /// if the file does not exist. Defaults to <c>true</c>.
    /// </summary>
    public bool ThrowIfNotExists { get; set; } = true;
    
    /// <summary>
    /// Gets a value indicating whether the file exists at the given <see cref="Path"/>.
    /// </summary>
    public bool Exists { get; private set; }
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(Path));
        }
        
        Exists = File.Exists(Path);

        if (!Exists)
        {
            if (ThrowIfNotExists)
            {
                throw new FileNotFoundException($"File not found: {Path}", Path);
            }
            else
            {
                context.SetOutputFor<FileExistsStep, bool>(false);
            }
        }
        
        return Task.CompletedTask;
    }
}