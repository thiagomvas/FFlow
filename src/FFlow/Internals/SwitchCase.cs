using FFlow.Core;

namespace FFlow;

internal record SwitchCase(Func<IFlowContext, bool> Condition, nFFlowBuilder? Builder, string? Name = null);