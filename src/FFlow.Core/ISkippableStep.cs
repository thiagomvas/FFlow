namespace FFlow.Core;

/// <summary>
/// Represents a step in a workflow that can be skipped based on a condition.
/// </summary>
public interface ISkippableStep
{
    /// <summary>
    /// Sets a condition that determines whether the step should be skipped.
    /// </summary>
    /// <param name="skipCondition"> The condition to evaluate. If this function returns true, the step execution is skipped.</param>
    void SetSkipCondition(Func<IFlowContext, bool> skipCondition);
}