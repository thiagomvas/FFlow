using FFlow.Core;

namespace FFlow;

internal record SwitchCase(Func<IFlowContext, bool> Condition, WorkflowBuilderBase? Builder, string? Name = null);