using FFlow.Core;

namespace FFlow.Validation.Annotations;

public class RequireRegexAttribute : BaseFlowValidationAttribute
{
    public string Key { get; }
    public string Pattern { get; }
    
public RequireRegexAttribute(string key, string pattern)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentException("Pattern cannot be null or empty.", nameof(pattern));

        Key = key;
        Pattern = pattern;
    }
    public override IFlowStep CreateValidationStep()
    {
        return new RegexPatternStep(Key, Pattern);
    }
}