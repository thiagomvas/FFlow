using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("File Write/Append Text")]
[StepTags("file", "io")]
public class FileWriteTextStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ContextSourceKey { get; set; } = string.Empty;
    public bool Append { get; set; } = false;
    public bool AppendNewLine { get; set; } = false;
    
    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(Path));
        }

        if (!string.IsNullOrWhiteSpace(ContextSourceKey))
        {
            var contextValue = context.GetValue<string>(ContextSourceKey);
            if (contextValue != null)
            {
                Content = contextValue;
            }
        }

        if (Append)
        {
            if (AppendNewLine)
                Content = Environment.NewLine + Content;
            await File.AppendAllTextAsync(Path, Content, cancellationToken);
        }
        else
        {
            await File.WriteAllTextAsync(Path, Content, cancellationToken);
        }
    }
}