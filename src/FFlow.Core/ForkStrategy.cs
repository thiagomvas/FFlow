namespace FFlow.Core;

/// <summary>
/// Represents the strategy for handling forked steps in a workflow.
/// </summary>
public enum ForkStrategy
{
    /// <summary>
    /// Waits for all forked steps to complete before proceeding with the workflow.
    /// </summary>
    WaitForAll,
    /// <summary>
    /// Proceeds with the workflow without waiting for forked steps to complete. It will wait for them when the main step is completed.
    /// </summary>
    FireAndForget,
}