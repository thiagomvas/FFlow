using System.Security.Cryptography;
using FFlow.Core;

namespace FFlow.Steps.FileIO;

[StepName("Checksum")]
[StepTags("file", "io", "checksum")]
public class ChecksumStep : FlowStep
{
    public string Path { get; set; } = string.Empty;
    public ChecksumAlgorithm Algorithm { get; set; } = ChecksumAlgorithm.SHA256; 
    public string SaveToKey { get; set; } = string.Empty;
    public string Checksum { get; private set; } = string.Empty;
    public string CompareWith { get; set; } = string.Empty; 
    
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(Path));
        }
        
        if (!File.Exists(Path))
        {
            throw new FileNotFoundException($"File not found: {Path}", Path);
        }

        using var stream = File.OpenRead(Path);

        byte[] hashBytes = Algorithm switch
        {
            ChecksumAlgorithm.MD5 => MD5.HashData(stream),
            ChecksumAlgorithm.SHA1 => SHA1.HashData(stream),
            ChecksumAlgorithm.SHA256 => SHA256.HashData(stream),
            ChecksumAlgorithm.SHA384 => SHA384.HashData(stream),
            ChecksumAlgorithm.SHA512 => SHA512.HashData(stream),
            _ => throw new ArgumentOutOfRangeException()
        };
        Checksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        
        context.SetOutputFor<ChecksumStep, string>(Checksum);

        if (!string.IsNullOrWhiteSpace(SaveToKey))
        {
            context.SetValue(SaveToKey, Checksum);
        }
        
        if (!string.IsNullOrWhiteSpace(CompareWith) && !string.Equals(Checksum, CompareWith, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Checksum mismatch. Calculated: {Checksum}, Expected: {CompareWith}");
        }

        return Task.CompletedTask;
    }
    
    public enum ChecksumAlgorithm
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }
}