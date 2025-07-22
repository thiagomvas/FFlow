using FFlow;
using FFlow.Core;
using FFlow.Extensions;

namespace Workflow.Tests.Shared;

public class TestWorkflowDefinition : IWorkflowDefinition
{
    public TestWorkflowDefinition()
    {
        MetadataStore.SetName("Test Workflow")
            .SetDescription("A workflow for testing purposes.");
    }
    public IWorkflow Build()
    {
        return CreateBuilder()
            .Build();
    }

    public IWorkflowBuilder CreateBuilder()
    {
        return new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<DelayedStep>()
            .Then<TestStep>();
    }

    public IWorkflowMetadataStore MetadataStore { get; } = new InMemoryMetadataStore();
}