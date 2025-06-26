using FFlow.Core;

namespace Workflow.Tests.Shared;

public class ExceptionStep : FlowStep
{
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("This step always fails.");
    }
}