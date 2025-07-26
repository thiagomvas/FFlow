using FFlow.Core;

namespace FFlow.Visualization;

/// <summary>
/// Configuration flags to configure how a <see cref="WorkflowGraph"/> should be generated.
/// </summary>
[Flags]
public enum VisualizationFlags : uint
{
    /// <summary> Displays all the nodes. </summary>
    None = 0,
    /// <summary> Hides all instances of <see cref="InputSetterStep"/>. </summary>
    IgnoreInputSetters = 1 << 0,
    /// <summary> Hides all instances of <see cref="OutputSetterStep"/>. </summary>
    IgnoreOutputSetters = 1 << 1,
    /// <summary> Hides any steps with a <see cref="SilentStepAttribute"/> </summary>
    IgnoreSilentSteps = 1 << 2,
    /// <summary> Hides all instances of  <see cref="InputSetterStep"/> and <see cref="OutputSetterStep"/> </summary>
    IgnoreInputOutputSetters = IgnoreInputSetters | IgnoreOutputSetters,
    /// <summary> Applies all the filters. </summary>
    All = uint.MaxValue, 
}