using FFlow.Core;
namespace FFlow.Validation;

public static class ValidationExtensions
{
    /// <summary>
    /// Registers the validation decorators in the workflow builder.
    /// </summary>
    /// <param name="builder">The builder responsible for creating the steps</param>
    /// <returns>The same <see cref="WorkflowBuilderBase"/> instance.</returns>
    public static FFlowBuilder UseValidators(this FFlowBuilder builder)
    {
        builder.WithDecorator(step => new ValidatorDecorator(step));
        return builder;
    }
    /// <summary>
    /// Checks if a specific key exists in the workflow context.
    /// </summary>
    /// <param name="builder">The workflow builder to attach the validation step into.</param>
    /// <param name="key">The key to check for.</param>
    /// <returns>The current instance of <see cref="WorkflowBuilderBase"/>.</returns>
    public static WorkflowBuilderBase RequireKey(this WorkflowBuilderBase builder, string key)
    {
        return builder.RequireKeys(key);
    }
        
    /// <summary>
    /// Checks if multiple specific keys exist in the workflow context.
    /// </summary>
    /// <param name="builder">The workflow builder to attach the validation step into.</param>
    /// <param name="keys">The keys to check for.</param>
    /// <returns>The current instance of <see cref="WorkflowBuilderBase"/>.</returns>
    public static WorkflowBuilderBase RequireKeys(this WorkflowBuilderBase builder, params string[] keys)
    {
        if (keys == null || keys.Length == 0) throw new ArgumentException("Keys cannot be null or empty.", nameof(keys));
        
        foreach (var key in keys)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(keys));
        }
        
        var step = new HasKeyStep(keys);
        builder.AddStep(step);
        return builder;
    }
        
    /// <summary>
    /// Checks if a specific key exists in the workflow context and matches a given pattern.
    /// </summary>
    /// <param name="builder">The workflow builder to attach the validation step into.</param>
    /// <param name="key">The key to check for.</param>
    /// <param name="pattern">The regex pattern to check for.</param>
    /// <returns>The current instance of <see cref="WorkflowBuilderBase"/>.</returns>
    public static WorkflowBuilderBase RequireRegex(this WorkflowBuilderBase builder, string key, string pattern)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentException("Pattern cannot be null or empty.", nameof(pattern));
        
        var step = new RegexPatternStep(key, pattern);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Checks if the specified keys in the workflow context are not null.
    /// </summary>
    /// <param name="builder">The workflow builder to attach the validation step into.</param>
    /// <param name="keys">The keys in which to check the values are not null</param>
    /// <returns>The current instance of <see cref="WorkflowBuilderBase"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when any of the keys passed are null or empty.</exception>
    public static WorkflowBuilderBase RequireNotNull(this WorkflowBuilderBase builder, params string[] keys)
    {
        if (keys == null || keys.Length == 0) throw new ArgumentException("Keys cannot be null or empty.", nameof(keys));
        
        foreach (var key in keys)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(keys));
        }
        
        var step = new NotNullStep(keys);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Checks if the specified keys in the workflow context are not empty.
    /// </summary>
    /// <param name="builder">The workflow builder to attach the validation step into.</param>
    /// <param name="keys">The keys in which to check the values are not null</param>
    /// <returns>The current instance of <see cref="WorkflowBuilderBase"/>.</returns>
    /// <remarks>
    /// This only works for <see cref="string"/> and <see cref="ICollection{T}"/> types.
    /// </remarks>
    public static WorkflowBuilderBase RequireNotEmpty(this WorkflowBuilderBase builder, params string[] keys)
    {
        if (keys == null || keys.Length == 0) throw new ArgumentException("Keys cannot be null or empty.", nameof(keys));
        
        foreach (var key in keys)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(keys));
        }
        
        var step = new NotEmptyStep(keys);
        builder.AddStep(step);
        return builder;
    }
    
    
    
    
}