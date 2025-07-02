namespace FFlow.Core;

/// <summary>
/// Defines a step in a workflow that can be retried upon failure.
/// </summary>
public interface IRetryableFlowStep
{
    /// <summary>
    /// Sets the retry policy for this step. The policy defines how many times the step should be retried in case of failure, and under what conditions.
    /// </summary>
    /// <param name="retryPolicy">
    /// The retry policy to apply to this step. This should implement the IRetryPolicy interface, which defines the retry logic.
    /// </param>
    /// <returns></returns>
    IRetryPolicy SetRetryPolicy(IRetryPolicy retryPolicy);
}