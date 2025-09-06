using FFlow.Core;

namespace FFlow.Steps.FileIO;

/// <summary>
/// Step that checks if a file exists at the specified path.
/// </summary>
[StepName("File Exists")]
[StepTags("file", "io")]
public class FileExistsStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    public bool ThrowIfNotExists { get; set; } = true;
    
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