using FFlow.Core;

namespace FFlow.Steps.FileIO;

public class FileWriteTextStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ContextSourceKey { get; set; } = string.Empty;
    public bool Append { get; set; } = false;
    public bool AppendNewLine { get; set; } = false;
    
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
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
            File.AppendAllText(Path, Content);
        }
        else
        {
            File.WriteAllText(Path, Content);
        }

        return Task.CompletedTask;
    }
}