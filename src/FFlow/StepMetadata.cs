namespace FFlow;

public record StepMetadata(string Id, string Name, string? Description = null, IEnumerable<string>? Tags = null);