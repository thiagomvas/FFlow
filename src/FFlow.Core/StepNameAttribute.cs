namespace FFlow.Core;

[AttributeUsage(AttributeTargets.Class)]
public class StepNameAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the name of the step.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StepNameAttribute"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the step.</param>
    public StepNameAttribute(string name)
    {
        Name = name;
    }
}
