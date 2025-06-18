using FFlow.Core;

namespace FFlow.Validation.Annotations;

/// <summary>
/// Checks if the specified keys exist in the flow context.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class RequireKeyAttribute : BaseFlowValidationAttribute
{
    public string[] Keys { get; }
    public RequireKeyAttribute(params string[] Keys)
    {
        if (Keys == null || Keys.Length == 0) throw new ArgumentException("Keys cannot be null or empty.", nameof(Keys));
        
        foreach (var key in Keys)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(Keys));
        }
        
        this.Keys = Keys;
    }

    public override IFlowStep CreateValidationStep()
    {
        return new HasKeyStep(Keys);
    }
}