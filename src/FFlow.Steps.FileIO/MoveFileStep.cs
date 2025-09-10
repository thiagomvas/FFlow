using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("Move File")]
[StepTags("file", "io")]
public class MoveFileStep : FlowStep
{
    public string SourcePath { get; set; } = string.Empty;
    public string DestinationPath { get; set; } = string.Empty;
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

        if (File.Exists(DestinationPath))
        {
            if (Overwrite)
            {
                File.Delete(DestinationPath);
            }
            else
            {
                throw new IOException($"Destination file already exists: {DestinationPath}");
            }
        }

        File.Move(SourcePath, DestinationPath);
        return Task.CompletedTask;
    }
}