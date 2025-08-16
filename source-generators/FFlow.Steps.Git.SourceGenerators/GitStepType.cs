namespace FFlow.Steps.Git.SourceGenerators;

public readonly record struct GitStepType(string StepClass, string? StringParam, string? StringProperty)
{
    public string StepName => StepClass.Replace("Step", "");
    public string MethodName => $"{StepName}";
    public string StepClass { get; } = StepClass;
    public string? StringParam { get; } = StringParam;
    public string? StringProperty { get; } = StringProperty;
}