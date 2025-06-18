using FFlow.Core;

namespace FFlow.Validation.Annotations;

public class RequireNotNullAttribute : BaseFlowValidationAttribute
{
    public string[] Keys { get; }
    
    public RequireNotNullAttribute(params string[] keys)
    {
        if (keys == null || keys.Length == 0) throw new ArgumentException("Keys cannot be null or empty.", nameof(keys));

        foreach (var key in keys)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(keys));
        }

        Keys = keys;
    }
    public override IFlowStep CreateValidationStep()
    {
        return new NotNullStep(Keys);
    }
}