using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("Copy File")]
[StepTags("file", "io")]
public class CopyFileStep : FlowStep
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

        File.Copy(SourcePath, DestinationPath, Overwrite);
        return Task.CompletedTask;
    }
    
}