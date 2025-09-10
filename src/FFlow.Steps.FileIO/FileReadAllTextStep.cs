using FFlow.Core;

namespace FFlow.Steps.FileIO;
/// <summary>
/// A workflow step that reads the entire contents of a file as text.
/// The result can be exposed as step output and optionally stored in the workflow context.
/// </summary>
[StepName("File Read All Text")]
[StepTags("file", "io")]
public class FileReadAllTextStep : FlowStep
{
    /// <summary>
    /// Gets or sets the path of the file to read.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional key under which the file content
    /// will be saved into the workflow context.
    /// </summary>
    public string SaveToKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets the text content of the file after it has been read.
    /// </summary>
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