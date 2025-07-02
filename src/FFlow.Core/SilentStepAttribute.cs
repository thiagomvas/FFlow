namespace FFlow.Core;

/// <summary>
/// Marks a step as silent, indicating that it should be ignored by events.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SilentStepAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SilentStepAttribute"/> class.
    /// </summary>
    public SilentStepAttribute()
    {
        // No additional initialization required
    }
}