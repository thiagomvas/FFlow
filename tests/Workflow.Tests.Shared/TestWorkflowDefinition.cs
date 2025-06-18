using FFlow;
using FFlow.Core;

namespace Workflow.Tests.Shared;

public class TestWorkflowDefinition : IWorkflowDefinition
{
    public IWorkflow Build()
    {
        return new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<DelayedStep>()
            .Then<TestStep>()
            .Build();
    }
}