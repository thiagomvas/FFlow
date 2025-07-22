namespace FFlow.Visualization;

[Flags]
public enum VisualizationFlags : uint
{
    None = 0,
    IgnoreInputSetters = 1 << 0,
    IgnoreOutputSetters = 1 << 1,
    IgnoreSilentSteps = 1 << 2,
    IgnoreInputOutputSetters = IgnoreInputSetters | IgnoreOutputSetters,
    All = uint.MaxValue, 
}