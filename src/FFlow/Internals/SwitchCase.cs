using FFlow.Core;

namespace FFlow;

internal record SwitchCase(Func<IFlowContext, bool> Condition, IWorkflowBuilder? Builder, string? Name = null);