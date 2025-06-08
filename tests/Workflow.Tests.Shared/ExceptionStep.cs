using FFlow.Core;

namespace Workflow.Tests.Shared;

public class ExceptionStep : IFlowStep
{
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("This step always fails.");
    }
}