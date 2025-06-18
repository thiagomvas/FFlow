using System.Text.RegularExpressions;
using FFlow.Core;

namespace FFlow.Validation;

/// <summary>
/// Checks if the value associated with the specified key in the flow context matches a given regular expression pattern.
/// </summary>
internal class RegexPatternStep : IFlowStep
{
    private readonly string _key;
    private readonly string _pattern;

    public RegexPatternStep(string key, string pattern)
    {
        _key = key ?? throw new ArgumentNullException(nameof(key));
        _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
    }

    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        if (!context.TryGet<string>(_key, out var value))
        {
            throw new FlowValidationException($"Key '{_key}' not found in the context.");
        }

        if (!Regex.IsMatch(value, _pattern))
        {
            throw new FlowValidationException($"Value '{value}' does not match the pattern '{_pattern}'.");
        }

        return Task.CompletedTask;
    }
}