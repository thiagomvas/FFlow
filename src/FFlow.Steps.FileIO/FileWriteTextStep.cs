using FFlow.Core;

namespace FFlow.Steps.FileIO;
/// <summary>
/// A workflow step that writes or appends text content to a file.
/// Content can be provided directly or retrieved from the workflow context.
/// </summary>
[StepName("File Write/Append Text")]
[StepTags("file", "io")]
public class FileWriteTextStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the file to write to.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text content to be written to the file.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the key used to fetch text content from the workflow context.
    /// If provided and found, this value will overwrite <see cref="Content"/>.
    /// </summary>
    public string ContextSourceKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to append to the file if it already exists.
    /// If <c>false</c>, the file will be overwritten.
    /// </summary>
    public bool Append { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to prepend a new line before the appended content.
    /// Only has effect when <see cref="Append"/> is <see langword="true"/>
    /// </summary>
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