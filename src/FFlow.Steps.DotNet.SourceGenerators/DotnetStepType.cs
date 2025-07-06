namespace FFlow.Steps.DotNet.SourceGenerators;

public readonly record struct DotnetStepType(string StepClass, string? StringParam, string? StringProperty)
{
    public string StepName => StepClass.Replace("Step", "");
    public string MethodName => $"{StepName}";
    public string StepClass { get; } = StepClass;
    public string? StringParam { get; } = StringParam;
    public string? StringProperty { get; } = StringProperty;
}