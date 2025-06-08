using FFlow.Core;

namespace FFlow.Demo;

public class HelloWorkflow : IWorkflowDefinition
{
    public IWorkflow Build()
    {
        return new FFlowBuilder()
            .StartWith<HelloStep>()
            .Build();
    }
}