namespace FFlow.Steps.Git.SourceGenerators;

public readonly record struct GitStepType(string StepClass, (string Param, string Property)[][] ParamPropertyPairsByAttribute)
{
    public string StepName => StepClass.Replace("Step", "");
    public string MethodName => StepName;
    public string StepClass { get; } = StepClass;
    public (string Param, string Property)[][] ParamPropertyPairsByAttribute { get; } = ParamPropertyPairsByAttribute;
}
