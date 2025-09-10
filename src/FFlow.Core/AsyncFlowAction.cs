namespace FFlow.Core;

/// <summary>
/// Represents a delegate for a workflow action that can be executed asynchronously.
/// </summary>
/// <param name="context">The workflow context in which the action is executed.</param>
/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public delegate void AsyncFlowAction(IFlowContext context, CancellationToken cancellationToken = default);