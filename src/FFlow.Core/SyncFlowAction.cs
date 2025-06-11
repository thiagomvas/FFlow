namespace FFlow.Core;

/// <summary>
/// Represents a delegate for a workflow action that executes synchronously.
/// </summary>
/// <param name="context">The workflow context in which the action is executed.</param>
/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
public delegate void SyncFlowAction(IFlowContext context, CancellationToken cancellationToken = default);