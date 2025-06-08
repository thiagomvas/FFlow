using FFlow.Core;

namespace FFlow.Demo;

public class HelloWorkflow : IWorkflowDefinition
{
    public IWorkflow Build()
    {
        return new FFlowBuilder<object?>()
            .StartWith<HelloStep>()
            .Build();
    }
}