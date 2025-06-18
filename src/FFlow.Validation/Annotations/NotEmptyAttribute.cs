using FFlow.Core;

namespace FFlow.Validation.Annotations;


public class NotEmptyAttribute : BaseFlowValidationAttribute
{
    public string[] Keys { get; }
    
public NotEmptyAttribute(params string[] keys)
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
        return new NotEmptyStep(Keys);
    }
}