namespace FFlow.Core;

public delegate Task FlowAction(IFlowContext context, CancellationToken cancellationToken = default);
