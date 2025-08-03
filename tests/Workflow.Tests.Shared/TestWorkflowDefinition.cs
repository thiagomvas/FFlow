using FFlow;
using FFlow.Core;
using FFlow.Extensions;

namespace Workflow.Tests.Shared;

public class TestWorkflowDefinition : WorkflowDefinition
{
    public TestWorkflowDefinition()
    {
        MetadataStore.SetName("Test Workflow")
            .SetDescription("A workflow for testing purposes.");
    }

    public override void OnConfigure(WorkflowBuilderBase builder)
    {
        builder
            .StartWith<TestStep>()
            .Then<DelayedStep>()
            .Then<TestStep>();
    }

    public override Action<WorkflowOptions> OnConfigureOptions()
    {
        return options =>
        {
        };
    }

    public IWorkflow Build()
    {
        return CreateBuilder()
            .Build();
    }


    public IWorkflowMetadataStore MetadataStore { get; } = new InMemoryMetadataStore();
}