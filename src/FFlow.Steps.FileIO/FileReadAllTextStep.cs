using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("File Read All Text")]
[StepTags("file", "io")]
public class FileReadAllTextStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    public string SaveToKey { get; set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(Path));
        }
        
        if (!File.Exists(Path))
        {
            throw new FileNotFoundException($"File not found: {Path}", Path);
        }

        Content = await File.ReadAllTextAsync(Path, cancellationToken);
        context.SetOutputFor<FileReadAllTextStep, string>(Content);

        if (!string.IsNullOrWhiteSpace(SaveToKey))
        {
            context.SetValue(SaveToKey, Content);
        }
    }
}