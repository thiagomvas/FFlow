using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("File Write/Append Bytes")]
[StepTags("file", "io")]
public class FileWriteBytesStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    public byte[] Content { get; set; } = [];
    public string ContextSourceKey { get; set; } = string.Empty;
    public bool Append { get; set; } = false;
    
    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(Path));
        }

        if (!string.IsNullOrWhiteSpace(ContextSourceKey))
        {
            var contextValue = context.GetValue<byte[]>(ContextSourceKey);
            if (contextValue != null)
            {
                Content = contextValue;
            }
        }

        if (Append && File.Exists(Path))
        {
            var existingContent = await File.ReadAllBytesAsync(Path, cancellationToken);
            var combinedContent = new byte[existingContent.Length + Content.Length];
            Buffer.BlockCopy(existingContent, 0, combinedContent, 0, existingContent.Length);
            Buffer.BlockCopy(Content, 0, combinedContent, existingContent.Length, Content.Length);
            await File.WriteAllBytesAsync(Path, combinedContent, cancellationToken);
        }
        else
        {
            await File.WriteAllBytesAsync(Path, Content, cancellationToken);
        }
    }
}