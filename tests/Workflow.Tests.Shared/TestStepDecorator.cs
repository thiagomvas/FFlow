using FFlow.Core;

namespace Workflow.Tests.Shared;

public class TestStepDecorator : BaseStepDecorator
{
    public TestStepDecorator(IFlowStep innerStep) : base(innerStep)
    {
        
    }

    public override Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        context.SetValue("decorated_counter", context.GetValue<int>("decorated_counter") + 1);
        return _innerStep.RunAsync(context, cancellationToken);
    }
}