using FFlow.Core;
using Microsoft.Extensions.DependencyInjection;
using Workflow.Tests.Shared;

namespace FFlow.Extensions.Microsoft.DependencyInjection.Tests;

public class Tests
{
    [Test]
    public async Task AddFlow_ShouldRegisterAllWorkflowDefinitionsFound()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow(typeof(TestWorkflowDefinition).Assembly);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var workflowDefinitions = serviceProvider.GetServices<IWorkflowDefinition>().ToList();
        
        Assert.That(workflowDefinitions, Is.Not.Empty, "No workflow definitions were registered.");
        Assert.That(workflowDefinitions.Count, Is.EqualTo(1), "Expected exactly one workflow definition to be registered.");
    }
    
    [Test]
    public async Task AddFlow_ShouldRegisterAllStepsFound()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow(typeof(TestWorkflowDefinition).Assembly).AddTransient<TestService>()
            .AddTransient<IFlowStep, DiStep>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var flowSteps = serviceProvider.GetServices<IFlowStep>().ToList();
        
        Assert.That(flowSteps, Is.Not.Empty, "No flow steps were registered.");
        // Check for test steps
        Assert.That(flowSteps.Any(step => step.GetType() == typeof(TestStep)), Is.True, "TestStep was not registered.");
        Assert.That(flowSteps.Any(step => step.GetType() == typeof(TestStepDecorator)), Is.False, "A decorator was registered.");
        Assert.That(flowSteps.Any(step => step.GetType() == typeof(DiStep)), Is.True, "DiStep was not registered.");
    }
}