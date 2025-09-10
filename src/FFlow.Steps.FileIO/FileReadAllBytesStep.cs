using FFlow.Core;

namespace FFlow.Steps.FileIO;
/// <summary>
/// A workflow step that reads the entire contents of a file into a byte array.
/// The result can be exposed as step output and optionally stored in the workflow context.
/// </summary>
[StepName("File Read All Bytes")]
[StepTags("file", "io")]
public class FileReadAllBytesStep : FlowStep
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
    /// Gets the byte array content of the file after it has been read.
    /// </summary>
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