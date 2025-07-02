namespace FFlow.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class StepTagsAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the tags associated with the step.
    /// </summary>
    public string[] Tags { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StepTagsAttribute"/> class with the specified tags.
    /// </summary>
    /// <param name="tags">The tags to associate with the step.</param>
    public StepTagsAttribute(params string[] tags)
    {
        Tags = tags ?? throw new ArgumentNullException(nameof(tags));
    }
}