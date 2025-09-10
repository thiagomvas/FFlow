using FFlow.Core;

namespace FFlow.Steps.FileIO;
/// <summary>
/// A workflow step that writes or appends binary content to a file.
/// Content can be provided directly or retrieved from the workflow context.
/// </summary>
[StepName("File Write/Append Bytes")]
[StepTags("file", "io")]
public class FileWriteBytesStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the file to write to.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the binary content to be written to the file.
    /// </summary>
    public byte[] Content { get; set; } = [];

    /// <summary>
    /// Gets or sets the key used to fetch the binary content from the workflow context.
    /// If provided and found, this value will overwrite <see cref="Content"/>.
    /// </summary>
    public string ContextSourceKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to append to the file if it already exists.
    /// If <c>false</c>, the file will be overwritten.
    /// </summary>
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