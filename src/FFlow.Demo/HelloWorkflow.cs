using FFlow.Core;
using FFlow.Extensions;
using FFlow.Observability.Listeners;

namespace FFlow.Demo;

public class HelloWorkflow : WorkflowDefinition
{
    public HelloWorkflow()
    {
        MetadataStore.SetName("Hello Workflow")
            .SetDescription("A simple workflow that greets the world and then says goodbye.");
    }

    public override void OnConfigure(IWorkflowBuilder builder)
    {
        builder
            .StartWith<HelloStep>()
            .Input<HelloStep, string>(step => step.Name, "World")
            .Delay(1000)
            .Then<GoodByeStep>()
            .Input<GoodByeStep, string>(step => step.Name, "World");
    }

    public override Action<WorkflowOptions> OnConfigureOptions()
    {
        return options =>
        {
            options.WithEventListener(new ConsoleFlowEventListener());
        };
    }
}