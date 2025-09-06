using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("File Read All Bytes")]
[StepTags("file", "io")]
public class FileReadAllBytesStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    public string SaveToKey { get; set; } = string.Empty;
    public byte[] Content { get; private set; } = [];
    
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

        Content = await File.ReadAllBytesAsync(Path, cancellationToken);
        context.SetOutputFor<FileReadAllBytesStep, byte[]>(Content);

        if (!string.IsNullOrWhiteSpace(SaveToKey))
        {
            context.SetValue(SaveToKey, Content);
        }
    }
}